using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poképedia.Model
{
    public class Species
    {
        public int Base_Happiness { get; set; }

        public int Capture_Rate { get; set; }

        public Color Color { get; set; }

        public List<Egg> Egg_Groups { get; set; }

        public Evolution_Chain Evolution_Chain { get; set; }

        public Evolves_from_species Evolves_From_Species { get; set; }
    }
}
