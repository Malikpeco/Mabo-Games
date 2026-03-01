import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateReviewCommand } from './reviews-api.models';

@Injectable({
  providedIn: 'root'
})
export class ReviewsApiService {
  private readonly baseUrl = `${environment.apiUrl}/api/reviews`;
  private http = inject(HttpClient);


  
  createReview(command: CreateReviewCommand): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}`, command);
  }

  
}
