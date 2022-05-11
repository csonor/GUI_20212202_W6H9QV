using ColorCross.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ColorCross.ViewModel
{
	class GameWindowViewModel : ObservableRecipient
	{
		IColorCrossLogic logic;

		public int ButtonSize { get; set; }

		public ColorCrossData Datas { get; private set; }


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
			this.Datas = logic.Datas;
			this.ButtonSize = 15;
			this.selectedColor = -1;

		}

		public GameWindowViewModel()
		{
			this.selectedColor = -1;
			this.Datas = new ColorCrossData();
		}
	}
}