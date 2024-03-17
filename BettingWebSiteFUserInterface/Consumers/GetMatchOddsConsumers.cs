
using BettingWebSiteFUserInterface.ViewModels;
using MassTransit;

using Shared.Events;

namespace MatchOddsApi.Consumers
{
    public class GetMatchOddsConsumers : IConsumer<MatchOddsEvent>
    {
        public static MatchOddsEvent MatchOddsEvent = new MatchOddsEvent();
        public static List<MatchOddsVm> matchOddsVmList = new List<MatchOddsVm>();
        private readonly IPublishEndpoint publishEndpoint;

        public GetMatchOddsConsumers(IPublishEndpoint publishEndpoint)
        {
           

            this.publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<MatchOddsEvent> context)
        {
            MatchOddsEvent = context.Message;
            //03:00 NİK Diriangen FC Masachapa FC 3 1.00 7.02 17.20 + 42
            matchOddsVmList.Clear();

            foreach (var x in MatchOddsEvent.MatchOddsEvents) {
                string[] ayristirilmisMetinler = x.Datas.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                if(ayristirilmisMetinler.Length == 9)
                {
                    matchOddsVmList.Add(new MatchOddsVm()
                    {
                        MatchTime = ayristirilmisMetinler[0],
                        League = ayristirilmisMetinler[1],
                        Team1 = ayristirilmisMetinler[2],
                        Team2 = ayristirilmisMetinler[3],
                        Ms1 = ayristirilmisMetinler[5],
                        Ms0 = ayristirilmisMetinler[6],
                        Ms2 = ayristirilmisMetinler[7]
                    });
                }
                else if (ayristirilmisMetinler.Length == 10)
                {
                    matchOddsVmList.Add(new MatchOddsVm()
                    {
                        MatchTime = ayristirilmisMetinler[0],
                        League = ayristirilmisMetinler[1],
                        Team1 = ayristirilmisMetinler[2],
                        Team2 = ayristirilmisMetinler[3],
                        Ms1 = ayristirilmisMetinler[6],
                        Ms0 = ayristirilmisMetinler[7],
                        Ms2 = ayristirilmisMetinler[8]
                    });
                }
             
            }
            
        }
    }
}
