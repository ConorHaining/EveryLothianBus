using System;
using System.IO;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Hubs;
using EveryBus.Services;
using EveryBus.Services.Background;
using EveryBus.Services.Interfaces;
using McMaster.AspNetCore.LetsEncrypt;
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
            // services.AddLetsEncrypt().PersistDataToDirectory(new DirectoryInfo("/ssl/"), null);
            services.AddRazorPages();
            services.AddSignalR(ops => ops.EnableDetailedErrors = true);
            services.AddCors(o => o.AddPolicy("OpenPolicy", builder =>
            {
                builder.WithOrigins("https://edi-bus.glitch.me", "https://lacy-jungle-bramble.glitch.me")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            }));

            services.AddControllers().AddJsonOptions(options =>
            {
            });
            services.AddHttpClient("polling", client => { })
            .AddTransientHttpErrorPolicy(builder => builder.CircuitBreakerAsync(
               handledEventsAllowedBeforeBreaking: 3,
               durationOfBreak: TimeSpan.FromSeconds(15)
            //    onBreak: Stop the service & time, log, report emergency
            ));

            services.AddHostedService<LocationFetching>();

            services.AddSingleton<BusLocationsProvider>();
            services.AddSingleton<IObserver<VehicleLocation[]>, PersistLocations>();
            services.AddSingleton<IObserver<VehicleLocation[]>, BroadcastLocations>();
            services.AddSingleton<IRouteColourService, RouteColourService>();

            services.AddMemoryCache();
            // // # services.AddSingleton<IPollingService, PollingService>();
            services.AddSingleton<IRouteService, RouteService>();
            services.AddDbContextPool<BusContext>(
                // ops => ops.UseMySql(@""));
                ops => ops.UseMySql(Configuration.GetValue<string>("DatabaseSettings:ConnectionString")));
            services.AddTransient<IVehicleLocationsService, VehicleLocationsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, BusContext busContext)
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
            app.UseCors("OpenPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<BusHub>("/busHub");
            });
            
            busContext.Database.Migrate();

            // app.ApplicationServices.GetService<IPollingService>();
            var routes = app.ApplicationServices.GetService<IRouteService>();
            routes.CreateRoutes();
            app.ApplicationServices.GetServices<IObserver<VehicleLocation[]>>();

        }
    }
}
