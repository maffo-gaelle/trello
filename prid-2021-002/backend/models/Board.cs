using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace prid_2021_002.Models {

    public class Board {
        [Key]
        public int BoardId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public string PicturePath { get; set; }

        public virtual User Author { get; set;}
        public int AuthorId { get; set; }

        public virtual IList<List> Lists { get; set; } = new List<List>();

        public virtual IList<UserBoard> BoardUsers { get; set; } = new List<UserBoard>();

        [NotMapped]
        public IEnumerable<User> Collaborators {
            get => BoardUsers.Select(ub => ub.User);
        }
   
    }
}