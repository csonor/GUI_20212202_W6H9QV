using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorCross.Logic
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
