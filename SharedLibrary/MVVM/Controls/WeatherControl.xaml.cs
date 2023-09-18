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
using SharedLibrary.MVVM.Models;
using JsonConverter = Newtonsoft.Json.JsonConverter;

namespace SharedLibrary.MVVM.Controls
{
   
    public partial class WeatherControl : UserControl, INotifyPropertyChanged
    {
        private string? _temperature;
        private string? _condition;

        public string? Temperature
        {
            get
            {
                return _temperature;
            }
            set
            {
                _temperature = value; OnPropertyChanged(nameof(Temperature));
            }
        }

        public string? Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _temperature = value; OnPropertyChanged(nameof(Condition));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WeatherControl()
        {
            InitializeComponent();
            DataContext = this;
            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMinutes(15)
            };
            timer.Tick += async (s, e) => await GetWeatherAsync();
            Loaded += async (s, e) => await GetWeatherAsync();
            timer.Start();
        }

        private async Task GetWeatherAsync()
        {
            using HttpClient http = new HttpClient();
            try
            {
                var data = JsonConvert.DeserializeObject<dynamic>(await http.GetStringAsync("api string"));
                Temperature = data!.main.temp.ToString();

                switch (data!.weather[0].description.ToString())
                {
                    
                }




            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to get weather data. Error: {ex.Message}");
                Temperature = "--";
            }
        }


        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
