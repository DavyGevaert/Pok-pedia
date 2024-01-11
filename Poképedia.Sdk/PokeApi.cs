using Poképedia.Model;
using Poképedia.Sdk.Abstractions;
using System.Net.Http;
using System.Net.Http.Json;

namespace Poképedia.Sdk
{
    public class PokeApi : IPokeApi
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0

        private readonly IHttpClientFactory _httpClientFactory;

        public PokeApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RootObject> GetRootObject()
        {
            var httpClient = _httpClientFactory.CreateClient("Poképedia");

            var route = "https://pokeapi.co/api/v2/pokemon?limit=100000&offset=0";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<RootObject>();

            if (result is null)
            {
                return new RootObject();
            }

            return result;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("Poképedia");

            var route = $"https://pokeapi.co/api/v2/pokemon-species/{name}";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<Pokemon>();

            if (result is null)
            {
                return new Pokemon();
            }

            return result;
        }

        public async Task<Species> GetSpeciesByPokemonNameAsync(string name)
        {
            var httpClient = _httpClientFactory.CreateClient("Poképedia");

            var route = $"https://pokeapi.co/api/v2/pokemon-species/{name}";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<Species>();

            if (result is null)
            {
                return new Species();
            }

            return result;
        }
    }
}
