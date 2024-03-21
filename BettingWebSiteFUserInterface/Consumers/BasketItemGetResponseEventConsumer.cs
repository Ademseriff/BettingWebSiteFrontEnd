using MassTransit;
using Shared.Events;
using System.Diagnostics;

namespace BettingWebSiteFUserInterface.Consumers
{
    public class BasketItemGetResponseEventConsumer : IConsumer<BasketItemGetResponseEvent>
    {
        public static BasketItemGetResponseEvent basketItemGetResponseEventstatic = new BasketItemGetResponseEvent();

        public async Task Consume(ConsumeContext<BasketItemGetResponseEvent> context)
        {
            if (context.Message != null && context.Message.messages != null)
            {
                basketItemGetResponseEventstatic.Id = context.Message.Id;
                basketItemGetResponseEventstatic.messages = context.Message.messages;
            }
            else
            {
                // Handle the case when context.Message or context.Message.messages is null
                // For example, log an error or perform some default action.
            }
        }
    }
}
