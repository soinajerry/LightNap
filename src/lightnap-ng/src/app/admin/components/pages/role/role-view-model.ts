import { AdminUser, Role } from "@admin/models";

export interface RoleViewModel
 {
    role: Role;
    users: Array<AdminUser>;
 }
