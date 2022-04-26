using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorCross.Backend
{
	struct ColorNumber
	{
		public int Count;
		public Color Color;
	}

	struct LineOfColors
	{
		public bool IsDone;
		public List<ColorNumber> Colors;
	}
}
