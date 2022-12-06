using System;
using System.Collections.Generic;

namespace heat_server.Models
{
    public partial class Scout
    {
        public int ScoutKey { get; set; }
        public int TeamKey { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActiveFlag { get; set; }
    }
}
