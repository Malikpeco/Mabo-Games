import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { CreateGameRequest, GameDetailsDto, GetStorefrontGamesRequest, GetStorefrontGamesResponse } from "./games-api.models";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn:'root'
})

export class GamesApiService{
    private http = inject(HttpClient);
    
    private readonly baseUrl = `${environment.apiUrl}/api/games`;


    storefront(request?: GetStorefrontGamesRequest): Observable<GetStorefrontGamesResponse>{
        const params = request ? buildHttpParams(request as any) : undefined;
        return this.http.get<GetStorefrontGamesResponse>(`${this.baseUrl}/storefront`,{params});
    }

    getById(id: number): Observable<GameDetailsDto> {
        return this.http.get<GameDetailsDto>(`${this.baseUrl}/${id}`);
    }

    create(request: CreateGameRequest): Observable<void> {
        return this.http.post<void>(this.baseUrl, request);
    }

    update(id: number, request: CreateGameRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, request);
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

}