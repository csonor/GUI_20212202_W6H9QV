using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace ColorCross.Logic
{
	class LineOfColors:ObservableObject
	{
		bool isDone;
		public bool IsDone { get { return this.isDone; } set { SetProperty(ref isDone, value); } }
		public List<ColorNumber> Colors { get; set; }

		public struct ColorNumber : IEquatable<ColorNumber>
		{
			public int Count { get; set; }
			public int Color { get; set; }

			public bool Equals(ColorNumber other)
			{
				return $"{Count}{Color}" == $"{other.Count}{other.Color}";
			}
		}
	}

	public class CellData : ObservableObject
	{
		int x;
		int y;
		int color;

		public int X
		{
			get { return this.x; }
			set
			{
				SetProperty(ref this.x, value);
			}
		}
		public int Y
		{
			get { return this.y; }
			set
			{
				SetProperty(ref this.y, value);
			}
		}
		public int Color
		{
			get { return this.color; }
			set
			{
				SetProperty(ref this.color, value);
			}
		}
	}

}
