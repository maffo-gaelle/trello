using System;
using System.Collections.Generic;

namespace prid_2021_002.Models
{
    public class ListDTO
    {
        public int ListId { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public UserBoardDTO Board { get; set; }
        public int BoardId { get; set; }
        public IList<CardDTO> Cards {get; set; }
        public IEnumerable<UserDTO> Collaborators { get; set; }
    }

    public class ListDTOU
    {
        public int ListId { get; set; }
        public string Title { get; set; }
        public int Position { get; set; }
        public int BoardId { get; set; }
    }
}