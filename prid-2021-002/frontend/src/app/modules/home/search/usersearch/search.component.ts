import { Component, Inject, OnDestroy } from "@angular/core";
import { FormBuilder, FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { Team } from "src/app/core/models/team";
import { User } from "src/app/core/models/User";
import { UserService } from "src/app/core/services/user.service";
import { TeamService } from "src/app/core/services/team.service";
import { SearchService} from "src/app/core/services/searchuser.service";
@Component({
    selector: 'app-search',
    templateUrl: './search.component.html'
})
export class SearchComponent
{
    public frm: FormGroup;
    public ctlSearch: FormControl;
    public searchValue:String;
    public isIn: boolean;
    public teams:Team[];
    public users:User[];
    public results:String[];

    constructor(
        public dialogRef: MatDialogRef<SearchComponent>,
        private fb: FormBuilder,
        private teamService: TeamService,
        private userService: UserService,
        private searchService: SearchService
    ){
        this.ctlSearch=this.fb.control('',[Validators.required],[]);
        this.frm=this.fb.group({search:this.ctlSearch});
        this.teamService.getAll().subscribe(teams=>{this.teams=teams;})
        this.userService.getCollection().subscribe(users=>{this.users=users;})
    }

    onNoClick(): void
    {
        this.dialogRef.close();
    }

    cancel()
    {
        this.dialogRef.close();
    }

    search()
    {
        this.searchService.search(this.searchValue);
    }

    onClick()
    {
        
    }
}