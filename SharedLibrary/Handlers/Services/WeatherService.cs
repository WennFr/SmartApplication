using Newtonsoft.Json;
using SharedLibrary.MVVM.Models.Weather;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;

namespace SharedLibrary.Handlers.Services
{
    public class WeatherService
    {
        private readonly Timer _timer;
        private readonly HttpClient _http;

        public string? CurrentWeatherCondition { get; private set; }
        public string? CurrentTemperature { get; private set; }
        public string? CurrentHumidity { get; private set; }
        public int WeatherUpdateMinutes { get; set; } = 15;

        public event Action? WeatherUpdated;


        public WeatherService(HttpClient http)
        {
            _http = http;
            Task.Run(SetCurrentWeatherAsync);
            _timer = new Timer(60000 * WeatherUpdateMinutes);
            _timer.Elapsed += async (s, e) => await SetCurrentWeatherAsync();
            _timer.Start();
        }


        private async Task SetCurrentWeatherAsync()
        {

            try
            {

                var (latitude, longitude) = (59.3294, 18.0687);

                var data = JsonConvert.DeserializeObject<WeatherData>(await _http.GetStringAsync(
                    $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&hourly=relativehumidity_2m&current_weather=true"));

                CurrentTemperature = data.Current_Weather.Temperature.ToString();
                CurrentHumidity = GetRelativeHumidity(data).ToString();
                CurrentWeatherCondition = GetWeatherConditionIcon(data.Current_Weather.WeatherCode);

            }
            catch (Exception e)
            {
                CurrentTemperature = "--";
                CurrentWeatherCondition = ("--");
            }


            WeatherUpdated?.Invoke();

        }

        public int GetRelativeHumidity(WeatherData weatherData)
        {
            var currentTime = DateTime.Now;

            int currentIndex = weatherData.Hourly.Time.IndexOf(currentTime.ToString("yyyy-MM-ddTHH:00"));

            if (currentIndex != -1)
            {
                var currentHumidity = weatherData.Hourly.RelativeHumidity_2m[currentIndex];

                return currentHumidity;
            }

            return 0;
        }

        private string GetWeatherConditionIcon(int value)
        {
            var condition = "";

            switch (value)
            {
                case 0:
                    condition = "\ue28f"; // Clear sky
                    break;
                case 1:
                case 2:
                case 3:
                    condition = "\uf6c4"; // Mainly clear, partly cloudy, and overcast
                    break;
                case 45:
                case 48:
                    condition = "\uf74e"; // Fog and depositing rime fog
                    break;
                case 51:
                case 53:
                case 55:
                    condition = "\uf738"; // Drizzle: Light, moderate, and dense intensity
                    break;
                case 56:
                case 57:
                    condition = "\uf738"; // Freezing Drizzle: Light and dense intensity
                    break;
                case 61:
                case 63:
                case 65:
                    condition = "\uf740"; // Rain: Slight, moderate, and heavy intensity
                    break;
                case 66:
                case 67:
                    condition = "\uf740"; // Freezing Rain: Light and heavy intensity
                    break;
                case 71:
                case 73:
                case 75:
                    condition = "\uf742"; // Snow fall: Slight, moderate, and heavy intensity
                    break;
                case 77:
                    condition = "\uf742"; // Snow grains
                    break;
                case 80:
                case 81:
                case 82:
                    condition = "\uf738"; // Rain showers: Slight, moderate, and violent
                    break;
                case 85:
                case 86:
                    condition = "\uf742"; // Snow showers slight and heavy
                    break;
                case 95:
                    condition = "\uf76c"; // Thunderstorm: Slight or moderate
                    break;
                case 96:
                case 99:
                    condition = "\uf76c"; // Thunderstorm with slight and heavy hail
                    break;
                default:
                    condition = "\ue137"; // Default icon
                    break;
            }

            return condition;

        }



    

    }
}
