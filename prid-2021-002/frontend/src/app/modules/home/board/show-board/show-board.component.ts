import { CdkDrag, CdkDragDrop, moveItemInArray, transferArrayItem } from '@angular/cdk/drag-drop';
// import { ChangeDetectionStrategy } from '@angular/core';
import { Component, OnInit, ViewChild } from '@angular/core';
import { AsyncValidatorFn, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatMenuTrigger } from '@angular/material/menu';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import * as _ from 'lodash';
import { Observable } from 'rxjs';
import { Board } from 'src/app/core/models/board';
import { Card } from 'src/app/core/models/card';
import { List } from 'src/app/core/models/list';
import { User } from 'src/app/core/models/User';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { BoardService } from 'src/app/core/services/board.service';
import { CardService } from 'src/app/core/services/card.service';
import { ListService } from 'src/app/core/services/list.service';
import { UserService } from 'src/app/core/services/user.service';
import { DetailsCardComponent } from '../../card/details-card.component';
import { ConfirmDeleteBoardComponent } from '../../confirmDelete/confirm-delete-board/confirm-delete-board.component';
import { ConfirmDeleteComponent } from '../../confirmDelete/confirm-delete-list/confirm-delete.component';
import { ConfirmDeleteUserComponent } from '../../confirmDelete/confirm-delete-user/confirm-delete-user.component';
import { EditListComponent } from '../../list/edit-list/edit-list.component';
import { EditBoardComponent } from '../edit-board/edit-board.componenet';

