import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ListProductsRequest,
  ListProductsResponse,
  GetProductByIdQueryDto,
  CreateProductCommand,
  UpdateProductCommand
} from './products-api.models';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root'
})
export class ProductsApiService {
  private readonly baseUrl = `${environment.apiUrl}/Products`;
  private http = inject(HttpClient);

  /**
   * GET /Products
   * List products with optional query parameters.
   */
  list(request?: ListProductsRequest): Observable<ListProductsResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;

    return this.http.get<ListProductsResponse>(this.baseUrl, {
      params,
    });
  }

  /**
   * GET /Products/{id}
   * Get a single product by ID.
   */
  getById(id: number): Observable<GetProductByIdQueryDto> {
    return this.http.get<GetProductByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  /**
   * POST /Products
   * Create a new product.
   * @returns ID of the newly created product
   */
  create(payload: CreateProductCommand): Observable<number> {
    return this.http.post<number>(this.baseUrl, payload);
  }

  /**
   * PUT /Products/{id}
   * Update an existing product.
   */
  update(id: number, payload: UpdateProductCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * DELETE /Products/{id}
   * Delete a product.
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
