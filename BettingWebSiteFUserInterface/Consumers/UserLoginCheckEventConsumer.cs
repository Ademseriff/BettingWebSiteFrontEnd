using BettingWebSiteFUserInterface.Areas.Auth.Controllers;
using MassTransit;
using Shared.Events;

namespace BettingWebSiteFUserInterface.Consumers
{
    public class UserLoginCheckEventConsumer : IConsumer<UserLoginCheckEventResponse>
    {
        public static bool IsValid = false;
        public async Task Consume(ConsumeContext<UserLoginCheckEventResponse> context)
        {
            IsValid = context.Message.IsValid;
            if(context.Message.TotalPrice == null)
            {
                Auth1Controller.userLoginCheckEventstatic.TotalPrice = "0";
            }
            else if(context.Message.TotalPrice != null)
            {
                Auth1Controller.userLoginCheckEventstatic.TotalPrice = context.Message.TotalPrice;
            }
           
        }
    }
}
