using C42G02Demo.DAL.Data.Configurations;
using C42G02Demo.DAL.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C42G02Demo.DAL.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            // Sends options object to Parameterised Ctor in Base so ot uses the Connection string
            // at OnConfiguring Of Base
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("server =DESKTOP-ENSOJQ8\\MSSQLSERVER01; Database=C42G02MVC; Trusted_Connection=True; TrustServerCertificate=True; MultipleActiveResultSets = True");
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For One Configuration :
            // modelBuilder.ApplyConfiguration<Department>(new DepartmentConfigurations());

            // For All Configurations :
            modelBuilder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder); // Required ONLY If We have DbContext DbSets (There is a time we are gonna need it)
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
