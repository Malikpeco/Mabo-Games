import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { CreateStripeCheckoutSessionRequest, CreateStripeCheckoutSessionResponse } from './payments-api.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/payments`;
  private http = inject(HttpClient);


  
  createStripeCheckoutSession(orderId: number): Observable<CreateStripeCheckoutSessionResponse> {
    const body : CreateStripeCheckoutSessionRequest = {orderId};
    return this.http.post<CreateStripeCheckoutSessionResponse>(`${this.baseUrl}/stripe/checkout-session`, body);
  }

  
}
