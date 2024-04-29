using Azure.Messaging.EventHubs.Consumer;
using Microsoft.Azure.Devices;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Services
{
    public class IotHubManager
    {

        private string _connectionString = string.Empty;
        private RegistryManager _registryManager;
        private ServiceClient _serviceClient;
        private EventHubConsumerClient _consumerClient;


        public IotHubManager(string connectionString)
        {
            _connectionString = connectionString;
            _registryManager = RegistryManager.CreateFromConnectionString(_connectionString);
            _serviceClient = ServiceClient.CreateFromConnectionString(_connectionString);
        }

        public async Task<Device> GetDeviceAsync(string deviceId)
        {

            try
            {
                var device = await _registryManager.GetDeviceAsync(deviceId);
                if (device != null)
                {
                    return device;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<Device> RegisterDeviceAsync(string deviceId)
        {
            try
            {
                var device = await _registryManager!.AddDeviceAsync(new Device(deviceId));
                if (device != null)
                    return device;
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }

            return null!;
        }

        public string GenerateConnectionString(Device device)
        {
            try
            {
                return $"{_connectionString.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
