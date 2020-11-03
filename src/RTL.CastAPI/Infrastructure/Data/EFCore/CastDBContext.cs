using Microsoft.EntityFrameworkCore;
using RTL.CastAPI.Model;

namespace RTL.CastAPI.Infrastructure.Data.EFCore
{
    public class CastDBContext : DbContext
    {
        public DbSet<Show> Shows { get; set; }
        public DbSet<Person> People { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseInMemoryDatabase("CastDB");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Show>(e =>
            {
                e.HasIndex(p => p.ExternalId);
                e.HasMany(p => p.Cast).WithOne(p => p.Show)
                    .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            });

            builder.Entity<Person>(e => e.HasIndex(p => p.ExternalId));

            builder.Entity<CastMember>(e =>
            {
                e.HasKey(x => new { x.ShowId, x.PersonId });
            });
        }

    }
}
