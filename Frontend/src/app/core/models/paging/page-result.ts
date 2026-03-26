// src/app/core/models/page-result.ts

export interface PageResult<T> {
  items: T[];
  total: number;
}
