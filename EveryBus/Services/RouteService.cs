using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using EveryBus.Domain;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EveryBus.Services
{
    public class RouteService : IRouteService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public RouteService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public IEnumerable<Service> GetRoutes()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var busContext = scope.ServiceProvider.GetRequiredService<BusContext>();

                return busContext.Services.ToList();
            }
        }

    }
} 