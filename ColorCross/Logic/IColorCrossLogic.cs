using System.Collections.Generic;
using System.Windows.Media;

namespace ColorCross.Logic
{
	internal interface IColorCrossLogic
	{
		int ClickCount { get; set; }
		List<Color> Colors { get; }
		LineOfColors[] Columns { get; }
		LineOfColors[] Rows { get; }
		List<List<CellData>> Status { get; }

		bool CheckIfImageIsDone();
		bool Click(int x, int y, int color);
		void GameEnd();
		void ImageReader(string filePath);
		void ResetGame();
	}
}