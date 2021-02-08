import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { User } from "src/app/core/models/User";

@Component({
    selector: 'app-confirm-delete-user',
    templateUrl: 'confirm-delete-user.component.html',
    styleUrls: ['./confirm-delete-user.component.scss'],
  })
  export class ConfirmDeleteUserComponent {

    constructor(
      public dialogRef: MatDialogRef<ConfirmDeleteUserComponent>,
      @Inject(MAT_DIALOG_DATA) public data: {user: User, confirm: boolean, add: boolean}) {}

    onNoClick(): void {
        this.data.confirm = false;
        this.dialogRef.close(this.data);
    }

    onClick(): void {
        this.data.confirm = true;
        this.dialogRef.close(this.data);
    }

  }
