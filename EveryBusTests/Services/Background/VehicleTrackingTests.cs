using EveryBus.Domain.Models;
using EveryBus.Services.Background;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EveryBus.Tests.Services.Background
{

    [TestFixture]
    class VehicleTrackingTests
    {
        private ILogger<VehicleTracking> logger;
        private IConfigurationRoot configuration;
        private IHttpClientFactory httpClientFactory;
        private MockHttpMessageHandler httpMessageHander;
        private ServiceProvider serviceProvider;
        private IObserver<List<VehicleLocation>> observer;

        [SetUp]
        public void SetUp()
        {
            logger = Substitute.For<ILogger<VehicleTracking>>();

            var inMemorySettings = new Dictionary<string, string>
            {
                {"tfeopendata:pollInterval", "1"},
                {"tfeopendata:address", "https://example.com/" }
            };
            configuration = new ConfigurationBuilder()
                                   .AddInMemoryCollection(inMemorySettings)
                                   .Build();

            httpMessageHander = new MockHttpMessageHandler("{}", HttpStatusCode.OK);
            var client = new HttpClient(httpMessageHander);
            httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);

            observer = Substitute.For<IObserver<List<VehicleLocation>>>();
            var services = new ServiceCollection();
            services.AddScoped(x => observer);
            serviceProvider = services.BuildServiceProvider();

        }

        [Test]
        public async Task StartAsync_ShouldLogTheJobIsStarting()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            logger.Received().LogInformation("VehicleTracking is starting.");
        }

        [Test]
        public async Task StartAsync_ShouldReturnTheJobIfComplete()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            cancellationTokenSource.Cancel();
            await vehicleTracking.StartAsync(cancellationToken);

            logger.Received().LogInformation("VehicleTracking was cancelled.");
        }

        [Test]
        public async Task ExecuteAsync_CallTheProvidedObservers()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);


            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            observer.Received().OnNext(Arg.Any<List<VehicleLocation>>());
        }

        [Test]
        public async Task ExecuteAsync_ShouldLogThatARequestWasMade()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            logger.Received().LogDebug("Request raised.");
        }

        [Test]
        public async Task ExecuteAsync_ShouldAddVehicleLocationsToRequestUri()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            httpMessageHander.Request.RequestUri.PathAndQuery.ShouldContain("vehicle_locations");
        }

        [Test]
        public async Task ExecuteAsync_ShouldLogAWarningWhenUriIsNull()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"tfeopendata:pollInterval", "1"},
            };
            configuration = new ConfigurationBuilder()
                                   .AddInMemoryCollection(inMemorySettings)
                                   .Build();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            logger.Received().LogWarning("Request has been unsucessful.");
        }

        [Test]
        public async Task ExecuteAsync_ShouldLogAWarningWhenRequestIsUnsuccessful()
        {
            httpMessageHander = new MockHttpMessageHandler("{}", HttpStatusCode.InternalServerError);
            var client = new HttpClient(httpMessageHander);
            httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            logger.Received().LogWarning("Request has been unsucessful.");
        }

        [Test]
        public async Task ExecuteAsync_ShouldDeserializeTheResponse()
        {
            httpMessageHander = new MockHttpMessageHandler("{\"last_updated\":1604181355,\"vehicles\":[{\"vehicle_id\":\"17\",\"last_gps_fix\":1604181315,\"latitude\":55.93389,\"longitude\":-3.105528,\"speed\":22,\"heading\":203,\"service_name\":\"30\",\"destination\":\"Clovenstone\",\"journey_id\":\"5183\",\"next_stop_id\":\"36234394\",\"vehicle_type\":\"bus\"}]}", HttpStatusCode.OK);
            var client = new HttpClient(httpMessageHander);
            httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            var vehicleTracking = new VehicleTracking(logger, configuration, httpClientFactory, serviceProvider);

            await vehicleTracking.StartAsync(cancellationToken);
            cancellationTokenSource.Cancel();

            observer.Received().OnNext(Arg.Any<List<VehicleLocation>>());
        }


    }
}
