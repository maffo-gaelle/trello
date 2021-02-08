using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_2021_002.Models;
using PRID_Framework;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace prid_2021_002.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TeamsController:ControllerBase
    {
        private readonly PridContext _context;
        public TeamsController(PridContext context)
        {
            _context=context;
        }

        //[Authorized(Role.Admin)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetCollection()
        {
            return (await _context.Teams.ToListAsync()).ToDTO();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDTO>> Get(int id)
        {
            var team=await _context.Teams.FindAsync(id);
            if(team==null)
                return NotFound();
            //ici je retourne un statu 200-ok avec un objet team associé
            return Ok(team.ToDTO());
        }

        [HttpGet("getTeams/{id}")]
        public async Task<ActionResult<IEnumerable<TeamDTO>>> GetTeamsByUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
            if(user!=currentUser)
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action");
            return (user.Teams).ToDTO();
        }

        [HttpGet("getUsersNotCollaborators/{id}")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> getUsersNotCollaborators(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            var users = await _context.Users.ToListAsync();
            Console.WriteLine(team.Collaborators.Count());
            foreach(var u in team.Collaborators) {
                if(users.Contains(u)) {
                    users.Remove(u);
                }
            }
            Console.WriteLine(users.Count);
            return users.ToDTO();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<TeamDTO>> Post(TeamDTO team)
        {
            var other=await _context.Teams.FindAsync(team.TeamId);
            if(other!=null)
            {
                Console.WriteLine("team is not null: " + team.TeamId);
                var err=new ValidationErrors().Add("This Id already in use", nameof(team.TeamId));
                return BadRequest(err);
            }
            var newTeam=new Team(){Teamname=team.Teamname};    
            _context.Teams.Add(newTeam);
            var res=await _context.SaveChangesAsyncWithValidation();
            if(!res.IsEmpty) 
                return BadRequest(res);
            //permet de renvoyer un réponse ayant un statut HTTP 201 - Created
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
            await this.addOrRemoveCollaborators(newTeam.TeamId,currentUser.UserId,true);
            return CreatedAtAction(nameof(Get), new {id=newTeam.TeamId}, newTeam.ToDTO());
        }

        //[Authorized(Role.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TeamDTO TeamDTO)
        {
            Console.WriteLine(TeamDTO.Teamname);
            if(id!=TeamDTO.TeamId)
                return BadRequest("mauvaise requetes des id");
            var team=await _context.Teams.FindAsync(id);
            if(team==null)
                return NotFound();
            team.Teamname=TeamDTO.Teamname;
            var res=await _context.SaveChangesAsyncWithValidation();
            if(!res.IsEmpty)
                return BadRequest(res);
            return NoContent();
        }

        //[Authorized(Role.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if(team==null)
                return NotFound();
            var users=team.Collaborators;
            foreach(var u in users)
                await this.addOrRemoveCollaborators(id,u.UserId,false);
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("availableTeamname/{Teamname}")]
        public async Task<ActionResult<bool>> TeamnameUsed(string teamname)
        {
            var team=await _context.Teams.SingleOrDefaultAsync(t => t.Teamname==teamname);
            return team==null;
        }

        [HttpGet("getUsers/{id}")] 
        public async Task<ActionResult<IEnumerable<UserDTO>>> getUsers(int id)
        {
            var team=await _context.Teams.FindAsync(id);
            return team.Collaborators.ToDTO();
        }

        [HttpGet("addOrRemoveCollaborators/{teamid}/{userid}/{add}")]
        public async Task<ActionResult> addOrRemoveCollaborators(int teamid, int userid, Boolean add)
        {
            Console.WriteLine("j'entre dans addOrRemoveCollaborators");
            var canaddremove=true;
            var team=await _context.Teams.FindAsync(teamid);
            // var users=team.Collaborators;
            var currentUser = await _context.Users.SingleOrDefaultAsync(u => u.Pseudo == User.Identity.Name);
            // if(team.TeamUsers.Count==0)
            //     canaddremove=true;
            // foreach(var u in users)
            //     if(currentUser.UserId==u.UserId||currentUser.Role.Equals(2))
            //         canaddremove=true;
            if(canaddremove)
            {
                Console.WriteLine("j'entre dans addOrRemoveCollaborators: canaddremove");
                var user = await _context.Users.FindAsync(userid);
                var teamUser = new UserTeam();
                teamUser.Team = team;
                teamUser.User = user;
                if(add)
                {
                    _context.UserTeams.Add(teamUser);
                    _context.SaveChanges();
                }else{
                    Console.WriteLine("test remove");
                    _context.UserTeams.Remove(teamUser);
                    Console.WriteLine("test save");
                    _context.SaveChanges();
                }
                return Ok();
            }
            return Ok();
        }
    }
}