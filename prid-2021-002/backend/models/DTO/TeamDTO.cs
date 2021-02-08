using System;
using System.Collections.Generic;

namespace prid_2021_002.Models
{
    public class TeamDTO
    {
        public int TeamId { get; set; }
        public string Teamname { get; set; }
        public IEnumerable<UserDTO> Collaborators { get; set; }
    }

    public class UserTeamDTO
    {
        public int TeamId { get; set; }
        public string Teamname { get; set; }
        public DateTime Timestamp { get; set; }
        public string PicturePath {get; set; }
    }
}