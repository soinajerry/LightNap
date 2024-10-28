import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { RouterLink } from "@angular/router";
import { RoutePipe } from "@routing";
import { IdentityService } from "src/app/identity/services/identity.service";
import { ButtonModule } from "primeng/button";
import { take } from "rxjs";
import { LayoutService } from "src/app/layout/services/layout.service";
import { RouteAliasService } from "@routing";

@Component({
  standalone: true,
  templateUrl: "./access-denied.component.html",
  imports: [CommonModule, RouterLink, RoutePipe, ButtonModule],
})
export class AccessDeniedComponent {
    layoutService = inject(LayoutService);
  #identityService = inject(IdentityService);
  #routeAliasService = inject(RouteAliasService);

  loggedIn$ = this.#identityService.watchLoggedIn$();

  constructor() {
    this.loggedIn$.pipe(take(1)).subscribe({
      next: loggedIn => {
        if (!loggedIn) {
          this.#routeAliasService.navigate("login");
        }
      },
    });
  }

  logOut() {
    this.#identityService.logOut().subscribe({
      next: response => {
        if (response.result) {
          this.#routeAliasService.navigate("login");
        }
      },
    });
  }
}
