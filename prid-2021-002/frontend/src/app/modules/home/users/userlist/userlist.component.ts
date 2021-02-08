import { Component, Injectable, OnDestroy, OnInit} from "@angular/core";
import { MatDialog } from "@angular/material/dialog";
import { MatSnackBar } from "@angular/material/snack-bar";
import * as _ from "lodash";
import { Observable } from "rxjs";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { UserService } from "src/app/core/services/user.service";
import { ConfirmDeleteUserComponent } from "../../confirmDelete/confirm-delete-user/confirm-delete-user.component";
import { EditUserComponent } from "../edit-user/edit-user.component";

@Injectable({ providedIn: 'root'})

@Component({
    selector: 'app-userlist',
    templateUrl: './userlist.component.html',
    styleUrls: ['userlist.component.scss']
})
export class UserListComponent implements OnInit, OnDestroy {
    title: string = "USERS LIST";
    filter: string;
    users: any[] = [];
    usersBackup: User[] = [];
    picture = false;
    currentUser: User;


    constructor (
        private userService: UserService,
        public dialog: MatDialog,
        public snackBar: MatSnackBar,
        private authenticationService: AuthenticationService
    ) {
        this.currentUser = this.authenticationService.currentUser;
    }

    ngOnInit(): void {
        this.userService.getCollection().subscribe(users => {
            this.users = users;
            this.usersBackup = _.cloneDeep(users);
        });
    }

    //appelée chaque fois que le filtre est modifié par l'utilisateur
    filterChanged(filterValue: string) {
      var lowerFilter = filterValue.toLowerCase();
      this.users = _.filter(this.usersBackup, u => {
        const str = (u.pseudo + " " + u.firstName + " " + u.lastName).toLowerCase();
        return str.includes(lowerFilter);
      });
    }

    // appelée quand on clique sur le bouton "edit d'un membre "
    edit(user : User) {
        const dlg = this.dialog.open(EditUserComponent, {data: {user, isNew: false } });
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                console.log("utilisateur" + user.userId);
                _.assign(user, res);
                this.userService.update(user).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. the update has not been done! Please try again.`, 'Dismiss', { duration: 10000 })
                    } else {
                      this.snackBar.open(`The update has been done!.`, 'Dismiss', { duration: 10000 });
                      if(this.currentUser.role == 2)
                        this.refresh();
                      // if(user.userId == this.currentUser.userId)
                      //   sessionStorage.setItem('currentUser', JSON.stringify(user));
                    }
                });
            }
        })
    }

    create() {
        const user = new User({});
        const dlg = this.dialog.open(EditUserComponent, { data: { user, isNew: true } });
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                this.userService.create(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    }
                    this.refresh();
                });
            }
        })
    }

    refresh() {
        this.userService.getCollection().subscribe(users => {
            this.users = users;
        });
    }

    // appelée quand on clique sur le bouton delete  d'un user
    deleteUser(user: User) {

      let add: boolean;

      const dlg = this.dialog.open(ConfirmDeleteUserComponent, {data: {user: user, confirm : false, add: true }});
      dlg.beforeClosed().subscribe(res => {
        console.log(res)
        if(res && res.confirm == true) {
            _.assign(user, res);
            this.userService.delete(user).subscribe(res => {
              console.log(res);
                if(!res) {
                  this.snackBar.open(`There was an error at the server. The delete has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                } else {
                  this.snackBar.open(`SUCCESS!. The collaborator and the cards of which he is the author have been deleted.`, 'Dismiss', { duration: 10000 });
                  this.refresh();
                }
            });
        }
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

    ngOnDestroy(): void {
        this.snackBar.dismiss;
    }


}
