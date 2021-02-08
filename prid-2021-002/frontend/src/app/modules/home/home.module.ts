import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HomeComponent } from './home/home.component';
import { HomeRoutingModule } from './home.routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { WidgetModule } from '../../core/widgets/widgets.module';
import { RestrictedComponent } from './restricted/RestrictedComponent';
import { LoginComponent } from './login/login.component';
import { UnknownComponent } from './Unknown/unknown.component';
import { SignupComponent } from './signup/signup.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { UserListComponent } from './users/userlist/userlist.component';
import { LoggedOutComponent } from './logged-out/logged-out.component';
import { TeamListComponent } from './team/teamlist/teamlist.component';
import { AdduserTeamComponent } from './team/adduser-team/adduser-team.component';
import { ShowTeamComponent } from './team/show-team/show-team.component';
import { EditTeamComponent } from './team/edit-team/edit-team.componenet';
import { BoardListComponent } from './board/boardlist/boardlist.component';
import { ShowBoardComponent } from './board/show-board/show-board.component';
import { EditBoardComponent } from './board/edit-board/edit-board.componenet';
import { EditListComponent } from './list/edit-list/edit-list.component';
import { ConfirmDeleteComponent } from './confirmDelete/confirm-delete-list/confirm-delete.component';
import { FooterComponent } from 'src/app/core/widgets/footer/footer.component';
import { ConfirmDeleteBoardComponent } from './confirmDelete/confirm-delete-board/confirm-delete-board.component';
import { ConfirmDeleteUserComponent } from './confirmDelete/confirm-delete-user/confirm-delete-user.component';
import { DetailsCardComponent } from './card/details-card.component';

@NgModule({
    declarations:[
        HomeComponent,
        RestrictedComponent,
        LoginComponent,
        UnknownComponent,
        SignupComponent,
        EditUserComponent,
        UserListComponent,
        AdduserTeamComponent,
        TeamListComponent,
        BoardListComponent,
        LoggedOutComponent,
        ShowTeamComponent,
        EditTeamComponent,
        ShowBoardComponent,
        EditBoardComponent,
        EditListComponent,
        ConfirmDeleteComponent,
        ConfirmDeleteBoardComponent,
        ConfirmDeleteUserComponent,
        DetailsCardComponent,
        FooterComponent
    ],
    imports:[
        BrowserModule,
        CommonModule,
        HomeRoutingModule,
        WidgetModule,
        FormsModule,
        ReactiveFormsModule
    ],
    exports:[
        CommonModule,
        BrowserModule,
    ],
    entryComponents:[AdduserTeamComponent, EditUserComponent, EditBoardComponent, EditListComponent, ConfirmDeleteComponent, ConfirmDeleteBoardComponent, ConfirmDeleteUserComponent, DetailsCardComponent],
    schemas:[CUSTOM_ELEMENTS_SCHEMA],
    providers:[]
})

export class HomeModule{
}
