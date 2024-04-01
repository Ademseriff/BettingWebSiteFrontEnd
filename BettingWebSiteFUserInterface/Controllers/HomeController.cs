
using BettingWebSiteFUserInterface.Areas.Auth.Controllers;
using BettingWebSiteFUserInterface.Consumers;
using BettingWebSiteFUserInterface.Models;
using BettingWebSiteFUserInterface.ViewModels;
using MassTransit;
using MatchOddsApi.Consumers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Events;
using System.Diagnostics;
using System.Globalization;

namespace BettingWebSiteFUserInterface.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPublishEndpoint publishEndpoint;
        public static float TotalRateStatic = 1;
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
            float TotalRate = 1;
            List<BasketVm> basketVms = new();
            if(BasketItemGetResponseEventConsumer.basketItemGetResponseEventstatic.messages != null)
            {
                foreach (var a in BasketItemGetResponseEventConsumer.basketItemGetResponseEventstatic.messages)
                {
                    float rate;
                    if (float.TryParse(a.Rate, NumberStyles.Any, CultureInfo.InvariantCulture, out rate))
                    {
                        TotalRate *= rate;
                    }
                    else
                    {
                        // Rate bir string olarak dönüştürülemediğinde yapılacak işlem
                        // Hata işleme veya loglama gibi bir işlem yapılabilir.
                    }
                }

                TotalRateStatic = TotalRate;
                basketVms = BasketItemGetResponseEventConsumer.basketItemGetResponseEventstatic.messages.Select(s => new BasketVm()
                {

                    Rate = s.Rate,
                    MatchSide = s.MatchSide,
                    Tc = s.Tc,
                    Team1 = s.Team1,
                    Team2 = s.Team2,
                    TotalRate = TotalRate
                }).ToList();
            }
            else
            {
                basketVms.Add(new BasketVm()
                {
                    Rate ="0",
                    TotalRate =0,
                    MatchSide =Shared.Enums.MatchSideEnum.ms0,
                    Tc ="0",
                    Team1 = "0",
                    Team2 = "0"
                });
            }

            return View(basketVms);

           
        }

        [HttpPost]
        public async Task<IActionResult> Sepet(string basketVMJson,string Tutar,string TotalMoney,string TotalRate)
        {
            
            List<BasketVm> basketVM = JsonConvert.DeserializeObject<List<BasketVm>>(basketVMJson);
            if(basketVM != null)
            {
                OrderComplatedEvent orderComplatedEvent = new()
                {
                 PaidMoney = Tutar,
                 TotalMoney = TotalMoney,
                 TotalRate=TotalRate,
                  state = Shared.Enums.StateEnum.suspend, 
                 contentLists = basketVM.Select(oi => new Shared.Messages.OrderComplatedEventMessage()
                 {
                     Tc = oi.Tc,
                     MatchSide =oi.MatchSide,
                     Team1 =oi.Team1,
                     Team2 = oi.Team2
                 }).ToList()
                };
                MoneyDecreaseEvent moneyDecreaseEvent = new()
                {
                    Tc = Auth1Controller.userLoginCheckEventstatic.Tc,
                    Money = Tutar,
                    WhiceSide = Shared.Enums.MoneyTransactionEnum.minus
                };
                BasketClearEvent basketClearEvent = new()
                {
                    Tc = basketVM[0].Tc,
                };
              
                await publishEndpoint.Publish(moneyDecreaseEvent);
                await publishEndpoint.Publish(orderComplatedEvent);
                await publishEndpoint.Publish(basketClearEvent);
                return RedirectToAction("LoadPay", "Home", new { Area = "" });
            }


            return RedirectToAction("LoadPay", "Home", new { Area = "" });
        }

        [HttpPost]
        public async Task<IActionResult> BasketClear(string Tc)
        {
            if (Tc != null)
            {
                BasketClearEvent basketClearEvent = new()
                {
                    Tc = Tc,
                };
              await  publishEndpoint.Publish(basketClearEvent);
            }
            return RedirectToAction("LoadPay", "Home", new { Area = "" });
        }

        [HttpGet]
        public IActionResult LoadPay()
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
