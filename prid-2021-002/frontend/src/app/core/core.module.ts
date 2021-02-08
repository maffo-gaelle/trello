import { NgModule } from "@angular/core";
import { ServiceModule } from "./services/service.module";
import { WidgetModule } from "./widgets/widgets.module";

@NgModule({
    imports: [
        ServiceModule,
        WidgetModule
    ],
    providers: [
    ],
    declarations: [

    ]
})

export class CoreModule {}