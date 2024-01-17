using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poképedia.Model
{
	public class Ability
	{
		public string Name { get; set; }

		public string Url { get; set; }

		public bool Is_Hidden { get; set; }

		public int Slot { get; set; }
	}
}
