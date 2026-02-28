import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { OrdersApiService } from '../../../api-services/orders/orders-api.service';
import { PaymentsApiService } from '../../../api-services/payments/payments-api.service';

@Component({
  selector: 'app-payment',
  standalone: false,
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.scss',
})
export class PaymentComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private ordersApi = inject(OrdersApiService);
  private paymentsApi = inject(PaymentsApiService);

  isSuccessMode = false;
  errorMessage = '';

  ngOnInit(): void {
    this.isSuccessMode = this.route.snapshot.data['mode'] === 'success';
    if (this.isSuccessMode) {
      this.handleSuccessReturn();
      return;
    }

    this.ordersApi.createOrder()
      .pipe(switchMap(orderId => this.paymentsApi.createStripeCheckoutSession(orderId)))
      .subscribe({
        next: response => {
          if (!response.checkoutUrl) {
            this.errorMessage = 'Stripe checkout URL was not returned.';
            return;
          }

          const stripeWindow = window.open(response.checkoutUrl, '_blank');
          if (!stripeWindow) {
            window.location.href = response.checkoutUrl;
            return;
          }

          this.router.navigate(['/public/browse-games']);
        },
        error: err => {
          this.errorMessage = err?.error?.message ?? 'Payment could not start.';
        }
      });
  }

  private handleSuccessReturn(): void {
    const openerWindow = window.opener;
    if (openerWindow && !openerWindow.closed) {
      try {
        openerWindow.location.href = `${window.location.origin}/public/library`;
        openerWindow.focus();
      } catch {
      }

      setTimeout(() => {
        window.close();
      }, 250);
      return;
    }

    this.router.navigate(['/public/library']);
  }
}
