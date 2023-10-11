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
using SharedLibrary.MVVM.Models.Dto;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class ConfigurationViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly WeatherService _weatherService;
        private readonly SmartAppDbService _dbService;

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


        public ConfigurationViewModel(IServiceProvider serviceProvider, WeatherService weatherService, SmartAppDbService dbService)
        {
            _serviceProvider = serviceProvider;
            _weatherService = weatherService;
            _dbService = dbService;

            WeatherUpdateMinutes = weatherService.WeatherUpdateMinutes.ToString();
            IotHubConnectionString = _dbService.IotHubConnectionString!;
            EventHubEndpoint = _dbService.EventHubEndpoint!;
            EventHubName = _dbService.EventHubName!;
            ConsumerGroup = _dbService.ConsumerGroup!;
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
        private void UpdateConnectionStrings()
        {
            var connectionStringsDto = new ConnectionStringsDto
            {
                IotHubConnectionString = IotHubConnectionString,
                EventHubEndpoint = EventHubEndpoint,
                EventHubName = EventHubName,
                ConsumerGroup = ConsumerGroup,
            };

            _dbService.CreateNewConnectionStrings(connectionStringsDto);



            //string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //System.Diagnostics.Process.Start(appPath);
            //Application.Current.Shutdown();



        }

        [RelayCommand]
        private void RestoreConnectionStrings()
        {
            _dbService.ResetConnectionStrings();

            //string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //System.Diagnostics.Process.Start(appPath);
            //Application.Current.Shutdown();
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
