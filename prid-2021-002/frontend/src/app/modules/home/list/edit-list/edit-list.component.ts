import { Component, Inject, OnDestroy } from "@angular/core";
import { AsyncValidatorFn, FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Board } from "src/app/core/models/board";
import { List } from "src/app/core/models/list";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { BoardService } from "src/app/core/services/board.service";
import { ListService } from "src/app/core/services/list.service";

@Component({
    selector: 'app-edit-list',
    templateUrl: './edit-list.component.html',
    styleUrls: ['edit-list.component.scss']
})
export class EditListComponent {

    public frm: FormGroup;
    public ctlTitle: FormControl;
    public authorId: number;
    currentUser: User;
    public isNew: boolean;
    public board: Board;

    constructor(
        public dialogRef: MatDialogRef<EditListComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { list: List; boardId: number; isNew: boolean},
        private fb: FormBuilder,
        private boardService: BoardService,
        private listService: ListService,
        private authService : AuthenticationService,
    ) {
        this.currentUser = this.authService.currentUser;
        //this.board = this.boardService.get(this.data.boardId).subscribe();

        this.ctlTitle = this.fb.control('', [
            Validators.required
        ], [this.listExists()]); 

        this.frm = this.fb.group({
            title : this.ctlTitle
        });

        console.log(data);
        this.isNew = data.isNew;
        this.frm.patchValue(data.list);
    }
    listExists(): AsyncValidatorFn {
        let timeout: NodeJS.Timer;
        return (ctl: FormControl) => {
            clearTimeout(timeout);
            const title = ctl.value;
            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if(ctl.pristine) {
                        resolve(null);
                    } else {
                        this.listService.listExists(title, this.data.boardId).subscribe(list => {
                            resolve(!list ? { listExists: true } : null);
                        })
                    }
                }, 300);
            });
        };
    }

    update() {
        const data = this.frm.value;
        // data.authorId = this.currentUser.userId;
        data.boardId = this.data.boardId;
        console.log(data);
        this.dialogRef.close(data);
    }

    onNoClick(): void {
        this.dialogRef.close();
    }

    cancel() {
        this.dialogRef.close();
    }

}
