import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export interface PublisherAutocompleteDto {
  id: number;
  name: string;
}

export interface PublisherCountryDto {
  id: number;
  name: string;
}

export interface PublisherGameDto {
  id: number;
  name: string;
  description?: string | null;
  releaseDate?: string;
}

export interface ListPublisherDto {
  id: number;
  name: string;
  country: PublisherCountryDto;
  games: PublisherGameDto[];
}

export class ListPublishersRequest extends BasePagedQuery {
  search?: string | null;
}

export type ListPublishersResponse = PageResult<ListPublisherDto>;

export interface CreatePublisherRequest {
  name: string;
  countryId: number;
}

export interface UpdatePublisherRequest {
  name: string;
  countryId: number;
}

export type GetPublisherByIdResponse = ListPublisherDto;
