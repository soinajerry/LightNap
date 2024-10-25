import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot } from "@angular/router";
import { ROUTE_HELPER } from "@core/helpers";
import { IdentityService } from "src/app/identity/services/identity.service";
import { map } from "rxjs";

export const adminGuard = (next: ActivatedRouteSnapshot) => {
  const authenticationService = inject(IdentityService);
  const routeHelper = inject(ROUTE_HELPER);

  return authenticationService
    .watchLoggedInToRole$("Administrator")
    .pipe(map(isAdmin => (isAdmin ? true : createUrlTreeFromSnapshot(next, routeHelper.getRoute("access-denied")))));
};
