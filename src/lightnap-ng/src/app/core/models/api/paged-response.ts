export interface PagedResponse<T> {
  data: Array<T>;
  pageNumber: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}
