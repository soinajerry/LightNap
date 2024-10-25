import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot } from "@angular/router";
import { ROUTE_HELPER } from "@core/helpers";
import { IdentityService } from "src/app/identity/services/identity.service";
import { map } from "rxjs";

export const authGuard = (next: ActivatedRouteSnapshot) => {
  const routeHelper = inject(ROUTE_HELPER);
  return inject(IdentityService)
    .watchLoggedIn$()
    .pipe(map(isLoggedIn => (isLoggedIn ? true : createUrlTreeFromSnapshot(next, routeHelper.getRoute("login")))));
};
