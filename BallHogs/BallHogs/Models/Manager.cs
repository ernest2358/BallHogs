using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class Manager
    {
        public int ManagerID { get; set; }
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public ICollection<BHTeam> BHTeams { get; set; }

        public Manager(string userName)
        {
            UserName = userName;
            Wins = 0;
            Losses = 0;
        }
    }
}
