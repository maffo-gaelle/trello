using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_2021_002.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Security.Claims;
using prid_2021_002.Helpers;

namespace prid_2021_002.Controllers {
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ListsController : ControllerBase {
        private readonly PridContext _context;

        public ListsController(PridContext context) {
            _context = context;
        }

        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListDTO>>> GetCollection() {
            return (await _context.Lists.ToListAsync()).ToDTO();
        }

        // [HttpGet("GetCardsOrdered/{id}")]
        // public async Task<ActionResult<IEnumerable<ListDTO>>> GetCardsOrdered(int id) {
        //     var list = await _context.Lists.FindAsync(id);
        //     var cards =  _context.Cards.Where(c => c.ListId == list.ListId).OrderBy(c => c.Position);

        //     return cards.ToDTO();
        // }

        [HttpGet("{id}")]
        public async Task<ActionResult<ListDTO>> Get(int id) {
            var list = await _context.Lists.FindAsync(id);
            if(list == null)
                return NotFound();

            return Ok(list.ToDTO());
            
        }

        [HttpPost]
        public async Task<ActionResult<ListDTO>> Post(ListDTOU list) {

            var curentUser = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);
            var board = await _context.Boards.FindAsync(list.BoardId);

            if(curentUser != board.Author.Pseudo && !board.Collaborators.Contains(user) && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            var other = await _context.Lists.FirstOrDefaultAsync(l => l.Title == list.Title && l.BoardId == list.BoardId);
            if(other != null) {
                Console.WriteLine("Cette liste existe déjà");
                return BadRequest("Cette liste existe déjà");
            }


            var newList = new List(){
                Title = list.Title,
                Position = board.Lists.Count(),
                BoardId = list.BoardId
            };

            _context.Lists.Add(newList);
            _context.SaveChanges();
            
            return CreatedAtAction(nameof(Get), new { id = newList.ListId }, newList.ToDTOU());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ListDTO listDTO) {

            var curentUser = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);
            var board = await _context.Boards.FindAsync(listDTO.BoardId);
              
            if(curentUser != board.Author.Pseudo && !board.Collaborators.Contains(user) && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(id != listDTO.ListId)
                return BadRequest("mauvaise requetes des id");

            var list =await _context.Lists.FindAsync(id);

            if(list == null)
                return NotFound();
            list.Title = listDTO.Title;
            list.BoardId = listDTO.BoardId;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {

            var list = await _context.Lists.FindAsync(id);
            var boardId = list.BoardId;
            var board = await _context.Boards.FindAsync(boardId);
            var curentUser = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);
              
            if(curentUser != board.Author.Pseudo && !board.Collaborators.Contains(user) && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(list == null)
                return NotFound();
            
            foreach(var l in board.Lists) {
                if(l.Position > list.Position) {
                    l.Position--;
                    _context.SaveChanges();
                }
            }
            
            if(list.Cards.Count() != 0) {
                foreach(var c in list.Cards) {
                    if(c.Collaborators.Count() != 0) {
                        foreach(var uc in c.CardUsers) {
                            _context.UserCards.Remove(uc);
                        }
                        _context.SaveChanges();
                    }
                }
            } 
            _context.Cards.RemoveRange(list.Cards);

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("availableTitleList/{title}/{id}")]
        public async Task<ActionResult<bool>> listExists(string title, int id) {
            var board = await _context.Boards.FindAsync(id);
            var lists = board.Lists;
            foreach(var l in board.Lists) {
                if(l.Title == title) {
                    return false;
                }
            }

            return true;
        }

        [HttpGet("changeAllCard/{id}/{listid}")]
        public async Task<ActionResult> changeAllCard(int id, int listid) {
            var list = await _context.Lists.FindAsync(id);

            var boardId = list.BoardId;
            var board = await _context.Boards.FindAsync(boardId);
            var curentUser = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);
              
            if(curentUser != board.Author.Pseudo && !board.Collaborators.Contains(user) && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");

            if(list.Cards.Count() != 0) {
                foreach(var c in list.Cards) {
                    c.ListId = listid;
                }
                _context.SaveChanges();
            }
            
            return Ok();
        }

        [HttpPut("changePositionCard/{id}/{previous}/{current}")]
        public async Task<ActionResult> changePositionCard(int id, int previous, int current, ListDTO list) {
            var liste = await _context.Lists.FindAsync(id);
            var cardPevious = await _context.Cards.FirstOrDefaultAsync(c => c.Position == previous && c.ListId == list.ListId);

            var boardId = list.BoardId;
            var board = await _context.Boards.FindAsync(boardId);
            var curentUser = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Pseudo == curentUser);
              
            if(curentUser != board.Author.Pseudo && !board.Collaborators.Contains(user) && !User.IsInRole(Role.Admin.ToString())) 
                return BadRequest("Vous n'avez pas les droits pour effectuer cette action");
            
            foreach(var c in liste.Cards) {
                if(c.Position < previous && c.Position >= current) {
                    c.Position++ ;
                } else if(c.Position > previous && c.Position <= current) {
                    c.Position-- ;
                }
               
            }

            cardPevious.Position = current;
            _context.SaveChanges();

            return Ok();
        }
    }
}