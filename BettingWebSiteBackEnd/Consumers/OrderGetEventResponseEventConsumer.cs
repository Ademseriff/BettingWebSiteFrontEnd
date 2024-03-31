using MassTransit;
using Shared.Events;

namespace BettingWebSiteBackEnd.Consumers
{
    public class OrderGetEventResponseEventConsumer : IConsumer<OrderGetEventResponseEvent>
    {
        public static OrderGetEventResponseEvent orderGetEventResponseEvent = new OrderGetEventResponseEvent();
        public async Task Consume(ConsumeContext<OrderGetEventResponseEvent> context)
        {
            orderGetEventResponseEvent = null;
            orderGetEventResponseEvent = context.Message;
           
        }
    }
}
