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


    }
}
