import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, Input, OnInit } from "@angular/core";
import { ApiResponse, RoutePipe, SuccessApiResponse } from "@core";
import { CardModule } from "primeng/card";
import { combineLatest, map, Observable } from "rxjs";
import { RouterLink } from "@angular/router";
import { TableModule } from "primeng/table";
import { ButtonModule } from "primeng/button";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { UserViewModel } from "./user-view-model";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";

@Component({
  standalone: true,
  templateUrl: "./user.component.html",
  imports: [CommonModule, CardModule, TableModule, ButtonModule, RouterLink, RoutePipe, ErrorListComponent, ApiResponseComponent],
})
export class UserComponent implements OnInit {
  #adminService = inject(AdminService);

  @Input() userId!: string;

  header = "Loading user...";
  subHeader = "";
  errors: string[] = [];

  viewModel$ = new Observable<ApiResponse<UserViewModel>>();

  ngOnInit() {
    this.#refreshRole();
  }

  #refreshRole() {
    this.viewModel$ = combineLatest([this.#adminService.getUser(this.userId), this.#adminService.getUserRoles(this.userId)]).pipe(
      map(([userResponse, rolesResponse]) => {
        if (!userResponse.result) return userResponse as any as ApiResponse<UserViewModel>;
        if (!rolesResponse.result) return rolesResponse as any as ApiResponse<UserViewModel>;

        this.header = `Manage User: ${userResponse.result.userName}`;
        this.subHeader = userResponse.result.email;

        return new SuccessApiResponse<UserViewModel>({
          user: userResponse.result,
          roles: rolesResponse.result,
        });
      })
    );
  }

  removeUserFromRole(role: string) {
    this.errors = [];
    this.#adminService.removeUserFromRole(this.userId, role).subscribe({
      next: response => {
        if (!response.result) {
          this.errors = response.errorMessages;
          return;
        }

        this.#refreshRole();
      },
    });
  }
}
