import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { GetIgdbGameDetailsDto, SearchIgdbGameDto } from './igdb-api.models';

@Injectable({
  providedIn: 'root',
})
export class IgdbApiService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/igdb`;

  searchGames(search: string): Observable<SearchIgdbGameDto[]> {
    const params = new HttpParams().set('search', search);
    return this.http.get<SearchIgdbGameDto[]>(`${this.baseUrl}/search`, { params });
  }

  getGameDetails(id: number): Observable<GetIgdbGameDetailsDto> {
    return this.http.get<GetIgdbGameDetailsDto>(`${this.baseUrl}/${id}`);
  }
}
