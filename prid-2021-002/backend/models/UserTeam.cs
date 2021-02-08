using prid_2021_002.Models;

namespace prid_2021_002
{
    public class UserTeam
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
    }
}