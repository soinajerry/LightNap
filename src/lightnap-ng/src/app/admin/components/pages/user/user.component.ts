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
import { DropdownModule } from "primeng/dropdown";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";

@Component({
  standalone: true,
  templateUrl: "./user.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    TableModule,
    ButtonModule,
    RouterLink,
    RoutePipe,
    ErrorListComponent,
    ApiResponseComponent,
    DropdownModule,
  ],
})
export class UserComponent implements OnInit {
  #adminService = inject(AdminService);
  #fb = inject(FormBuilder);

  @Input() userId!: string;

  header = "Loading user...";
  subHeader = "";
  errors: string[] = [];

  addUserToRoleForm = this.#fb.group({
    role: this.#fb.control("", [Validators.required]),
  });

  viewModel$ = new Observable<ApiResponse<UserViewModel>>();

  roles$ = this.#adminService.getRoles();

  ngOnInit() {
    this.#refreshUser();
  }

  #refreshUser() {
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

        this.#refreshUser();
      },
    });
  }

  addUserToRole() {
    this.errors = [];
    this.#adminService.addUserToRole(this.userId, this.addUserToRoleForm.value.role).subscribe({
      next: response => {
        if (!response.result) {
          this.errors = response.errorMessages;
          return;
        }

        this.#refreshUser();
      },
    });
  }
}
