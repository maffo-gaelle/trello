using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;

namespace prid_2021_002.Models
{
    public class Card: IValidatableObject
    {
        [Key]
        public int CardId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Title { get; set; }

        public string Description { get; set; }

        public int Position { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;

        public virtual User Author { get; set; }
        public int AuthorId { get; set; }

        public virtual List List { get; set; }
        public int ListId { get; set; }

        public virtual IList<UserCard> CardUsers { get; set; } = new List<UserCard>();
        
        public IEnumerable<User> Collaborators {
            get => CardUsers.Select(uc => uc.User);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var tab = List.Board;

            foreach(var l in tab.Lists) {

                var cards = List.Cards;
                foreach(var t in cards) {
                    if(t.Title == Title) {
                        yield return new ValidationResult("This card already exists", new[] { nameof(Title) });
                    }
                }
            }
        }
    }
}