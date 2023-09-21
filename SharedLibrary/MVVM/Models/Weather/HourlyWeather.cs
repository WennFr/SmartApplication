using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Models.Weather
{
    public class HourlyWeather
    {
        public List<string> Time { get; set; }

        public List<int> RelativeHumidity_2m { get; set; }

    }
}
