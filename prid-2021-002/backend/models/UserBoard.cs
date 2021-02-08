namespace prid_2021_002.Models {
    public class UserBoard {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int BoardId { get; set; }
        public virtual Board Board { get; set; }
    }
}