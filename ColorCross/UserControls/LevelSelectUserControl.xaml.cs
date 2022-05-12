using ColorCross.Logic;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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
			UserControl_Loaded(this, new RoutedEventArgs());
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{
			var path = Directory.GetFiles(Path.Combine("Images"), "*bmp");
			if (File.Exists("levels.json"))
				completedLevels = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("levels.json"));

			wrp.Children.Clear();
			for (int i = 0; i < path.Length; i++)
			{
				int j = i;
				Bitmap bmp = new Bitmap(path[i]);
				if (completedLevels.Contains(path[i]))
				{
					var brush = new ImageBrush();
					brush.ImageSource = new BitmapImage(new Uri(path[i], UriKind.Relative));
					string fileName = new string(path[i].Split('\\')[1].TakeWhile(x => x != '.').ToArray());

					wrp.Children.Add(new Button
					{
						Name = fileName,
						Margin = new Thickness(50),
						Padding = new Thickness(50),
						Background = brush,
						BorderThickness = new Thickness(3),
						ContextMenu = (ContextMenu)Resources["contextMenu"]
					});
				}
				else
					wrp.Children.Add(new Button
					{
						Content = $"{bmp.Width} x {bmp.Height}",
						Margin = new Thickness(50),
						Padding = new Thickness(50),
						BorderThickness = new Thickness(3),
						Command = new RelayCommand(() => OpenGame(path[j]))
					});
			}
		}

		private static Button FindClickedItem(object sender)
		{
			var mi = (MenuItem)sender;
			if (mi == null)
			{
				return null;
			}
			var cm = (ContextMenu)mi.CommandParameter;
			if (cm == null)
			{
				return null;
			}
			return (Button)cm.PlacementTarget;
		}

		private void ClickNumber_Click(object sender, RoutedEventArgs e)
		{
			var clickedItem = FindClickedItem(sender);
			if (clickedItem != null)
			{
				var data = JsonSerializer.Deserialize<AllData>(File.ReadAllText(clickedItem.Name + ".json"));
				int clicks = data.ClickCount;
				int timer = data.Timer;
				MessageBox.Show($"Kattintások száma: {clicks}\nEltelt idő: {timer}");
			}
		}

		private void Reset_Click(object sender, RoutedEventArgs e)
		{
			var clickedItem = FindClickedItem(sender);
			if (clickedItem != null)
			{
				File.Delete(clickedItem.Name + ".json");
				completedLevels.Remove(completedLevels.FirstOrDefault(x => x.Contains(clickedItem.Name)));
				var json = JsonSerializer.Serialize(completedLevels);
				File.WriteAllText("levels.json", json);
			}
			UserControl_Loaded(this, new RoutedEventArgs());
		}
	}
}
