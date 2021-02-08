using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_2021_002.Helpers;
using prid_2021_002.Models;
using PRID_Framework;

namespace prid_2021_002.Controllers {
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CardsController: ControllerBase {
        private readonly PridContext _context;

        public CardsController(PridContext context) {
            _context = context;
        }

        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardDTO>>> GetCollection() {
            return (await _context.Cards.ToListAsync()).ToDTO();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CardDTO>> Get(int id) {
            var card = await _context.Cards.FindAsync(id);
            if(card == null)
                return NotFound();
            return Ok(card.ToDTO());
            
        }

        [HttpPost]
        public async Task<ActionResult<CardDTO>> Post(CardDTOU card) {

            // var curentUser = User.Identity.Name;
            // var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);

            // if(curentUser != card.List.Board.Author.Pseudo || !card.List.Board.Collaborators.Contains(user) || !User.IsInRole(Role.Admin.ToString())) 
            //     return BadRequest("Vous n'avez pas les droits pour effectuer cette action");
            var list = await _context.Lists.FindAsync(card.ListId);
            var board = await _context.Boards.FindAsync(list.BoardId);
            
            foreach(var l in board.Lists) {
                foreach(var c in l.Cards) {
                    if(c.Title == card.Title) {
                        return BadRequest("This card already exists in this board");
                    }
                }
            }
            Console.WriteLine("l'id est "+card.CardId);

            var newCard = new Card(){
                Title = card.Title,
                Position = list.Cards.Count,
                AuthorId = card.AuthorId,
                ListId = card.ListId
            };
            
                
            _context.Cards.Add(newCard);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = newCard.CardId }, newCard.ToDTOU());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CardDTOU cardDTOU) {
            if(id != cardDTOU.CardId)
                return BadRequest("mauvaise requetes des id");

            var card =await _context.Cards.FindAsync(id);

            if(card == null)
                return NotFound();
            card.Title = cardDTOU.Title;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            Console.WriteLine("ok j'entre dans cette fonction delete card");
            var card = await _context.Cards.FindAsync(id);
            var list = await _context.Lists.FindAsync(card.ListId);
            

            if(card == null)
                return NotFound();

            if(card.CardUsers.Count != 0) {
                foreach(var uc in card.CardUsers) {
                    _context.UserCards.Remove(uc);
                }
            }
           
            _context.Cards.Remove(card);
            foreach(var c in list.Cards) {
                if(c.Position > card.Position) {
                    c.Position--;
                }
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("availableTitleCard/{title}/{id}")]
        public async Task<ActionResult<bool>> cardExists(string title, int id) {
            var board = await _context.Boards.FindAsync(id);
            var lists = board.Lists;
            foreach(var l in board.Lists) {
                foreach(var card in l.Cards) {
                    if(card.Title == title) {
                        return false;
                    }
                }
               
            }

            return true;
        }

        [HttpGet("getList/{id}")]
        public async Task<ActionResult<ListDTO>> getList(int id) {
            var card = await _context.Cards.FindAsync(id);
           
            var list = await _context.Lists.FindAsync(card.ListId);
            if(list == null)
                return NotFound();
            return Ok(list.ToDTO());
        }
    }
}
