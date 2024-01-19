using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly HttpClient _httpClient;

        public static string NextPage { get; set; }
        public static string PreviousPage { get; set; }

        public PokeApi(HttpClient httpClient)
        {
            _httpClient = httpClient; 
            _httpClient.BaseAddress = new System.Uri("https://pokeapi.co/api/v2/");
        }

        public async Task<List<Pokemon>> GetPokemonListAsync()
        {
            string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
            int offset = 0;
            int limit = 10;

            string requestUrl = $"{baseUrl}?offset={offset}&limit={limit}";


            var httpResponse = await _httpClient.GetAsync(requestUrl);
                        
            httpResponse.EnsureSuccessStatusCode();


            var jsonContent = await httpResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Pokemon>(jsonContent);

            if (result.Results is null)
            {
                result.Next = "";
            }
            
            // add static variables so they remember the pages in another method
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

        public async Task<string> DownloadPokemonSpritesAsync(string url)
        {
            var result = "";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    dynamic pokemonData = JObject.Parse(json);

                    // Get the front default sprite URL
                    string spriteUrl = pokemonData.sprites.front_default;

                    result = spriteUrl;

                    // Download the sprite image
                    HttpResponseMessage spriteResponse = await client.GetAsync(spriteUrl);
                    if (spriteResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"Downloaded sprite.");
                    }
                    else
                    {
                        Console.WriteLine($"Failed to download sprite @ " + url + ".");
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to fetch Pokémon data  @ " + url + ". Status code: " + response.StatusCode + "");
                }
            }

            return result;
        }

        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {

            var route = $"https://pokeapi.co/api/v2/pokemon-species/{name}";

            var httpResponse = await _httpClient.GetAsync(route);

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
            var route = $"https://pokeapi.co/api/v2/pokemon-species/{name}";

            var httpResponse = await _httpClient.GetAsync(route);

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

