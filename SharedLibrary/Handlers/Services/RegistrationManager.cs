using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Handlers.Services
{
    public class RegistrationManager
    {
        private string _connectionString = string.Empty;
        private DeviceClient deviceClient = null!;

        public async Task<string> RegisterDevice(string deviceId, string deviceType)
        {
            try
            {
                using var httpClient = new HttpClient();
                var result = await httpClient.PostAsync($"https://fw-smart-af.azurewebsites.net/api/DeviceRegistration?deviceId={deviceId}&code=pfwA54Y0hVDgQ_XJpOJLsiGDOnKykwXwEYg4SyZk1aUbAzFuyXhLGw==", null!);
                _connectionString = await result.Content.ReadAsStringAsync();

                deviceClient = DeviceClient.CreateFromConnectionString(_connectionString, Microsoft.Azure.Devices.Client.TransportType.Mqtt);

                var twinCollection = new TwinCollection();
                twinCollection["deviceType"] = $"{deviceType}";
                await deviceClient.UpdateReportedPropertiesAsync(twinCollection);

                var twin = await deviceClient.GetTwinAsync();

                Console.WriteLine("Device Connected!");
                Console.WriteLine($"{twin.Properties.Reported["deviceType"]}");


                return _connectionString;
            }
            catch (Exception e)
            {
               
            }

            return _connectionString;
        }

    }
}
