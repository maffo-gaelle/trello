using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace prid_2021_002.Models
{
    public class Team : IValidatableObject
    {
        [Key]
        public int TeamId { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Teamname { get; set; }

        public virtual IList<UserTeam> TeamUsers { get; set; } = new List<UserTeam>();

        public IEnumerable<User> Collaborators {
            get => TeamUsers.Select(ut => ut.User);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var currContext = validationContext.GetService(typeof(PridContext)) as PridContext;
            Debug.Assert(currContext != null); 
            
            var team = (from t in currContext.Teams
                        where t.Teamname == Teamname
                        select t).FirstOrDefault();

            if(team !=null) {
                yield return new ValidationResult("This team already exists", new[] { nameof(Teamname) });
            }
        }
    }
}