/**
 * Pagination parameters for API requests.
 * Corresponds to: PageRequest.cs
 */
export class PageRequest {
  page: number;
  pageSize: number;

  constructor(page: number = 1, pageSize: number = 1000) {
    this.page = page;
    this.pageSize = pageSize;
  }
}
