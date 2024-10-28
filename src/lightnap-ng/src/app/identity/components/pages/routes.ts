import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "login", data: { alias: "login" }, loadComponent: () => import("./login/login.component").then(m => m.LoginComponent) },
  {
    path: "reset-password",
    data: { alias: "reset-password" },
    loadComponent: () => import("./reset-password/reset-password.component").then(m => m.ResetPasswordComponent),
  },
  {
    path: "reset-instructions-sent",
    data: { alias: "reset-instructions-sent" },
    loadComponent: () => import("./reset-instructions-sent/reset-instructions-sent.component").then(m => m.ResetInstructionsSentComponent),
  },
  { path: "register", data: { alias: "register" }, loadComponent: () => import("./register/register.component").then(m => m.RegisterComponent) },
  {
    path: "new-password/:email/:token",
    loadComponent: () => import("./new-password/new-password.component").then(m => m.NewPasswordComponent),
  },
  {
    path: "verify-code",
    data: { alias: "verify-code" },
    loadComponent: () => import("./verify-code/verify-code.component").then(m => m.VerifyCodeComponent),
  },
];
