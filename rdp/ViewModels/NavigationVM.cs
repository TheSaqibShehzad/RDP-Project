using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using rdp.Utilities;

namespace rdp.ViewModels
{

    class NavigationVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand DashboardCommand { get; set; }
        public ICommand TweaksCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        private void Dashboard(object obj) => CurrentView = new DashboardVM();
        private void Tweaks(object obj) => CurrentView = new TweaksVM();
     


        public NavigationVM()
        {
            DashboardCommand = new RelayCommand(Dashboard);
            TweaksCommand = new RelayCommand(Tweaks);


            // Startup Page
            CurrentView = new DashboardVM();
        }
    }

}
