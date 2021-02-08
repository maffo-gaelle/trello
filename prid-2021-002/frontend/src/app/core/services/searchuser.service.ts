import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Team } from '../models/team';
import { User } from '../models/User';
import { Board } from '../models/Board';
import { catchError, map } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { UserService } from "src/app/core/services/user.service";
import { TeamService } from "src/app/core/services/team.service";
import { BoardService } from "src/app/core/services/board.service";

@Injectable({ providedIn: 'root' })
export class SearchService
{
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string){}

    private users:User[];
	private teams:Team[];
	private teamService: TeamService;
	private userService: UserService;
	private boardService: BoardService;

    search(searchValue:String)
    {
		var results:String[];
		let user:any;
		let team:any;
        for(team in this.teams)
			if(team.teamname.startsWith(searchValue))
			{
				results.push("t."+team.teamname);
				let tusers:User[];
				this.teamService.getUsers(team.teamId).subscribe(tusers=>{tusers = this.users;});
				for(user in tusers)
					results.push("u."+user.pseudo);
			}
		for(user in this.users)
			if(user.pseudo.startsWith(searchValue))
				results.push("u."+user.pseudo);
    }

    addintteam(team:Team,user:User)
    {
		this.teamService.addremove(team,user,true).subscribe();
	}

	addinboard(board:Board,user:User)
    {
		this.boardService.addCollaborator(board,user,true).subscribe();
	}

	removeinboard(board:Board,user:User)
	{
		this.boardService.addCollaborator(board,user,false).subscribe();
	}

	removeintteam(team:Team,user:User)
	{
		this.teamService.addremove(team,user,false).subscribe();
	}

    onClik(elem:String)
    {
        if(elem.startsWith("t"))
		{
			var tusers:User[];
			let team:any;
			for(team in this.teams)
			{
				if(team.teamname===elem.substring(2))
				{
					this.teamService.getUsers(team.teamId).subscribe(users=>{tusers=users;});
					let user:any;
					for(user in tusers)
					{
						//this.addorremoveinboard(board,user,true);
					}
				}
			}
		}else{
			let user:any;
			for(user in this.users)
				if(user.pseudo===elem.substring(2))
				{
					//this.addorremoveinboard(board,user,true);
				}
		}
    }
}
