using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ColorCross.UserControls
{
	/// <summary>
	/// Interaction logic for GameMenuUserControl.xaml
	/// </summary>
	public partial class LevelSelectUserControl : UserControl
	{
		List<string> completedLevels = new List<string>();

		public LevelSelectUserControl()
		{
			InitializeComponent();
		}

		private void OpenGame(string path)
		{
			GameWindow gw = new GameWindow(path);
			gw.ShowDialog();
			if (gw.DialogResult == true)
				completedLevels.Add(path);

			var json = JsonSerializer.Serialize(completedLevels);
			File.WriteAllText("levels.json", json);
			wrp.Children.Clear();
			UserControl_Loaded(this, new RoutedEventArgs());
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var path = Directory.GetFiles(Path.Combine("Images"), "*bmp");
			if (File.Exists("levels.json"))
				completedLevels = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("levels.json"));

			for (int i = 0; i < path.Length; i++)
			{
				int j = i;
				Bitmap bmp = new Bitmap(path[i]);
				if (!completedLevels.Contains(path[i]))
					wrp.Children.Add(new Button
					{
						Content = $"{bmp.Width} x {bmp.Height}",
						Margin = new Thickness(50),
						Padding = new Thickness(50),
						Command = new RelayCommand(() => OpenGame(path[j]))
					});
				else
				{
					var brush = new ImageBrush();
					brush.ImageSource = new BitmapImage(new Uri(path[i], UriKind.Relative));
					wrp.Children.Add(new Button
					{
						Margin = new Thickness(50),
						Padding = new Thickness(50),
						Background = brush
					});
				}
			}
		}
	}
}
