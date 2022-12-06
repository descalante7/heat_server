using System;
using System.Collections.Generic;

namespace heat_server.Models
{
    public partial class ScoutingReport
    {
        public int ReportKey { get; set; }
        public int ScoutKey { get; set; }
        public int PlayerKey { get; set; }
        public int TeamKey { get; set; }
        public int? Defense { get; set; }
        public int? Rebound { get; set; }
        public int? Shooting { get; set; }
        public int? Assist { get; set; }
        public string Comments { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? ModifiedDateTime { get; set; }
    }
}
