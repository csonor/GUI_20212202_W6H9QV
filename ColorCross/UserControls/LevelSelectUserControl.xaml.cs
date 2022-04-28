using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
using System.Windows.Navigation;


namespace ColorCross.UserControls
{
	/// <summary>
	/// Interaction logic for GameMenuUserControl.xaml
	/// </summary>
	public partial class LevelSelectUserControl : UserControl
	{
		public LevelSelectUserControl()
		{
			InitializeComponent();
			var path = Directory.GetFiles(Path.Combine("Images"), "*bmp");
			for (int i = 0; i < path.Length; i++)
			{
				int j = i;
				Bitmap bmp = new Bitmap(path[i]);
				wrp.Children.Add(new Button
				{
					Content = $"{bmp.Width} x {bmp.Height}",
					Margin = new Thickness(50),
					Padding = new Thickness(50),
					Command = new RelayCommand(() => OpenGame(path[j]))

				});
			}
		}

		private void OpenGame(string path)
		{
			GameWindow gw = new GameWindow(path);
			gw.ShowDialog();
		}
	}
}
