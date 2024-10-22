import { Route } from "@angular/router";

export const ROUTES: Route[] = [
  { path: "", loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "users", loadComponent: () => import("./users/users.component").then(m => m.UsersComponent) },
  { path: "users/:userId", loadComponent: () => import("./user/user.component").then(m => m.UserComponent) },
  { path: "roles", loadComponent: () => import("./roles/roles.component").then(m => m.RolesComponent) },
  { path: "roles/:role", loadComponent: () => import("./role/role.component").then(m => m.RoleComponent) },
];
