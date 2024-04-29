using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    public class FanDataMessage
    {

        public string id { get; set; } = Guid.NewGuid().ToString();

        public bool DeviceOn { get; set; }

        public string Speed { get; set; }

        public string Location { get; set; }

        public DateTime CurrentTime { get; set; }
    }
}
