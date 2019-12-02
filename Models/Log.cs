using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShServer.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string SSID { get; set; }
        public string BSSID { get; set; }
        public double AvgLevel { get; set; }
        public double number_of_Mentions { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public virtual LocationRoom LocRomm { get; set; }
        
    }
}
