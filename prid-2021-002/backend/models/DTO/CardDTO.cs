using System;
using System.Collections.Generic;

namespace prid_2021_002.Models
{
    public class CardDTO
    {
        public int CardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public DateTime Timestamp { get; set;}
        public UserDTO Author {get; set; }
        public int AuthorId {get; set; }
        public virtual ListDTO List { get; set; }
        public int ListId { get; set; }
        public IEnumerable<UserDTOU> Collaborators { get; set; }
    }

    public class CardDTOU
    {
        public int CardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Position { get; set; }
        public DateTime Timestamp { get; set;}
        public int AuthorId {get; set; }
        public int ListId { get; set; }
    }
}