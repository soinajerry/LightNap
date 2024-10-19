import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ROUTE_HELPER, RoutePipe } from "@core";
import { IdentityService } from "@core/services/identity.service";
import { BlockUIModule } from "primeng/blockui";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { AppConfigComponent } from "src/app/layout/config/app.config.component";
import { LayoutService } from "src/app/layout/service/app.layout.service";

@Component({
  standalone: true,
  templateUrl: "./login.component.html",
  imports: [
    ReactiveFormsModule,
    AppConfigComponent,
    RouterModule,
    ButtonModule,
    InputTextModule,
    CheckboxModule,
    BlockUIModule,
    RoutePipe,
    PasswordModule,
  ],
})
export class LoginComponent {
  layoutService = inject(LayoutService);
  #identityService = inject(IdentityService);
  #fb = inject(FormBuilder);
  #routeHelper = inject(ROUTE_HELPER);

  form = this.#fb.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    rememberMe: this.#fb.control(true),
  });

  errors: Array<string> = [];

  logIn() {
    const value = this.form.value;

    this.#identityService
      .logIn({
        email: value.email!,
        password: value.password!,
        rememberMe: value.rememberMe!,
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
            this.#routeHelper.navigate("verify-code", value.email!);
          } else {
            this.#routeHelper.navigate("home");
          }
        },
      });
  }
}
