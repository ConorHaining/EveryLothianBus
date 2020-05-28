using NUnit.Framework;
using Shouldly;
using NSubstitute;
using EveryBus.Services.Background;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using EveryBus.Domain.Models;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace EveryBus.Tests.Services.Background
{
    [TestFixture]
    public class RouteFetchingTests
    {
        [Test]
        public void ExecuteAsync_ShouldBroadcastToObservers_WhenNewDataArrives()
        {
            Assert.Fail();
        }

        [Test]
        public void PollAsync_ShouldAddEndpoint_WhenRequestIsMade()
        {
            var logger = Substitute.For<ILogger<RouteFetching>>();
            var testSettings = new Dictionary<string, string>
            {
                {"tfeopendata:address", "https://example.com/"},
                {"lothianApi:address", "https://example.com/"},
                {"tfeopendata:pollInterval", "1500"},
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(testSettings)
                                                          .Build();
            var observers = new List<IObserver<VehicleLocation[]>>();
            var clientMock = Substitute.For<HttpClient>();
            clientMock.BaseAddress = new Uri("https://example.com/");
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(clientMock);
            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            var routeFetching = new RouteFetching(
                logger,
                configuration,
                observers,
                httpClientFactory,
                scopeFactory);

            routeFetching.StartAsync(default(CancellationToken));
            routeFetching.StopAsync(default(CancellationToken));

            clientMock.Received()
                      .GetAsync(Arg.Is<string>(x => x.Contains("services")));
        }

        [Test]
        public async Task PollAsync_ShouldReturnDefault_WhenRequestIsUnsucessful()
        {
            var logger = Substitute.For<ILogger<RouteFetching>>();
            var testSettings = new Dictionary<string, string>
            {
                {"tfeopendata:address", "https://example.com/"},
                {"lothianApi:address", "https://example.com/"},
                {"tfeopendata:pollInterval", "1500"},
            };
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(testSettings)
                                                          .Build();
            var observers = new List<IObserver<VehicleLocation[]>>();
            var clientMock = Substitute.For<HttpClient>();
            clientMock.BaseAddress = new Uri("https://example.com/");
            clientMock.When(x => x.GetAsync("https://example.com/services"))
                      .Do(x => { throw new HttpRequestException(); });
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(clientMock);
            var scopeFactory = Substitute.For<IServiceScopeFactory>();
            var routeFetching = new RouteFetching(
                logger,
                configuration,
                observers,
                httpClientFactory,
                scopeFactory);

            var services = await routeFetching.PollAsync().ConfigureAwait(false);

            services.ShouldBe(default(ServicesResponse));
        }

        [Test]
        public void PollAsync_ShouldDeserialize_WhenRequestIsSucessful()
        {
            // var logger = Substitute.For<ILogger<RouteFetching>>();
            // var testSettings = new Dictionary<string, string>
            // {
            //     {"tfeopendata:address", "https://example.com/"},
            //     {"lothianApi:address", "https://example.com/"},
            //     {"tfeopendata:pollInterval", "1500"},
            // };
            // var configuration = new ConfigurationBuilder().AddInMemoryCollection(testSettings)
            //                                               .Build();
            // var observers = new List<IObserver<VehicleLocation[]>>();
            // var clientMock = Substitute.For<HttpClient>();
            // clientMock.BaseAddress = new Uri("https://example.com/");
            // clientMock.When(x =>  x.GetAsync(Arg.Any<String>()))
            //           .Do(x => { throw new HttpRequestException(); });
            // var httpClientFactory = Substitute.For<IHttpClientFactory>();
            // httpClientFactory.CreateClient(Arg.Any<string>()).Returns(clientMock);
            // var scopeFactory = Substitute.For<IServiceScopeFactory>();
            // var routeFetching = new RouteFetching(
            //     logger,
            //     configuration,
            //     observers,
            //     httpClientFactory,
            //     scopeFactory);

            // var services = await routeFetching.PollAsync().ConfigureAwait(false);

            // services.ShouldBe(default(ServicesResponse));
        }

        [Test]
        public void FindUniqueServices_ShouldReturnService_WhenCannotFindDestination()
        {
            Assert.Fail();
        }

        [Test]
        public void FindUniqueServices_ShouldReturnService_WhenStopsAreDifferent()
        {
            Assert.Fail();
        }

        [Test]
        public void FindUniqueServices_ShouldNotReturnService_WhenCanFindDestination_AndSameStops()
        {
            Assert.Fail();
        }
    }
}