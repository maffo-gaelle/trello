import { Component, Injectable, OnDestroy, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import * as _ from 'lodash';
import { Board } from 'src/app/core/models/board';
import { User } from 'src/app/core/models/User';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { BoardService } from 'src/app/core/services/board.service';
import { ConfirmDeleteBoardComponent } from '../../confirmDelete/confirm-delete-board/confirm-delete-board.component';
import { EditBoardComponent } from '../edit-board/edit-board.componenet';



@Component({
    selector: 'app-boardlist',
    templateUrl: './boardlist.component.html',
    styleUrls: ['./boards-user.component.scss']
})

// @Injectable({ providedIn: 'root'})
export class BoardListComponent implements OnInit, OnDestroy {
    public allboards: Board[]=[];
    boards: Board[] = [];
    collaboratorBoards: Board[] = [];
    currentUser: User;

    constructor(
        private boardService: BoardService,
        private authService : AuthenticationService,
        public dialog: MatDialog,
        public snackBar: MatSnackBar
    )
    {
       this.currentUser = this.authService.currentUser;
    }

    ngOnInit(): void {
        this.getuserboards();
        this.boardService.getCollaboratorBoards(this.currentUser.userId).subscribe(boards => {
            this.collaboratorBoards = boards;
            console.log(boards);
        });

        if(this.currentUser.role==2) {
          this.getall();
          console.log("on est là");
        }

    }

    create() {
        const board = new Board({});
        const dlg = this.dialog.open(EditBoardComponent, { data: { board, isNew: true } });
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                this.boardService.create(res).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.getuserboards();
                    }else {
                      this.snackBar.open(`SUCCESS! a new board has been created!`, 'Dismiss', { duration: 10000 });
                      this.ngOnInit();
                    }
                })
            }
        })
    }

    update(board: Board) {
        const dlg = this.dialog.open(EditBoardComponent, {data: {board, isNew: false }});
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                _.assign(board, res);
                this.boardService.update(board).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.refresh();
                    } else {
                      this.snackBar.open(`SUCCESS! The update of board has been done!`, 'Dismiss', { duration: 10000 });
                      this.refresh();
                    }
                })
            }
        })
    }

    deleteBoard(board: Board) {
      const dlg = this.dialog.open(ConfirmDeleteBoardComponent, {data: {board: board, confirm : false, deleteboard: true }});
      dlg.beforeClosed().subscribe(res => {
        console.log(res)
          if(res && res.confirm == true) {
              _.assign(board, res);
              this.boardService.deleteBoard(board).subscribe(res => {
                  if(!res) {
                    this.snackBar.open(`There was an error at the server. The reset has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                  } else {
                    this.snackBar.open(`SUCCESS!. the board have been deleted.`, 'Dismiss', { duration: 10000 });
                  }
                  this.ngOnInit();
              })
          }
      })
    }

    getuserboards() {
        this.boardService.getBoards(this.currentUser.userId).subscribe(boards => {
            this.boards = boards;
        });
    }
    refresh() {
        this.getuserboards();
        this.boardService.getCollaboratorBoards(this.currentUser.userId).subscribe(boards => {
            this.collaboratorBoards = boards;
        });
        if(this.currentUser.role==2)
            this.getall();
    }

    ngOnDestroy(): void {
        this.snackBar.dismiss;
    }

    getall() {
        this.boardService.getAll().subscribe(boards => {
            this.allboards= boards;
            console.log("bbb" + boards);
            console.log("ccc" + this.allboards);
        })
    }
}
