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
import { FocusContentLayout } from "src/app/layout/components/controls/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";
import { ErrorListComponent } from "../../../controls/error-list/error-list.component";

@Component({
  standalone: true,
  templateUrl: "./reset-password.component.html",
  imports: [ReactiveFormsModule, RouterModule, ButtonModule, PasswordModule, InputTextModule, RoutePipe, FocusContentLayout, BlockUIModule, ErrorListComponent],
})
export class ResetPasswordComponent {
  #identityService = inject(IdentityService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);
  layoutService = inject(LayoutService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
  });

  blockUi = false;
  errors: Array<string> = [];

  resetPassword() {
    this.blockUi = true;
    this.#identityService
      .resetPassword({ email: this.form.value.email })
      .pipe(take(1))
      .subscribe({
        next: response => {
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
        complete: () => (this.blockUi = false),
      });
  }
}
