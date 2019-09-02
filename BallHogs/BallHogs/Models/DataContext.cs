using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<BHTeam> BHTeams { get; set; }
        public virtual DbSet<ManagerTeam> ManagerTeams { get; set; }
        public virtual DbSet<PlayersOnTeams> PlayersOnTeams { get; set; }

        internal Task GetUserIdAsync(ClaimsPrincipal user)
        {
          throw new NotImplementedException();
        }
    }
}
