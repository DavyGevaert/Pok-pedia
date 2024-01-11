using Microsoft.AspNetCore.Mvc;
using Poképedia.Model;
using Poképedia.Mvc.Models;
using Poképedia.Sdk;
using System.Diagnostics;

namespace Poképedia.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly PokeApi _pokemonApi;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,
                                PokeApi pokeApi)
        {
            _logger = logger;
            _pokemonApi = pokeApi;
        }

        public async Task<IActionResult> Index()
        {
            RootObject rootObj = await _pokemonApi.GetRootObject();

            return View(rootObj);
        }

		public async Task<IActionResult> Details(Pokemon pokemon)
		{
			var poké = await _pokemonApi.GetPokemonByNameAsync(pokemon.Name);

            var species = await _pokemonApi.GetSpeciesByPokemonNameAsync(pokemon.Name);

            poké.Species = species;

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
