using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedLibrary.Handlers.Services;
using SharedLibrary.MVVM.Models;
using SharedLibrary.MVVM.Models.Entities;

namespace SharedLibrary.Contexts
{
    public class SmartAppDbContext : DbContext
    {

        public SmartAppDbContext()
        {
        }

        public SmartAppDbContext(DbContextOptions<SmartAppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            try
            {
                Database.Migrate();
            }
            catch { }
        }

        public bool DatabaseExists()
        {
            return Database.CanConnect(); 
        }

        public bool ConnectionStringsExists()
        {
            return Settings.Any(s => s.Id == 1);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<SmartAppSettings> Settings { get; set; }



    }
}
