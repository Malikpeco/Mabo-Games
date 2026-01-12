import { PageResult } from "../../core/models/paging/page-result";  
import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { GenreDto } from "../genres/genres-api.models";

export class GetStorefrontGamesRequest extends BasePagedQuery{
    search?:string | null;
    sort?: string | null;
    genreIds?:number[] | null;
}

export interface GameScreenshotsDto{
    imageURL:string;
    gameId:number;
}

export interface StorefrontGameDto{
    id:number;
    name:string;
    price:number;
    releaseDate:string;
    coverImageURL?:string;
    publisherId:number;
    publisherName:string;
    screenshots: GameScreenshotsDto[];
    genres: GenreDto[];
}

export type GetStorefrontGamesResponse = PageResult<StorefrontGameDto>;