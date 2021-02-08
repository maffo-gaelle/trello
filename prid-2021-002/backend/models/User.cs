using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel.DataAnnotations.Schema;

namespace prid_2021_002.Models {
    public enum Role{
        Admin = 2, Owner = 1, Member = 0
    }

    public class User : IValidatableObject {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        [MaxLength(10, ErrorMessage = "Maximum 3 characters")]
        [RegularExpression("^[A-Za-z][A-Za-z0-9_]{2,9}$")]
        public string Pseudo { get; set; }

        [Required(ErrorMessage = "Required")]
        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        [MaxLength(10, ErrorMessage = "Maximum 3 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        [RegularExpression("^[A-Za-z0-9](([_\\.\\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\\.\\-]?[a-zA-Z0-9]+)*)\\.([A-Za-z]{2,})$")]
        public string Email { get; set; }

        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        [MaxLength(50, ErrorMessage = "Maximum 3 characters")]
        public string FirstName { get; set; }

        [MinLength(3, ErrorMessage = "Minimum 3 characters")]
        [MaxLength(50, ErrorMessage = "Maximum 3 characters")]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string PicturePath { get; set; }

        public Role Role {get; set; } = Role.Member;

        public virtual IList<Board> Boards { get; set; } = new List<Board>();
        
        public virtual IList<Card> Cards { get; set;} = new List<Card>();

        public virtual IList<UserBoard> UserBoards { get; set; } = new List<UserBoard>();

        [NotMapped]
        public IEnumerable<Board> CollaboratorsBoards {
            get => UserBoards.Select(ub => ub.Board);
        }

        public virtual IList<UserCard> UserCards { get; set; } = new List<UserCard>();

        public IEnumerable<Card> CollaboratorsCards {
            get => UserCards.Select(uc => uc.Card);
        }

        public virtual IList<UserList> UserLists { get; set; } = new List<UserList>();

        public IEnumerable<List> CollaboratorsLists {
            get => UserLists.Select(ul => ul.List);
        }

        public virtual IList<UserTeam> UserTeams { get; set; } = new List<UserTeam>();

        public IEnumerable<Team> CollaboratorsTeams {
            get => UserTeams.Select(ut => ut.Team);
        }

        [NotMapped]
        public IEnumerable<Team> Teams {
            get => UserTeams.Select(ub => ub.Team);
        }

        [NotMapped]
        public string Token { get; set; }

        public int? Age {
            get {
                if(!BirthDate.HasValue) {
                    return null;
                }
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Value.Year;
                if(BirthDate.Value.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
            //Ici on recupère le contexte actuel , le as context est pour transformer l'objet DbContext en un objet 
            //MsnContext pour avoir accès à tous les attributs de la classe MsnContext 
            var currContext = validationContext.GetService(typeof(PridContext)) as PridContext;
            Debug.Assert(currContext != null); 
            //ICi c'est pour que si il y'a une erreur que l'applivcation plante. 
            //L'erreur est que si currContext est null, que ça genère une erreur console
            // Debug.Assert(currContext != null);

            var user = (from u in currContext.Users 
                        where u.Pseudo == Pseudo 
                        select u).FirstOrDefault();

            var userEmail = (from u in currContext.Users 
                        where u.Email == Email 
                        select u).FirstOrDefault();
            
            Console.WriteLine("L'email dans la base de donnée " + userEmail);
            if (user != null)
                yield return new ValidationResult("This user already exists", new[] { nameof(Pseudo) });

            if (userEmail != null)
                yield return new ValidationResult("This Email already exists", new[] { nameof(Email) });
            
            if (BirthDate.HasValue && BirthDate.Value.Date > DateTime.Today)
                yield return new ValidationResult("Can't be born in the future in this reality", new[] { nameof(BirthDate) });
            else if (Age.HasValue && Age < 18)
                yield return new ValidationResult("Must be 18 years old", new[] { nameof(BirthDate) });

            if(FirstName != null && LastName == null)
                yield return new ValidationResult("The Lastname is required", new[] { nameof(LastName) });
                
            if(FirstName == null && LastName != null)
                yield return new ValidationResult("The Firstname is required", new[] { nameof(FirstName) });

                
        }
    }
}
