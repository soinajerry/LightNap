import { AdminUser } from "@admin";
import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { EmptyPagedResponse, RoutePipe, SuccessApiResponse } from "@core";
import { ApiResponseComponent } from "@core/components/controls/api-response/api-response.component";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DropdownModule } from "primeng/dropdown";
import { TableModule } from "primeng/table";
import { ToggleButtonModule } from "primeng/togglebutton";
import { debounceTime, map, startWith, Subject, switchMap, tap } from "rxjs";

@Component({
  standalone: true,
  templateUrl: "./users.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ApiResponseComponent,
    TableModule,
    ButtonModule,
    RouterModule,
    RoutePipe,
    DropdownModule,
    ErrorListComponent,
    ToggleButtonModule,
  ],
})
export class UsersComponent {
  pageSize = 2;

  #adminService = inject(AdminService);
  #fb = inject(FormBuilder);

  form = this.#fb.group({
    sortBy: this.#fb.control("user-name"),
    reverseSort: this.#fb.control(false),
  });

  errors = new Array<string>();

  #lazyLoadEventSubject = new Subject<{ first: number }>();
  users$ = this.#lazyLoadEventSubject.pipe(
    switchMap(event =>
      this.#adminService.searchUsers({
        sortBy: this.form.value.sortBy,
        reverseSort: this.form.value.reverseSort,
        pageSize: this.pageSize,
        pageNumber: event.first / this.pageSize + 1,
      })
    ),
    tap(response => console.log(response)),
    // We need to bootstrap the p-table with a response to get the whole process running. We do it this way to fake an empty response
    // so we can avoid a redundant call to the API.
    startWith(new SuccessApiResponse(new EmptyPagedResponse<AdminUser>()))
  );

  sortBys$ = this.#adminService.getAppConfiguration().pipe(map(response => response.result?.userSortOptions ?? []));

  constructor() {
    this.form.valueChanges.pipe(takeUntilDestroyed(), debounceTime(250)).subscribe(() => {
      this.#lazyLoadEventSubject.next({ first: 0 });
    });
  }

  loadUsersLazy(event: any) {
    this.#lazyLoadEventSubject.next(event);
  }

  deleteUser(userId: string) {
    this.#adminService.deleteUser(userId).subscribe(response => {
      if (!response.result) {
        this.errors = response.errorMessages;
        return;
      }
      this.#lazyLoadEventSubject.next({ first: 0 });
    });
  }
}
