using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GooberEatsAPI.Models.Interfaces;
using GooberEatsAPI.Models.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GooberEatsAPI
{
    public class Startup
    {
        /// <summary>
        /// Enables dependency injection via config file.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Constructor method to add our User Secrets file to the Configuration property.
        /// </summary>
        public Startup()
        {
            var builder = new ConfigurationBuilder().AddEnvironmentVariables();

            builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Allows the application to utilize the MVC architecture pattern.
            services.AddMvc();

            // Dependency Injection mappings.
            services.AddTransient<IPlaces, PlacesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // Maps our default Controller route.
                endpoints.MapControllerRoute("Default", "{controller=Places}/{action=Index}/{id?}");
            });
        }
    }
}
