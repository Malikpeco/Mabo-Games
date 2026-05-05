import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

export interface GenreDto{
    id: number;
    name: string;
    gameCount?: number;
}

export class ListGenresRequest extends BasePagedQuery {
    search?: string | null;
}

export type ListGenresResponse = PageResult<GenreDto>;

export interface UpdateGenreRequest {
    name: string;
}