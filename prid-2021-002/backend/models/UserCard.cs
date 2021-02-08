using prid_2021_002.Models;

namespace prid_2021_002 {
    public class UserCard {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CardId { get; set; }
        public virtual Card Card { get; set; }
    }
}