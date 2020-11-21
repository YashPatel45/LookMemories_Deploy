using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LookMemories_WEB.Common;
using LookMemories_WEB.Interfaces;
using LookMemories_WEB.Model.DataBase;
using LookMemories_WEB.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LookMemories_WEB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. This method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
            }).AddRazorRuntimeCompilation().AddNToastNotifyToastr();

            services.AddIdentity<AccountUser, IdentityRole>(setupAction =>
            {
                setupAction.Password.RequiredLength = 6; //Default is 6
                setupAction.Password.RequireLowercase = true;
                setupAction.Password.RequireUppercase = true;
                setupAction.Password.RequireDigit = true;
                setupAction.Password.RequireNonAlphanumeric = true;
                //setupAction.Password.RequiredUniqueChars = 2;
                setupAction.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<DBContext>()
        .AddDefaultTokenProviders();

            #region Authentication
            services.ConfigureApplicationCookie(configure =>
            {
                configure.Cookie.Name = "lookmemories";
                configure.LoginPath = "/Account/Login";
                configure.AccessDeniedPath = "/Account/AccessDenied";

                configure.ExpireTimeSpan = TimeSpan.FromDays(1);


            });
            #endregion

            //CHECK CONNECTION 
            services.AddDbContextPool<DBContext>(optionsAction => optionsAction.UseSqlServer(Configuration["ConeectionString:DefaultConnectionString"]));

            services.AddSingleton<DataProtectionPurposeString>();
            services.AddScoped<IAlbums, AlbumRepository>();
            services.AddScoped<IPhoto, PhotoRepository>();

         
        }

        // This method gets called by the runtime. This method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,DBContext dBContext)
        {
            using (dBContext)
            {
                dBContext.Database.Migrate();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseNToastNotify();
            app.UseMvc(configureRoutes =>
            {
                configureRoutes.MapRoute("custom", "{controller=Account}/{action=Login}/{id?}");
            });
            
           
        }
    }
}
