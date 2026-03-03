import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";
import { GetStorefrontGamesResponse, GameDetailsDto } from "../games/games-api.models";
import { ListFavouritesQueryRequest, ListFavouritesQueryResponse } from "./favourites-api.models";

@Injectable({
    providedIn:'root'
})

export class FavouritesApiService{
    private http = inject(HttpClient);
    
    private readonly baseUrl = `${environment.apiUrl}/api/favourites`;


    listFavouritesQuery(request?: ListFavouritesQueryRequest): Observable<ListFavouritesQueryResponse>{
        const params = request ? buildHttpParams(request as any) : undefined;
        return this.http.get<ListFavouritesQueryResponse>(`${this.baseUrl}`,{params});
    }

    addToFavourites(gameId: number): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}`, { gameId });
    }
    
    removeFromFavourites(gameId: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${gameId}`);
    }

}