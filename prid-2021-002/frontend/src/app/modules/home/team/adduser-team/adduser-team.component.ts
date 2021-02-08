import { Component, Inject, OnDestroy } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Team } from "src/app/core/models/team";
import { User } from "src/app/core/models/User";
import { AuthenticationService } from "src/app/core/services/authentication.service";
import { TeamService } from "src/app/core/services/team.service";
import { SearchService } from "src/app/core/services/searchuser.service";

@Component({
    selector: 'app-adduser-team',
    templateUrl: './adduser-team.component.html'
})

export class AdduserTeamComponent
{
    public frm:FormGroup;
    public ctlsearch:FormControl;
    currentUser:User;
    public isNew: boolean;

    constructor(
        public dialogRef:MatDialogRef<AdduserTeamComponent>,
        @Inject(MAT_DIALOG_DATA) public data:{team:Team,isNew: boolean},
        private fb:FormBuilder,
        private teamService:TeamService,
        private searchService:SearchService,
        private authService:AuthenticationService,
    ){
        this.currentUser = this.authService.currentUser;
        this.ctlsearch=this.fb.control('',[Validators.required],[]);
        this.frm=this.fb.group({searchvalu:this.ctlsearch});
        this.isNew = data.isNew;
        console.log(data);
        this.frm.patchValue(data.team);
    }

    update()
    {
        const data=this.frm.value;
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