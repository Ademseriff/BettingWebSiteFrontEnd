
using BettingWebSiteFUserInterface.ViewModels;
using MassTransit;
using MatchOddsApi.Consumers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;
using System.Diagnostics;

namespace BettingWebSiteFUserInterface.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint publishEndpoint;

        public HomeController(ILogger<HomeController> logger,IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            this.publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await publishEndpoint.Publish(new MatchOddsEventRequest() { });
            await Task.Delay(TimeSpan.FromSeconds(12));

            ViewBag.matchodds = GetMatchOddsConsumers.matchOddsVmList;
            return View();

            //return View(GetMatchOddsConsumers.matchOddsVmList);
        }
        [HttpPost]
        public async Task<IActionResult> Index(MatchOddsVm matchOddsVm)
        {
           



            return View();
        }
        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Privacy(CustomerAddedEvent customerAddedEvent)
        {
            await publishEndpoint.Publish(customerAddedEvent);
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
