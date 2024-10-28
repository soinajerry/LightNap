import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "landing" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "access-denied", data: { alias: "access-denied" }, loadComponent: () => import("../support/access-denied/access-denied.component").then(m => m.AccessDeniedComponent) },
  { path: "error", data: { alias: "error" }, loadComponent: () => import("../support/error/error.component").then(m => m.ErrorComponent) },
  { path: "not-found", data: { alias: "not-found" }, loadComponent: () => import("../support/not-found/not-found.component").then(m => m.NotFoundComponent) },
  { path: "terms-and-conditions", data: { alias: "terms-and-conditions" }, loadComponent: () => import("../support/terms-and-conditions/terms-and-conditions.component").then(m => m.TermsAndConditionsComponent) },
  { path: "privacy-policy", data: { alias: "privacy-policy" }, loadComponent: () => import("../support/privacy-policy/privacy-policy.component").then(m => m.PrivacyPolicyComponent) },
];
