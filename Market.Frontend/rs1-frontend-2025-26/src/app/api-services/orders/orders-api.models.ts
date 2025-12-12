import { PageResult } from '../../core/models/paging/page-result';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

// === ENUMS ===

/**
 * Order status enum
 * Corresponds to: OrderStatusType.cs
 */
export enum OrderStatusType {
  /** Order is created but not yet confirmed */
  Draft = 1,
  /** Order is confirmed and awaiting payment */
  Confirmed = 2,
  /** Payment received and order is being processed */
  Paid = 3,
  /** Order has been shipped or delivered */
  Completed = 4,
  /** Order has been cancelled */
  Cancelled = 5
}

// === QUERIES (READ) ===

/**
 * Query parameters for GET /Orders
 * Corresponds to: ListOrdersQuery.cs
 */
export class ListOrdersRequest extends BasePagedQuery {
  search?: string | null;
  // Future filters: status?, dateFrom?, dateTo?, userId?
  constructor() {
    super();
  }
}

/**
 * Query parameters for GET /Orders/with-items
 * Corresponds to: ListOrdersWithItemsQuery.cs
 */
export class ListOrdersWithItemsRequest extends BasePagedQuery {
  search?: string | null;
}

/**
 * User info in list response
 */
export interface ListOrdersQueryDtoUser {
  userFirstname: string | null;
  userLastname: string | null;
  userAddress: string | null;
  userCity: string | null;
}

/**
 * Response item for GET /Orders
 * Corresponds to: ListOrdersQueryDto.cs
 */
export interface ListOrdersQueryDto {
  id: number;
  referenceNumber: string | null;
  user: ListOrdersQueryDtoUser;
  orderedAtUtc: string; // ISO date string
  paidAtUtc: string | null; // ISO date string
  status: OrderStatusType;
  totalAmount: number;
  note: string | null;
}

/**
 * Product info in order items
 */
export interface ListOrdersWithItemsQueryDtoItemProduct {
  id: number;
  name: string | null;
  price: number;
}

/**
 * Order item in list with items
 */
export interface ListOrdersWithItemsQueryDtoItem {
  id: number;
  product: ListOrdersWithItemsQueryDtoItemProduct;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
  discountAmount: number;
  subtotal: number;
  total: number;
}

/**
 * User info in list with items response
 */
export interface ListOrdersWithItemsQueryDtoUser {
  userFirstname: string | null;
  userLastname: string | null;
  userAddress: string | null;
  userCity: string | null;
}

/**
 * Response item for GET /Orders/with-items
 * Corresponds to: ListOrdersWithItemsQueryDto.cs
 */
export interface ListOrdersWithItemsQueryDto {
  id: number;
  referenceNumber: string | null;
  user: ListOrdersWithItemsQueryDtoUser;
  orderedAtUtc: string; // ISO date string
  paidAtUtc: string | null; // ISO date string
  status: OrderStatusType;
  totalAmount: number;
  note: string | null;
  items: ListOrdersWithItemsQueryDtoItem[];
}

/**
 * User info in GetById response
 */
export interface GetByIdOrderQueryDtoUser {
  userFirstname: string | null;
  userLastname: string | null;
  userAddress: string | null;
  userCity: string | null;
}

/**
 * Product info in GetById order item
 */
export interface GetByIdOrderQueryDtoItemProduct {
  productId: number;
  productName: string | null;
  productCategoryName: number;
}

/**
 * Order item in GetById response
 */
export interface GetByIdOrderQueryDtoItem {
  id: number;
  product: GetByIdOrderQueryDtoItemProduct;
  quantity: number;
  unitPrice: number;
  discountPercent: number;
  discountAmount: number;
  subtotal: number;
  total: number;
}

/**
 * Response for GET /Orders/{id}
 * Corresponds to: GetOrderByIdQueryDto.cs
 */
export interface GetOrderByIdQueryDto {
  id: number;
  referenceNumber: string | null;
  user: GetByIdOrderQueryDtoUser;
  orderedAtUtc: string; // ISO date string
  paidAtUtc: string | null; // ISO date string
  status: OrderStatusType;
  totalAmount: number;
  note: string | null;
  items: GetByIdOrderQueryDtoItem[];
}

/**
 * Paged response for GET /Orders
 */
export type ListOrdersResponse = PageResult<ListOrdersQueryDto>;

/**
 * Paged response for GET /Orders/with-items
 */
export type ListOrdersWithItemsResponse = PageResult<ListOrdersWithItemsQueryDto>;

// === COMMANDS (WRITE) ===

/**
 * Order item for create command
 */
export interface CreateOrderCommandItem {
  productId: number;
  quantity: number;
}

/**
 * Command for POST /Orders
 * Corresponds to: CreateOrderCommand.cs
 */
export interface CreateOrderCommand {
  note?: string | null;
  items?: CreateOrderCommandItem[];
}

/**
 * Order item for update command
 */
export interface UpdateOrderCommandItem {
  id?: number; // Optional - if present, updates existing item
  productId: number;
  quantity: number;
}

/**
 * Command for PUT /Orders/{id}
 * Corresponds to: UpdateOrderCommand.cs
 */
export interface UpdateOrderCommand {
  note?: string | null;
  items?: UpdateOrderCommandItem[];
}
