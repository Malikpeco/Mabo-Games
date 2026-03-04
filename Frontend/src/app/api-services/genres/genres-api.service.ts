import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { GenreDto } from "./genres-api.models";


@Injectable({
    providedIn:'root'
})

export class GenresApiService{
    private http = inject(HttpClient);

    list(){
        return this.http.get<GenreDto[]>(`${environment.apiUrl}/api/genres`);
    }
}