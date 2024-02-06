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

            var pokemonList = await _pokemonApi.GetPokemonListAsyncNextOrPrevious(PokeApi.NextPage);

            if (pokemonList is null)
            {
                model.Results = null;
                model.disabled = true;
            }
            else
            {
                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);
                }

                model.disabled = false;
                model.Results = pokemonList;
            }

            return View("Index", model);
        }

        public async Task<IActionResult> PreviousPage()
        {
            HomeViewModel model = new HomeViewModel();

            var pokemonList = await _pokemonApi.GetPokemonListAsyncNextOrPrevious(PokeApi.PreviousPage);

            if (pokemonList is null)
            {
                model.Results = null;
                model.disabled = true;
            }
            else
            {
                foreach (var pokemon in pokemonList)
                {
                    pokemon.Image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);
                }

                model.disabled = false;
                model.Results = pokemonList;
            }           

            return View("Index", model);
        }

        public async Task<IActionResult> Details(Pokemon pokemon)
		{
			var poké = await _pokemonApi.GetPokemonByNameAsync(pokemon.Name);

            var species = await _pokemonApi.GetSpeciesByPokemonNameAsync(pokemon.Name);

            if (species.Color is null) { species.Color = new Color(); };
            if (species.Egg_Groups is null) { species.Egg_Groups = new List<Egg>(); };
            if (species.Evolution_Chain is null) { species.Evolution_Chain = new Evolution_Chain(); };
            if (species.Evolves_From_Species is null) { species.Evolves_From_Species = new Evolves_from_species(); };

            poké.Species = species;

            var image = await _pokemonApi.DownloadPokemonSpritesAsync(pokemon.Url);

            poké.Image = image;

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
