import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "login", loadComponent: () => import("./login/login.component").then(m => m.LoginComponent) },
  { path: "reset-password", loadComponent: () => import("./reset-password/reset-password.component").then(m => m.ResetPasswordComponent) },
  {
    path: "reset-instructions-sent",
    loadComponent: () => import("./reset-instructions-sent/reset-instructions-sent.component").then(m => m.ResetInstructionsSentComponent),
  },
  { path: "register", loadComponent: () => import("./register/register.component").then(m => m.RegisterComponent) },
  { path: "new-password", loadComponent: () => import("./new-password/new-password.component").then(m => m.NewPasswordComponent) },
  { path: "new-password/:email/:token", loadComponent: () => import("./new-password/new-password.component").then(m => m.NewPasswordComponent) },
  { path: "verify-code", loadComponent: () => import("./verify-code/verify-code.component").then(m => m.VerifyCodeComponent) },
];
