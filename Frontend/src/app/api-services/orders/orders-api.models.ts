import { PageResult } from '../../core/models/paging/page-result';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export class ListOrdersRequest extends BasePagedQuery {
  search?: string | null;
  statusFilter?: string | null;
  dateFrom?: string | null;
  dateTo?: string | null;
}

export interface ListOrdersUserDto {
  id: number;
  username: string;
  email: string;
}

export interface ListOrdersGameDto {
  id: number;
  name: string;
  coverImageURL?: string;
  publisherId: number;
  publisherName: string;
  price: number;
}

export interface ListOrdersQueryDto {
  id: number;
  orderDate: string;
  status: string;
  totalAmount: number;
  user: ListOrdersUserDto;
  games: ListOrdersGameDto[];
}

export interface OrderPaymentDto {
  id: number;
  paymentStatus: string;
  total: number;
  date: string;
  stripeCheckoutSessionId?: string | null;
  stripePaymentIntentId?: string | null;
}

export interface OrderDetailsGameDto {
  id: number;
  name: string;
  coverImageURL?: string;
  publisherId: number;
  publisherName: string;
  price: number;
}

export interface OrderDetailsDto {
  id: number;
  orderDate: string;
  status: string;
  totalAmount: number;
  user: ListOrdersUserDto;
  payment?: OrderPaymentDto | null;
  games: OrderDetailsGameDto[];
}

export type ListOrdersResponse = PageResult<ListOrdersQueryDto>;
