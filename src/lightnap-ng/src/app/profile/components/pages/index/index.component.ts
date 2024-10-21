import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ROUTE_HELPER } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { ProfileService } from "@profile/services/profile.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, TableModule, ButtonModule, CardModule],
})
export class IndexComponent {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  #routeHelper = inject(ROUTE_HELPER);

  profile$ = this.#profileService.getProfile();

  logOut() {
    this.#identityService.logOut().subscribe({
      next: response => {
        if (response.result) {
          this.#routeHelper.navigate("landing");
        }
      },
    });
  }
}
