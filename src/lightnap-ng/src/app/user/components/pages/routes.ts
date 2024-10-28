import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "home" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
];
