using AutoMapper;
using C42G02Demo.BLL.Interfacies;
using C42G02Demo.BLL.Repositories;
using C42G02Demo.DAL.Data;
using C42G02Demo.DAL.Model;
using C42G02Demo.PL.MappingProfiles;
using C42G02Demo.PL.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace C42G02Demo.PL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; // Configuration has EVERYTHING Related to AppSetting
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            
            //services.AddSingleton<AppDbContext>();  // Per Application
            //services.AddScoped<AppDbContext>();     // Per Request
            //services.AddTransient<AppDbContext>();     // Per Operation

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); // Way 1 (Most Used) 
                //options.UseSqlServer(Configuration.GetSection("ConnectionStrings")["DefaultConnection"]); // Way 2
            }); //Scoped by default

            services.AddScoped<IDepartmentRepository,DepartmentRepository>();
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            
            //services.AddAutoMapper(c => c.AddProfile(new EmployeeProfile()));
            //services.AddAutoMapper(c => c.AddProfile(new UserProfile()));
            services.AddAutoMapper(M => M.AddProfiles(new List<Profile> { new EmployeeProfile(), new UserProfile() , new RoleProfile()}));

            services.AddIdentity<ApplicationUser, IdentityRole>(Options => 
            { 
                Options.Password.RequireNonAlphanumeric = true; // * @ #
                Options.Password.RequireDigit = true; // 12356
                Options.Password.RequireLowercase = true; // acshvjbsd
                Options.Password.RequireUppercase = true; // EVJRENL
            }).AddEntityFrameworkStores<AppDbContext>()
              .AddDefaultTokenProviders();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                Options.LoginPath = "Account/Login";
                Options.AccessDeniedPath = "Home/Error";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
