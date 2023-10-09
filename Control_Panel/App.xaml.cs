using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Models;
using SharedLibrary.Services;
using SharedLibrary.MVVM.ViewModels;

namespace Control_Panel
{
    public partial class App : Application
    {
        public IHost? AppHost { get; set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                })
               .ConfigureServices((config, services) =>
                {
                    services.AddTransient<HttpClient>();
                    services.AddSingleton<DateTimeService>();
                    services.AddSingleton<WeatherService>();
                    services.AddSingleton<FileManager>();
                    services.AddSingleton(new IotHubManager(new IotHubManagerOptions
                    {
                        IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                        EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                        EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                        ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                    }));
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>();
                    services.AddSingleton<AddDeviceViewModel>();
                    services.AddSingleton<AllDevicesViewModel>();
                    services.AddSingleton<ConfigurationViewModel>();
                    services.AddSingleton<MainWindowViewModel>();
                    services.AddSingleton<MainWindow>();
                })
                .Build();

        }

        protected override async void OnStartup(StartupEventArgs args)
        {
            await AppHost!.StartAsync();

            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(args);
        }
    }
}
