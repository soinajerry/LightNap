import { UpdateAdminUserRequest, SearchAdminUsersRequest } from "@admin/models";
import { inject, Injectable } from "@angular/core";
import { DataService } from "./data.service";

@Injectable({
  providedIn: "root",
})
export class AdminService {
  #dataService = inject(DataService);

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
