import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreatePublisherRequest,
  GetPublisherByIdResponse,
  ListPublishersRequest,
  ListPublishersResponse,
  PublisherAutocompleteDto,
  UpdatePublisherRequest,
} from './publishers-api.models';
import { buildHttpParams } from '../../core/models/build-http-params';

@Injectable({
  providedIn: 'root',
})
export class PublishersApiService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/publishers`;

  list(request?: ListPublishersRequest): Observable<ListPublishersResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;
    return this.http.get<ListPublishersResponse>(this.baseUrl, { params });
  }

  listForDropdown(request?: ListPublishersRequest): Observable<PublisherAutocompleteDto[]> {
    return new Observable<PublisherAutocompleteDto[]>((subscriber) => {
      this.list(request).subscribe({
        next: (response) => {
          const mapped = (response.items ?? []).map((publisher) => ({
            id: publisher.id,
            name: publisher.name,
          }));

          subscriber.next(mapped);
          subscriber.complete();
        },
        error: (error) => subscriber.error(error),
      });
    });
  }

  autocomplete(term: string): Observable<PublisherAutocompleteDto[]> {
    const trimmedTerm = term.trim();
    if (!trimmedTerm) {
      return of([]);
    }

    const params = new HttpParams().set('term', trimmedTerm);
    return this.http.get<PublisherAutocompleteDto[]>(`${this.baseUrl}/autocomplete`, { params });
  }

  create(request: CreatePublisherRequest): Observable<number> {
    return this.http.post<number>(this.baseUrl, request);
  }

  getById(id: number): Observable<GetPublisherByIdResponse> {
    return this.http.get<GetPublisherByIdResponse>(`${this.baseUrl}/${id}`);
  }

  update(id: number, request: UpdatePublisherRequest): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
