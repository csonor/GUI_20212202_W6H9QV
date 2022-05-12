using ColorCross.UserControls;
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
			closebutton.Visibility = Visibility.Collapsed;
		}

		private void exit_Click(object sender, RoutedEventArgs e)
		{
			Resizer.Exit(this);
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

		private void LevelSelector_Click(object sender, RoutedEventArgs e)
		{
			menu.Visibility = Visibility.Visible;
			menu.Content = new LevelSelectUserControl();
			closebutton.Visibility = Visibility.Visible;
			stackpanel.Visibility = Visibility.Collapsed;
		}
		private void Helper_click(object sender, RoutedEventArgs e)
		{

			menu.Visibility = Visibility.Visible;
			menu.Content = new HelpUserControl();
			closebutton.Visibility = Visibility.Visible;
			stackpanel.Visibility = Visibility.Collapsed;


		}
		private void Upload_Click(object sender, RoutedEventArgs e)
		{
			menu.Visibility = Visibility.Visible;
			menu.Content = new CustomImageUserControl();
			closebutton.Visibility = Visibility.Visible;
			stackpanel.Visibility = Visibility.Collapsed;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			menu.Visibility = Visibility.Collapsed;
			closebutton.Visibility = Visibility.Collapsed;
			stackpanel.Visibility = Visibility.Visible;
		}
	}
}
