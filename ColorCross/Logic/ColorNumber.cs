using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ColorCross.Logic
{
	struct LineOfColors
	{
		public bool IsDone;
		public List<ColorNumber> Colors;

		public struct ColorNumber : IEquatable<ColorNumber>
		{
			public int Count;
			public Color Color;

			public bool Equals(ColorNumber other)
			{
				return $"{Count}{Color}" == $"{other.Count}{other.Color}";
			}
		}
	}
}
