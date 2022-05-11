using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;

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

	public class CellData : ObservableObject
    {
		int x;
		int y;
		int color;

		public int X
        {
			get {  return this.x;}
			set
            {
				SetProperty(ref this.x, value);
            }
        }
		public int Y
        {
			get {  return this.y;}
			set
            {
				SetProperty(ref this.y, value);
            }
        }
		public int Color
        {
			get {  return this.color;}
			set
            {
				SetProperty(ref this.color, value);
            }
        }
    }
	
}
