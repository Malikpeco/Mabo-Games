import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UploadImageResponse } from './screenshots-api.models';

@Injectable({
  providedIn: 'root',
})
export class ScreenshotsApiService {
  private http = inject(HttpClient);

  private readonly baseUrl = `${environment.apiUrl}/api/screenshots`;

  uploadImage(imageFile: File): Observable<UploadImageResponse> {
    const formData = new FormData();
    formData.append('imageFile', imageFile, imageFile.name);

    return this.http.post<UploadImageResponse>(`${this.baseUrl}/upload`, formData);
  }
}
