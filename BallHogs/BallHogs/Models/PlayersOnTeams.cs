using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class PlayersOnTeams
    {
        public int PlayersOnTeamsId { get; set; }
        public int DatumId { get; set; }
        public Datum Datum { get; set; }
        public int BHTeamId { get; set; }
        public BHTeam BHTeam { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int Year { get; set; }
        public float PPG { get; set; }
        public float Steals { get; set; }
        public float Rebounds { get; set; }


    }
}
