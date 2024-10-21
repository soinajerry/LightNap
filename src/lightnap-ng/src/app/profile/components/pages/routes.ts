import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "devices", loadComponent: () => import("./devices/devices.component").then(m => m.DevicesComponent) },
  { path: "change-password", loadComponent: () => import("./change-password/change-password.component").then(m => m.ChangePasswordComponent) },
];
