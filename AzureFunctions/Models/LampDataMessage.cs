using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace AzureFunctions.Models
{
    public class LampDataMessage
    {

        public string id { get; set; } = Guid.NewGuid().ToString();
        public bool DeviceOn { get; set; }
        public double TemperatureCelsius { get; set; }
        public string Location { get; set; }
        public DateTime CurrentTime { get; set; }



    }
}
