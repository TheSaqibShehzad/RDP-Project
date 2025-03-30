using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using rdp.Models;

namespace rdp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set Dashboard as the default page
            var defaultPage = sidebar.Items.Cast<NavButton>().FirstOrDefault(btn => btn.Navlink.Equals("/Views/Dashboard.xaml"));

            // Check if the Dashboard button exists and set it as the selected item
            if (defaultPage != null)
            {
                sidebar.SelectedItem = defaultPage;
                var selected = sidebar.SelectedItem as NavButton;
                navframe.Navigate(selected.Navlink);
            }
        }

        private void sidebar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selected = sidebar.SelectedItem as NavButton;

            navframe.Navigate(selected.Navlink);

        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.MouseDevice.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
