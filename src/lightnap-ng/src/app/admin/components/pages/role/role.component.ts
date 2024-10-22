import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject, Input, OnInit } from "@angular/core";
import { ApiResponse, RoutePipe, SuccessApiResponse } from "@core";
import { CardModule } from "primeng/card";
import { combineLatest, map, Observable } from "rxjs";
import { RoleViewModel } from "./role-view-model";
import { RouterLink } from "@angular/router";
import { TableModule } from "primeng/table";
import { ButtonModule } from "primeng/button";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";

@Component({
  standalone: true,
  templateUrl: "./role.component.html",
  imports: [CommonModule, CardModule, TableModule, ButtonModule, RouterLink, RoutePipe, ErrorListComponent],
})
export class RoleComponent implements OnInit {
  #adminService = inject(AdminService);

  @Input() role!: string;

  header = "Loading role...";
  subHeader = "";
  errors: string[] = [];

  viewModel$ = new Observable<ApiResponse<RoleViewModel>>();

  ngOnInit() {
    this.#refreshRole();
  }

  #refreshRole() {
    this.viewModel$ = combineLatest([this.#adminService.getRole(this.role), this.#adminService.getUsersInRole(this.role)]).pipe(
      map(([roleResponse, usersResponse]) => {
        if (!roleResponse.result) return roleResponse as any as ApiResponse<RoleViewModel>;
        if (!usersResponse.result) return usersResponse as any as ApiResponse<RoleViewModel>;

        this.header = `Manage Users In Role: ${roleResponse.result.displayName}`;
        this.subHeader = roleResponse.result.description;

        return new SuccessApiResponse<RoleViewModel>({
          role: roleResponse.result,
          users: usersResponse.result,
        });
      })
    );
  }

  removeUserFromRole(userId: string) {
    this.errors = [];
    this.#adminService.removeUserFromRole(userId, this.role).subscribe({
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
