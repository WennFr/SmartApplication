using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Contexts;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Models;
using SharedLibrary.MVVM.Models.Entities;
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
                    services.AddDbContext<SmartAppDbContext>((provider, options) =>
                    {
                        options.UseSqlite($"DataSource=Database.sqlite.db", x => x.MigrationsAssembly(nameof(SharedLibrary)));
                    }, ServiceLifetime.Scoped);


                    using (var scope = services.BuildServiceProvider().CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<SmartAppDbContext>();

                        if (!dbContext.DatabaseExists())
                            dbContext.Database.Migrate();

                        if (!dbContext.ConnectionStringsExists())
                        {
                            dbContext.Settings.Add(new SmartAppSettings
                            {
                                Id = 1,
                                IotHubConnectionString = config.Configuration.GetConnectionString("IotHub")!,
                                EventHubEndpoint = config.Configuration.GetConnectionString("EventHubEndpoint")!,
                                EventHubName = config.Configuration.GetConnectionString("EventHubName")!,
                                ConsumerGroup = config.Configuration.GetConnectionString("ConsumerGroup")!
                            });

                            dbContext.SaveChanges();
                        }
                    }

                    services.AddSingleton<IotHubManager>();
                    services.AddSingleton<SmartAppDbService>();
                    services.AddSingleton<DateTimeService>();
                    services.AddSingleton<WeatherService>();
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
            var iotHubManager = AppHost!.Services.GetRequiredService<IotHubManager>();
            iotHubManager.Initialize();
            var smartAppService = AppHost!.Services.GetRequiredService<SmartAppDbService>();
            smartAppService.Initialize();


            mainWindow.Show();

            base.OnStartup(args);
        }
    }
}