@Component({
    selector: 'show-board',
    templateUrl: './show-board.component.html',
    styleUrls: ['./show-board.component.scss'],
    // changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ShowBoardComponent implements OnInit{

  @ViewChild('menuTrigger') menuTrigger: MatMenuTrigger;

    private boardId = this.route.snapshot.params.id;
    public board: Board;
    public lists: List[];
    public notcollaborators: User[];
    public confirm : boolean;
    public frm: FormGroup;
    //public form: FormGroup;
    public ctlTitle: FormControl;
    currentUser : User;
    titleCard: string;
    value: string = '';
    i : number = 0;

    constructor(
        private route: ActivatedRoute,
        public boardService: BoardService,
        public listService: ListService,
        public cardService: CardService,
        public userService: UserService,
        private authService : AuthenticationService,
        public router: Router,
        private fb: FormBuilder,
        public dialog: MatDialog,
        public snackBar: MatSnackBar
    ) {
        this.formBuilder();
    }

    formBuilder() {
      this.ctlTitle = this.fb.control('', [
        Validators.required
      ], [this.cardExists()]);
      this.frm = this.fb.group({
          title : this.ctlTitle,
      });
    }

    ngOnInit(): void {
      this.currentUser = this.authService.currentUser;
      this.boardService.get(this.boardId).subscribe(b => {
          this.board = b;
          this.lists = b.lists;
          this.lists.sort((a: List, b:List) => {
            return a.position - b.position;
          });
          this.lists.forEach(l => {
              this.getCards(l);
          });
      });
    }

    getCards(list: List) {
      console.log(list);
      return list.cards.sort((a: Card, b:Card) => {
        return a.position - b.position;
      });
    }

    cardExists(): AsyncValidatorFn {
      let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const title = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.cardService.cardExists(title, this.board.boardId).subscribe(card => {
                            resolve(!card ? { cardExists: true } : null);
                        })
                    }
                }, 300);
            });
        };
    }

    get f() { return this.frm.controls; }

    drop(event: CdkDragDrop<Card[]>, l: List) {
      if (event.previousContainer === event.container) {
        moveItemInArray(l.cards, event.previousIndex, event.currentIndex);
        this.cardService.changePositionCard(l, event.previousIndex,  event.currentIndex).subscribe();

      }
    }

    sortPredicate(index: number, item: CdkDrag<number>) {
        return (index + 1) % 2 === item.data % 2;
    }

    createList() {
      var list = new List({});
      const dlg = this.dialog.open(EditListComponent, {data: {list, boardId: this.boardId, isNew: true }});
      dlg.beforeClosed().subscribe(res => {
          if(res) {
              _.assign(list, res);
              this.listService.create(list).subscribe(res => {
                  if (!res) {
                      this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                  } else {
                    this.snackBar.open(`Success!!. A new List has been created!`, 'Dismiss', { duration: 10000 });
                  }
                  this.ngOnInit();
              });
          }
      });
    }

    updateList(list: List) {
        const dlg = this.dialog.open(EditListComponent, {data: {list, boardId: this.boardId, isNew: false }});
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                _.assign(list, res);
                this.listService.update(list).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                    }
                })
            }
        })
    }

    deleteList(list: List) {
        const dlg = this.dialog.open(ConfirmDeleteComponent, {data: {list, confirm : false }});
        dlg.beforeClosed().subscribe(res => {
          console.log(res)
            if(res && res.confirm == true) {
                _.assign(list, res);
                this.listService.deleteList(list).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        // this.ngOnInit();
                    } else {
                      this.snackBar.open(`SUCCESS!. The list has been deleted.`, 'Dismiss', { duration: 10000 });
                    }
                    this.ngOnInit();
                })
            }
        })
    }

    createCard(list: List) {
      var card = new Card({});
      card.title = this.f.title.value;
      card.listId = list.listId;
      card.authorId = this.currentUser.userId;
      this.cardService.create(card).subscribe(() => {
        card.openInput = false;
        this.f.title.setValue('');
        this.ngOnInit();
      });
    }

    updateCard(card: Card) {
      card.title = this.f.title.value;
      this.cardService.update(card).subscribe(() => {
        this.f.title.setValue('');
        this.ngOnInit();
      });
    }

    deleteCard(card: Card) {
      this.cardService.delete(card).subscribe(() => {
        this.ngOnInit();
      });
    }

    changeCard(board: Board, card: Card, oldList: List, newList: List) {
      this.cardService.changeCard(board, card, oldList, newList).subscribe(() => {
        this.ngOnInit();
      });
    }

    changeAllCards(l: List, list: List){
      this.listService.changeAllCard(l, list).subscribe(() => {
        this.ngOnInit();
      });
    }

    update(board: Board) {
        const dlg = this.dialog.open(EditBoardComponent, {data: {board, isNew: false }});
        dlg.beforeClosed().subscribe(res => {
            if(res) {
                _.assign(board, res);
                this.boardService.update(board).subscribe(res => {
                    if(!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                        this.ngOnInit();
                    }
                    this.ngOnInit();
                })
            }
        })
    }

    getUsersNotCollaborators(board: Board) {
      this.boardService.getUsersNotCollaborators(board).subscribe(users => {
        this.notcollaborators = users;
      });
    }

    get notCollaborators() {

      return this.getUsersNotCollaborators(this.board);
    }

    // usersNotParticipantsCard(card: Card) {
    //   let collaborators = this.board.collaborators;
    //   let participants = card.collaborators;
    //   var noPartipants = [];

    //   collaborators.forEach(c => {
    //     let i = 0;
    //     console.log(participants[i]);
    //     while(c.userId != participants[i].userId && i < participants.length){
    //       i++;
    //     }

    //     if(i == participants.length) {
    //       noPartipants.push(c);
    //       console.log(noPartipants.length);
    //     }
    //   });
    //   console.log(noPartipants.length);
    //   return noPartipants;
    // }

    addCollaborator(user: User) {
      let add = true;
      this.boardService.addCollaborator(this.board, user, add).subscribe(() => {
        this.getUsersNotCollaborators(this.board);
        this.ngOnInit();
      });
    }

    removeCollaborator(user: User) {
      var add: false;

      const dlg = this.dialog.open(ConfirmDeleteUserComponent, {data: {user: user, confirm : false, add: false }});
      dlg.beforeClosed().subscribe(res => {
        console.log(res)
          if(res && res.confirm == true) {
              _.assign(user, res);
              this.boardService.addCollaborator(this.board, user, res.add).subscribe(res => {
                console.log(res);
                  if(!res) {
                    this.snackBar.open(`There was an error at the server. The delete has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                  } else {
                    this.snackBar.open(`SUCCESS!. The collaborator and the cards of which he is the author have been deleted.`, 'Dismiss', { duration: 10000 });
                    this.getUsersNotCollaborators(this.board);
                    this.ngOnInit();
                  }
              })
          }
      })

    }

    resetBoard() {
      const dlg = this.dialog.open(ConfirmDeleteBoardComponent, {data: {board: this.board, confirm : false, deleteboard: false }});
      dlg.beforeClosed().subscribe(res => {
        console.log(res)
          if(res && res.confirm == true) {
              _.assign(this.board, res);
              this.boardService.resetBoard(this.board).subscribe(res => {
                  if(!res) {
                    this.snackBar.open(`SUCCESS!. All the elements of board have been deleted.`, 'Dismiss', { duration: 10000 });

                      // this.ngOnInit();
                  } else {
                    this.snackBar.open(`There was an error at the server. The reset has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                  }
                  this.ngOnInit();
              })
          }
      })

    }

    deleteBoard() {
      const dlg = this.dialog.open(ConfirmDeleteBoardComponent, {data: {board: this.board, confirm : false, deleteboard: true }});
      dlg.beforeClosed().subscribe(res => {
        console.log(res)
          if(res && res.confirm == true) {
              _.assign(this.board, res);
              this.boardService.deleteBoard(this.board).subscribe(res => {
                  if(!res) {
                    this.snackBar.open(`There was an error at the server. The delete has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                  } else {
                    this.snackBar.open(`SUCCESS!. The board have been deleted.`, 'Dismiss', { duration: 10000 });
                    this.router.navigate(['/boardlist']);
                  }
              })
          }
      })

    }

    moveRigth(l: List) {
      let moveRight = true;
      this.listService.moveRigth(this.board, l, moveRight).subscribe(() => {
        this.ngOnInit();
      });
    }

    moveLeft(l: List) {
      let moveRight = false;
      this.listService.moveRigth(this.board, l, moveRight).subscribe(() => {
        this.ngOnInit();
      });
    }

    changePositionList(l: List, i: number) {
      this.listService.changePositionList(this.board, l, i).subscribe(() => {
        this.ngOnInit();
      });
      console.log("ok move position")
    }

    addParticipant(card: Card, user: User) {
      let add = true;
      this.cardService.addParticipant(card, user, add).subscribe(() => {
        this.ngOnInit();
      });
      console.log("ok participant added")
    }

    removeParticipant(card:Card, user: User) {
      let add = false;
      this.cardService.addParticipant(card, user, add).subscribe(() => {
        this.ngOnInit();
      });
      console.log("ok participant removed")
    }

    DisplayDetailsOfCard(card: Card) {
      const dialogRef = this.dialog.open(DetailsCardComponent, {data: {card: card, confirm : false}});

      dialogRef.afterClosed().subscribe(result => {
        console.log(`Dialog result: ${result}`);
      });
    }

    openInputCard(card: Card) {
      card.openInput = true;
    }

    CloseInputCard(card: Card) {
      card.openInput = false;
    }

}
