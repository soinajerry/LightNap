import { inject } from "@angular/core";
import { ActivatedRouteSnapshot, createUrlTreeFromSnapshot } from "@angular/router";
import { IdentityService } from "src/app/identity/services/identity.service";
import { map, take } from "rxjs";
import { RouteAliasService } from "@routing";

export const authGuard = (next: ActivatedRouteSnapshot) => {
  const routeAliasService = inject(RouteAliasService);
  return inject(IdentityService)
    .watchLoggedIn$()
    .pipe(take(1), map(isLoggedIn => (isLoggedIn ? true : createUrlTreeFromSnapshot(next, routeAliasService.getRoute("login")))));
};
