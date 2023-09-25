using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Models;
using SharedLibrary.Services;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateTimeService _dateTimeService;
        private readonly WeatherService _weatherService;
        private readonly IotHubManager _iotHub;


        //public ObservableCollection<DeviceItem> Devices { get; set; } = new ObservableCollection<DeviceItem>();

        public HomeViewModel(IServiceProvider serviceProvider, DateTimeService dateTimeService, WeatherService weatherService ,IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _dateTimeService = dateTimeService;
            _weatherService = weatherService;
            _iotHub = iotHub;
            //_devices = new ObservableCollection<DeviceItem>();

            UpdateDateAndTime();
            UpdateWeather();
        }

        [ObservableProperty]
        private string? _title = "Home";

        [ObservableProperty]
        private string? _currentTime = "--:--";

        [ObservableProperty]
        private string? _currentDate;

        [ObservableProperty]
        private string? _currentWeatherCondition = "\ue137";

        [ObservableProperty]
        private string? _currentTemperature = "--";

        [ObservableProperty]
        private string? _currentHumidity = "--";

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }



        //private ObservableCollection<DeviceItem> _devices;
        //public ObservableCollection<DeviceItem> Devices
        //{
        //    get => _devices;
        //    set => SetValue(ref _devices, value);
        //}


        private void UpdateDateAndTime()
        {

            _dateTimeService.TimeUpdated += () =>
            {
                CurrentDate = _dateTimeService.CurrentDate;
                CurrentTime = _dateTimeService.CurrentTime;

            };
        }


        private void UpdateWeather()
        {

            _weatherService.WeatherUpdated += () =>
            {
                CurrentWeatherCondition = _weatherService.CurrentWeatherCondition;
                CurrentTemperature = _weatherService.CurrentTemperature;
                CurrentHumidity = _weatherService.CurrentHumidity;
            };
        }


        //private async Task GetDevicesAsync()
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            var twins = await _iotHub.GetDevicesAsTwinAsync();
        //            Devices.Clear();
        //            Devices = await _iotHub.GetDevicesAsDeviceItemAsync(twins);
        //            await Task.Delay(10000);

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }


        //}
    }
}
