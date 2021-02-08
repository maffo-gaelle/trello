using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace prid_2021_002.Models
{
    public class List : IValidatableObject
    {
        [Key]
        public int ListId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        public int Position { get; set; }

        public virtual Board Board { get; set; }
        public int BoardId { get; set; }

        public virtual IList<Card> Cards { get; set; } = new List<Card>();
        
        public virtual IList<UserList> ListUsers { get; set; } = new List<UserList>();

        public IEnumerable<User> Collaborators {
            get => ListUsers.Select(ul => ul.User);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currContext = validationContext.GetService(typeof(PridContext)) as PridContext;
            Debug.Assert(currContext != null); 
            

            var board = (from b in currContext.Boards 
                        where b.BoardId == BoardId 
                        select b).FirstOrDefault();
            Console.WriteLine(board.Title);

            var lists = board.Lists;
            //ici le l.title existe déja parce que c'est déjà enregistré comme liste
            foreach(var l in lists) {
                Console.WriteLine(l.Title);
                if(l.Title == Title) {
                    yield return new ValidationResult("This list already exists", new[] { nameof(Title) });
                }
            }
        }
    }
}