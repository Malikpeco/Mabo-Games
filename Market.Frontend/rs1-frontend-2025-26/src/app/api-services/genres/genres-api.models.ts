import { PageResult } from "../../core/models/paging/page-result";

export interface GenreDto{
    id: number;
    name: string;
}

export type ListGenresResponse = PageResult<GenreDto>;