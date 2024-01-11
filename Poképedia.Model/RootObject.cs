using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poképedia.Model
{
    public class RootObject
    {
        public int Count { get; set; }

        public List<Pokemon> Results { get; set; }
    }
}
