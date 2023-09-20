using Microsoft.Azure.Devices.Shared;
using SharedLibrary.Handlers.Services;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharedLibrary.MVVM.Models;
using Microsoft.Azure.Devices;

namespace Control_Panel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IotHubManager _iotHub;
        public ObservableCollection<Twin> DeviceTwinList { get; set; } = new ObservableCollection<Twin>();

        public ObservableCollection<DeviceItem> Devices { get; set; } = new ObservableCollection<DeviceItem>();



        public MainWindow(IotHubManager iotHub)
        {
            InitializeComponent();
            _iotHub = iotHub;
            this.DataContext = this;
            DeviceList.ItemsSource = Devices;
            Task.FromResult(GetDevicesTwinAsync());

        }

        private async Task GetDevicesTwinAsync()
        {
            try
            {
                while (true)
                {
                    var twins = await _iotHub.GetDevicesAsTwinAsync();
                    DeviceTwinList.Clear();
                    Devices.Clear();

                    foreach (var twin in twins)
                    {
                        DeviceTwinList.Add(twin);

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


        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? button = sender as Button;

                if (button != null)
                {
                    Twin? twin = button.DataContext as Twin;

                    if (twin != null)
                    {
                        string deviceId = twin.DeviceId;


                        if (!string.IsNullOrEmpty(deviceId))
                            await _iotHub.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "start"
                            });
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


        }

        private async void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? button = sender as Button;

                if (button != null)
                {
                    Twin? twin = button.DataContext as Twin;

                    if (twin != null)
                    {
                        string deviceId = twin.DeviceId;


                        if (!string.IsNullOrEmpty(deviceId))
                            await _iotHub.SendMethodAsync(new MethodDataRequest
                            {
                                DeviceId = deviceId,
                                MethodName = "stop"
                            });

                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }


        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }




    }
}
