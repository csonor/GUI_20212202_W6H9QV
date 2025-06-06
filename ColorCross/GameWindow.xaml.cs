using ColorCross.Logic;
using ColorCross.UI;
using ColorCross.ViewModel;
using ColorCross.WindowFunctions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace ColorCross
{
	/// <summary>
	/// Interaction logic for GameWindow.xaml
	/// </summary>
	public partial class GameWindow : Window
	{
		IColorCrossLogic logic = new ColorCrossLogic();
		GameWindowViewModel VM;
		private DispatcherTimer timer;
		public GameWindow(string path)
		{
			logic.ImageReader(path);
			VM = new GameWindowViewModel(logic);
			InitializeComponent();
			IntToColorConverter converter = (IntToColorConverter)FindResource("IntToColorConverter");
			converter.Colors = logic.Datas.Colors;
			IntToColorTextConverter converter2 = (IntToColorTextConverter)FindResource("IntToColorTextConverter");
			converter2.Colors = logic.Datas.Colors;
			List<CellData> colors = new List<CellData>();
			colors.Add(new CellData() { X = -1, Y = 0, Color = -1 });
			for (int i = 0; i < logic.Datas.Colors.Count; i++)
			{
				colors.Add(new CellData() { X = i, Y = 0, Color = i });
			}
			this.DataContext = this.VM;
			lst.ItemsSource = this.VM.Datas.Status;
			lst2.ItemsSource = colors;
			lstcols.ItemsSource = this.VM.Datas.Columns;
			lstrows.ItemsSource = this.VM.Datas.Rows;
			this.timer = new DispatcherTimer();

			timer.Interval = TimeSpan.FromSeconds(1);
			timer.Tick += TimerTickEvent;
			timer.Start();
		}

		private void TimerTickEvent(object sender, EventArgs e)
		{
			this.VM.Datas.Timer++;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Button b = (Button)sender;
			CellData o = (CellData)b.DataContext;
			if (VM.Click(o.X, o.Y))
			{
				MessageBox.Show("Kiraktad a képet!", "Kész a pálya", MessageBoxButton.OK, MessageBoxImage.Information);
				Close();
			}
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

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			DialogResult = logic.CheckIfImageIsDone();
			this.timer.Stop();
		}

		private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
		{
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
	}
}
