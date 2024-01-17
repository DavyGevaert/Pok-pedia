using Poképedia.Model;

namespace Poképedia.Mvc.Models
{
	public class HomeViewModel
	{
		public List<Pokemon> Results { get; set; }

		public string NextPage { get; set; }

		public string PreviousPage { get; set; }
	}
}
