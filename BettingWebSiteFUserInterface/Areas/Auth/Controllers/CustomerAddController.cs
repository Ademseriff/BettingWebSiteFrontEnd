using BettingWebSiteFUserInterface.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Events;

namespace BettingWebSiteFUserInterface.Areas.Auth.Controllers
{
    [Area("Auth")]
    public class CustomerAddController : Controller
    {
        private readonly IPublishEndpoint publishEndpoint;

        public CustomerAddController(IPublishEndpoint publishEndpoint)
        {
            this.publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CustomerAdd customerAdd)
        {
            if (customerAdd != null)
            {
                await publishEndpoint.Publish(new CustomerAddedEvent()
                {
                    HumanIdentity = customerAdd.HumanIdentity,
                    Name = customerAdd.Name,
                    Password = customerAdd.Password,
                    Surname = customerAdd.Surname,
                });
            }
            return View();
        }
    }
}
