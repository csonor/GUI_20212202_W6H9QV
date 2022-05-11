namespace ColorCross.Logic
{
	internal interface IColorCrossLogic
	{
		ColorCrossData Datas { get; }

		bool CheckIfImageIsDone();
		bool Click(int x, int y, int color);
		void GameEnd();
		void ImageReader(string filePath);
	}
}