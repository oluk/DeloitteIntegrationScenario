using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CITYAPI.Entities;
using Microsoft.EntityFrameworkCore;
namespace CITYAPI.Data
{
   public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        


    public DbSet<User> Users { get; set; }
    public DbSet<City> Cities { get; set; }
    }
     
}