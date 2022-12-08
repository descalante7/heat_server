using System;
using System.Security.Cryptography.Xml;

namespace heat_server.Models
{
    public partial class ScoutingReportsResponse
    {
        public int TeamKey { get; set; }
        public string TeamNickName { get; set; }
        public string Conference { get; set; }
        public PlayerData[] PlayerData { get; set; }
    }

    public partial class PlayerData
    {
        public string PlayerKey { get; set; }
        public string PlayerName { get; set; }
        public string BirthDate { get; set; }
        public ReportData[] Report {get; set;} 

    }

    public partial class ReportData
    {
        public int ScoutKey { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Comments { get; set; }
        public int Assist { get; set; }
        public int Defense { get; set; }
        public int Rebound { get; set; }
        public int Shooting { get; set; }
    }
}
