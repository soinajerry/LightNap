import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService, ErrorListComponent } from "@core";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { take } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./reset-password.component.html",
  imports: [ReactiveFormsModule, RouterModule, ButtonModule, PasswordModule, InputTextModule, RoutePipe, FocusContentLayout, ErrorListComponent],
})
export class ResetPasswordComponent {
  #identityService = inject(IdentityService);
  #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeAlias = inject(RouteAliasService);
  layoutService = inject(LayoutService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
  });

  errors: Array<string> = [];

  resetPassword() {
    this.#blockUi.show({ message: "Resetting password..." });
    this.#identityService
      .resetPassword({ email: this.form.value.email })
      .pipe(take(1))
      .subscribe({
        next: response => {
          if (response.result) {
            this.#routeAlias.getRoute("reset-instructions-sent");
          } else {
            if (response.errorMessages?.length) {
              this.errors = response.errorMessages;
            } else {
              this.errors = ["An unexpected error occurred."];
            }
          }
        },
        complete: () => this.#blockUi.hide(),
      });
  }
}
