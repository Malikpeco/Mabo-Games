import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { GetStorefrontGamesRequest, GetStorefrontGamesResponse } from "./games-api.models";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";
import { UntypedFormBuilder } from "@angular/forms";

@Injectable({
    providedIn:'root'
})

export class GamesApiService{
    private http = inject(HttpClient);
    
    private readonly baseUrl = `${environment.apiUrl}/api/games`;


    //GET /api/games/storefront
    storefront(request?: GetStorefrontGamesRequest): Observable<GetStorefrontGamesResponse>{
        const params = request ? buildHttpParams(request as any) : undefined;

        return this.http.get<GetStorefrontGamesResponse>(`${this.baseUrl}/storefront`,{
            params
        });
    }
}