using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_2021_002.Helpers;
using prid_2021_002.Models;
using PRID_Framework;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;

namespace prid_2021_002.Controllers {
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase {
        private readonly PridContext _context;
        public CardsController cardsControlleur;

        public BoardsController(PridContext context) {
            _context = context;
            this.cardsControlleur = new CardsController(context);
            //Grâce à _context, le contrôleur pourra interagir avec le modèle et la bd à travers EF
        }

        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetCollection() {
            return (await _context.Boards.ToListAsync()).ToDTO();
        }

        [HttpGet("getBoards/{id}")]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetBoardsByUser(int id) {
            var user = await _context.Users.FindAsync(id);
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
           
            if(user != currentUser)
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action");

            return (user.Boards).ToDTO();
        }

        [HttpGet("getCollaboratorBoards/{id}")]
        public async Task<ActionResult<IEnumerable<BoardDTO>>> GetCollaboratorBoardsByUser(int id) {
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
            var user = await _context.Users.FindAsync(id);

            if(user != currentUser)
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action");

            return (user.CollaboratorsBoards).ToDTO();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BoardDTO>> Get(int id) {
            var board = await _context.Boards.FindAsync(id);
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);

            if(User.Identity.Name != board.Author.Pseudo && !User.IsInRole(Role.Admin.ToString()) && !board.Collaborators.Contains(currentUser)) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(board == null)
                return NotFound();

            return Ok(board.ToDTO());
        }

        [HttpPost]
        public async Task<ActionResult<UserBoardDTO>> Post(UserBoardDTO board) {
            var other = await _context.Boards.FindAsync(board.BoardId);
            if(other != null) {
                var err = new ValidationErrors().Add("This Id already in use", nameof(board.BoardId));
                return BadRequest(err);
            }
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);

            if(currentUser.UserId != board.AuthorId) 
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action");

            var newBoard = new Board(){
                Title =  board.Title,
                PicturePath = board.PicturePath,
                AuthorId = board.AuthorId
            };

            _context.Boards.Add(newBoard);

            var res = await _context.SaveChangesAsyncWithValidation();
            if(!res.IsEmpty) 
                return BadRequest(res);

            //permet de renvoyer un réponse ayant un statut HTTP 201 - Created
            return CreatedAtAction(nameof(Get), new { id = newBoard.BoardId }, newBoard.ToDTOU());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserBoardDTO boardDTO) {
            var Author = await _context.Users.FindAsync(boardDTO.AuthorId);
              
            if(User.Identity.Name != Author.Pseudo && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(id != boardDTO.BoardId)
                return BadRequest("mauvaise requetes des id");

            var board =await _context.Boards.FindAsync(id);

            if(board == null)
                return NotFound();
            board.Title = boardDTO.Title;
            board.PicturePath = boardDTO.PicturePath;
            
            var res = await _context.SaveChangesAsyncWithValidation();
            if(!res.IsEmpty)
                return BadRequest(res);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var board = await _context.Boards.FindAsync(id);
            Console.WriteLine(board.BoardId);
            Console.WriteLine(User.Identity.Name );
            if(User.Identity.Name != board.Author.Pseudo && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(board == null)
                return NotFound();
            await this.resetboard(board.BoardId);

            if(board.Collaborators != null) {
                foreach(var bU in board.BoardUsers) {
                    _context.UserBoards.Remove(bU);
                }
            }
            
            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("changeCardOfList/{boardId}/{cardId}/{oldlistId}/{newlistId}")]
        public async Task<ActionResult> ChangeCard(int boardId, int cardId, int oldlistId, int newlistId ) {

            var board = await _context.Boards.FindAsync(boardId);
            var oldList = await _context.Lists.FirstOrDefaultAsync(l => l.ListId == oldlistId && l.BoardId == boardId);
            var newList = await _context.Lists.FirstOrDefaultAsync(l => l.ListId == newlistId && l.BoardId == boardId);
            var card = await _context.Cards.FirstOrDefaultAsync(c => c.ListId == oldlistId && c.CardId == cardId);

            foreach(var c in oldList.Cards) {
                if(c.Position > card.Position) {
                    c.Position--;
                }
            }

            card.ListId = newlistId;
            card.Position = newList.Cards.Count;
            _context.SaveChanges();
            
            return Ok();
        }

        [HttpGet("getUsersNotCollaborators/{id}")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> getUsersNotCollaborators(int id) {
            
            var board = await _context.Boards.FindAsync(id);
              
            if(User.Identity.Name != board.Author.Pseudo && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            var users = await _context.Users.ToListAsync();
            users.Remove(board.Author);

            foreach(var u in board.Collaborators) {
                if(users.Contains(u)) {
                    users.Remove(u);
                }
            }
            
            return users.ToDTO();
        }

        [HttpGet("addOrRemoveCollaborators/{boardid}/{userid}/{add}")]
        public async Task<ActionResult<BoardDTO>> addOrRemoveCollaborators(int boardid, int userid, Boolean add) {
            
            Console.WriteLine("id user envoyé de deleteUser "+ userid);
            var board = await _context.Boards.FindAsync(boardid);
            var user = await _context.Users.FindAsync(userid);
            List<Card> authorCard = new List<Card>();

            var boardUser = new UserBoard();
            boardUser.Board = board;
            boardUser.User = user;
            if(add) {
                _context.UserBoards.Add(boardUser);
                _context.SaveChanges();

            } else {
                Console.WriteLine("nombre de liste envoyé de deleteUser "+ board.BoardId);
                if(board.Lists  != null) {
                    foreach(var l in board.Lists) {
                        if(l.Cards.Count() != 0) {
                            foreach(var c in l.Cards) {
                                if(c.Collaborators.Count() != 0) {
                                    foreach(var uc in c.CardUsers) {
                                        if(uc.User == user) {
                                            _context.UserCards.Remove(uc);
                                        }
                                    }
                                }
                                if(c.Author == user) {
                                    authorCard.Add(c);
                                }
                            }
                        }
                    }
                }
                foreach(var ac in authorCard) {
                    await this.cardsControlleur.Delete(ac.CardId);
                }
                _context.UserBoards.Remove(boardUser);
                _context.SaveChanges();
            }
            return board.ToDTO();
        }


        [HttpGet("addOrRemoveParticipants/{cardid}/{userid}/{add}")]
        public async Task<ActionResult> addOrRemoveParticipants(int cardid, int userid, Boolean add) {
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
            var card = await _context.Cards.FindAsync(cardid);
            var list = await _context.Lists.FindAsync(card.ListId);
            var board = await _context.Boards.FindAsync(list.BoardId);


            if(User.Identity.Name != board.Author.Pseudo && !User.IsInRole(Role.Admin.ToString()) && !board.Collaborators.Contains(currentUser)) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            var user = await _context.Users.FindAsync(userid);
            
            var cardUser = new UserCard();
            cardUser.Card = card;
            cardUser.User = user;
            
            if(add) {
                if(card.Collaborators.Contains(user)) 
                    return BadRequest("Ce collaborateur participe déjà sur cette carte");

                _context.UserCards.Add(cardUser);
                _context.SaveChanges();
            } else {
                // if(!card.Collaborators.Contains(user)) 
                //     return NotFound("Collaborateur non trouvé");
                _context.UserCards.Remove(cardUser);
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpGet("reset/{id}")]
        public async Task<ActionResult> resetboard(int id) {
            var board = await _context.Boards.FindAsync(id);
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);

            if(User.Identity.Name != board.Author.Pseudo && !User.IsInRole(Role.Admin.ToString())) 
                return Forbid("Vous n'avez pas les droits pour effectuer cette action");


            foreach(var l in board.Lists) {
                if(l.Cards.Count() != 0) {
                    foreach(var c in l.Cards) {
                        if(c.CardUsers.Count != 0) {
                            foreach(var uc in c.CardUsers) {
                            _context.UserCards.Remove(uc);
                            }
                        }
                        _context.Cards.Remove(c);
                    }
                    
                }
                _context.Lists.Remove(l);
            }
                
            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("moveRigthOrLeft/{boardId}/{listId}/{moveRigth}")]
        public async Task<ActionResult> moveRigth(int boardId, int listId, Boolean moveRigth) {

            var board = await _context.Boards.FindAsync(boardId);
            var list = await _context.Lists.FindAsync(listId);
            var nb = board.Lists.Count() - 1;
            if(moveRigth) {
                if(list.Position < nb) {
                    var posRigth = list.Position + 1;
                    var rigthList = await _context.Lists.FirstOrDefaultAsync(l => l.Position == posRigth && l.BoardId == boardId);
                    var tmp = list.Position;
                    list.Position = rigthList.Position;
                    rigthList.Position = tmp;
                    _context.SaveChanges();
                }
            } else {
                var posLeft = list.Position - 1;
                var leftList = await _context.Lists.FirstOrDefaultAsync(l => l.Position == posLeft && l.BoardId == boardId);
                var tmp = list.Position;
                list.Position = leftList.Position;
                leftList.Position = tmp;
                
                _context.SaveChanges();
            }
            
            return Ok();
        }

        [HttpGet("changePosition/{boardId}/{listId}/{index}")]
        public async Task<ActionResult> changePositionList(int boardId, int listId, int index) {

            var board = await _context.Boards.FindAsync(boardId);
            var list = await _context.Lists.FindAsync(listId);

            foreach(var l in board.Lists) {
                if(l.Position <= index && l != list && l.Position > list.Position) {
                    l.Position--;
                    _context.SaveChanges();
                } else if(l.Position >= index && l != list && l.Position < list.Position) {
                    l.Position++;
                }
            }

            list.Position = index;
            _context.SaveChanges();

            return Ok();
        }
    }
    
}