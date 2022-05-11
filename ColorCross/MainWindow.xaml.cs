using ColorCross.WindowFunctions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ColorCross
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			Resizer.SetIntials(this);
		}

		private void exit_Click(object sender, RoutedEventArgs e)
		{
			Resizer.Exit();
		}

		private void max_Click(object sender, RoutedEventArgs e)
		{
			Button btn = (Button)sender;
			Resizer.DoMaximize(this, btn);
		}

		private void min_Click(object sender, RoutedEventArgs e)
		{
			Resizer.Minimize(this);
		}
		private void Border_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed)
				this.DragMove();
		}
	}
}
