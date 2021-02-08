using System;
using System.Collections.Generic;

namespace prid_2021_002.Models
{
    public class BoardDTO
    {
        public int BoardId { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string PicturePath {get; set; }
        public UserDTO Author { get; set; }
        public int AuthorId { get; set; }
        public IEnumerable<UserDTO> Collaborators { get; set; }
        public IList<ListDTO> Lists { get; set; }
    }

    public class UserBoardDTO
    {
        public int BoardId { get; set; }
        public string Title { get; set; }
        public DateTime Timestamp { get; set; }
        public string PicturePath {get; set; }
        public int AuthorId { get; set; }
    }
}