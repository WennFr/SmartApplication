using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
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
using JsonConverter = Newtonsoft.Json.JsonConverter;
using Microsoft.Azure.Amqp.Framing;
using SharedLibrary.MVVM.Models.Weather;

namespace SharedLibrary.MVVM.Controls
{

    public partial class WeatherControl : UserControl
    {
        public WeatherControl()
        {
            InitializeComponent();
        }


        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register(
                "Condition",
                typeof(string),
                typeof(WeatherControl),
                new PropertyMetadata(string.Empty));

        public string Condition
        {
            get { return (string)GetValue(ConditionProperty); }
            set { SetValue(ConditionProperty, value); }
        }

        public static readonly DependencyProperty TemperatureProperty =
            DependencyProperty.Register(
                "Temperature",
                typeof(string),
                typeof(WeatherControl),
                new PropertyMetadata(string.Empty));

        public string Temperature
        {
            get { return (string)GetValue(TemperatureProperty); }
            set { SetValue(TemperatureProperty, value); }
        }

        public static readonly DependencyProperty HumidityProperty =
            DependencyProperty.Register(
                "Humidity",
                typeof(string),
                typeof(WeatherControl),
                new PropertyMetadata(string.Empty));

        public string Humidity
        {
            get { return (string)GetValue(HumidityProperty); }
            set { SetValue(HumidityProperty, value); }
        }






    }
}
