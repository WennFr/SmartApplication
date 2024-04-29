using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Models
{
    public class PrinterDataMessage
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public bool DeviceOn { get; set; }
        public bool HasPaper { get; set; }
        public string Location { get; set; } 
        public DateTime CurrentTime { get; set; }
    }
}
