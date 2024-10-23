import { Role, SearchAdminUsersRequest, UpdateAdminUserRequest } from "@admin/models";
import { inject, Injectable } from "@angular/core";
import { ApiResponse, SuccessApiResponse } from "@core";
import { combineLatest, map, of, tap } from "rxjs";
import { DataService } from "./data.service";

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
    if (this.#rolesResponse) return of(this.#rolesResponse);
    return this.#dataService.getRoles().pipe(
      tap(response => {
        if (response.result) {
          this.#rolesResponse = response;
        }
      })
    );
  }

  getRole(roleName: string) {
    return this.getRoles().pipe(
      map(response => {
        if (!response.result) return response as any as ApiResponse<Role>;
        return new SuccessApiResponse(response.result.find(role => role.name === roleName));
      })
    );
  }

  getUserRoles(userId: string) {
    return combineLatest([this.getRoles(), this.#dataService.getUserRoles(userId)]).pipe(
      map(([rolesResponse, userRolesResponse]) => {
        if (!rolesResponse.result) return rolesResponse as any as ApiResponse<Array<Role>>;
        if (!userRolesResponse.result) return userRolesResponse as any as ApiResponse<Array<Role>>;
        return new SuccessApiResponse(userRolesResponse.result.map(userRole => rolesResponse.result.find(role => role.name === userRole)));
      })
    );
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
