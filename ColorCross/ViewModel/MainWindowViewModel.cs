using ColorCross.UserControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace ColorCross.ViewModel
{
	public class MainWindowViewModel : ObservableRecipient
	{


		private object currentView;

		public object CurrentView
		{
			get { return currentView; }
			set
			{
				currentView = value;
				OnPropertyChanged();
			}
		}


		public RelayCommand OpenHelpCommand { get; set; }
		public HelpUserControl HelpView { get; set; }

		public RelayCommand OpenGameMenuCommand { get; set; }
		public LevelSelectUserControl GameMenuView { get; set; }


		public MainWindowViewModel()
		{

			GameMenuView = new LevelSelectUserControl();
		
			HelpView = new HelpUserControl();

			currentView = GameMenuView;

		;
			OpenHelpCommand = new RelayCommand(() =>
			{
				CurrentView = HelpView;
			});
			OpenGameMenuCommand = new RelayCommand(() =>
			{
				CurrentView = GameMenuView;
			});
		

		}
	}
}
