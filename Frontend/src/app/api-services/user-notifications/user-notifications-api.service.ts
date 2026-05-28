import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserNotificationDto } from './user-notifications-api.models';

@Injectable({
  providedIn: 'root',
})
export class UserNotificationsApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/user-notifications`;

  list(): Observable<UserNotificationDto[]> {
    return this.http.get<UserNotificationDto[]>(`${this.baseUrl}/list`);
  }

  getById(id: number): Observable<UserNotificationDto> {
    return this.http.get<UserNotificationDto>(`${this.baseUrl}/${id}`);
  }

  clearRead(): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/ClearRead`);
  }
}