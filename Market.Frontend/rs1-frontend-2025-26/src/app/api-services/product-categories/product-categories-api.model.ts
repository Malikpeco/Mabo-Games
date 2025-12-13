import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

// === QUERIES (READ) ===

/**
 * Query parameters for GET /ProductCategories
 * Corresponds to: ListProductCategoriesQuery.cs
 */
export class ListProductCategoriesRequest extends BasePagedQuery {
  search?: string | null;
  onlyEnabled?: boolean | null;
}

/**
 * Response item for GET /ProductCategories
 * Corresponds to: ListProductCategoriesQueryDto.cs
 */
export interface ListProductCategoriesQueryDto {
  id: number;
  name: string;
  isEnabled: boolean;
}

/**
 * Response for GET /ProductCategories/{id}
 * Corresponds to: GetProductCategoryByIdQueryDto.cs
 */
export interface GetProductCategoryByIdQueryDto {
  id: number;
  name: string;
  isEnabled: boolean;
}

/**
 * Paged response for GET /ProductCategories
 */
export type ListProductCategoriesResponse = PageResult<ListProductCategoriesQueryDto>;

// === COMMANDS (WRITE) ===

/**
 * Command for POST /ProductCategories
 * Corresponds to: CreateProductCategoryCommand.cs
 */
export interface CreateProductCategoryCommand {
  name: string;
}

/**
 * Command for PUT /ProductCategories/{id}
 * Corresponds to: UpdateProductCategoryCommand.cs
 */
export interface UpdateProductCategoryCommand {
  name: string;
}
