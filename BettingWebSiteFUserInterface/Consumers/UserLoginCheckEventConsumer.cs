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
        }
    }
}
