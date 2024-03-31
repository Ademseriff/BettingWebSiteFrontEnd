using BettingWebSiteBackEnd.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;
using System.Diagnostics;

namespace BettingWebSiteBackEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint publishEndpoint;

        public HomeController(ILogger<HomeController> logger,IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
        }

        public async Task<IActionResult> Index()
        {
            OrderGetEventRequest orderGetEventRequest = new OrderGetEventRequest()
            {
                OrderGetEventId = Guid.NewGuid().ToString(),
            };
           await publishEndpoint.Publish(orderGetEventRequest);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
