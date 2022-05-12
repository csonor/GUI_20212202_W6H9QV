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
	class AllData
	{
		public AllData(int clickCount, List<List<CellData>> Status, int timer)
		{
			this.ClickCount = clickCount;
			this.Status = Status;
			this.Timer = timer;
		}

		public int ClickCount { get; }
		public List<List<CellData>> Status { get; }
		public int Timer { get; }
	}

	class ColorCrossData : ObservableObject
	{
		int _clickCount;
		int[,] _pixels;
		int _timer;
		List<List<CellData>> _status;
		List<Color> _colors;
		LineOfColors[] _rows;
		LineOfColors[] _columns;


		public int[,] Pixels { get { return this._pixels; } internal set { SetProperty(ref this._pixels, value); } }
		public int ClickCount { get { return this._clickCount; } internal set { SetProperty(ref this._clickCount, value); } }
		public LineOfColors[] Rows { get { return this._rows; } internal set { SetProperty(ref this._rows, value); } }
		public LineOfColors[] Columns { get { return this._columns; } internal set { SetProperty(ref this._columns, value); } }
		public List<Color> Colors { get { return this._colors; } internal set { SetProperty(ref this._colors, value); } }
		public List<List<CellData>> Status { get { return this._status; } internal set { SetProperty(ref this._status, value); } }
		public int Timer { get { return this._timer; } internal set { SetProperty(ref this._timer,value); } }


		public ColorCrossData()
		{
			_pixels = new int[0, 0];
			_clickCount = 0;
			_status = new List<List<CellData>>();
			_colors = new List<Color>();
			_rows = new LineOfColors[0];
			_columns = new LineOfColors[0];
			_timer = 0;
		}
	}

	class ColorCrossLogic : IColorCrossLogic
	{

		int numberOfColoredLinesAndColumns;
		int currentCorrectLines;
		Bitmap bmp;
		string fileName;

		ColorCrossData datas;
		public ColorCrossData Datas { get { return this.datas; } }

		public int[,] Pixels { get { return datas.Pixels; } private set { datas.Pixels = value; } }
		public int ClickCount { get { return datas.ClickCount; } private set { datas.ClickCount = value; } }
		public LineOfColors[] Rows { get { return datas.Rows; } private set { datas.Rows = value; } }
		public LineOfColors[] Columns { get { return datas.Columns; } private set { datas.Columns = value; } }
		public List<Color> Colors { get { return datas.Colors; } private set { datas.Colors = value; } }
		public List<List<CellData>> Status { get { return datas.Status; } private set { datas.Status = value; } }
		public int Timer { get { return datas.Timer; } private set { datas.Timer = value; } }

		public ColorCrossLogic()
		{
			this.datas = new ColorCrossData();

			numberOfColoredLinesAndColumns = 0;
			currentCorrectLines = 0;
			bmp = new Bitmap(1, 1);
			fileName = string.Empty;
		}

		public void ImageReader(string filePath)
		{
			bmp = new Bitmap(filePath);
			Rows = new LineOfColors[bmp.Height];
			Columns = new LineOfColors[bmp.Width];

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
			if (data == null) return;
			ClickCount = data.ClickCount;
			Status = data.Status;
			Timer = data.Timer;
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
				Status.Add(rd);
			}
		}

		void CountUniqueColors()
		{
			Pixels = new int[bmp.Height, bmp.Width];
			for (int i = 0; i < bmp.Height; i++)
			{
				for (int j = 0; j < bmp.Width; j++)
				{
					var color = bmp.GetPixel(j, i);
					var newColor = Color.FromArgb(color.A, color.R, color.G, color.B);
					if (!Colors.Contains(newColor) && color.Name != "0")
						Colors.Add(newColor);
					Pixels[i, j] = Colors.IndexOf(newColor);
				}
			}
		}

		void CountRowColors()
		{
			for (int i = 0; i < Rows.Length; i++)
			{
				if (Rows[i] == null) Rows[i] = new LineOfColors();
				for (int j = 0; j < bmp.Width; j++)
				{
					int k = j + 1;
					int sum = 0;
					var color = Pixels[i, j];
					while (k < bmp.Width && Pixels[i, k] == color)
					{
						sum++;
						k++;
					}
					if (color != -1)
					{
						if (Rows[i].Colors == null)
							Rows[i].Colors = new List<LineOfColors.ColorNumber>();
						Rows[i].Colors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
					}
					j = k - 1;
				}
				Rows[i].IsDone = false;
				numberOfColoredLinesAndColumns++;
			}
		}

		void CountColumnColors()
		{
			for (int i = 0; i < Columns.Length; i++)
			{
				if (Columns[i] == null) Columns[i] = new LineOfColors();
				for (int j = 0; j < bmp.Height; j++)
				{
					int k = j + 1;
					int sum = 0;
					var color = Pixels[j, i];
					while (k < bmp.Height && Pixels[k, i] == color)
					{
						sum++;
						k++;
					}
					if (color != -1)
					{
						if (Columns[i].Colors == null)
							Columns[i].Colors = new List<LineOfColors.ColorNumber>();
						Columns[i].Colors.Add(new LineOfColors.ColorNumber { Color = color, Count = sum + 1 });
					}
					j = k - 1;
				}
				Columns[i].IsDone = false;
				numberOfColoredLinesAndColumns++;
			}
		}

		void CheckForCorrectLines()
		{
			for (int i = 0; i < Pixels.GetLength(0); i++)
				ColorCheck(i, 0);

			for (int i = 0; i < Pixels.GetLength(1); i++)
			{
				ColorCheck(0, i);
			}
		}

		public bool Click(int x, int y, int color)
		{
			Status[x][y].Color = color;
			ColorCheck(x, y);
			ClickCount++;
			int timer_for_save = Timer;
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
			for (int j = 0; j < Pixels.GetLength(1); j++)
			{
				int k = j + 1;
				int sum = 0;
				var color = Status[x][j].Color;
				while (k < Pixels.GetLength(1) && Status[x][k].Color == color)
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
			return rowColors.SequenceEqual(Rows[x].Colors);
		}

		bool ColumnCheck(int y)
		{
			var columnColors = new List<LineOfColors.ColorNumber>();
			for (int i = 0; i < Pixels.GetLength(0); i++)
			{
				int k = i + 1;
				int sum = 0;
				var color = Status[i][y].Color;
				while (k < Pixels.GetLength(0) && Status[k][y].Color == color)
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
			return columnColors.SequenceEqual(Columns[y].Colors);
		}

		public bool CheckIfImageIsDone()
		{
			return numberOfColoredLinesAndColumns == currentCorrectLines;
		}

		public void GameEnd()
		{
			int timer_for_save = Timer;
			var json = JsonSerializer.Serialize(new AllData(ClickCount, Status, Timer), typeof(AllData));
			File.WriteAllText($"{fileName}.json", json);
		}
	}
}
