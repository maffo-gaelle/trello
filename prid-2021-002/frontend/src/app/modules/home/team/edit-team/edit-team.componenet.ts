import { Component, Inject, OnDestroy } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Team } from "src/app/core/models/team";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { TeamService } from "src/app/core/services/team.service";

@Component({
    selector: 'app-edit-team',
    templateUrl: './edit-team.component.html'
})
export class EditTeamComponent
{
    public frm: FormGroup;
    public ctlTeamname: FormControl;
    currentUser: User;
    public isNew: boolean;

    constructor(
        public dialogRef: MatDialogRef<EditTeamComponent>,
        @Inject(MAT_DIALOG_DATA) public data: {team: Team; isNew: boolean},
        private fb: FormBuilder,
        private teamService: TeamService,
        private authService : AuthenticationService,
    ){
        this.currentUser = this.authService.currentUser;
        this.ctlTeamname = this.fb.control('',[Validators.required],[]);
        this.frm = this.fb.group({teamname : this.ctlTeamname});
        console.log(data);
        this.isNew = data.isNew;
        this.frm.patchValue(data.team);
    }

    update()
    {
        const data = this.frm.value;
        console.log(data);
        this.dialogRef.close(data);
    }

    onNoClick(): void
    {
        this.dialogRef.close();
    }

    cancel()
    {
        this.dialogRef.close();
    }
}