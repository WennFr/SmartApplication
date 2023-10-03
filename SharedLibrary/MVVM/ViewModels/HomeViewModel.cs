using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Models;
using SharedLibrary.Services;
using System.Reflection.Metadata;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateTimeService _dateTimeService;
        private readonly WeatherService _weatherService;
        private readonly IotHubManager _iotHub;


        public HomeViewModel(IServiceProvider serviceProvider, DateTimeService dateTimeService, WeatherService weatherService, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _dateTimeService = dateTimeService;
            _weatherService = weatherService;
            _iotHub = iotHub;

            UpdateDateAndTime();
            UpdateWeather();

            UpdateDeviceList();
            _iotHub.DevicesUpdated += UpdateDeviceList;
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

        [ObservableProperty]
        private ObservableCollection<DeviceItemViewModel>? _devices;

        [RelayCommand]
        private void NavigateToSettings(object parameter)
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }


        [RelayCommand]
        private async Task ExecuteStartStopButton(object parameter)
        {
            try
            {
                if (parameter is DeviceItemViewModel device)
                {
                    if (device.IsActive)
                    {
                        var deviceId = device.DeviceId;

                        // Stop the device
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            await _iotHub.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "stop"
                            });
                        }
                    }
                    else
                    {
                        // Start the device
                        var deviceId = device.DeviceId;
                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            await _iotHub.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "start"
                            });

                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

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



        private void UpdateDeviceList()
        {
            Devices = new ObservableCollection<DeviceItemViewModel>(_iotHub.CurrentDevices
                .Select(device => new DeviceItemViewModel(device)).ToList());
        }



        //private async Task UpdateDevices()
        //{
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            _iotHub.DevicesUpdated += () =>
        //            {
        //                Application.Current.Dispatcher.Invoke(() =>
        //                {
        //                    Devices = ConvertToDeviceItemViewModels(_iotHub.CurrentDevices);
        //                });
        //            };

        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //    }
        //}

        //private ObservableCollection<DeviceItemViewModel> ConvertToDeviceItemViewModels(List<DeviceItem> devices)
        //{
        //    var deviceViewModels = new ObservableCollection<DeviceItemViewModel>();

        //    foreach (var deviceItem in devices)
        //    {
        //        var deviceViewModel = new DeviceItemViewModel(deviceItem);
        //        deviceViewModels.Add(deviceViewModel);
        //    }

        //    return deviceViewModels;
        //}

    }
}
