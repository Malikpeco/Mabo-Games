import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { ListUserGamesRequest, ListUserGamesResponse } from "./user-games-api.models";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn:'root'
})

export class UserGamesApiService{
    private http = inject(HttpClient);
    
    private readonly baseUrl = `${environment.apiUrl}/api/usergames`;

    
    listUserGames(requst?:ListUserGamesRequest): Observable<ListUserGamesResponse>{
        const params = requst ? buildHttpParams(requst as any) : undefined;
        return this.http.get<ListUserGamesResponse>(this.baseUrl,{params});
    }
}