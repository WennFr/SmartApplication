using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SharedLibrary.Handlers.Services;
using Newtonsoft.Json.Linq;
using SharedLibrary.MVVM.Models.DeviceConfiguration;

namespace Lamp_Device
{
    public partial class App : Application
    {

        public IHost? AppHost { get; set; }



        public App()
        {
            InitializeApp().GetAwaiter().GetResult();
        }


        private async Task InitializeApp()
        {
            await DeviceRegistrationSetup();

            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
                .ConfigureServices((config, services) =>
                {
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new DeviceConfiguration(config.Configuration.GetConnectionString("LampDevice")!));
                    services.AddTransient<DeviceManager>();
                    services.AddSingleton<NetworkManager>();
                })
                .Build();
        }


        private async Task DeviceRegistrationSetup()
        {

            var connectionString = string.Empty;
            try
            {
                var configurationBuilder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();


                var root = configurationBuilder.Build();
                connectionString = root.GetConnectionString("LampDevice");
            }
            catch (Exception e) { }


            if (string.IsNullOrEmpty(connectionString))
            {
                var newDeviceId = "lamp_device";
                var deviceType = "Lamp";

                var registrationManager = new RegistrationManager();
                connectionString = await registrationManager.RegisterDevice(newDeviceId, deviceType);

                var newConfig = new JObject(
                    new JProperty("ConnectionStrings", new JObject(
                        new JProperty("LampDevice", connectionString)
                    ))
                );


                var appSettingsPath = "../../../appsettings.json";
                File.WriteAllText(appSettingsPath, newConfig.ToString(Formatting.Indented));
                File.WriteAllText("appsettings.json", newConfig.ToString(Formatting.Indented));


            }

        }




        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();   // med DI
            mainWindow.Show();

            base.OnStartup(e);
        }

    }
}
