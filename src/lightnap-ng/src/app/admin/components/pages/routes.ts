import { AppRoute } from "@routing";

export const Routes: AppRoute[] = [
  { path: "", data: { alias: "admin-home" }, loadComponent: () => import("./index/index.component").then(m => m.IndexComponent) },
  { path: "users", data: { alias: "admin-users" }, loadComponent: () => import("./users/users.component").then(m => m.UsersComponent) },
  { path: "users/:userId", data: { alias: "admin-user" }, loadComponent: () => import("./user/user.component").then(m => m.UserComponent) },
  { path: "roles", data: { alias: "admin-roles" }, loadComponent: () => import("./roles/roles.component").then(m => m.RolesComponent) },
  { path: "roles/:role", data: { alias: "admin-role" }, loadComponent: () => import("./role/role.component").then(m => m.RoleComponent) },
];
