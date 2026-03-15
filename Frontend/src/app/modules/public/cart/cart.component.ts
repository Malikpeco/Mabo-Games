import { Component, Inject, inject, OnInit } from '@angular/core';
import { CartsApiService } from '../../../api-services/carts/carts-api.service';
import { CartDto, CartItemDto } from '../../../api-services/carts/carts-api.models';
import { DialogHelperService } from '../../shared/services/dialog-helper.service';
import { DialogButton } from '../../shared/models/dialog-config.model';

@Component({
  selector: 'app-cart',
  standalone: false,
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
})
export class CartComponent
implements OnInit{
  
  cart:CartDto | null = null;
  api=inject(CartsApiService);
  activeTab: 'cart' | 'saved' = 'cart';
  dialog=inject(DialogHelperService);
  
  ngOnInit(): void {
    this.loadCart();
  }

  private loadCart(): void {
    this.api.getCart().subscribe(cart => {
      this.cart = cart;
    });
  }

  get cartItems(): CartItemDto[] {
    return (this.cart?.cartItems ?? []).filter(item => !item.isSaved);
  }

  get savedItems(): CartItemDto[] {
    return (this.cart?.cartItems ?? []).filter(item => item.isSaved);
  }

  setTab(tab: 'cart' | 'saved'): void {
    this.activeTab = tab;
  }

  toggleSavedState(cartItemId:number):void{
    this.api.switchItemState(cartItemId).subscribe({
      next: () => {
        this.loadCart();
      },
      error: (error) => {
        console.error("Error switching item state:", error);
      }
    });
  }

  removeFromCart(gameId:number):void{
    
    this.dialog.confirm("Remove from cart","Are you sure you want to remove this game from the cart?").subscribe({
      next:response=>{
        if(response?.button===DialogButton.YES){
          this.api.removeFromCart(gameId).subscribe({
          next: () => {
            this.loadCart();
          },
          error: (error) => {
            console.error("Error removing item from cart:", error);
          }
        });
        }
      }
    })
    
    
    
  }

  clearCart(): void{
    this.dialog.confirm("Clear cart","Are you sure you want to clear the cart?").subscribe({
      next:response=>{
        if(response?.button===DialogButton.YES){
          this.api.clearCart().subscribe({
            next: () => {
              this.loadCart();
            },
            error: (error) => {
              console.error("Error clearing cart:", error);
            }
          });
        }
      }
    })
    
  }
}
