import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import * as _ from 'lodash';
import { BoardListComponent } from 'src/app/modules/home/board/boardlist/boardlist.component';
import { EditBoardComponent } from 'src/app/modules/home/board/edit-board/edit-board.componenet';
import { EditUserComponent } from 'src/app/modules/home/users/edit-user/edit-user.component';
import { UserListComponent } from 'src/app/modules/home/users/userlist/userlist.component';
import { Board } from '../../models/board';
import { Role, User } from '../../models/User';
import { AuthenticationService } from '../../services/authentication.service';
import { BoardService } from '../../services/board.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss']
})
export class NavMenuComponent implements OnInit {
  isExpanded = false;
  boards: Board[] = [];

  constructor(
    private router: Router,
    private authenticationService: AuthenticationService,
    public userService: UserService,
    public boardService: BoardService,
    public userlist: UserListComponent,
    // private boardlist: BoardListComponent,
    public dialog : MatDialog,
    public snackBar: MatSnackBar,

  ) {}

  ngOnInit(): void {
    if(this.currentUser) {
      this.boardService.getBoards(this.currentUser.userId).subscribe(boards => {
        this.boards = boards;
      });
      this.boardService.getAll().subscribe(boards => {
        this.boards = boards;
      });
    }
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  get currentUser() {
    return this.authenticationService.currentUser;
  }

  get isAdmin() {
      return this.currentUser && this.currentUser.role === Role.Admin;
  }

  edit(user : User) {
    const dlg = this.dialog.open(EditUserComponent, {data: {user, isNew: false } });
    dlg.beforeClosed().subscribe(res => {
        if(res) {
            console.log("utilisateur" + user.userId);
            _.assign(user, res);
            this.userService.update(user).subscribe(res => {
                if(!res) {
                    this.snackBar.open(`There was an error at the server. the update has not been done! Please try again.`, 'Dismiss', { duration: 10000 })
                }
                // sessionStorage.setItem('currentUser', JSON.stringify(user));
                if(this.currentUser.role == 2)
                  this.userlist.ngOnInit();
            });
        }
    })
}

  create() {
    const board = new Board({});
    const dlg = this.dialog.open(EditBoardComponent, { data: { board, isNew: true } });
    dlg.beforeClosed().subscribe(res => {
        if(res) {
            this.boardService.create(res).subscribe(res => {
                if(!res) {
                    this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                } else {
                  this.snackBar.open(`Success! the board has been created.`, 'Dismiss', { duration: 10000 });
                  // this.boardlist.ngOnInit();
                }
            })
        }
    })
  }


  logout() {
      this.authenticationService.logout();
      this.router.navigate(['/logged-out']);
  }
}
