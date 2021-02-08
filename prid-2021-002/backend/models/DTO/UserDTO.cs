using System;
using System.Collections.Generic;

namespace prid_2021_002.Models
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Email {get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PicturePath {get; set; }
        public Role Role { get; set; }
        public virtual IList<UserBoardDTO> Boards { get; set; }
        public virtual IEnumerable<TeamDTO> CollaboratorsTeams { get; set; }
        public virtual IEnumerable<UserTeamDTO> Teams { get; set; }
        public virtual IEnumerable<UserBoardDTO> CollaboratorsBoards { get; set; }
        public virtual IEnumerable<CardDTO> CollaboratorsCards { get; set; }
        public virtual IEnumerable<ListDTO> CollaboratorsLists { get; set; }
        public virtual IList<CardDTO> Cards { get; set;}
    }


    public class UserDTOU
    {
        public int UserId { get; set; }
        public string Pseudo { get; set; }
        public string Password { get; set; }
        public string Email {get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string PicturePath {get; set; }
        public Role Role { get; set; }
    }
    /**
    *  Le but de cette classe est de représenter les données d'un user telles qu'elles vont être échangées entre le front-end et le back-end donc
    * quand les données d'un membre devra être envoyé au frontend, ce user sera serialisé sous la forme d'un string json avant d'être envoyé et quand le 
    * le frontend enverra un string json, celui ci sera serialisé en UserDTO
    */
}