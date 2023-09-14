using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class MethodDataRequest
    {
        public string DeviceId { get; set; } = null!;
        public string MethodName { get; set; } = null!;
        public object? Payload { get; set; }
        public int ResponseTimeout { get; set; } = 30;

    }
}
