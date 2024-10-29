import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "user-home" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
];
