import { PageResult } from '../../core/models/paging/page-result';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

// === QUERIES (READ) ===

/**
 * Query parameters for GET /Products
 * Corresponds to: ListProductsQuery.cs
 */
export class ListProductsRequest extends BasePagedQuery {
  search?: string | null;
  // Future filters: categoryId?, isEnabled?, priceMin?, priceMax?
}

/**
 * Response item for GET /Products
 * Corresponds to: ListProductsQueryDto.cs
 */
export interface ListProductsQueryDto {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  stockQuantity: number;
  categoryName: string;
  isEnabled: boolean;
}

/**
 * Response for GET /Products/{id}
 * Corresponds to: GetProductByIdQueryDto.cs
 */
export interface GetProductByIdQueryDto {
  id: number;
  name: string;
  description?: string | null;
  price: number;
  stockQuantity: number;
  categoryName: string;
  categoryId: number;
  isEnabled: boolean;
}

/**
 * Paged response for GET /Products
 */
export type ListProductsResponse = PageResult<ListProductsQueryDto>;

// === COMMANDS (WRITE) ===

/**
 * Command for POST /Products
 * Corresponds to: CreateProductCommand.cs
 */
export interface CreateProductCommand {
  name: string;
  description?: string | null;
  price: number;
  categoryId: number;
}

/**
 * Command for PUT /Products/{id}
 * Corresponds to: UpdateProductCommand.cs
 */
export interface UpdateProductCommand {
  name: string;
  description?: string | null;
  price: number;
  categoryId: number;
}
