using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using Color = System.Windows.Media.Color;

namespace ColorCross.Logic
{
	class ColorCrossLogic : IColorCrossLogic
	{
		int[,] pixels;
		List<List<CellData>> status;
		List<Color> colors;
		LineOfColors[] rows;
		LineOfColors[] columns;
		int numberOfColoredLinesAndColumns; 
		int currentCorrectLines;
		Bitmap bmp;
		string fileName;
		public int ClickCount { get; set; }

		public LineOfColors[] Rows { get => rows; }
		public LineOfColors[] Columns { get => columns; }
		public List<Color> Colors { get => colors; }
		public List<List<CellData>> Status { get => status; }

		public ColorCrossLogic()
		{
			pixels = new int[0, 0];
			status = new List<List<CellData>>();
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
			else CreateCellDataList();

			CountUniqueColors();
			CountRowColors();
			CountColumnColors();
		}

		void LoadPixelsFromFile()
		{
			var json = File.ReadAllText(fileName + ".json");
			var data = JsonSerializer.Deserialize<AllData>(json);
			ClickCount = data.ClickCount;
			status = data.Status;
		}

		void CreateCellDataList()
		{
			for (int i = 0; i < bmp.Height; i++)
			{
				List<CellData> rd = new List<CellData>();
				for (int j = 0; j < bmp.Width; j++)
				{
					CellData cd = new CellData() { X = i, Y = j, Color = -1 }; //temp
					rd.Add(cd);
				}
				status.Add(rd);
			}
		}

		void CountUniqueColors()
		{
			pixels = new int[bmp.Height, bmp.Width];
			for (int i = 0; i < bmp.Height; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					var color = bmp.GetPixel(j, i);
					var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
					if (!colors.Contains(newColor) && color.Name != "0")
						colors.Add(newColor);
					pixels[i, j] = colors.IndexOf(newColor);
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
					var color = pixels[i, j];
					while (k < bmp.Width && pixels[i, k] == color)
					{
						sum++;
						k++;
					}
					if (color != -1)
					{
						if (rows[i].Colors == null)
							rows[i].Colors = new List<LineOfColors.ColorNumber>();
						rows[i].Colors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
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
					var color = pixels[j, i];
					while (k < bmp.Height && pixels[k, i] == color)
					{
						sum++;
						k++;
					}
					if (color != -1)
					{
						if (columns[i].Colors == null)
							columns[i].Colors = new List<LineOfColors.ColorNumber>();
						columns[i].Colors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
					}
					j = k - 1;
				}
				columns[i].IsDone = false;
				numberOfColoredLinesAndColumns++;
			}
		}

		public bool Click(int x, int y, int color)
		{
			status[x][y].Color = color;
			ClickCount++;
			return Check();
		}

		public bool Check()
		{
			bool equal = true;
			int i = 0;
			while (i < pixels.GetLength(0))
			{
				int j = 0;
				while (j < pixels.GetLength(1) && equal)
				{
					equal = pixels[i, j] == status[i][j].Color;
					j++;
				}
				i++;
			}
			return equal;
		}

		public void GameEnd()
		{
			var json = JsonSerializer.Serialize(new AllData(ClickCount, status), typeof(AllData));
			File.WriteAllText($"{fileName}.json", json);
		}

		public void ResetGame()
		{
			var path = fileName + ".json";
			if (File.Exists(path))
				File.Delete(path);
			status.ForEach(x => x = new List<CellData>());
		}

		class AllData
		{
			public AllData(int clickCount, List<List<CellData>> status)
			{
				ClickCount = clickCount;
				Status = status;
			}

			public int ClickCount { get; }
			public List<List<CellData>> Status { get; }
		}
	}
}
