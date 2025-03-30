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

namespace rdp.Views
{
    /// <summary>
    /// Interaction logic for SpaceUsageControl.xaml
    /// </summary>
    public partial class UCSpaceUsage : UserControl
    {
        public static readonly DependencyProperty UsedSpaceColorProperty =
    DependencyProperty.Register("UsedSpaceColor", typeof(Brush), typeof(UCSpaceUsage), new PropertyMetadata(Brushes.Purple));

        public static readonly DependencyProperty UsedSpaceTextProperty =
            DependencyProperty.Register("UsedSpaceText", typeof(string), typeof(UCSpaceUsage), new PropertyMetadata("Used space"));

        public static readonly DependencyProperty FreeSpaceColorProperty =
            DependencyProperty.Register("FreeSpaceColor", typeof(Brush), typeof(UCSpaceUsage), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty FreeSpaceTextProperty =
            DependencyProperty.Register("FreeSpaceText", typeof(string), typeof(UCSpaceUsage), new PropertyMetadata("Free space"));
        public UCSpaceUsage()
        {
            InitializeComponent();
        }
        public Brush UsedSpaceColor
        {
            get { return (Brush)GetValue(UsedSpaceColorProperty); }
            set { SetValue(UsedSpaceColorProperty, value); }
        }

        public string UsedSpaceText
        {
            get { return (string)GetValue(UsedSpaceTextProperty); }
            set { SetValue(UsedSpaceTextProperty, value); }
        }

        public Brush FreeSpaceColor
        {
            get { return (Brush)GetValue(FreeSpaceColorProperty); }
            set { SetValue(FreeSpaceColorProperty, value); }
        }

        public string FreeSpaceText
        {
            get { return (string)GetValue(FreeSpaceTextProperty); }
            set { SetValue(FreeSpaceTextProperty, value); }
        }
    }
}
