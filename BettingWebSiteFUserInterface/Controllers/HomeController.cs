
using BettingWebSiteFUserInterface.Areas.Auth.Controllers;
using BettingWebSiteFUserInterface.Consumers;
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
            BasketAddedEvent basketAddedEvent = new BasketAddedEvent();

            if (matchOddsVm.Ms0 != null)
            {
                basketAddedEvent.Tc = Auth1Controller.userLoginCheckEventstatic.Tc;
                basketAddedEvent.Team1 = matchOddsVm.Team1;
                basketAddedEvent.Team2 = matchOddsVm.Team2;
                basketAddedEvent.MatchSide = (Shared.Enums.MatchSideEnum)0;
                basketAddedEvent.Rate = matchOddsVm.Ms0;
                await publishEndpoint.Publish(basketAddedEvent);
            }
            else if (matchOddsVm.Ms1 != null)
            {
                basketAddedEvent.Tc = Auth1Controller.userLoginCheckEventstatic.Tc;
                basketAddedEvent.Team1 = matchOddsVm.Team1;
                basketAddedEvent.Team2 = matchOddsVm.Team2;
                basketAddedEvent.MatchSide = (Shared.Enums.MatchSideEnum)1;
                basketAddedEvent.Rate = matchOddsVm.Ms1;
                await publishEndpoint.Publish(basketAddedEvent);
            }
            else if (matchOddsVm.Ms2 != null)
            {
                basketAddedEvent.Tc = Auth1Controller.userLoginCheckEventstatic.Tc;
                basketAddedEvent.Team1 = matchOddsVm.Team1;
                basketAddedEvent.Team2 = matchOddsVm.Team2;
                basketAddedEvent.MatchSide = (Shared.Enums.MatchSideEnum)2;
                basketAddedEvent.Rate = matchOddsVm.Ms2;
                await publishEndpoint.Publish(basketAddedEvent);
            }


            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        [HttpGet]
        public async Task<IActionResult> Sepet()
        {
             string tcc = Auth1Controller.userLoginCheckEventstatic.Tc;

            if (tcc != null)
            {
                BasketItemGetRepuestEvent basketItemGetEvent = new()
                {
                    tc = tcc
                };
                await publishEndpoint.Publish(basketItemGetEvent);
            }

            await  Task.Delay(6000);


            ViewBag.BasketItemGetResponseEventConsumer = BasketItemGetResponseEventConsumer.basketItemGetResponseEventstatic;

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
