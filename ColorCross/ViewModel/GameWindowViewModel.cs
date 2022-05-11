using ColorCross.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ColorCross.ViewModel
{
	class GameWindowViewModel : ObservableRecipient
	{
		IColorCrossLogic logic;

		public int ButtonSize { get; set; }
		public List<LineOfColors> Rows { get; private set; }
		public List<LineOfColors> Columns { get; private set; }
		public int ClickCount { get; private set; }

		public List<List<CellData>> Statuses
		{
			get; private set;
		}

		int selectedColor;
		public int SelectedColor
		{
			get { return selectedColor; }
			set { SetProperty(ref selectedColor, value); }
		}

		public bool Click(int x, int y)
		{
			return this.logic.Click(x, y, selectedColor);
		}

		public GameWindowViewModel(IColorCrossLogic logic)
		{
			this.logic = logic;
			this.Statuses = logic.Status;
			this.selectedColor = 0;
			this.Rows = new List<LineOfColors>(logic.Rows);
			this.Columns = new List<LineOfColors>(logic.Columns);
			this.ButtonSize = 10;
		}

		public GameWindowViewModel()
		{
			this.selectedColor = 0;
		}
	}
}