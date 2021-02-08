import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Role } from 'src/app/core/models/User';
import { AuthGuard } from 'src/app/core/services/auth.guard';
import { TeamListComponent } from './team/teamlist/teamlist.component';
import { ShowTeamComponent } from './team/show-team/show-team.component';
import { BoardListComponent } from './board/boardlist/boardlist.component';
import { ShowBoardComponent } from './board/show-board/show-board.component';
import { HomeComponent } from './home/home.component';
import { LoggedOutComponent } from './logged-out/logged-out.component';
import { LoginComponent } from './login/login.component';
import { RestrictedComponent } from './restricted/RestrictedComponent';
import { SignupComponent } from './signup/signup.component';
import { UnknownComponent } from './Unknown/unknown.component';
import { EditUserComponent } from './users/edit-user/edit-user.component';
import { UserListComponent } from './users/userlist/userlist.component';

const routes:Routes=[
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'login', component: LoginComponent , data: {title: 'PAGE_TITLE.LOGIN'}},
    { path: 'signup', component: SignupComponent, data: {title: 'PAGE_TITLE.SIGNUP'}},
    { path: 'userlist', component: UserListComponent, canActivate: [AuthGuard], data: {title: 'PAGE_TITLE.USERS'}},
    { path: 'teamlist', component: TeamListComponent, data: {title: 'PAGE_TITLE.TEAMS'}},
    { path: 'show-team/:id', component: ShowTeamComponent, data: {title: 'PAGE_TITLE.TEAM'}},
    { path: 'boardlist', component: BoardListComponent, data: {title: 'PAGE_TITLE.BOARDS'}},
    { path: 'show-board/:id', component: ShowBoardComponent, data: {title: 'PAGE_TITLE.BOARD'}},
    { path: 'logged-out', component: LoggedOutComponent, data: {title: 'PAGE_TITLE.LOGGEDOUT'}},
    { path: 'restricted', component: RestrictedComponent },
    { path: '**', component: UnknownComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class HomeRoutingModule
{
}
