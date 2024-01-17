using Newtonsoft.Json;
using Poképedia.Model;
using Poképedia.Sdk.Abstractions;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace Poképedia.Sdk
{
    public class PokeApi : IPokeApi
    {
        // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-8.0

        private readonly IHttpClientFactory _httpClientFactory;

        private readonly HttpClient _httpClient;

        public static string NextPage { get; set; }
        public static string PreviousPage { get; set; }

        public PokeApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _httpClient = new HttpClient(); 
            _httpClient.BaseAddress = new System.Uri("https://pokeapi.co/api/v2/");
        }

        public async Task<List<Pokemon>> GetPokemonListAsync()
        {
            

            var httpResponse = await _httpClient.GetAsync("pokemon");
                        
            httpResponse.EnsureSuccessStatusCode();


            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Pokemon>(jsonContent);

            if (result.Results is null)
            {
                result.Next = "";
            }
            
            NextPage = result.Next;
            PreviousPage = result.Previous;

            return result?.Results;
        }

        public async Task<List<Pokemon>> GetNextPokemonListAsync()
        {

            var httpResponse = await _httpClient.GetAsync(NextPage);

            httpResponse.EnsureSuccessStatusCode();


            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Pokemon>(jsonContent);

            if (result.Results is null)
            {
                result.Next = "";
            }

            NextPage = result.Next;
            PreviousPage = result.Previous;

            return result?.Results;
        }

        public async Task<List<Pokemon>> GetPreviousPokemonListAsync()
        {
            var httpResponse = await _httpClient.GetAsync(PreviousPage);

            httpResponse.EnsureSuccessStatusCode();


            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Pokemon>(jsonContent);

            if (result.Results is null)
            {
                result.Next = "";
            }

            NextPage = result.Next;
            PreviousPage = result.Previous;

            return result?.Results;
        }

        // additional methods, needs to be tested

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

