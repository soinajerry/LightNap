import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ROUTE_HELPER } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { IdentityService } from "src/app/identity/services/identity.service";
import { ProfileService } from "@profile/services/profile.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, TableModule, ButtonModule, CardModule, ApiResponseComponent],
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
