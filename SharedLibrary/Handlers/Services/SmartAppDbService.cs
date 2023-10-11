using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Contexts;
using SharedLibrary.MVVM.Models.Dto;
using SharedLibrary.MVVM.Models.Entities;

namespace SharedLibrary.Handlers.Services
{
    public class SmartAppDbService
    {
        private readonly SmartAppDbContext _context;

        public string? IotHubConnectionString { get; private set; }
        public string? EventHubEndpoint { get; private set; }
        public string? EventHubName { get; private set; }
        public string? ConsumerGroup { get; private set; }



        public SmartAppDbService(SmartAppDbContext context)
        {
            _context = context;
        }


        public void Initialize()
        {
            IotHubConnectionString = _context.Settings.OrderBy(s => s.Id).LastOrDefault()?.IotHubConnectionString!;
            EventHubEndpoint = _context.Settings.OrderBy(s => s.Id).LastOrDefault()?.EventHubEndpoint!;
            EventHubName = _context.Settings.OrderBy(s => s.Id).LastOrDefault()?.EventHubName!;
            ConsumerGroup = _context.Settings.OrderBy(s => s.Id).LastOrDefault()?.ConsumerGroup!;

        }


        public void CreateNewConnectionStrings(ConnectionStringsDto connectionStringsDto)
        {
            SmartAppSettings smartAppSettings = connectionStringsDto;
            _context.Settings.Add(smartAppSettings);

            _context.SaveChanges();
        }


        public void ResetConnectionStrings()
        {
            var allButTheFirst = _context.Settings.Skip(1).ToList();
            foreach (var item in allButTheFirst)
                _context.Settings.Remove(item);

            _context.SaveChanges();
        }


    }
}
