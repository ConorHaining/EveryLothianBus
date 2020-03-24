using System;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Hubs;
using EveryBus.Services;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;

namespace EveryBus
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR(ops => ops.EnableDetailedErrors = true);

            services.AddControllers().AddJsonOptions(options =>
            {
            });
            services.AddHttpClient("polling", client => { })
            .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
               handledEventsAllowedBeforeBreaking: 3,
               durationOfBreak: TimeSpan.FromSeconds(15)
            //    onBreak: Stop the service & time, log, report emergency
            ));

            services.AddHostedService<DataRefreshService>();

            services.AddSingleton<BusLocationsProvider>();
            services.AddSingleton<IObserver<VehicleLocation[]>, PersistLocations>();
            services.AddSingleton<IObserver<VehicleLocation[]>, BroadcastLocations>();
            services.AddSingleton<IRouteColourService, RouteColourService>();

            services.AddMemoryCache();
            // // # services.AddSingleton<IPollingService, PollingService>();
            services.AddSingleton<IRouteService, RouteService>();
            services.AddDbContextPool<BusContext>(
                ops => ops.UseMySql(@"server=db;user=dbuser;password=dbuserpassword;database=buses;"));
                // ops => ops.UseMySql(@"server=localhost;port=1234;user=dbuser;password=dbuserpassword;database=buses;"));
            services.AddTransient<IVehicleLocationsService, VehicleLocationsService>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<BusHub>("/busHub");
            });

            // app.ApplicationServices.GetService<IPollingService>();
            // var routes = app.ApplicationServices.GetService<IRouteService>();
            // routes.CreateRoutes();
            // app.ApplicationServices.GetServices<IObserver<VehicleLocation[]>>();

            // busContext.Database.Migrate();
        }
    }
}
