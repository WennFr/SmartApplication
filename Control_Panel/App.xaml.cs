using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Handlers.Services;
using SharedLibrary.Models;

namespace Control_Panel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new IotHubManager(new IotHubManagerOptions
                    {
                        IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                        EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                        EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                        ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                    }));
                })
                .Build();

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
