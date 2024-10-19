import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "access-denied", loadComponent: () => import("./access-denied/access-denied.component").then(m => m.AccessDeniedComponent) },
  { path: "error", loadComponent: () => import("./error/error.component").then(m => m.ErrorComponent) },
  { path: "notfound", loadComponent: () => import("./notfound/notfound.component").then(m => m.NotfoundComponent) },
  { path: "terms-and-conditions", loadComponent: () => import("./terms-and-conditions/terms-and-conditions.component").then(m => m.TermsAndConditionsComponent) },
  { path: "privacy-policy", loadComponent: () => import("./privacy-policy/privacy-policy.component").then(m => m.PrivacyPolicyComponent) },
];
