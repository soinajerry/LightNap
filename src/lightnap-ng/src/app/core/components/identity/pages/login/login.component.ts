import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ROUTE_HELPER, RoutePipe } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";
import { ErrorListComponent } from "../../../controls/error-list/error-list.component";
import { BlockUIModule } from "primeng/blockui";

@Component({
  standalone: true,
  templateUrl: "./login.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    InputTextModule,
    CheckboxModule,
    RoutePipe,
    PasswordModule,
    FocusContentLayout,
    ErrorListComponent,
    BlockUIModule
  ],
})
export class LoginComponent {
  layoutService = inject(LayoutService);
  #identityService = inject(IdentityService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    rememberMe: this.#fb.control(true),
  });

  errors: Array<string> = [];
  blockUi = false;

  logIn() {
    this.blockUi = true;

    this.#identityService
      .logIn({
        email: this.form.value.email,
        password: this.form.value.password,
        rememberMe: this.form.value.rememberMe,
        deviceDetails: navigator.userAgent,
      })
      .subscribe({
        next: response => {
          if (!response?.result) {
            if (response?.errorMessages?.length) {
              this.errors = response.errorMessages;
            } else {
              this.errors = ["An unexpected error occurred."];
            }
          } else if (response.result.twoFactorRequired) {
            this.#routeHelper.navigate("verify-code", this.form.value.email);
          } else {
            this.#routeHelper.navigate("home");
          }
        },
        complete: () => (this.blockUi = false),
      });
  }
}
