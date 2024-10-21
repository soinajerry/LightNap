import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "access-denied", loadComponent: () => import("../support/access-denied/access-denied.component").then(m => m.AccessDeniedComponent) },
  { path: "error", loadComponent: () => import("../support/error/error.component").then(m => m.ErrorComponent) },
  { path: "not-found", loadComponent: () => import("../support/not-found/not-found.component").then(m => m.NotFoundComponent) },
  { path: "terms-and-conditions", loadComponent: () => import("../support/terms-and-conditions/terms-and-conditions.component").then(m => m.TermsAndConditionsComponent) },
  { path: "privacy-policy", loadComponent: () => import("../support/privacy-policy/privacy-policy.component").then(m => m.PrivacyPolicyComponent) },
];
