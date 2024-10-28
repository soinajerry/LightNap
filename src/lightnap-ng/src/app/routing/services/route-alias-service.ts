import { inject, Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { RouteAlias } from "../models/route-alias";
import { AppRoute } from "../models/app-route";

@Injectable({
  providedIn: "root",
})
export class RouteAliasService {
  #router = inject(Router);
  #routeMap = new Map<RouteAlias, Array<string>>();

  constructor() {
    this.#router.config.forEach(route => this.#buildRoutes(route, ["/"]));
  }

  #buildRoutes(route: AppRoute, path: Array<string>) {
    if (route.path?.length) {
      path.push(route.path);
    }

    const alias = route.data?.alias;
    if (alias) {
      if (this.#routeMap.has(alias)) {
        const exisitingPath = this.#routeMap.get(alias).join("/");
        const newPath = path.join("/");
        throw new Error(`Duplicate route for '${alias}': Two route paths have the same alias. See route: '${exisitingPath}' and '${newPath}'.`);
      }
      this.#routeMap.set(alias, [...path]);
    }

    if (route.children) {
      route.children.forEach(child => this.#buildRoutes(child, [...path]));
    }
  }

  navigate(view: RouteAlias, value?: any) {
    return this.#router.navigate(this.getRoute(view, value));
  }

  navigateWithReplace(view: RouteAlias, value?: any) {
    return this.#router.navigate(this.getRoute(view, value), { replaceUrl: true });
  }

  getRoute(view: RouteAlias, value?: any) {
    const route = this.#routeMap.get(view);
    if (!route) throw new Error(`Unexpected route alias '${view}'.`);

    // Append the value to the route if it's a dynamic route. This can be improved later on
    switch (view) {
      case "verify-code":
      case "admin-user":
      case "admin-role":
        return [...route, value];
    }

    return route;
  }
}
