using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SharedLibrary.MVVM.Models
{
    public class DeviceItem
    {

        public DeviceItem()
        {
            StartStopButtonCommand = new RelayCommand(ExecuteStartStopButtonCommand);
        }


        public string? DeviceId { get; set; }
        public string? DeviceType { get; set; }
        public string? Placement { get; set; }
        public bool IsActive { get; set; } = false;

        public string? Icon => SetIcon();
      
        private string SetIcon()
        {
            switch (DeviceType?.ToLower())
            {
                case "lamp":
                    return "\uf0eb";

                case "fan":
                    return "\ue004";

                default:
                    return "\uf2db";
            }

        }


        public ICommand StartStopButtonCommand { get; }

        private void ExecuteStartStopButtonCommand()
        {
            // Your logic for starting/stopping the device here
        }

    }
}
