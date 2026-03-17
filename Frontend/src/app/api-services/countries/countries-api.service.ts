import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CountryAutocompleteDto } from './countries-api.models';

@Injectable({
  providedIn: 'root',
})
export class CountriesApiService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${environment.apiUrl}/api/countries`;

  autocomplete(term: string): Observable<CountryAutocompleteDto[]> {
    const trimmedTerm = term.trim();
    if (!trimmedTerm) {
      return of([]);
    }

    const params = new HttpParams().set('term', trimmedTerm);
    return this.http.get<CountryAutocompleteDto[]>(`${this.baseUrl}/autocomplete`, { params });
  }
}
