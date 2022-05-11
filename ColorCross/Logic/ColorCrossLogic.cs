using Microsoft.Toolkit.Mvvm.ComponentModel;
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
		//int clickCount;
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
			CheckForCorrectLines();
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
				if (rows[i] == null) rows[i] = new LineOfColors();
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
				if (columns[i] == null) columns[i] = new LineOfColors();
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

		void CheckForCorrectLines()
		{
			for (int i = 0; i < pixels.GetLength(0); i++)
				ColorCheck(i, 0);

			for (int i = 0; i < pixels.GetLength(1); i++)
			{
				ColorCheck(0, i);
			}
		}

		public bool Click(int x, int y, int color)
		{
			status[x][y].Color = color;
			ColorCheck(x, y);
			ClickCount++;
			return CheckIfImageIsDone();
		}

		void ColorCheck(int x, int y)
		{
			if (RowCheck(x))
			{
				if (!Rows[x].IsDone)
				{
					currentCorrectLines++;
					Rows[x].IsDone = true;
				}
			}
			else if (Rows[x].IsDone)
			{
				currentCorrectLines--;
				Rows[x].IsDone = false;
			}

			if (ColumnCheck(y))
			{
				if (!Columns[y].IsDone)
				{
					currentCorrectLines++;
					Columns[y].IsDone = true;
				}
			}
			else if (Columns[y].IsDone)
			{
				currentCorrectLines--;
				Columns[y].IsDone = false;
			}
		}

		bool RowCheck(int x)
		{
			var rowColors = new List<LineOfColors.ColorNumber>();
			for (int j = 0; j < pixels.GetLength(1); j++)
			{
				int k = j + 1;
				int sum = 0;
				var color = status[x][j].Color;
				while (k < pixels.GetLength(1) && status[x][k].Color == color)
				{
					sum++;
					k++;
				}
				if (color != -1)
				{
					rowColors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
				}
				j = k - 1;
			}
			return rowColors.SequenceEqual(rows[x].Colors);
		}

		bool ColumnCheck(int y)
		{
			var columnColors = new List<LineOfColors.ColorNumber>();
			for (int i = 0; i < pixels.GetLength(0); i++)
			{
				int k = i + 1;
				int sum = 0;
				var color = status[i][y].Color;
				while (k < pixels.GetLength(0) && status[k][y].Color == color)
				{
					sum++;
					k++;
				}
				if (color != -1)
				{
					columnColors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
				}
				i = k - 1;
			}
			return columnColors.SequenceEqual(columns[y].Colors);
		}

		public bool CheckIfImageIsDone()
		{
			return numberOfColoredLinesAndColumns == currentCorrectLines;
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
			CheckForCorrectLines();
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
