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
		Bitmap bmp;

		public LineOfColors[] Rows { get => rows; }
		public LineOfColors[] Columns { get => columns; }
		public List<Color> Colors { get => colors; }

		public ColorCrossLogic()
		{
			colors = new List<Color>();
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

		public Button[] CreateButtons()
		{
			pixels = new Color[bmp.Height, bmp.Width];
			for (int i = 0; i < pixels.GetLength(0); i++)
			{
				for (int j = 0; j < pixels.GetLength(1); j++)
				{
					pixels[i, j] = Color.FromArgb(0, 0, 0, 0);
				}
			}

			var buttons = new Button[bmp.Width * bmp.Height];
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i] = new Button
				{
					Name = i.ToString(),
					Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0)),
					BorderThickness = new System.Windows.Thickness(0),
					Command = new RoutedCommand() //TODO button chamge method
				};
			}
			return buttons;
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

		int[] GetRowAndColumnFromId(int id)
		{
			int[] rowCol = new int[2];
			rowCol[0] = id / bmp.Width;
			rowCol[1] = id % bmp.Width;
			return rowCol;
		}
	}
}
