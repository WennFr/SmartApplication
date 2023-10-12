using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

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


        Notifier notifier = new Notifier(cfg =>
        {
            cfg.PositionProvider = new WindowPositionProvider(
                parentWindow: Application.Current.MainWindow,
                corner: Corner.TopRight,
                offsetX: 10,
                offsetY: 10);

            cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                notificationLifetime: TimeSpan.FromSeconds(3),
                maximumNotificationCount: MaximumNotificationCount.FromCount(5));

            cfg.Dispatcher = Application.Current.Dispatcher;
        });


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

            var isRegistered = await _iotHub.RegisterDeviceAsync(deviceId, deviceType, location);
            var message = "";

            if (isRegistered)
            {
                message = $"{deviceId} added!";
                notifier.ShowSuccess(message);
                NavigateToSettings();
            }
            else
            {
                message = $"Something went wrong.";
                notifier.ShowError(message);
            }
        }

    }
}
