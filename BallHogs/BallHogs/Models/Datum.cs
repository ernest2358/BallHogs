using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BallHogs.Models
{
    public class Datum
    {
        public int Id { get; set; }

        [Display(Name = "Player First Name")]
        public string First_name { get; set; }

        public int? Height_feet { get; set; }

        public int? Height_inches { get; set; }

        [Display(Name = "Player Last Name")]
        public string Last_name { get; set; }

        [Display(Name = "Position")]
        public string Position { get; set; }

        public Team Team { get; set; }

        public int? Weight_pounds { get; set; }

    }
}
