import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import { ListOrdersRequest, ListOrdersResponse, OrderDetailsDto } from './orders-api.models';

@Injectable({
  providedIn: 'root'
})
export class OrdersApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/orders`;
  private http = inject(HttpClient);

  getOrders(request?: ListOrdersRequest): Observable<ListOrdersResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListOrdersResponse>(this.baseUrl, { params });
  }

  getById(orderId: number): Observable<OrderDetailsDto> {
    return this.http.get<OrderDetailsDto>(`${this.baseUrl}/${orderId}`);
  }

  createOrder(): Observable<number> {
    return this.http.post<number>(`${this.baseUrl}/CreateOrder`, {});
  }

  cancelOrder(orderId: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/CancelOrder/${orderId}`, {});
  }
}
