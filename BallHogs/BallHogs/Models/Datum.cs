using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class Datum
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public int? Height_feet { get; set; }
        public int? Height_inches { get; set; }
        public string Last_name { get; set; }
        public string Position { get; set; }
        public Team Team { get; set; }
        public int? Weight_pounds { get; set; }
    }
}
