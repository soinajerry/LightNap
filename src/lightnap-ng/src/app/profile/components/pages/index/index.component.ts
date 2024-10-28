import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { IdentityService } from "src/app/identity/services/identity.service";
import { ProfileService } from "@profile/services/profile.service";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";
import { RouteAliasService } from "@routing";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, TableModule, ButtonModule, CardModule, ApiResponseComponent],
})
export class IndexComponent {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  #routeAliasService = inject(RouteAliasService);

  profile$ = this.#profileService.getProfile();

  logOut() {
    this.#identityService.logOut().subscribe({
      next: response => {
        if (response.result) {
          this.#routeAliasService.navigate("landing");
        }
      },
    });
  }
}
