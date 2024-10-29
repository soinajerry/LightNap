import { Component, Input, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService } from "@core";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { confirmPasswordValidator } from "@core/helpers/form-helpers";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { PasswordModule } from "primeng/password";
import { IdentityService } from "src/app/identity/services/identity.service";
import { AppConfigComponent } from "src/app/layout/components/controls/app-config/app-config.component";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./new-password.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    ButtonModule,
    PasswordModule,
    CheckboxModule,
    AppConfigComponent,
    RoutePipe,
    ErrorListComponent,
    FocusContentLayout,
  ],
})
export class NewPasswordComponent {
  #identityService = inject(IdentityService);
  #blockUi = inject(BlockUiService);
  layoutService = inject(LayoutService);
  #fb = inject(FormBuilder);
  #routeAliasService = inject(RouteAliasService);

  @Input() email = "";
  @Input() token = "";

  errors: Array<string> = [];

  form = this.#fb.nonNullable.group(
    {
      password: this.#fb.control("", [Validators.required]),
      confirmPassword: this.#fb.control("", [Validators.required]),
      rememberMe: this.#fb.control(false),
    },
    { validators: [confirmPasswordValidator("password", "confirmPassword")] }
  );

  newPassword() {
    this.#blockUi.show({ message: "Setting new password..." });
    this.#identityService
      .newPassword({
        email: this.email,
        password: this.form.value.password,
        token: this.token,
        deviceDetails: navigator.userAgent,
        rememberMe: this.form.value.rememberMe,
      })
      .subscribe({
        next: response => {
          if (response?.result) {
            this.#routeAliasService.navigate("user-home");
          } else if (response?.errorMessages?.length) {
            this.errors = response?.errorMessages;
          } else {
            this.errors = ["An unexpected error occurred."];
          }
        },
        complete: () => this.#blockUi.hide(),
      });
  }
}
