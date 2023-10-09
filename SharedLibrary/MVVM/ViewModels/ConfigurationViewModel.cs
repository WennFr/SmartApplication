using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class ConfigurationViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WeatherService _weatherService;

        private string _weatherUpdateMinutes;
        private string _iotHubConnectionString;
        private string _eventHubEndPoint;
        private string _eventHubName;
        private string _consumerGroup;


        public string WeatherUpdateMinutes
        {
            get { return _weatherUpdateMinutes; }
            set { SetProperty(ref _weatherUpdateMinutes, value); }
        }

        public string IotHubConnectionString
        {
            get { return _iotHubConnectionString; }
            set { SetProperty(ref _iotHubConnectionString, value); }
        }

        public string EventHubEndpoint
        {
            get { return _eventHubEndPoint; }
            set { SetProperty(ref _eventHubEndPoint, value); }
        }

        public string EventHubName
        {
            get { return _eventHubName; }
            set { SetProperty(ref _eventHubName, value); }
        }

        public string ConsumerGroup
        {
            get { return _consumerGroup; }
            set { SetProperty(ref _consumerGroup, value); }
        }


        public ConfigurationViewModel(IServiceProvider serviceProvider, WeatherService weatherService)
        {
            _serviceProvider = serviceProvider;
            _weatherService = weatherService;

            WeatherUpdateMinutes = weatherService.WeatherUpdateMinutes.ToString();
        }

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }

        [RelayCommand]
        private void ChangeWeatherUpdateMinutes()
        {
            int newWeatherUpdateMinutes;
            if (int.TryParse(WeatherUpdateMinutes, out newWeatherUpdateMinutes))
            {
                _weatherService.WeatherUpdateMinutes = newWeatherUpdateMinutes;
                NavigateToSettings();
            }

        }

        [RelayCommand]
        private void OpenWeatherApiDocumentation()
        {
            string url = "https://open-meteo.com/en/docs";

            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
            }
        }

    }
}
