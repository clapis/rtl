using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using RTL.CastAPI.Application.Commands.SyncMetadata;
using RTL.CastAPI.Configuration;
using RTL.CastAPI.HostedServices;
using RTL.CastAPI.Infrastructure.Data;
using RTL.CastAPI.Infrastructure.Data.EFCore;
using RTL.CastAPI.Infrastructure.TvMaze;

namespace RTL.WebHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<TvMazeSettings>(Configuration.GetSection("TvMaze"));
            services.Configure<SynchronizationSettings>(Configuration.GetSection("Synchronization"));

            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(Startup));

            services.AddTransient<ThrottleHandler>();
            services.AddHttpClient<TvMazeHttpClient>()
                    .AddHttpMessageHandler<ThrottleHandler>();
            
            services.AddTransient<ITvMazeHttpClient>(provider => provider.GetRequiredService<TvMazeHttpClient>());

            services.AddDbContext<CastDBContext>();
            services.AddHostedService<SynchronizationService>();
            services.AddTransient<IScrapingService, ScrapingService>();
            services.AddTransient<IShowsRepository, ShowsRepository>();
            services.AddTransient<IPeopleRepository, PeopleRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RTL Cast API", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(opts => opts.SwaggerEndpoint("/swagger/v1/swagger.json", "RTL Cast API v1"));

            app.UseMvc();
        }
    }
}
