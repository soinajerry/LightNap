
import { Component, inject } from "@angular/core";
import { RouterModule } from "@angular/router";
import { RoutePipe } from "@core";
import { AppConfigComponent } from "src/app/layout/config/app.config.component";
import { LayoutService } from "src/app/layout/service/app.layout.service";

@Component({
  standalone: true,
  templateUrl: "./reset-instructions-sent.component.html",
  imports: [AppConfigComponent, RouterModule, RoutePipe],
})
export class ResetInstructionsSentComponent {
  layoutService = inject(LayoutService);
}
