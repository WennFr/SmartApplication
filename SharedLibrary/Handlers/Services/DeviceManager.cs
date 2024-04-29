using Microsoft.Azure.Devices.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedLibrary.Models;
using System.Diagnostics;
using Microsoft.Azure.Devices.Shared;
using SharedLibrary.MVVM.Models.DeviceConfiguration;

namespace SharedLibrary.Handlers.Services
{
    public class DeviceManager
    {
        public DeviceConfiguration Configuration { get; set; }

        public bool AllowSending() => Configuration.AllowSending;

        public DeviceManager(DeviceConfiguration config)
        {
            Configuration = config;
            Task.WhenAll(DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "allowSending", Configuration.AllowSending),
                Configuration.DeviceClient.SetMethodDefaultHandlerAsync(DirectMethodCallback, null), 
                SetTelemetryIntervalAsync(), NetworkManager.CheckConnectivityAsync());

        }


        public async Task SendDeviceTypeAsync(string deviceType)
        {
            try
            {
                var _deviceType = await DeviceTwinManager
                    .GetDesiredTwinPropertyAsync(Configuration.DeviceClient, "deviceType");

                if (_deviceType != null)
                {
                    await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "deviceType",
                        _deviceType);
                }

                else
                {
                    await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "deviceType",
                        deviceType);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating DeviceType: {ex.Message}");
            }
        }




        private async Task SetTelemetryIntervalAsync()
        {

            var _telemetryInterval = await DeviceTwinManager
                .GetDesiredTwinPropertyAsync(Configuration.DeviceClient, "telemetryInterval");

            if (_telemetryInterval != null)
            {
                Configuration.TelemetryInterval = int.Parse(_telemetryInterval.ToString()!);
            }

            await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "telemetryInterval",
                Configuration.TelemetryInterval);

        }

        public async Task<bool> SendLatestMessageAsync(string payload)
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(payload));
                await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "latestMessage", payload);
                await Task.Delay(10);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> SendDataToCosmosDbAsync(string payload)
        {
            try
            {
                var data = new Message(Encoding.UTF8.GetBytes(payload));
                await Configuration.DeviceClient.SendEventAsync(data);
                await Task.Delay(10);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }




        public async Task<bool> SendOperationalStatusAsync(string payload)
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(payload));
                await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "deviceOn", payload);
                await Task.Delay(10);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }

        public async Task<bool> SendLocationAsync(string payload)
        {
            try
            {
                var message = new Message(Encoding.UTF8.GetBytes(payload));
                await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "location", payload);
                await Task.Delay(10);
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return false;
        }



        private async Task<MethodResponse> DirectMethodCallback(MethodRequest req, object userContext)
        {

            var res = new MethodDataResponse();

            try
            {
                res.Message = $"Method: {req.Name} executed successfully.";

                switch (req.Name.ToLower())
                {
                    case "start":
                        await SendOperationalStatusAsync(JsonConvert.SerializeObject("true"));
                        Configuration.AllowSending = true;
                        await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "allowSending", Configuration.AllowSending);
                        break;

                    case "stop":
                        await  SendOperationalStatusAsync(JsonConvert.SerializeObject("false"));
                        Configuration.AllowSending = false;
                        await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "allowSending", Configuration.AllowSending);
                        break;

                    case "settelemetryinterval":

                        int desiredInterval;
                        if (int.TryParse(req.DataAsJson, out desiredInterval))
                        {
                            Configuration.TelemetryInterval = desiredInterval;
                            await DeviceTwinManager.UpdateReportedTwinPropertyAsync(Configuration.DeviceClient, "telemetryInterval", Configuration.TelemetryInterval);

                            res.Payload = desiredInterval.ToString();

                            return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 200);
                        }

                        break;

                    default:
                        res.Message = $"Method: {req.Name} could not be found.";
                        return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 404);

                }

                await Task.Delay(10);
                return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 200);

            }
            catch (Exception ex)
            {
                res.Message = $"Error: {ex.Message}";
                return new MethodResponse(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(res)), 400);

            }
        }


    }
}
