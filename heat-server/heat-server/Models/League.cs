using System;
using System.Collections.Generic;

namespace heat_server.Models
{
    public partial class League
    {
        public League()
        {
            TeamLeagueKeyDomesticNavigation = new HashSet<Team>();
            TeamLeagueKeyNavigation = new HashSet<Team>();
        }

        public int LeagueKey { get; set; }
        public string LeagueName { get; set; }
        public string Country { get; set; }
        public bool? ActiveSource { get; set; }
        public int? LeagueGroupKey { get; set; }
        public int? LeagueCustomGroupKey { get; set; }
        public bool? SearchDisplayFlag { get; set; }

        public virtual ICollection<Team> TeamLeagueKeyDomesticNavigation { get; set; }
        public virtual ICollection<Team> TeamLeagueKeyNavigation { get; set; }
    }
}
