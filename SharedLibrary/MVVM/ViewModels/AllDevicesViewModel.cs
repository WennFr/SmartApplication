using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace SharedLibrary.MVVM.ViewModels
{
    public partial class AllDevicesViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IotHubManager _iotHub;


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


        public AllDevicesViewModel(IServiceProvider serviceProvider, IotHubManager iotHub)
        {
            _serviceProvider = serviceProvider;
            _iotHub = iotHub;

            UpdateDeviceList();
            _iotHub.DevicesUpdated += UpdateDeviceList;
        }

        [ObservableProperty]
        private ObservableCollection<DeviceItemViewModel>? _devices;

        [RelayCommand]
        private void NavigateToSettings()
        {
            var mainWindowViewModel = _serviceProvider.GetRequiredService<MainWindowViewModel>();
            mainWindowViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
        }

        [RelayCommand]
        private async Task RemoveDevice(object parameter)
        {
            try
            {
                var message = "";

                if (parameter is DeviceItemViewModel device)
                {
                    var isRemoved = await _iotHub.RemoveDevice(device.DeviceId);

                    if (isRemoved)
                    {
                        message = $"{device.DeviceId} was removed!";
                        notifier.ShowSuccess(message);
                        NavigateToSettings();
                    }

                    else
                    {
                        message = $"Could not remove device.";
                        notifier.ShowError(message);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void UpdateDeviceList()
        {
            Devices = new ObservableCollection<DeviceItemViewModel>(_iotHub.CurrentDevices
                .Select(device => new DeviceItemViewModel(device)).ToList());
        }

    }
}
