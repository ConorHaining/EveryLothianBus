using EveryBus.Controller;
using EveryBus.Domain.Models;
using EveryBus.Services.Interfaces;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Shouldly;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace EveryBus.Tests.Controllers
{
    [TestFixture]
    class DeparturesControllerTests
    {
        [Test]
        public async Task GetLiveDeparturesAsync_GetGetStop_ForGivenAtcoCode()
        {
            var client = new HttpClient(new MockHttpMessageHandler("[]", HttpStatusCode.OK));
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 234 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            await controller.GetLiveDeparturesAsync("AtcoCode");

            stopsService.Received().GetByAtocCode("AtcoCode");
        }

        [Test]
        public async Task GetLiveDeparturesAsync_ShouldRequestLiveDeparturesFromTFEAsync()
        {
            var handler = new MockHttpMessageHandler("[]", HttpStatusCode.OK);
            var client = new HttpClient(handler);
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 123456789 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            await controller.GetLiveDeparturesAsync("AtcoCode");

            handler.Request.RequestUri.ToString().ShouldContain("http://tfe-opendata.com");
        }

        [Test]
        public async Task GetLiveDeparturesAsync_ShouldRequestLiveDeparturesFromTFE_WithStopIdAsync()
        {
            var handler = new MockHttpMessageHandler("[]", HttpStatusCode.OK);
            var client = new HttpClient(handler);
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 123456789 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            await controller.GetLiveDeparturesAsync("AtcoCode");

            handler.Request.RequestUri.ToString().ShouldContain("123456789");
        }

        [Test]
        public async Task GetLiveDeparturesAsync_ShouldAddRouteColourToEachRouteAsync()
        {
            var handler = new MockHttpMessageHandler("[{}]", HttpStatusCode.OK);
            var client = new HttpClient(handler);
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var colours = new RouteColours { Colour = "#6f0ac2", TextColor = "#a927a3" };
            routeColourService.Get(Arg.Any<string>()).Returns(colours);
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 123456789 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            var response = await controller.GetLiveDeparturesAsync("AtcoCode");

            response[0].RouteColour.ShouldBe("#6f0ac2");
        }

        [Test]
        public async Task GetLiveDeparturesAsync_ShouldAddTextColourToEachRouteAsync()
        {
            var handler = new MockHttpMessageHandler("[{}]", HttpStatusCode.OK);
            var client = new HttpClient(handler);
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var colours = new RouteColours { Colour = "#6f0ac2", TextColor = "#a927a3" };
            routeColourService.Get(Arg.Any<string>()).Returns(colours);
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 123456789 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            var response = await controller.GetLiveDeparturesAsync("AtcoCode");

            response[0].TextColour.ShouldBe("#a927a3");
        }

        [Test]
        public async Task GetLiveDeparturesAsync_ShouldNotModifyTheRouteName()
        {
            var handler = new MockHttpMessageHandler("[{\"routeName\":\"XX\"}]", HttpStatusCode.OK);
            var client = new HttpClient(handler);
            var logger = Substitute.For<ILogger<DeparturesController>>();
            var httpClientFactory = Substitute.For<IHttpClientFactory>();
            httpClientFactory.CreateClient(Arg.Any<string>()).Returns(client);
            var routeColourService = Substitute.For<IRouteColourService>();
            var colours = new RouteColours { Colour = "#6f0ac2", TextColor = "#a927a3" };
            routeColourService.Get(Arg.Any<string>()).Returns(colours);
            var stopsService = Substitute.For<IStopsService>();
            stopsService.GetByAtocCode(Arg.Any<string>()).Returns(new Stop { StopId = 123456789 });
            var controller = new DeparturesController(
                logger,
                httpClientFactory,
                routeColourService,
                stopsService);

            var response = await controller.GetLiveDeparturesAsync("AtcoCode");

            response[0].RouteName.ShouldBe("XX");
        }
    }
}
