import { PageResult } from "../../core/models/paging/page-result";  
import { BasePagedQuery } from "../../core/models/paging/base-paged-query";
import { GenreDto } from "../genres/genres-api.models";



export class ListUserGamesRequest extends BasePagedQuery{

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

export interface ListUserGamesQueryDto{
    id:number;
    userId:number;
    gameId:number;
    game:StorefrontGameDto;
    purchaseDate:string;
}


export type ListUserGamesResponse = PageResult<ListUserGamesQueryDto>;