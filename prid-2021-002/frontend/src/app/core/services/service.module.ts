import { CommonModule } from "@angular/common";
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { AuthGuard } from "./auth.guard";
import { AuthenticationService } from "./authentication.service";
import { BoardService } from "./board.service";
import { ListService } from "./list.service";
import { StateService } from "./state.service";
import { UserService } from "./user.service";

@NgModule({
    imports : [
        CommonModule,
        HttpClientModule,
    ],
    providers: [
        UserService,
        AuthGuard,
        AuthenticationService,
        StateService,
        BoardService,
        ListService
    ]
})

export class ServiceModule{}