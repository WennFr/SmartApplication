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
using SharedLibrary.MVVM.Core;
using SharedLibrary.Services;
using SharedLibrary.MVVM.ViewModels;

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
                    services.AddSingleton<NavigationStore>();
                    services.AddSingleton<DateTimeService>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton(new IotHubManager(new IotHubManagerOptions
                    {
                        IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                        EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                        EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                        ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                    }));
                    services.AddSingleton<HomeViewModel>();
                    services.AddSingleton<SettingsViewModel>();
                })
                .Build();

        }

        protected override async void OnStartup(StartupEventArgs args)
        {
            
            var mainWindow = AppHost!.Services.GetRequiredService<MainWindow>();
            var navigationStore = AppHost!.Services.GetService<NavigationStore>();
            var dateTimeService = AppHost!.Services.GetService<DateTimeService>();
            var iotHub = AppHost!.Services.GetService<IotHubManager>();


            navigationStore!.CurrentViewModel = new HomeViewModel(navigationStore, dateTimeService!, iotHub);

            await AppHost!.StartAsync();
            mainWindow.Show();
            base.OnStartup(args);
        }
    }
}
