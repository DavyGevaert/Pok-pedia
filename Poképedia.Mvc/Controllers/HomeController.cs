using Microsoft.AspNetCore.Mvc;
using Poképedia.Model;
using Poképedia.Mvc.Models;
using Poképedia.Sdk;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Web;
using static System.Net.WebRequestMethods;

namespace Poképedia.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly PokeApi _pokemonApi;

        public HomeController(PokeApi pokeApi)
        {
            _pokemonApi = pokeApi; 
        }

        public async Task<IActionResult> Index()
        {
            HomeViewModel model = new HomeViewModel();

            var pokemonList = await _pokemonApi.GetPokemonListAsync();

            foreach (var pokemon in pokemonList)
            {
                pokemon.Image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);
            }

            model.Results = pokemonList;

            return View(model);
        }

        public async Task<IActionResult> NextPage()
        {
            HomeViewModel model = new HomeViewModel();

            var pokemonList = await _pokemonApi.GetNextPokemonListAsync();

            if (pokemonList is null)
            {
                // do nothing
                pokemonList = new List<Pokemon>();
                pokemonList.Add(new Pokemon { Name = "Next page cannot be rendered", Url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png" });

                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadSpriteIfDataIsNotFoundAsync(pokemon.Url);
                }
            }
            else
            {
                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);
                }
            }

            model.Results = pokemonList;

            return View("Index", model);
        }

        public async Task<IActionResult> PreviousPage()
        {
            HomeViewModel model = new HomeViewModel();

            var pokemonList = await _pokemonApi.GetPreviousPokemonListAsync();

            if (pokemonList is null)
            {
                // do nothing
                pokemonList = new List<Pokemon>();
                pokemonList.Add(new Pokemon { Name = "Previous page cannot be rendered", Url = "https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/25.png" });

                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadSpriteIfDataIsNotFoundAsync(pokemon.Url);
                }
            }
            else
            {
                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);
                }
            }

            
            

            model.Results = pokemonList;

            return View("Index", model);
        }

        public async Task<IActionResult> Details(Pokemon pokemon)
		{
			var poké = await _pokemonApi.GetPokemonByNameAsync(pokemon.Name);

            var species = await _pokemonApi.GetSpeciesByPokemonNameAsync(pokemon.Name);

            poké.Species = species;

            // var image = await _pokemonApi.GetPokemonImageByIdAsync(1);

            return View(poké);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
