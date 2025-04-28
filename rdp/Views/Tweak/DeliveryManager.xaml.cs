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

namespace rdp.Views.Tweak
{
    /// <summary>
    /// Interaction logic for UCMitigations.xaml
    /// </summary>
    public partial class DeliveryManager : UserControl
    {
        public DeliveryManager()
        {
            InitializeComponent();
        }
        public void Toggle_Checked(object sender, RoutedEventArgs e)
        {
            Tweaks.ApiTest("67f3c677faa3d510823951f9", true, false);
        }
        public void Toggle_UnChecked(object sender, RoutedEventArgs e)
        {
            Tweaks.ApiTest("680e1d02048cca3a9d587880", true, false);
        }
    }
}
