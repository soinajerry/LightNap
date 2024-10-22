import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { RouteHelper, SupportedRoutes } from "@core";

export class AppRouteHelper implements RouteHelper {
  #router = inject(Router);

  #root = ["/"];

  #identity = ["/", "identity"];
  #login = [...this.#identity, "login"];
  #resetPassword = [...this.#identity, "reset-password"];
  #resetInstructionsSent = [...this.#identity, "reset-instructions-sent"];
  #newPassword = [...this.#identity, "new-password"];
  #register = [...this.#identity, "register"];
  #verifyCode = [...this.#identity, "verify-code"];

  #profile = ["/", "profile"];
  #devices = [...this.#profile, "devices"];
  #changePassword = [...this.#profile, "change-password"];

  #admin = ["/", "admin"];
  #adminUsers = [...this.#admin, "users"];
  #adminRoles = [...this.#admin, "roles"];
  #adminRole = [...this.#admin, "role"];

  navigate(view: SupportedRoutes, value?: any) {
    return this.#router.navigate(this.getRoute(view, value));
  }

  navigateWithReplace(view: SupportedRoutes, value?: any) {
    return this.#router.navigate(this.getRoute(view, value), { replaceUrl: true });
  }

  getRoute(view: SupportedRoutes, value?: any) {
    switch (view) {
      case "landing":
        return this.#root;
      case "home":
        return [...this.#root, "home"];
      case "about":
        return [...this.#root, "about"];
      case "terms-and-conditions":
        return [...this.#root, "terms-and-conditions"];
      case "privacy-policy":
        return [...this.#root, "privacy-policy"];
      case "access-denied":
        return [...this.#root, "access-denied"];

      case "profile":
        return this.#profile;
      case "devices":
        return this.#devices;
      case "change-password":
        return this.#changePassword;

      case "login":
        return this.#login;
      case "reset-instructions-sent":
        return this.#resetInstructionsSent;
      case "reset-password":
        return this.#resetPassword;
      case "new-password":
        return this.#newPassword;
      case "register":
        return this.#register;
      case "verify-code":
        if (!value) {
          return this.#verifyCode;
        }
        return this.#getVerifyCode(value as string);
      case "admin":
        return this.#admin;
      case "admin-users":
        return this.#adminUsers;
      case "admin-user":
        return [...this.#adminUsers, value];
      case "admin-roles":
        return this.#adminRoles;
        case "admin-role":
            return [...this.#adminRoles, value];
        }

    throw new Error(`Unexpected view '${view}'. Did you add a path for it in AppRouteHelper?`);
  }

  #getVerifyCode(email: string) {
    return [...this.#verifyCode, email];
  }
}
