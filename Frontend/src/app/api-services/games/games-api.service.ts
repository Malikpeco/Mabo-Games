import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { CreateGameRequest, GameDetailsDto, GetStorefrontGamesRequest, GetStorefrontGamesResponse, UpdateGameRequest } from "./games-api.models";
import { Observable } from "rxjs";
import { buildHttpParams } from "../../core/models/build-http-params";

@Injectable({
    providedIn:'root'
})

export class GamesApiService{
    private http = inject(HttpClient);
    
    private readonly baseUrl = `${environment.apiUrl}/api/games`;

    private buildGameFormData(request: CreateGameRequest | UpdateGameRequest): FormData {
        const formData = new FormData();

        formData.append('Name', request.name);
        formData.append('Price', request.price.toString());

        if (request.description !== undefined && request.description !== null) {
            formData.append('Description', request.description);
        }

        formData.append('ReleaseDate', request.releaseDate);
        formData.append('PublisherId', request.publisherId.toString());
        formData.append('CoverImageURL', request.coverImageURL);

        for (const genreId of request.genreIds) {
            formData.append('GenreIds', genreId.toString());
        }

        for (const screenshotUrl of request.screenshotUrls) {
            formData.append('ScreenshotUrls', screenshotUrl);
        }

        if (request.file) {
            formData.append('File', request.file, request.file.name);
        }

        return formData;
    }


    storefront(request?: GetStorefrontGamesRequest): Observable<GetStorefrontGamesResponse>{
        const params = request ? buildHttpParams(request as any) : undefined;
        return this.http.get<GetStorefrontGamesResponse>(`${this.baseUrl}/storefront`,{params});
    }

    getById(id: number): Observable<GameDetailsDto> {
        return this.http.get<GameDetailsDto>(`${this.baseUrl}/${id}`);
    }

    create(request: CreateGameRequest): Observable<void> {
        return this.http.post<void>(this.baseUrl, this.buildGameFormData(request));
    }

    update(id: number, request: UpdateGameRequest): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/${id}`, this.buildGameFormData(request));
    }

    delete(id: number): Observable<void> {
        return this.http.delete<void>(`${this.baseUrl}/${id}`);
    }

}