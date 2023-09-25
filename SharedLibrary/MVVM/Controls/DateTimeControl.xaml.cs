using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace SharedLibrary.MVVM.Controls
{

    public partial class DateTimeControl : UserControl
    {
        
        public DateTimeControl()
        {
            InitializeComponent();
        }



        public static readonly DependencyProperty CurrentTimeProperty =
            DependencyProperty.Register(
                "CurrentTime",
                typeof(string),
                typeof(DateTimeControl),
                new PropertyMetadata(string.Empty));

        public string CurrentTime
        {
            get { return (string)GetValue(CurrentTimeProperty); }
            set { SetValue(CurrentTimeProperty, value); }
        }

        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register(
                "CurrentDate",
                typeof(string),
                typeof(DateTimeControl),
                new PropertyMetadata(string.Empty));

        public string CurrentDate
        {
            get { return (string)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }
    }


}



