using prid_2021_002.Models;

namespace prid_2021_002
{
    public class UserList
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ListId { get; set; }
        public virtual List List { get; set; }
    }
}