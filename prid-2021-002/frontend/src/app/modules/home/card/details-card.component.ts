import { Component, Inject } from "@angular/core";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Card } from "src/app/core/models/card";
import { List } from "src/app/core/models/list";
import { CardService } from "src/app/core/services/card.service";

@Component({
  selector: 'app-details-card',
  templateUrl: 'details-card.component.html',
  styleUrls: ['./details-card.component.scss']
})
export class DetailsCardComponent {

  list: List;

  constructor(
    public dialogRef: MatDialogRef<DetailsCardComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {card: Card, confirm: boolean},
    public cardService: CardService
    ) {
      this.cardService.getList(this.data.card).subscribe(l => {
        this.list = l;
      });
    }

  onNoClick(): void {
      this.data.confirm = false;
      this.dialogRef.close(this.data);
  }

  onClick(): void {
      this.data.confirm = true;
      this.dialogRef.close(this.data);
  }
}
