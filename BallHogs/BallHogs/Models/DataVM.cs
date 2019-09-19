using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class DataVM
    {
        public int BHTeamId { get; set; }
        public string TeamName { get; set; }
        public string ManagerName { get; set; }
        public BHTeam BHTeam { get; set; }
        public List<PlayersOnTeams> Players { get; set; }
        public List<Datum> HouseholdPlayers { get; set; }
    }
}
