import { Component, OnDestroy, OnInit} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from "lodash";
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { User } from "src/app/core/models/User";
import { UserService } from "src/app/core/services/user.service";
import { TeamService } from "src/app/core/services/team.service";
import { Team } from "src/app/core/models/Team";
import { AdduserTeamComponent } from "../adduser-team/adduser-team.component";

@Component({
    selector: 'app-show-team',
    templateUrl: './show-team.component.html',
    styleUrls: ['userlist.component.scss']
})
export class ShowTeamComponent implements OnInit, OnDestroy
{
    title: string="Liste des utilisateurs dans la team";
    searchvalue: string;
    private teamId = this.route.snapshot.params.id;
    currentUser:User;
    currentteam: Team;
    users: User[]=[];
    notmembers: User[]=[];
    usersFiltre: User[] = [];
    picture = false;

    constructor(
        private route: ActivatedRoute,
        private authService:AuthenticationService,
        private teamService: TeamService,
        private userService: UserService,
        public dialog:MatDialog,
        public snackBar:MatSnackBar
    ){
        this.currentUser = this.authService.currentUser;
    }

    ngOnInit(): void
    {
        this.teamService.get(this.teamId).subscribe(team=>{this.currentteam=team;});
        this.refresh();
        this.teamService.getUsers(this.teamId).subscribe(users => {this.usersFiltre = _.cloneDeep(users);});
    }

    search(searchvalue:string)
    {

    }

    filterChanged(filterValue: string)
    {
        var lowerFilter = filterValue.toLowerCase();
        this.users = _.filter(this.usersFiltre, u => {
        const str = (u.pseudo + " " + u.firstName + " " + u.lastName).toLowerCase();
        return str.includes(lowerFilter);
        });
      }

      showBirthdate(user : User) {
        console.log(user);
          return user.birthDate != null;
      }

      ifPicturePath(user : User) {
          if(user.picturePath != null)
              return this.picture == true;
      }

      getUsersNotInTeam()
      {
        // this.teamService.getUsersNotMembers(this.currentteam).subscribe(users =>
        //   {this.notmembers = users;
        //   });
      }

      get notMembers()
      {
        return this.getUsersNotInTeam();
      }

    refresh()
    {
        this.teamService.getUsers(this.teamId).subscribe(users => {this.users = users;});
    }

    add()
    {
        const dlg=this.dialog.open(AdduserTeamComponent,{data:{this:this.currentteam}});
        dlg.beforeClosed().subscribe(res => {
            if(res)
            {
                _.assign(this.currentteam, res);
                this.teamService.update(this.currentteam).subscribe(res => {
                    if(!res)
                    {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`,'Dismiss',{duration: 10000});
                        this.refresh();
                    }
                    else
                        this.refresh();
                })
            }
        })
    }

    remove(user:User)
    {
       this.teamService.addremove(this.currentteam,user,false).subscribe();
       this.refresh();
    }

    ngOnDestroy():void
    {
        this.snackBar.dismiss;
    }
}
