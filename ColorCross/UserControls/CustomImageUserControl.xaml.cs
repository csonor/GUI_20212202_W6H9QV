using Microsoft.Win32;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;

namespace ColorCross.UserControls
{
	/// <summary>
	/// Interaction logic for CustomImageUserControl.xaml
	/// </summary>
	public partial class CustomImageUserControl : UserControl
	{
		public CustomImageUserControl()
		{
			InitializeComponent();
		}
		private void btnOpenFile_Click(object sender, RoutedEventArgs e)
		{
			OpenFileDialog dialog = new OpenFileDialog();
			dialog.Title = "Open Image";
			dialog.Filter = "bmp files (*.bmp)|*.bmp";
			if (dialog.ShowDialog() == true)
			{
				string path = dialog.FileName;
				Bitmap bmp = new Bitmap(path);
				if (bmp.Width > 50 || bmp.Height > 50)
					MessageBox.Show("A kép mérete túl nagy!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
				else new GameWindow(path).ShowDialog();
			}
		}
	}
}
