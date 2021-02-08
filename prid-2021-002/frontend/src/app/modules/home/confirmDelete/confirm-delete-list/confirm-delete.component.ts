import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { List } from "src/app/core/models/list";

@Component({
    selector: 'app-confirm-delete',
    templateUrl: 'confirm-delete.component.html',
    styleUrls: ['./confirm-delete.component.scss'],
  })
  export class ConfirmDeleteComponent {

    constructor(
      public dialogRef: MatDialogRef<ConfirmDeleteComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {list: List, confirm: boolean}) {}

    onNoClick(): void {
        this.data.confirm = false;
        this.dialogRef.close(this.data);
    }

    onClick(): void {
        this.data.confirm = true;
        this.dialogRef.close(this.data);
    }

  }
