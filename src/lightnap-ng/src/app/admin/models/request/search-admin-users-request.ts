import { PaginationRequest } from "@core";
import { SearchAdminUsersSortBy } from "./search-admin-users-sort-by";

export interface SearchAdminUsersRequest extends PaginationRequest {
    email?: string;
    userName?: string;
    sortBy: SearchAdminUsersSortBy;
    reverseSort: boolean;
}
