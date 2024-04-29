using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Models.LampTelemetryDataModel
{
    public class LampTelemetryDataModel
    {
        public string ContainerName { get; set; } = "lamp_data";

        public bool IsLampOn { get; set; }

        public double TemperatureCelsius { get; set; }

        public string Location { get; set; }

        public DateTime CurrentTime { get; set; }

    }
}
