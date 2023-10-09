using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public string WeatherUpdateMinutes
        {
            get { return _weatherUpdateMinutes; }
            set { SetProperty(ref _weatherUpdateMinutes, value); }
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


    }
}
