using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using CommunityToolkit.Mvvm.Input;
using SharedLibrary.MVVM.ViewModels;
using SharedLibrary.Services;

namespace SharedLibrary.MVVM.ViewModels
{
   public partial class SettingsViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly DateTimeService _dateTimeService;
        private readonly IotHubManager _iotHub;


        public SettingsViewModel(IServiceProvider serviceProvider, DateTimeService dateTimeService, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _dateTimeService = dateTimeService;
            _iotHub = iotHub;
        }



        [RelayCommand]
        private void NavigateToHome()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
        }

        [RelayCommand]
        private void ShowAddDevice()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AddDeviceViewModel>();
        }

        [RelayCommand]
        private void ShowAllDevices()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<AllDevicesViewModel>();
        }

        [RelayCommand]
        private void ShowConfiguration()
        {

        }

        [RelayCommand]
        private void ExitApplication()
        {
            Environment.Exit(0);
        }



    }
}
