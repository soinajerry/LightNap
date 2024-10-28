import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "profile" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "devices", data: { alias: "devices" }, loadComponent: () => import("./devices/devices.component").then(m => m.DevicesComponent) },
  {
    path: "change-password",
    data: { alias: "change-password" },
    loadComponent: () => import("./change-password/change-password.component").then(m => m.ChangePasswordComponent),
  },
];
