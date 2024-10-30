import { AdminUser, Role } from "./response";

/**
 * Interface representing a user with full details including roles for an administrative context.
 */
export interface AdminUserWithRoles {
    /**
     * The timestamp when the user was last modified.
     * @type {AdminUser}
     */
    user: AdminUser;

    /**
     * The timestamp when the user was created.
     * @type {Array<Role>}
     */
    roles: Array<Role>;
}
