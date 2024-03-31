using BettingWebSiteBackEnd.Consumers;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace BettingWebSiteBackEnd.Controllers
{
    public class CouponsController : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;
       
        public CouponsController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        public IActionResult Index()
        {
            OrderGetEventResponseEvent orderGetEventResponsecshtml = new();
            if (OrderGetEventResponseEventConsumer.orderGetEventResponseEvent != null) {
               orderGetEventResponsecshtml = OrderGetEventResponseEventConsumer.orderGetEventResponseEvent; }
           
            return View(orderGetEventResponsecshtml);
        }
    }
}
