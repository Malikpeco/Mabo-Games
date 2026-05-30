import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UploadGameFileResponse } from './supabase-api.models';

@Injectable({
  providedIn: 'root',
})
export class SupabaseApiService {
  private http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiUrl}/api/supabase`;

  uploadGameFile(file: File): Observable<UploadGameFileResponse> {
    const formData = new FormData();
    formData.append('file', file, file.name);

    return this.http.post<UploadGameFileResponse>(`${this.baseUrl}/upload`, formData);
  }

  getGameDownloadUrl(gameId: number): Observable<string> {
    return this.http.get(`${this.baseUrl}/${gameId}`, { responseType: 'text' });
  }
}
