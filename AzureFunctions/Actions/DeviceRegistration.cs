using System.Net;
using AzureFunctions.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Actions
{
    public class DeviceRegistration
    {
        private readonly IotHubManager _iotHubManager;
        public DeviceRegistration(IotHubManager deviceManager)
        {
            _iotHubManager = deviceManager;
        }

        [Function("DeviceRegistration")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function,  "post")] HttpRequestData req)
        {
            var deviceId = req.Query["deviceId"];
            if (!string.IsNullOrEmpty(deviceId))
            {
                var device = await _iotHubManager.GetDeviceAsync(deviceId);
                device ??= await _iotHubManager.RegisterDeviceAsync(deviceId);

                if (device != null)
                {
                    var connectionString = _iotHubManager.GenerateConnectionString(device);
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        return GenerateHttpResponse(req, HttpStatusCode.OK, connectionString);
                    }
                    else
                    {
                        return GenerateHttpResponse(req, HttpStatusCode.BadRequest, "An error occured! Connectionstring was not created.");
                    }
                }

                return GenerateHttpResponse(req, HttpStatusCode.BadRequest, "An error occured! Parameter deviceId is required.");
            }

            return null!;
        }



        private HttpResponseData GenerateHttpResponse(HttpRequestData req, HttpStatusCode statusCode, string content)
        {

            var response = req.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
            response.WriteString(content);

            return response;
        }
    }


}
