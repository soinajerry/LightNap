
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { RoutePipe, ROUTE_HELPER, BlockUiService } from "@core";
import { IdentityService } from "src/app/identity/services/identity.service";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { AppConfigComponent } from "src/app/layout/components/controls/app-config/app-config.component";
import { LayoutService } from "src/app/layout/services/layout.service";
import { BlockUIModule } from "primeng/blockui";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { confirmPasswordValidator } from "@core/helpers/form-helpers";

@Component({
  standalone: true,
  templateUrl: "./register.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    ButtonModule,
    PasswordModule,
    CheckboxModule,
    AppConfigComponent,
    RoutePipe,
    ErrorListComponent,
    FocusContentLayout
],
})
export class RegisterComponent {
  #identityService = inject(IdentityService);
    #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);
  layoutService = inject(LayoutService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    confirmPassword: this.#fb.control("", [Validators.required]),
    userName: this.#fb.control("", [Validators.required]),
    agreedToTerms: this.#fb.control(false, [Validators.requiredTrue]),
    rememberMe: this.#fb.control(true),
  },
  { validators: [confirmPasswordValidator("password", "confirmPassword")] }
);

  errors: Array<string> = [];

  register() {
    this.#blockUi.show({ message: "Registering..." });

    this.#identityService.register({
        email: this.form.value.email,
        password: this.form.value.password,
        confirmPassword: this.form.value.confirmPassword,
        deviceDetails: navigator.userAgent,
        rememberMe: this.form.value.rememberMe,
        userName: this.form.value.userName
      }).subscribe({
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
      complete: () => this.#blockUi.hide(),
    });
  }
}
