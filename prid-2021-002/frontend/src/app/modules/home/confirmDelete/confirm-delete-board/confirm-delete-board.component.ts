import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Board } from "src/app/core/models/board";

@Component({
    selector: 'app-confirm-delete-board',
    templateUrl: 'confirm-delete-board.component.html',
    styleUrls: ['./confirm-delete-board.component.scss']
  })
  export class ConfirmDeleteBoardComponent {
    deleteBoard: boolean = false;

    constructor(
      public dialogRef: MatDialogRef<ConfirmDeleteBoardComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {board: Board, confirm: boolean, deleteboard: boolean}) {}

    onNoClick(): void {
        this.data.confirm = false;
        this.dialogRef.close(this.data);
    }

    onClick(): void {
        this.data.confirm = true;
        this.dialogRef.close(this.data);
    }

  }
