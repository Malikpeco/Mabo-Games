import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { environment } from "../../../environments/environment";
import { AddToCartCommand, CartDto } from "./carts-api.models";
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
      
      getCart(): Observable<CartDto> {
        return this.http.get<CartDto>(`${this.baseUrl}/GetCart`);
    }   


    removeFromCart(gameId: number): Observable<void> {
      return this.http.delete<void>(`${this.baseUrl}/RemoveFromCart/${gameId}`);
    }

    switchItemState(id: number): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/SwitchItemState/${id}`, {});
    }

    clearCart(): Observable<void> {
      return this.http.post<void>(`${this.baseUrl}/ClearCart`, {});
    }

}
