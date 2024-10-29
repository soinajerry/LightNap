import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService, ErrorListComponent } from "@core";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { IdentityService } from "src/app/identity/services/identity.service";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";

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
  ],
})
export class LoginComponent {
  layoutService = inject(LayoutService);
  #identityService = inject(IdentityService);
  #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeAliasService = inject(RouteAliasService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    rememberMe: this.#fb.control(true),
  });

  errors: Array<string> = [];

  logIn() {
    this.#blockUi.show({ message: "Logging in..." });

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
            this.#routeAliasService.navigate("verify-code", this.form.value.email);
          } else {
            this.#routeAliasService.navigate("user-home");
          }
        },
        complete: () => this.#blockUi.hide(),
      });
  }
}
