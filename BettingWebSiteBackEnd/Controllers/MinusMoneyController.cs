using BettingWebSiteBackEnd.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace BettingWebSiteBackEnd.Controllers
{
    public class MinusMoneyController : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;

        public MinusMoneyController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(MoneyDecreaseEvent x)
        {
            x.WhiceSide = Shared.Enums.MoneyTransactionEnum.minus;


            MoneyDecreaseEvent DecreaseEvent = x;
            await publishEndpoint.Publish(DecreaseEvent);
            return View();
        }
    }
}
