using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prid_2021_002.Models
{
    //Ici on transforme un user en userDTO c'est ce qui fait le lien entre la classe user et la classe userDTO on utilise un suerDTO dans lequel on met les données d'un user
    public static class DTOMappers
    {
        public static UserDTO ToDTO(this User user)
        {
            return new UserDTO
            {
                UserId = user.UserId,
                Pseudo = user.Pseudo,
                // we don't put the password in the DTO for security reasons
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                PicturePath = user.PicturePath,
                Role = user.Role,
                Boards = user.Boards.ToDTOU(),
                Cards = user.Cards.ToDTO(),
                CollaboratorsTeams = user.CollaboratorsTeams.ToDTO(),
                CollaboratorsBoards = user.CollaboratorsBoards.ToDTOU(),
                CollaboratorsCards = user.CollaboratorsCards.ToDTO(),
                CollaboratorsLists = user.CollaboratorsLists.ToDTO()
            };
        }

        public static UserDTOU ToDTOU(this User user)
        {
            return new UserDTOU
            {
                UserId = user.UserId,
                Pseudo = user.Pseudo,
                // we don't put the password in the DTO for security reasons
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                PicturePath = user.PicturePath,
                Role = user.Role,
            };
        }

        public static TeamDTO ToDTO(this Team team)
        {
            return new TeamDTO
            {
                TeamId=team.TeamId,
                Teamname=team.Teamname
            };
        }

        public static List<TeamDTO> ToDTO(this IEnumerable<Team> team)
        {
            return team.Select(t => t.ToDTO()).ToList();
        }
         
        //transforme une instance de User en une instance de UserDTO(création d'un objet UserDTO)
        public static List<UserDTO> ToDTO(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDTO()).ToList();
        }

        public static List<UserDTOU> ToDTOU(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDTOU()).ToList();
        }

        public static BoardDTO ToDTO(this Board board)
        {
            return new BoardDTO
            {
                BoardId = board.BoardId,
                Title = board.Title,
                Timestamp = board.Timestamp,
                PicturePath = board.PicturePath,
                Author = board.Author.ToDTO(),
                AuthorId = board.AuthorId,
                Collaborators = board.Collaborators.ToDTO(), 
                Lists = board.Lists.ToDTO()
            };
        }

        public static UserBoardDTO ToDTOU(this Board board)
        {
            return new UserBoardDTO
            {
                BoardId = board.BoardId,
                Title = board.Title,
                Timestamp = board.Timestamp,
                PicturePath = board.PicturePath,
                AuthorId = board.AuthorId,
            };
        }

        public static List<BoardDTO> ToDTO(this IEnumerable<Board> boards)
        {
            return boards.Select(b => b.ToDTO()).ToList();
        }

        public static List<UserBoardDTO> ToDTOU(this IEnumerable<Board> boards)
        {
            return boards.Select(b => b.ToDTOU()).ToList();
        }

        public static ListDTO ToDTO(this List list)
        {
            return new ListDTO
            {
                ListId = list.ListId,
                Title = list.Title,
                Position = list.Position,
                Board = list.Board.ToDTOU(),
                BoardId = list.BoardId,
                Cards = list.Cards.ToDTO(),
            };
        }

        public static ListDTO ToDTOU(this List list)
        {
            return new ListDTO
            {
                ListId = list.ListId,
                Title = list.Title,
                Position = list.Position,
                BoardId = list.BoardId,
            };
        }

        public static List<ListDTO> ToDTO(this IEnumerable<List> list)
        {
            return list.Select(l => l.ToDTO()).ToList();
        }

        public static CardDTO ToDTO(this Card card)
        {
            return new CardDTO
            {
                CardId = card.CardId,
                Title = card.Title,
                Description = card.Description,
                Position = card.Position,
                Timestamp = card.Timestamp,
                Collaborators = card.Collaborators.ToDTOU()
            };
        }
        public static CardDTOU ToDTOU(this Card card)
        {
            return new CardDTOU
            {
                CardId = card.CardId,
                Title = card.Title,
                Description = card.Description,
                Position = card.Position,
                Timestamp = card.Timestamp,
                ListId = card.ListId,
                AuthorId = card.AuthorId,
                
            };
        }

        public static List<CardDTO> ToDTO(this IEnumerable<Card> cards)
        {
            return cards.Select(c => c.ToDTO()).ToList();
        }
    }
}