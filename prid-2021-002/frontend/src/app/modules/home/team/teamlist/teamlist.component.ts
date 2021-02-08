import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as _ from 'lodash';
import { Team } from 'src/app/core/models/team';
import { User } from 'src/app/core/models/User';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { TeamService } from 'src/app/core/services/team.service';
import { EditTeamComponent } from '../edit-team/edit-team.componenet';

@Component({
    selector: 'app-teamlist',
    templateUrl: './teamlist.component.html',
    styleUrls: ['./teamlist.component.scss']
})
export class TeamListComponent implements OnInit, OnDestroy
{
    allteams:Team[]=[];
    teams:Team[]=[];
    currentUser:User;
  
    constructor(
        private teamService:TeamService,
        private authService:AuthenticationService,
        public dialog:MatDialog,
        public snackBar:MatSnackBar
    ) 
    {
       this.currentUser = this.authService.currentUser;
    }

    ngOnInit(): void
    {
        this.refresh();
    }

    create()
    {
        const team = new Team({});
        const dlg = this.dialog.open(EditTeamComponent,{data:{team, isNew: true}});
        dlg.beforeClosed().subscribe(res => {
            if(res)
            {
                this.teamService.create(res).subscribe(res => {
                    if(!res)
                    {
                        this.snackBar.open(`There was an error at the server. The team has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                    else
                        this.refresh();
                });
            }
        })
    }

    update(team: Team)
    {
        const dlg = this.dialog.open(EditTeamComponent, {data: {team, isNew: false }});
        dlg.beforeClosed().subscribe(res => {
            if(res)
            {
                _.assign(team, res);
                this.teamService.update(team).subscribe(res => {
                    if(!res)
                    {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                    else
                        this.refresh();
                })
            }
        })
    }

    delete(team: Team)
    {
        this.teamService.delete(team).subscribe(res=>{
            if(!res)
            {
                this.snackBar.open(`There was an error at the server. The delete has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                this.refresh();
            }
            else
                this.refresh();
        })
    }
    
    getuserteams()
    {
        this.teamService.getTeams(this.currentUser.userId).subscribe(teams => {this.teams=teams;});
    }

    refresh()
    {
        this.getuserteams();
        if(this.currentUser.role==2)
            this.getall();
    }

    ngOnDestroy(): void
    {
        this.snackBar.dismiss;
    }

    getall()
    {
        this.teamService.getAll().subscribe(teams=>{this.allteams=teams;})
    }
}