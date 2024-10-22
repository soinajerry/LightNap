import { UpdateAdminUserRequest, SearchAdminUsersRequest, Role } from "@admin/models";
import { inject, Injectable } from "@angular/core";
import { DataService } from "./data.service";
import { of, shareReplay, tap } from "rxjs";
import { ApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
export class AdminService {
  #dataService = inject(DataService);
  #rolesResponse?: ApiResponse<Array<Role>>;

  getUser(userId: string) {
    return this.#dataService.getUser(userId);
  }

  updateUser(userId: string, updateAdminUser: UpdateAdminUserRequest) {
    return this.#dataService.updateUser(userId, updateAdminUser);
  }

  deleteUser(userId: string) {
    return this.#dataService.deleteUser(userId);
  }

  searchUsers(searchAdminUsers: SearchAdminUsersRequest) {
    return this.#dataService.searchUsers(searchAdminUsers);
  }

  getRoles() {
    if (this.#rolesResponse) {
      return of(this.#rolesResponse);
    }
    return this.#dataService.getRoles().pipe(tap(response => {
        if (response.result) {
          this.#rolesResponse = response;
        }
    }));
  }

  getUserRoles(userId: string) {
    return this.#dataService.getUserRoles(userId);
  }

  getUsersInRole(role: string) {
    return this.#dataService.getUsersInRole(role);
  }

  addUserToRole(userId: string, role: string) {
    return this.#dataService.addUserToRole(userId, role);
  }

  removeUserFromRole(userId: string, role: string) {
    return this.#dataService.removeUserFromRole(userId, role);
  }
}
