using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace ColorCross.Logic
{
	class ColorCrossLogic
	{
		Color[,] pixels;
		List<Color> colors;
		LineOfColors[] rows;
		LineOfColors[] columns;
		int numberOfColoredLinesAndColumns;
		int currentCorrectLines;
		Bitmap bmp;

		public LineOfColors[] Rows { get => rows; }
		public LineOfColors[] Columns { get => columns; }
		public List<Color> Colors { get => colors; }

		public ColorCrossLogic()
		{

			pixels = new Color[0, 0];
			colors = new List<Color>();
			rows = new LineOfColors[0];
			columns = new LineOfColors[0];
			numberOfColoredLinesAndColumns = 0;
			currentCorrectLines = 0;
			bmp = new Bitmap(0, 0);
		}

		public void ImageReader(string fileName)
		{
			bmp = new Bitmap(fileName);
			rows = new LineOfColors[bmp.Height];
			columns = new LineOfColors[bmp.Width];

			for (int i = 0; i < bmp.Height; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					var color = bmp.GetPixel(j, i);
					var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
					if (!colors.Contains(newColor) && color.Name != "0")
						colors.Add(newColor);
				}
			}
			CountRowColors();
			CountColumnColors();
		}

		void CountRowColors()
		{
			for (int i = 0; i < rows.Length; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					int k = j + 1;
					int sum = 0;
					var color = bmp.GetPixel(j, i);
					var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
					while (k < bmp.Width && bmp.GetPixel(k, i) == color)
					{
						sum++;
						k++;
					}
					if (color.Name != "0")
					{
						if (rows[i].Colors == null)
							rows[i].Colors = new List<ColorNumber>();
						rows[i].Colors.Add(new ColorNumber { Color = newColor, Count = sum + 1 });
					}
					j = k - 1;
				}
				rows[i].IsDone = false;
			}
		}

		void CountColumnColors()
		{
			for (int i = 0; i < columns.Length; i++)
			{
				for (int j = 0; j < bmp.Height; j++)
				{
					int k = j + 1;
					int sum = 0;
					var color = bmp.GetPixel(i, j);
					var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
					while (k < bmp.Height && bmp.GetPixel(i, k) == color)
					{
						sum++;
						k++;
					}
					if (color.Name != "0")
					{
						if (columns[i].Colors == null)
							columns[i].Colors = new List<ColorNumber>();
						columns[i].Colors.Add(new ColorNumber { Color = newColor, Count = sum + 1 });
					}
					j = k - 1;
				}
				columns[i].IsDone = false;
			}
		}
	}
}
