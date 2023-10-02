using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using SharedLibrary.MVVM.Models;

namespace SharedLibrary.Handlers.Services
{
    public class IotHubManager
    {
        private RegistryManager _registryManager;
        private ServiceClient _serviceClient;
        private EventHubConsumerClient _consumerClient;
        private readonly System.Timers.Timer _timer;

        public event Action? DevicesUpdated;
        public List<DeviceItem>? CurrentDevices { get; private set; }

        public IotHubManager(IotHubManagerOptions options)
        {

            _registryManager = RegistryManager.CreateFromConnectionString(options.IotHubConnectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(options.IotHubConnectionString);
            _consumerClient = new EventHubConsumerClient(options.ConsumerGroup, options.EventHubEndpoint);


            CurrentDevices = new List<DeviceItem>();

            _timer = new System.Timers.Timer(5000);
            _timer.Elapsed += async (s, e) => await SetAllDevicesAsync();
            _timer.Start();

        }

        public async Task<CloudToDeviceMethodResult> SendMethodAsync(MethodDataRequest req)
        {
            try
            {
                var cloudMethod = new CloudToDeviceMethod(req.MethodName)
                {
                    ConnectionTimeout = new TimeSpan(req.ResponseTimeout)
                };


                if (req.Payload != null)
                    cloudMethod.SetPayloadJson(JsonConvert.SerializeObject(req.Payload));

                var result = await _serviceClient.InvokeDeviceMethodAsync(req.DeviceId, cloudMethod);
                if (result != null)
                {
                    return result;
                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return null!;
        }
        public async Task SetAllDevicesAsync()
        {
            var updated = false;

            try
            {
                var sqlQuery = "select * from  devices";


                var devicesTwin = await GetDevicesAsTwinAsync(sqlQuery);

                if (devicesTwin != null)
                {
                    CurrentDevices = await GetDevicesAsDeviceItemAsync(devicesTwin);
                    updated = true;

                }


                for (int i = CurrentDevices.Count - 1; i >= 0; i--)
                {
                    if (!devicesTwin.Any(x => x.DeviceId == CurrentDevices[i].DeviceId))
                    {
                        CurrentDevices.RemoveAt(i);
                        updated = true;
                    }
                }


                if (updated)
                    DevicesUpdated?.Invoke();
            }


            catch (Exception ex)
            {
                CurrentDevices = null!;
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }


        }
        public async Task<IEnumerable<Twin>> GetDevicesAsTwinAsync(string sqlQuery)
        {
            try
            {

                var devicesTwin = new List<Twin>();
                var result = _registryManager.CreateQuery(sqlQuery);

                if (result.HasMoreResults)
                    foreach (var twin in await result.GetNextAsTwinAsync())
                        devicesTwin.Add(twin);


                return devicesTwin;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }

            return null!;

        }

        public async Task<List<DeviceItem>> GetDevicesAsDeviceItemAsync(IEnumerable<Twin> devicesTwin)
        {

            try
            {
                var deviceItems = new List<DeviceItem>();

                foreach (var twin in devicesTwin)
                {

                    var isActive = false;
                    if (twin.Properties?.Reported.Contains("deviceOn") == true)
                        isActive = bool.TryParse(twin.Properties.Reported["deviceOn"].ToString(), out bool parsedValue) ? parsedValue : isActive;


                    var deviceType = "Unknown";
                    if (twin.Properties?.Reported.Contains("deviceType") == true)
                        deviceType = twin.Properties.Reported["deviceType"].ToString();

                    deviceItems.Add(new DeviceItem
                    {
                        DeviceId = twin.DeviceId,
                        DeviceType = deviceType,
                        IsActive = isActive
                    });

                }

                return deviceItems;


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }


            return null!;
        }

        public async Task<IEnumerable<string>> GetDevicesAsJsonAsync(string sqlQuery = "select * from  devices")
        {
            try
            {

                var devices = new List<string>();
                var result = _registryManager.CreateQuery(sqlQuery);

                if (result.HasMoreResults)
                    foreach (var device in await result.GetNextAsJsonAsync())
                        devices.Add(device);

                return devices;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{ex.Message}");
            }

            return null!;

        }



    }

}
