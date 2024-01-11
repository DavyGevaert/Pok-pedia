using Poképedia.Model;

namespace Poképedia.Sdk.Abstractions
{
    public interface IPokeApi
    {

        Task<Pokemon> GetPokemonByNameAsync(string name);

        Task<Species> GetSpeciesByPokemonNameAsync(string name);

        Task<RootObject> GetRootObject();
    }
}
