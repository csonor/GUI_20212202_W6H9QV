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
            this.selectedColor = 2;

        }

        public GameWindowViewModel()
        {
            this.selectedColor = 2;
        }
    }
}