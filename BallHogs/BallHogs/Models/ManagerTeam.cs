using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class ManagerTeam
    {
        public int ManagerTeamId { get; set; }
        public int ManagerId { get; set; }
        public Manager Manager{ get; set; }
        public int BHTeamId { get; set; }
        public BHTeam BHTeam { get; set; }

    }
}
