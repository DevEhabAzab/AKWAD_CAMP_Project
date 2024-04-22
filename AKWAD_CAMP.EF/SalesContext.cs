using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Protocols;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;


namespace AKWAD_CAMP.EF
{
    public partial class SalesContext : DbContext
    {

        public SalesContext() :base()
        {
                
        }

        public SalesContext(DbContextOptions<SalesContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("Sales");
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionStr);
        }

        
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
           

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
