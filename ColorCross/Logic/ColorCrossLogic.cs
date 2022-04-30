using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
		Color[][] pixels;
		List<Color> colors;
		LineOfColors[] rows;
		LineOfColors[] columns;
		int numberOfColoredLinesAndColumns;
		int currentCorrectLines;
		Bitmap bmp;
		string fileName;

		public LineOfColors[] Rows { get => rows; }
		public LineOfColors[] Columns { get => columns; }
		public List<Color> Colors { get => colors; }

		public ColorCrossLogic()
		{
			pixels = Array.Empty<Color[]>();
			colors = new List<Color>();
			rows = Array.Empty<LineOfColors>();
			columns = Array.Empty<LineOfColors>();
			numberOfColoredLinesAndColumns = 0;
			currentCorrectLines = 0;
			bmp = new Bitmap(1, 1);
			fileName = string.Empty;
		}

		public void ImageReader(string filePath)
		{
			bmp = new Bitmap(filePath);
			rows = new LineOfColors[bmp.Height];
			columns = new LineOfColors[bmp.Width];

			fileName = new string(filePath.Split('\\')[1].TakeWhile(x => x != '.').ToArray());
			if (File.Exists(fileName + ".json"))
				LoadPixelsFromFile();
			else CreatePixelArray();

			CountUniqueColors();
			CountRowColors();
			CountColumnColors();
		}

		void LoadPixelsFromFile()
		{
			var json = File.ReadAllText(fileName + ".json");
			pixels = JsonSerializer.Deserialize<Color[][]>(json);
		}

		void CreatePixelArray()
		{
			pixels = new Color[bmp.Height][];
			for (int i = 0; i < pixels.Length; i++)
			{
				pixels[i] = new Color[bmp.Width];
			}
		}

		void CountUniqueColors()
		{
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
							rows[i].Colors = new List<LineOfColors.ColorNumber>();
						rows[i].Colors.Add(new LineOfColors.ColorNumber { Color = newColor, Count = sum + 1 });
					}
					j = k - 1;
				}
				rows[i].IsDone = false;
				numberOfColoredLinesAndColumns++;
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
							columns[i].Colors = new List<LineOfColors.ColorNumber>();
						columns[i].Colors.Add(new LineOfColors.ColorNumber { Color = newColor, Count = sum + 1 });
					}
					j = k - 1;
				}
				columns[i].IsDone = false;
				numberOfColoredLinesAndColumns++;
			}
		}

		public bool ColorCheck(int i, int j)
		{
			if (RowCheck(i))
			{
				if (!Rows[i].IsDone)
				{
					currentCorrectLines++;
					Rows[i].IsDone = true;
				}
			}
			else if (Rows[i].IsDone)
			{
				currentCorrectLines--;
				Rows[i].IsDone = false;
			}

			if (ColumnCheck(j))
			{
				if (!Columns[j].IsDone)
				{
					currentCorrectLines++;
					Columns[j].IsDone = true;
				}
			}
			else if (Columns[j].IsDone)
			{
				currentCorrectLines--;
				Columns[j].IsDone = false;
			}

			return numberOfColoredLinesAndColumns == currentCorrectLines;
		}

		bool RowCheck(int i)
		{
			var rowColors = new List<LineOfColors.ColorNumber>();
			for (int j = 0; j < pixels[i].Length; j++)
			{
				int k = j + 1;
				int sum = 0;
				var color = pixels[i][j];
				while (k < pixels[i].Length && pixels[i][k] == color)
				{
					sum++;
					k++;
				}
				if (color.ToString() != "#00000000")
				{
					rowColors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
				}
				j = k - 1;
			}
			return rowColors.SequenceEqual(Rows[i].Colors);
		}

		bool ColumnCheck(int j)
		{
			var columnColors = new List<LineOfColors.ColorNumber>();
			for (int i = 0; i < pixels.Length; i++)
			{
				int k = i + 1;
				int sum = 0;
				var color = pixels[i][j];
				while (k < pixels.Length && pixels[k][j] == color)
				{
					sum++;
					k++;
				}
				if (color.ToString() != "#00000000")
				{
					columnColors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
				}
				i = k - 1;
			}
			return columnColors.SequenceEqual(Columns[j].Colors);
		}

		public void ChangePixelColor(System.Windows.Media.Brush brush, int i, int j)
		{
			pixels[i][j] = ((SolidColorBrush)brush).Color;
		}

		public void GameEnd(bool isCompleted)
		{
			if (isCompleted)
			{
				//TODO map done
			}
			else
			{
				var json = JsonSerializer.Serialize(pixels, typeof(Color[][]));
				File.WriteAllText($"{fileName}.json", json);
			}
		}

		public void ResetGame()
		{
			var path = fileName + ".json";
			if (File.Exists(path))
				File.Delete(path);
			CreatePixelArray();
		}
	}
}
