import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ROUTE_HELPER, RoutePipe } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { ButtonModule } from "primeng/button";
import { take } from "rxjs";

@Component({
  selector: "app-access-denied",
  standalone: true,
  templateUrl: "./access-denied.component.html",
  imports: [CommonModule, RouterLink, RoutePipe, ButtonModule],
})
export class AccessDeniedComponent {
  #identityService = inject(IdentityService);
  #routeHelper = inject(ROUTE_HELPER);

  loggedIn$ = this.#identityService.watchLoggedIn$();

  constructor() {
    this.loggedIn$.pipe(take(1)).subscribe({
      next: loggedIn => {
        if (!loggedIn) {
          this.#routeHelper.navigate("login");
        }
      },
    });
  }

  logOut() {
    this.#identityService.logOut().subscribe({
      next: response => {
        if (response.result) {
          this.#routeHelper.navigate("login");
        }
      },
    });
  }
}
