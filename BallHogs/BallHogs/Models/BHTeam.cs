using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class BHTeam
    {
        public int BHTeamId { get; set; }
        public string TeamName { get; set; }
        public string ManagerName { get; set; }


        public ICollection<Datum> Players { get; set; }

    }
}
