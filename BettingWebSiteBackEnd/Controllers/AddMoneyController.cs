using MassTransit;
using MassTransit.Testing;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace BettingWebSiteBackEnd.Controllers
{
    public class AddMoneyController : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;

        public AddMoneyController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(MoneyIncreaseEvent x)
        {
            x.WhiceSide = Shared.Enums.MoneyTransactionEnum.plus;
            MoneyIncreaseEvent moneyIncreaseEvent = x;
             await  publishEndpoint.Publish(moneyIncreaseEvent);
            return View();
        }
    }
}
