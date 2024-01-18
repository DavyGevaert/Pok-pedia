namespace Poképedia.Model
{
    public class Pokemon
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string Image { get; set; }

		public List<Pokemon> Results { get; set; }

        public string Next { get; set; }

        public string Previous { get; set; }

        public Species Species { get; set; }

        public IList<Ability> Abilities { get; set; } = new List<Ability>();

	}
}
