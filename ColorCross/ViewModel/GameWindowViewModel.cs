using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ColorCross.Logic;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace ColorCross.ViewModel
{
	class GameWindowViewModel : ObservableRecipient
	{
		IColorCrossLogic logic;

		//List<List<ColorData>> statuses;

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

		public void Click(int x, int y)
		{
			this.logic.Click(x, y, selectedColor);
		}
		public GameWindowViewModel(IColorCrossLogic logic)
		{
			this.logic = logic;
			this.Statuses = logic.Status;
			this.selectedColor = 0;
			this.Rows = new List<LineOfColors>(logic.Rows);
			this.Columns = new List<LineOfColors>(logic.Columns);


		}

		public GameWindowViewModel()
		{
			this.selectedColor = 0;
		}
	}
}