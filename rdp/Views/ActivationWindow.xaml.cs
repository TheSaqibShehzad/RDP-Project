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
using System.Windows.Shapes;

namespace rdp.Views
{
    /// <summary>
    /// Interaction logic for ActivationWindow.xaml
    /// </summary>
    public partial class ActivationWindow : Window
    {
        public ActivationWindow()
        {
            InitializeComponent();
        }
        private void ActivateButton_Click(object sender, RoutedEventArgs e)
        {
            // Open the next window
            var nextWindow = new MainWindow(); // Replace 'NextWindow' with your actual window class
            nextWindow.Show();

            // Close the current window
            this.Close();
        }
    }
}
