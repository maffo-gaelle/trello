import { Component, Inject, OnDestroy } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Board } from "src/app/core/models/board";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { BoardService } from "src/app/core/services/board.service";

@Component({
    selector: 'app-edit-board',
    templateUrl: './edit-board.component.html',
    styleUrls: ['edit-board.component.scss']
})
export class EditBoardComponent {

    public frm: FormGroup;
    public ctlTitle: FormControl;
    public authorId: number;
    currentUser: User;
    public isNew: boolean;

    constructor(
        public dialogRef: MatDialogRef<EditBoardComponent>,
        @Inject(MAT_DIALOG_DATA) public data: { board: Board; isNew: boolean},
        private fb: FormBuilder,
        private boardService: BoardService,
        private authService : AuthenticationService,
    ) {
        this.currentUser = this.authService.currentUser;
        this.ctlTitle = this.fb.control('', [
            Validators.required
        ], []);
        this.frm = this.fb.group({
            title : this.ctlTitle,
            authorId : this.currentUser.userId
        });
        console.log(data);
        this.isNew = data.isNew;
        this.frm.patchValue(data.board);
    }

    update() {
        const data = this.frm.value;
        data.authorId = this.currentUser.userId;
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
