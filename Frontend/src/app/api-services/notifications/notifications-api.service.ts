import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { SendNotificationRequest } from './notifications-api.models';

@Injectable({
  providedIn: 'root'
})
export class NotificationsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/user-notificatios`;
  private http = inject(HttpClient);

  /**
   * POST /user-notificatios/NotifyAll
   * Sends an admin notification to users.
   */
  sendNotification(payload: SendNotificationRequest): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/NotifyAll`, payload);
  }
}