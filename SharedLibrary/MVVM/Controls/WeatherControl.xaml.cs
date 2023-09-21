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
using Microsoft.Azure.Amqp.Framing;

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
                _condition = value; OnPropertyChanged(nameof(Condition));
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

                var (latitude, longitude) = (59.3294, 18.0687);

                var weatherResponse = await http.GetAsync($"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true");

                if (weatherResponse.IsSuccessStatusCode)
                {
                    var json = await weatherResponse.Content.ReadAsStringAsync();
                    var weatherData = JsonConvert.DeserializeObject<WeatherData>(json);
                    Temperature = weatherData.Current_Weather.Temperature.ToString();

                    switch (weatherData.Current_Weather.WeatherCode)
                    {
                        case 0:
                            Condition = "\ue28f"; // Clear sky
                            break;
                        case 1:
                        case 2:
                        case 3:
                            Condition = "\uf6c4"; // Mainly clear, partly cloudy, and overcast
                            break;
                        case 45:
                        case 48:
                            Condition = "\uf74e"; // Fog and depositing rime fog
                            break;
                        case 51:
                        case 53:
                        case 55:
                            Condition = "\uf738"; // Drizzle: Light, moderate, and dense intensity
                            break;
                        case 56:
                        case 57:
                            Condition = "\uf738"; // Freezing Drizzle: Light and dense intensity
                            break;
                        case 61:
                        case 63:
                        case 65:
                            Condition = "\uf740"; // Rain: Slight, moderate, and heavy intensity
                            break;
                        case 66:
                        case 67:
                            Condition = "\uf740"; // Freezing Rain: Light and heavy intensity
                            break;
                        case 71:
                        case 73:
                        case 75:
                            Condition = "\uf742"; // Snow fall: Slight, moderate, and heavy intensity
                            break;
                        case 77:
                            Condition = "\uf742"; // Snow grains
                            break;
                        case 80:
                        case 81:
                        case 82:
                            Condition = "\uf738"; // Rain showers: Slight, moderate, and violent
                            break;
                        case 85:
                        case 86:
                            Condition = "\uf742"; // Snow showers slight and heavy
                            break;
                        case 95:
                            Condition = "\uf76c"; // Thunderstorm: Slight or moderate
                            break;
                        case 96:
                        case 99:
                            Condition = "\uf76c"; // Thunderstorm with slight and heavy hail
                            break;
                        default:
                            Condition = "\ue137"; // Default icon
                            break;
                    }

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
