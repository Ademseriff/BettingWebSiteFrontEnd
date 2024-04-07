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
            else {
                orderGetEventResponsecshtml.OrderGetEventId = "boş ";
                orderGetEventResponsecshtml.coupunContents.Add(new() 
                { PaidMoney = "0",
                state= Shared.Enums.StateEnum.failed,
               TotalMoney="0",
               TotalRate="0",
               orderContentContents = new() { new()
               {
                    Tc ="0",
                    Team1 = "0",
                    Team2 = "0",
                    MatchSide = Shared.Enums.MatchSideEnum.ms0
               } }
                }  
                );
            }
           
            return View(orderGetEventResponsecshtml);
        }
        [HttpPost]
        public async Task<IActionResult> CouponsOkey(string Tc,string TotalMoney,string TotalRate)
        {
            MoneyIncreaseEvent moneyIncreaseEvent = new()
            {
                Money = TotalMoney,
                Tc = Tc,
                WhiceSide = Shared.Enums.MoneyTransactionEnum.plus
            };
            CouponComplatedEvent couponComplatedEvent = new() {
            Tc = Tc,
            TotalMoney = TotalMoney,
            TotalRate = TotalRate
            };
            MailGetEventRequest mailGetEventRequest = new()
            {
                Tc = Tc
            };
            await publishEndpoint.Publish(mailGetEventRequest);
            await Task.Delay(6000);
            MailSentEvent mailSentEvent = new()
            {
                EMail = MailGetEventResponseConsumer.Email,
                Price = int.Parse(TotalMoney),
                State = Shared.Enums.MailEnum.MoneyAdd
            };
            await publishEndpoint.Publish(mailSentEvent);
            await publishEndpoint.Publish(moneyIncreaseEvent);
            await publishEndpoint.Publish(couponComplatedEvent);
            return RedirectToAction("Index", "Home", new { Area = "" });
        }
        
        [HttpPost]
        public async Task<IActionResult> CouponsDenied(string Tc, string TotalMoney, string TotalRate)
        {
            CouponFailedEvent couponFailedEvent = new()
            {
                Tc=Tc,
                TotalMoney=TotalMoney,
                TotalRate=TotalRate
            };
            await publishEndpoint.Publish(couponFailedEvent);

            return RedirectToAction("Index", "Home", new { Area = "" });
        }
    }
}
