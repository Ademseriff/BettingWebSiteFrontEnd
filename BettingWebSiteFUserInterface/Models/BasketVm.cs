using Shared.Enums;
using System.Security.Principal;

namespace BettingWebSiteFUserInterface.Models
{
    public class BasketVm
    {
        public string Tc { get; set; }
        public string Team1 { get; set; }

        public string Team2 { get; set; }

        public MatchSideEnum MatchSide { get; set; }

        public string Rate { get; set; }

        public float TotalRate { get; set; }
    }
}
