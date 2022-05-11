using ColorCross.UserControls;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public RelayCommand OpenSettingsCommand { get; set; }
        public SettingsUserControl SettingsView { get; set; }

        public RelayCommand OpenScoreBoardCommand { get; set; }
        public ScoreBoardUserControl ScoreBoardView { get; set; }


        public RelayCommand OpenHelpCommand { get; set; }
        public HelpUserControl HelpView { get; set; }

        public RelayCommand OpenGameMenuCommand { get; set; }
        public LevelSelectUserControl GameMenuView { get; set; }


        public MainWindowViewModel()
        {

            GameMenuView = new LevelSelectUserControl();
            SettingsView = new SettingsUserControl();
            HelpView = new HelpUserControl();
            ScoreBoardView = new ScoreBoardUserControl();
            currentView = GameMenuView;

            OpenSettingsCommand = new RelayCommand(() =>
            {
                CurrentView = SettingsView;
            });
            OpenHelpCommand = new RelayCommand(() =>
            {
                CurrentView = HelpView;
            });
            OpenGameMenuCommand = new RelayCommand(() =>
            {
                CurrentView = GameMenuView;
            });
            OpenScoreBoardCommand = new RelayCommand(() =>
            {
                CurrentView = ScoreBoardView;
            });

        }
    }
}
