export interface SearchAdminUsersRequest {
    pageNumber: number;
    pageSize: number;
    email: string;
    userName: string;
    sortBy: string;
}
