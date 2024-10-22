import { AdminUser, Role } from "@admin/models";

export interface UserViewModel
 {
    user: AdminUser;
    roles: Array<Role>;
 }
