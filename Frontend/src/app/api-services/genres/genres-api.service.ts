import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { buildHttpParams } from "../../core/models/build-http-params";
import { ListGenresRequest, ListGenresResponse, UpdateGenreRequest } from "./genres-api.models";


@Injectable({
    providedIn:'root'
})

export class GenresApiService{
    private http = inject(HttpClient);
    private readonly baseUrl = `${environment.apiUrl}/api/genres`;

    list(request?: ListGenresRequest){
        const params = request ? buildHttpParams(request as any) : undefined;
        return this.http.get<ListGenresResponse>(this.baseUrl, { params });
    }

    create(name: string){
        return this.http.post<number>(this.baseUrl, { name });
    }

    update(id: number, request: UpdateGenreRequest) {
        return this.http.put<void>(`${this.baseUrl}/${id}`, request);
    }

    delete(id: number) {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }
}