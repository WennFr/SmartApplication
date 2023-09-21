using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Azure.Devices;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Core;
using SharedLibrary.MVVM.Models;
using SharedLibrary.Services;

namespace SharedLibrary.MVVM.ViewModels
{
    public class HomeViewModel : ObservableObject
    {
        private readonly IotHubManager _iotHub;
        private readonly NavigationStore _navigationStore;
        private readonly DateTimeService _dateTimeService;

        //public ObservableCollection<DeviceItem> Devices { get; set; } = new ObservableCollection<DeviceItem>();

        public HomeViewModel(NavigationStore navigationStore, DateTimeService dateTimeService, IotHubManager iotHub)
        {
            _navigationStore = navigationStore;
            _dateTimeService = dateTimeService;
            _iotHub = iotHub;
            _devices = new ObservableCollection<DeviceItem>();

            Task.FromResult(GetDevicesTwinAsync());
            Task.Run(GetDateTime);
        }

        // Navigation
        public ICommand NavigateToSettingsCommand =>
            new RelayCommand(() => _navigationStore.CurrentViewModel = new SettingsViewModel(_navigationStore, _dateTimeService, _iotHub));




        private string? _currentTime = "00:00";
        public string? CurrentTime
        {
            get => _currentTime; set => SetValue(ref _currentTime, value);
        }


        private string? _currentDate;
        public string? CurrentDate { get => _currentDate; set => SetValue(ref _currentDate, value); }


        private ObservableCollection<DeviceItem> _devices;
        public ObservableCollection<DeviceItem> Devices
        {
            get => _devices;
            set => SetValue(ref _devices, value);
        }


        private void GetDateTime()
        {
            while (true)
            {
                CurrentTime = _dateTimeService.CurrentTime;
                CurrentDate = _dateTimeService.CurrentDate;
                
            }
        }

        private async Task GetDevicesTwinAsync()
        {
            try
            {
                while (true)
                {
                    var twins = await _iotHub.GetDevicesAsTwinAsync();
                    Devices.Clear();

                    foreach (var twin in twins)
                    {

                        var isActive = false;
                        if (twin.Properties?.Reported.Contains("deviceOn") == true)
                            isActive = bool.TryParse(twin.Properties.Reported["deviceOn"].ToString(), out bool parsedValue) ? parsedValue : isActive;


                        var deviceType = "Unknown";
                        if (twin.Properties?.Reported.Contains("deviceType") == true)
                            deviceType = twin.Properties.Reported["deviceType"].ToString();

                        Devices.Add(new DeviceItem
                        {
                            DeviceId = twin.DeviceId,
                            DeviceType = deviceType,
                            IsActive = isActive
                        });
                    }


                    await Task.Delay(1000);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }
    }
}
