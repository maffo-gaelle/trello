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
    public class UsersController : ControllerBase {
        private readonly PridContext _context;
        public BoardsController boardsControllers;

        public UsersController(PridContext context) {
            _context = context;
            this.boardsControllers = new BoardsController(context);
        }

        [Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetCollection() {
            return (await _context.Users.ToListAsync()).ToDTO();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id) {
            var user = await _context.Users.FindAsync(id);
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);

            if(User.Identity.Name != user.Pseudo && !User.IsInRole(Role.Admin.ToString())) 
                return Forbid("Vous n'avez pas les droits pour effectuer cette action");

            if(user == null)
                return NotFound();
            //ici je retourne un statu 200-ok avec un objet user associé
            return Ok(user.ToDTO());
            //return user;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post(UserDTO user) {
            var other = await _context.Users.FindAsync(user.UserId);

            if(other != null) {
                Console.WriteLine("user  is not null: " + user.UserId);
                var err = new ValidationErrors().Add("This Id already in use", nameof(user.UserId));
                return BadRequest(err);
            }

            var newUser = new User(){
                Pseudo = user.Pseudo,
                Password = user.Password,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                PicturePath = user.PicturePath
            };
                

            _context.Users.Add(newUser);
            var res = await _context.SaveChangesAsyncWithValidation();
            if(!res.IsEmpty) 
                return BadRequest(res);

            //permet de renvoyer un réponse ayant un statut HTTP 201 - Created
            return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser.ToDTO());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, UserDTO userDTO) {
            Console.WriteLine(userDTO.Pseudo);
            if(id != userDTO.UserId)
                return BadRequest("mauvaise requetes des id");

            var user =await _context.Users.FindAsync(id);

            if(user == null)
                return NotFound();
            
            if(user.Pseudo != userDTO.Pseudo || userDTO.Pseudo != null) 
                user.Pseudo = userDTO.Pseudo;
            if(user.Email != userDTO.Email || userDTO.Email != null) 
                user.Email = userDTO.Email;
            user.FirstName = userDTO.FirstName;
            user.LastName = userDTO.LastName;
            user.BirthDate = userDTO.BirthDate;
            if(!string.IsNullOrWhiteSpace(userDTO.PicturePath))
                //On ajoute un timestamp à la fin de l'url pour générer un URL différent quand on change d'image
                //car sinon l'image ne se raffraichit pas parce que l'URL ne change pas et le browserla prend dans la cache
                user.PicturePath = userDTO.PicturePath + "?" + DateTime.Now.Ticks;
            else
                user.PicturePath = null;

            if(userDTO.Password !=  null && userDTO.Password != "")
                user.Password = userDTO.Password; 

            // var res = await _context.SaveChangesAsyncWithValidation();
            // if(!res.IsEmpty)
            //     return BadRequest(res);

            _context.SaveChanges();
            return NoContent();
        }

        [Authorized(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
                return NotFound();
            
            var boards = user.Boards;
            Console.WriteLine("delete user ok(1)");
            if(boards != null) {
                Console.WriteLine("nombre de tableau: "+boards.Count());
                foreach(var b in boards) {
                await this.boardsControllers.Delete(b.BoardId);
                    Console.WriteLine("delete user oh suppression tableaux(1)");
                }
            }
            
            Console.WriteLine("delete user oh(2)");
            if(user.UserBoards != null)
                _context.UserBoards.RemoveRange(user.UserBoards);
            Console.WriteLine("delete user oh(3)");
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //-----Méthodes pour faciliter l'upload des image----------------------
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] string pseudo, [FromForm]IFormFile picture) {
            if(picture != null && picture.Length > 0) {
                //var fileName = Path.GetFileName(picture.FileName);
                var fileName = pseudo + "-"  + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".jpg";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", fileName);
                using( var fileSrteam = new FileStream(filePath, FileMode.Create)) {
                    await picture.CopyToAsync(fileSrteam);
                }
                return Ok($"\"uploads/{fileName}\"");
            }
            return Ok();
        }


        [HttpPost("cancel")]
        public IActionResult Cancel([FromBody] dynamic data) {
            string picturePath = data.picturePath;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", picturePath);
            if(System.IO.File.Exists(path))
                System.IO.File.Delete(path);
            return Ok();
        }
        
        [HttpPost("confirm")]
        public IActionResult Confirm([FromBody] dynamic data) {
            string pseudo = data.pseudo;
            string PicturePath = data.picturePath;
            string newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", pseudo + ".jpg");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", PicturePath);
            if(System.IO.File.Exists(path)) {
                if(System.IO.File.Exists(newPath))
                    System.IO.File.Delete(newPath);
                System.IO.File.Move(path, newPath);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<User>> Authenticate(UserDTO data) {
            var user = await Authenticate(data.Pseudo, data.Password);
            Console.WriteLine("current user: " + User.Identity.Name);
            if(user == null)
                return BadRequest(new ValidationErrors().Add("User not found", "Pseudo"));
            if(user.Token == null)
                return BadRequest(new ValidationErrors().Add("Incorrect password", "Password"));

            return Ok(user);
        }

        private async Task<User> Authenticate(string pseudo, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == pseudo);
            

            if(user == null)
                return null;

            if(user.Password == password) {
                
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("my-super-secret-key");
                var tokenDescriptor = new SecurityTokenDescriptor {
                                                                       
                    Subject = new ClaimsIdentity(new Claim[]
                                                    {
                                                        new Claim(ClaimTypes.Name, user.Pseudo),
                                                        // new Claim(ClaimTypes.NameIdentifier, user.Pseudo),
                                                        new Claim(ClaimTypes.Role, user.Role.ToString())
                                                    }),
                    IssuedAt = DateTime.UtcNow,
                    Expires = DateTime.UtcNow.AddMinutes(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)                                                   
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                user.Token = tokenHandler.WriteToken(token);
            }
            user.Password = null;
            Console.WriteLine("(user allow: " + user + ")");
            return user;
        }

        [AllowAnonymous]
        [HttpGet("availableEmail/{email}")]
        public async Task<ActionResult<bool>> emailUsed(string email) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            return user == null;
        }

        [AllowAnonymous]
        [HttpGet("availablePseudo/{pseudo}")]
        public async Task<ActionResult<bool>> pseudoUsed(string pseudo) {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == pseudo);

            return user == null;
        }

    }
}