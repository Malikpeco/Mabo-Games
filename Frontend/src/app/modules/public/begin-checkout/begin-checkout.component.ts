import { Component, OnInit, inject } from '@angular/core';
import { CartItemDto } from '../../../api-services/carts/carts-api.models';
import { CartsApiService } from '../../../api-services/carts/carts-api.service';

@Component({
  selector: 'app-begin-checkout',
  standalone: false,
  templateUrl: './begin-checkout.component.html',
  styleUrl: './begin-checkout.component.scss',
})
export class BeginCheckoutComponent implements OnInit {
  private api = inject(CartsApiService);
  cartItems: CartItemDto[] = [];
  totalPrice = 0;

  ngOnInit(): void {
    this.api.getCart().subscribe(cart => {
      this.cartItems = (cart.cartItems ?? []).filter(item => !item.isSaved);
      this.totalPrice = this.cartItems.reduce((sum, item) => sum + item.price, 0);
    });
  }

}
