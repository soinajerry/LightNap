
import { Component, Input, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ROUTE_HELPER, RoutePipe } from "@core";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { IdentityService } from "@core/services/identity.service";
import { BlockUIModule } from "primeng/blockui";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { PasswordModule } from "primeng/password";
import { take } from "rxjs";
import { AppConfigComponent } from "src/app/layout/components/controls/app-config/app-config.component";
import { FocusContentLayout } from "src/app/layout/components/controls/focus-content-layout/focus-content-layout.component";
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
    BlockUIModule,
    RoutePipe,
    ErrorListComponent,
    FocusContentLayout
],
})
export class NewPasswordComponent {
  #identityService = inject(IdentityService);
  layoutService = inject(LayoutService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);

  @Input() email = "";
  @Input() token = "";

  blockUi: boolean = false;
  errors: Array<string> = [];

  form = this.#fb.nonNullable.group({
    password: this.#fb.control("", [Validators.required]),
    confirmPassword: this.#fb.control("", [Validators.required]),
    rememberMe: this.#fb.control(false),
  });

  constructor() {
    this.form.addValidators(() => {
      if (this.form.controls.password.value !== this.form.controls.confirmPassword.value) return { match_error: "Passwords must match." };
      return null;
    });
  }

  newPassword() {
    this.blockUi = true;
    this.#identityService
      .newPassword({
        email: this.email,
        password: this.form.value.password,
        token: this.token,
        deviceDetails: navigator.userAgent,
        rememberMe: this.form.value.rememberMe
      })
      .pipe(take(1))
      .subscribe({
        next: response => {
          if (response?.result) {
            this.#routeHelper.navigate("home");
          } else if (response?.errorMessages?.length) {
            this.errors = response?.errorMessages;
          } else {
            this.errors = ["An unexpected error occurred."];
          }
        },
        complete: () => (this.blockUi = false),
      });
  }
}
