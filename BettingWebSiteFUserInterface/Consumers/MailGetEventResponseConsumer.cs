using MassTransit;
using Shared.Events;

namespace BettingWebSiteFUserInterface.Consumers
{
    public class MailGetEventResponseConsumer : IConsumer<MailGetEventResponse>
    {
        public static string Email;
        public async Task Consume(ConsumeContext<MailGetEventResponse> context)
        {
            Email = context.Message.Email;
        }
    }
}
