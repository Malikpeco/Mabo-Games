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
export interface GameGenreDto{
    id :number;
    name:string;
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

export interface ReviewDto{
    id:number;
    rating:number;
    content?:string;
    date:string;
    userId:number;
    username?:string;
}



export interface ReviewSummaryDto{
    averageRating: number;
    count: number;
    items:ReviewDto[];
}

export interface PublisherDto{
    id:number;
    name:string;
    countryId:number;
    countryName?:string;
}

export interface GameDetailsDto {
    id: number;
    name: string;
    price: number;
    releaseDate: string;
    description?: string;
    coverImageURL?: string;
    publisher: PublisherDto;
    screenshots: GameScreenshotsDto[];
    genres: GameGenreDto[];
    reviews : ReviewSummaryDto[];
}





export type GetStorefrontGamesResponse = PageResult<StorefrontGameDto>;