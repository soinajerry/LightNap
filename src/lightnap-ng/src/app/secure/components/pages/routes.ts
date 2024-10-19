import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
];
