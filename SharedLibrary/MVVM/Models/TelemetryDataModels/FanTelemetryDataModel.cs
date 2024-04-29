using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Models.FanTelemetryDataModel
{
    public class FanTelemetryDataModel
    {
        public string ContainerName { get; set; } = "fan_data";

        public bool IsFanOn { get; set; }

        public string Speed { get; set; }

        public string Location { get; set; }

        public DateTime CurrentTime { get; set; }
    }
}
