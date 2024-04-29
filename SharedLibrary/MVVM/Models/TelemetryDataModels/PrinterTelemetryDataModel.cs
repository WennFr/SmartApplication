using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.Models.PrinterTelemetryDataModel
{
    public class PrinterTelemetryDataModel
    {
        public string ContainerName { get; set; } = "printer_data";

        public bool IsPrinterOn { get; set; }

        public bool HasPaper { get; set; }

        public string Location { get; set; }

        public DateTime CurrentTime { get; set; }

    }
}
