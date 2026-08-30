import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateStripeCheckoutSessionRequest, CreateStripeCheckoutSessionResponse, ConfirmStripeSessionRequest, ConfirmStripeSessionResponse } from './payments-api.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/payments`;
  private http = inject(HttpClient);

  createStripeCheckoutSession(orderId: number): Observable<CreateStripeCheckoutSessionResponse> {
    const body: CreateStripeCheckoutSessionRequest = { orderId };
    return this.http.post<CreateStripeCheckoutSessionResponse>(`${this.baseUrl}/stripe/checkout-session`, body);
  }

  confirmStripeSession(sessionId: string): Observable<ConfirmStripeSessionResponse> {
    const body: ConfirmStripeSessionRequest = { sessionId };
    return this.http.post<ConfirmStripeSessionResponse>(`${this.baseUrl}/stripe/confirm-session`, body);
  }
}
