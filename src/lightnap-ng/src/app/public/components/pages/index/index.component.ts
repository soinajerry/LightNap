import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { Router, RouterLink } from "@angular/router";
import { RoutePipe } from "@routing";
import { IdentityService } from "src/app/identity/services/identity.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DividerModule } from "primeng/divider";
import { RippleModule } from "primeng/ripple";
import { StyleClassModule } from "primeng/styleclass";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, DividerModule, StyleClassModule, ButtonModule, RippleModule, RouterLink, RoutePipe, CardModule],
})
export class IndexComponent {
  layoutService = inject(LayoutService);
  router = inject(Router);
  identityService = inject(IdentityService);

  loggedIn$ = this.identityService.watchLoggedIn$().pipe(takeUntilDestroyed());


}
