using ColorCross.Logic;
using ColorCross.UI;
using ColorCross.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ColorCross
{
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{

		public static IEnumerable<T> FindVisualChilds<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj == null) yield return (T)Enumerable.Empty<T>();
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
			{
				DependencyObject ithChild = VisualTreeHelper.GetChild(depObj, i);
				if (ithChild == null) continue;
				if (ithChild is T t) yield return t;
				foreach (T childOfChild in FindVisualChilds<T>(ithChild)) yield return childOfChild;
			}
		}

		IColorCrossLogic logic = new ColorCrossLogic();
		GameWindowViewModel VM;
		TimeSpan ts;

		public GameWindow(string path)
		{
			logic.ImageReader(path);
			VM = new GameWindowViewModel(logic);
			InitializeComponent();

			
			IntToColorConverter converter = (IntToColorConverter)FindResource("IntToColorConverter");
			converter.Colors = logic.Colors;
			List<CellData> colors = new List<CellData>();
			colors.Add(new CellData() { X = -1, Y = 0, Color = -1 });
			for (int i = 0; i < logic.Colors.Count; i++)
			{
				colors.Add(new CellData() { X = i, Y = 0, Color = i });
			}
			this.DataContext = this.VM;
			lst.ItemsSource = this.VM.Statuses;
			lst2.ItemsSource = colors;
			lstcols.ItemsSource = this.VM.Columns;
			lstrows.ItemsSource = this.VM.Rows;




		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button)sender;
			CellData o = (CellData)b.DataContext;
			VM.Click(o.X, o.Y);
		}

		private void ColorButton_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button)sender;
			CellData o = (CellData)b.DataContext;
			VM.SelectedColor = o.Color;
		}

		private void Window_Closed(object sender, EventArgs e)
		{
			logic.GameEnd();
		}

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {


        }
    }


}
