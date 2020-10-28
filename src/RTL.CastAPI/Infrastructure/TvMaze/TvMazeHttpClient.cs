using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RTL.CastAPI.Model;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RTL.CastAPI.Infrastructure.TvMaze
{
    /// <summary>
    /// API Documentation: http://www.tvmaze.com/api
    /// </summary>
    public class TvMazeHttpClient : ITvMazeHttpClient
    {
        private readonly HttpClient _client;

        public TvMazeHttpClient(HttpClient client, IOptions<TvMazeSettings> settings)
        {
            _client = client;
            _client.BaseAddress = settings.Value.BaseUrl;
        }

        public async Task<IEnumerable<Show>> GetShowIndexPage(int page = 0)
        {
            var response = await _client.GetAsync($"/shows?page={page}");

            // If response is NotFound, we've reached the last page of the index. 
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return Enumerable.Empty<Show>();

            response.EnsureSuccessStatusCode();

            return await DeserializeAsync<List<Show>>(response);
        }

        public async Task<IEnumerable<CastMember>> GetShowCastAsync(int showId)
        {
            var response = await _client.GetAsync($"/shows/{showId}/cast");

            response.EnsureSuccessStatusCode();

            return await DeserializeAsync<List<CastMember>>(response);
        }

        private async Task<T> DeserializeAsync<T>(HttpResponseMessage response) =>
            JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

    }
}
