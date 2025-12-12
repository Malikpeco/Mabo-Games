import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  ListProductCategoriesRequest,
  ListProductCategoriesResponse,
  GetProductCategoryByIdQueryDto,
  CreateProductCategoryCommand,
  UpdateProductCategoryCommand
} from './product-categories-api.model';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class ProductCategoriesApiService {
  private readonly baseUrl = `${environment.apiUrl}/ProductCategories`;
  private http = inject(HttpClient);

  /**
   * GET /ProductCategories
   * List categories with optional query parameters.
   */
  list(request?: ListProductCategoriesRequest): Observable<ListProductCategoriesResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;

    return this.http.get<ListProductCategoriesResponse>(this.baseUrl, {
      params,
    });
  }

  /**
   * GET /ProductCategories/{id}
   * Get a single category by ID.
   */
  getById(id: number): Observable<GetProductCategoryByIdQueryDto> {
    return this.http.get<GetProductCategoryByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  /**
   * POST /ProductCategories
   * Create a new category.
   * @returns ID of the newly created category
   */
  create(payload: CreateProductCategoryCommand): Observable<number> {
    return this.http.post<number>(this.baseUrl, payload);
  }

  /**
   * PUT /ProductCategories/{id}
   * Update an existing category.
   */
  update(id: number, payload: UpdateProductCategoryCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * DELETE /ProductCategories/{id}
   * Delete a category.
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  /**
   * PUT /ProductCategories/{id}/disable
   * Disable a category.
   */
  disable(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/disable`, {});
  }

  /**
   * PUT /ProductCategories/{id}/enable
   * Enable a category.
   */
  enable(id: number): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}/enable`, {});
  }
}
