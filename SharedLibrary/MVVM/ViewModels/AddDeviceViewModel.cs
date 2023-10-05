using CommunityToolkit.Mvvm.ComponentModel;
using SharedLibrary.Handlers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.ViewModels
{
    partial class AddDeviceViewModel : ObservableObject
    {
        private readonly IotHubManager _iotHub;


        public AddDeviceViewModel(IotHubManager iotHub)
        {
            _iotHub = iotHub;
        }


    }
}
