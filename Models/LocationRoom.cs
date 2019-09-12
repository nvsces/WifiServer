using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShServer.Models
{
    public class LocationRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
    }
}
