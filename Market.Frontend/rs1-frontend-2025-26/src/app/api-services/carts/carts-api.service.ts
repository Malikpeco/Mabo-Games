import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddToCartCommand } from "./carts-api.models";
import { Observable } from "rxjs";

@Injectable({
providedIn:'root'
})

export class CartsApiService {
    private http = inject(HttpClient);  
    private readonly baseUrl = `${environment.apiUrl}/carts`;

      addToCart(payload: AddToCartCommand): Observable<void> {
        return this.http.post<void>(`${this.baseUrl}/AddToCart`, payload);
      }
      
}