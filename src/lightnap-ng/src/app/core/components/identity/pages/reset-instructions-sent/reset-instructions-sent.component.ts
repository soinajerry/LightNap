
import { Component, inject } from "@angular/core";
import { RouterModule } from "@angular/router";
import { RoutePipe } from "@core";
import { AppConfigComponent } from "src/app/layout/components/controls/app-config/app-config.component";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./reset-instructions-sent.component.html",
  imports: [AppConfigComponent, RouterModule, RoutePipe],
})
export class ResetInstructionsSentComponent {
  layoutService = inject(LayoutService);
}
