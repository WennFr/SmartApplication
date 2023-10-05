using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class AddDeviceViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IotHubManager _iotHub;

        private string _deviceId;
        private string _deviceType;
        private string _location;

        public string DeviceId
        {
            get { return _deviceId; }
            set { SetProperty(ref _deviceId, value); }
        }

        public string DeviceType
        {
            get { return _deviceType; }
            set { SetProperty(ref _deviceType, value); }
        }

        public string Location
        {
            get { return _location; }
            set { SetProperty(ref _location, value); }
        }

        public AddDeviceViewModel(IServiceProvider serviceProvider, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _iotHub = iotHub;
        }

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }


        [RelayCommand]
        private async Task RegisterNewDevice()
        {
            var deviceId = DeviceId;
            var deviceType = DeviceType;
            var location = Location;

            var isRegistered = await _iotHub.RegisterDevice(deviceId, deviceType, location);

            if (isRegistered)
            {
                NavigateToSettings();
            }

        }

    }
}
