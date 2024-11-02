import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { BlockUiService, ErrorListComponent, ToastService } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ProfileService } from "@profile/services/profile.service";
import { RouteAliasService } from "@routing";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { tap } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [CommonModule, ErrorListComponent, ReactiveFormsModule, ButtonModule, CardModule, ApiResponseComponent],
})
export class IndexComponent {
  #identityService = inject(IdentityService);
  #profileService = inject(ProfileService);
  #routeAlias = inject(RouteAliasService);
  #blockUi = inject(BlockUiService);
  #toast = inject(ToastService);
  #fb = inject(FormBuilder);

  form = this.#fb.group({});
  errors = new Array<string>();

  profile$ = this.#profileService.getProfile().pipe(
    tap(response => {
      if (!response.result) return;
      // Set form values.
    })
  );

  updateProfile() {
    this.#blockUi.show({ message: "Updating profile..." });
    this.#profileService.updateProfile({}).subscribe({
      next: response => {
        if (!response.result) {
          this.errors = response.errorMessages;
          return;
        }

        this.#toast.success("Profile updated successfully.");
      },
      complete: () => this.#blockUi.hide(),
    });
  }

  logOut() {
    this.#blockUi.show({ message: "Logging out..." });
    this.#identityService.logOut().subscribe({
      next: response => {
        if (!response.result) {
          this.errors = response.errorMessages;
          return;
        }
        this.#routeAlias.navigate("landing");
      },
      complete: () => this.#blockUi.hide(),
    });
  }
}
