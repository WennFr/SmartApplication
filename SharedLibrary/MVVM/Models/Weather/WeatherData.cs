using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Models.Weather
{
    public class WeatherData
    {
        public CurrentWeather Current_Weather { get; set; }

        public HourlyWeather Hourly { get; set; }

    }
}
