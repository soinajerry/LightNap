import { PaginationRequest } from "@core";

export interface SearchAdminUsersRequest extends PaginationRequest {
    email: string;
    userName: string;
    sortBy: string;
}
