using Newtonsoft.Json;
using SharedLibrary.Handlers.Services;
using SharedLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharedLibrary.MVVM.Models.LampTelemetryDataModel;

namespace Lamp_Device
{
    public partial class MainWindow : Window
    {
        private readonly DeviceManager _deviceManager;



        public MainWindow(DeviceManager deviceManager)
        {
            InitializeComponent();
            _deviceManager = deviceManager;
            Task.WhenAll(SetDeviceTypeAsync(), SendTelemetryDataAsync(), CheckConnectivityAsync(), ToggleLampStateAsync());
        }


        private async Task ToggleLampStateAsync()
        {

            while (true)
            {

                if (_deviceManager.AllowSending())
                {
                    LampOnIcon.Visibility = Visibility.Visible;
                    LampOffIcon.Visibility = Visibility.Collapsed;
                }
                else
                {
                    LampOnIcon.Visibility = Visibility.Collapsed;
                    LampOffIcon.Visibility = Visibility.Visible;

                }

                await Task.Delay(1000);
            }
        }

        private async Task CheckConnectivityAsync()
        {
            while (true)
            {
                ConnectivityStatus.Text = await NetworkManager.CheckConnectivityAsync();
                await Task.Delay(1000);

            }
        }

        private async Task SetDeviceTypeAsync()
        {
            var deviceType = "Lamp";

            await _deviceManager.SendDeviceTypeAsync(deviceType);
        }




        private async Task SendTelemetryDataAsync()
        {

            while (true)
            {
                if (_deviceManager.Configuration.AllowSending)
                {
                    var dataModel = new LampTelemetryDataModel()
                    {
                        IsLampOn = true,
                        TemperatureCelsius = 2000,
                        Location = "Living Room",
                        CurrentTime = DateTime.Now
                    };

                    var telemetryDataJson = JsonConvert.SerializeObject(new
                    {
                        DeviceOn = dataModel.IsLampOn,
                        TemperatureCelsius = dataModel.TemperatureCelsius,
                        Location = dataModel.Location,
                        CurrentTime = dataModel.CurrentTime,
                        ContainerName = dataModel.ContainerName,
                    });

                    var latestMessageJson = JsonConvert.SerializeObject(new
                    {
                        TemperatureCelsius = dataModel.TemperatureCelsius,
                        CurrentTime = dataModel.CurrentTime,
                    });

                    var operationalStatusJson = JsonConvert.SerializeObject(dataModel.IsLampOn);

                    var locationString = dataModel.Location;


                    if (await _deviceManager.SendLatestMessageAsync(latestMessageJson) &&
                        await _deviceManager.SendOperationalStatusAsync(operationalStatusJson) &&
                        await _deviceManager.SendLocationAsync(locationString) &&
                        await _deviceManager.SendDataToCosmosDbAsync(telemetryDataJson))
                        CurrentMessageSent.Text = $"Message sent successfully: {latestMessageJson}, DeviceOn: {operationalStatusJson}, Location: {locationString}";


                    var telemetryInterval = _deviceManager.Configuration.TelemetryInterval;

                    await Task.Delay(telemetryInterval);
                }
            }

        }

    }
}
