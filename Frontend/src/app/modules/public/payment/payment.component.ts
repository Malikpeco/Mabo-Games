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
  isVerifying = false;
  statusMessage = '';
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

          // Direct redirect to Stripe Checkout
          window.location.href = response.checkoutUrl;
        },
        error: err => {
          this.errorMessage = err?.error?.message ?? 'Payment could not start.';
        }
      });
  }

  private handleSuccessReturn(): void {
    const sessionId = this.route.snapshot.queryParamMap.get('session_id');

    if (!sessionId) {
      this.statusMessage = 'Your purchase is complete. Returning to library...';
      this.finalizeRedirect();
      return;
    }

    this.isVerifying = true;
    this.statusMessage = 'Confirming your payment and updating your library...';

    this.paymentsApi.confirmStripeSession(sessionId).subscribe({
      next: response => {
        this.isVerifying = false;
        if (response.isSuccess) {
          this.statusMessage = 'Payment confirmed! Redirecting to your library...';
          this.finalizeRedirect();
        } else {
          this.errorMessage = response.message ?? 'Payment confirmation was unsuccessful.';
        }
      },
      error: () => {
        this.isVerifying = false;
        this.statusMessage = 'Purchase completed. Returning to your library...';
        this.finalizeRedirect();
      }
    });
  }

  private finalizeRedirect(): void {
    const openerWindow = window.opener;
    if (openerWindow && !openerWindow.closed) {
      try {
        openerWindow.location.href = `${window.location.origin}/public/library`;
        openerWindow.focus();
      } catch {
      }

      setTimeout(() => {
        window.close();
      }, 600);
      return;
    }

    setTimeout(() => {
      this.router.navigate(['/public/library']);
    }, 600);
  }
}
