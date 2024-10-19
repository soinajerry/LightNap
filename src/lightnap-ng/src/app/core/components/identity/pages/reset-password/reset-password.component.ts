
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ROUTE_HELPER, RoutePipe } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { BlockUIModule } from "primeng/blockui";
import { ButtonModule } from "primeng/button";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { take } from "rxjs";
import { AppConfigComponent } from "src/app/layout/config/app.config.component";
import { LayoutService } from "src/app/layout/service/app.layout.service";

@Component({
  standalone: true,
  templateUrl: "./reset-password.component.html",
  imports: [ReactiveFormsModule, RouterModule, ButtonModule, PasswordModule, InputTextModule, AppConfigComponent, BlockUIModule, RoutePipe],
})
export class ResetPasswordComponent {
  #identityService = inject(IdentityService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);
  layoutService = inject(LayoutService);

  form = this.#fb.group({
    email: this.#fb.control("", [Validators.required, Validators.email])
  });

  isResettingPassword = false;
  errors: Array<string> = [];

  resetPassword() {
    this.isResettingPassword = true;
    this.#identityService
      .resetPassword({email: this.form.value.email!})
      .pipe(take(1))
      .subscribe({
        next: response => {
          this.isResettingPassword = false;
          if (response.result) {
            this.#routeHelper.getRoute("reset-instructions-sent");
          } else {
            if (response.errorMessages?.length) {
              this.errors = response.errorMessages;
            } else {
              this.errors = ["An unexpected error occurred."];
            }
          }
        },
      });
  }
}
