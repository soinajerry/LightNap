import { Component } from "@angular/core";
import { RouterModule } from "@angular/router";
import { RoutePipe } from "@routing";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";

@Component({
  standalone: true,
  templateUrl: "./reset-instructions-sent.component.html",
  imports: [RouterModule, RoutePipe, FocusContentLayout],
})
export class ResetInstructionsSentComponent {}
