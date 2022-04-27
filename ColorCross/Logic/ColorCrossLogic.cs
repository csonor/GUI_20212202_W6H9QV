using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorCross.Backend
{
	class ColorCrossLogic : IImageRead
	{
		Color[,] pixels;
		List<Color> colors;
		LineOfColors[] rows;
		LineOfColors[] columns;
		Bitmap bmp;

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
					if (!colors.Contains(color) && color.Name != "0")
						colors.Add(color);
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
					while (k < bmp.Width && bmp.GetPixel(k, i) == color)
					{
						sum++;
						k++;
					}
					if (color.Name != "0")
					{
						if (rows[i].Colors == null)
							rows[i].Colors = new List<ColorNumber>();
						rows[i].Colors.Add(new ColorNumber { Color = color, Count = sum + 1 });
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
					while (k < bmp.Height && bmp.GetPixel(i, k) == color)
					{
						sum++;
						k++;
					}
					if (color.Name != "0")
					{
						if (columns[i].Colors == null)
							columns[i].Colors = new List<ColorNumber>();
						columns[i].Colors.Add(new ColorNumber { Color = color, Count = sum + 1 });
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
