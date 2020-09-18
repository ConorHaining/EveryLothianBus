using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Hubs;
using EveryBus.Services;
using EveryBus.Services.Background;
using EveryBus.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using System;
using System.Collections.Generic;

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
            //services.AddCors(o => o.AddPolicy("OpenPolicy", builder =>
            //{

            //}));
            services.AddCors(
                options => options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins(
                            "https://*.azurestaticapps.net"
                         )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                        .SetIsOriginAllowed(origin => new Uri(origin).Host == "127.0.0.1");
                    }
                )
            );

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
            services.AddHostedService<RouteFetching>();

            services.AddSingleton<BusLocationsProvider>();
            services.AddSingleton<IObserver<List<VehicleLocation>>, PersistLocations>();
            services.AddSingleton<IObserver<List<VehicleLocation>>, BroadcastLocations>();
            services.AddSingleton<IObserver<List<VehicleLocation>>, CacheLocations>();
            services.AddSingleton<IRouteColourService, RouteColourService>();
            services.AddSingleton<IRouteService, RouteService>();
            services.AddSingleton<IStopsService, StopsService>();

            services.AddMemoryCache();

            services.AddDbContextPool<BusContext>(options =>
            {
                options.UseSqlServer(Configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            });
            services.AddTransient<IVehicleLocationsService, VehicleLocationsService>();
            services.AddApplicationInsightsTelemetry(Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"]);
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
            app.UseCors();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<BusHub>("/busHub");
            });

            busContext.Database.Migrate();
        }
    }
}
