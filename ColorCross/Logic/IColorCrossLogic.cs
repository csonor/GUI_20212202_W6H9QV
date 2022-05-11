using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace ColorCross.Logic
{
	internal interface IColorCrossLogic
	{
		List<Color> Colors { get; }
		LineOfColors[] Columns { get; }
		LineOfColors[] Rows { get; }
		List<List<CellData>> Status { get; }

		void Click(int x, int y, int color);
		//bool ColorCheck(int i, int j);
		void GameEnd(bool isCompleted);
		void ImageReader(string filePath);
		void ResetGame();
	}
}