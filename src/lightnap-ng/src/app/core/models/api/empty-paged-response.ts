import { PagedResponse } from "./paged-response";

export class EmptyPagedResponse<T> implements PagedResponse<T> {
  data = [];
  pageNumber = 1;
  pageSize = 0;
  totalCount = 0;
  totalPages = 0;
}
