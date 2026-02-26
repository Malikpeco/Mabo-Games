import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap } from 'rxjs';
import { OrdersApiService } from '../../../api-services/orders/orders-api.service';
import { PaymentsApiService } from '../../../api-services/payments/payments-api.service';
import { ToasterService } from '../../../core/services/toaster.service';

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
  private toaster = inject(ToasterService);

  isLoading = false;
  isSuccessMode = false;
  hasError = false;
  errorMessage = '';
  openedInNewTab = false;

  ngOnInit(): void {
    this.isSuccessMode = this.route.snapshot.data['mode'] === 'success';
    if (this.isSuccessMode) {
      this.handleSuccessPopupReturn();
      return;
    }

    this.startPayment();
  }

  startPayment(): void {
    this.isLoading = true;
    this.hasError = false;
    this.errorMessage = '';

    this.ordersApi.createOrder()
      .pipe(switchMap(orderId => this.paymentsApi.createStripeCheckoutSession(orderId)))
      .subscribe({
        next: response => {
          this.isLoading = false;

          if (!response.checkoutUrl) {
            this.setError('Stripe checkout URL was not returned.');
            return;
          }

          const stripeWindow = window.open(response.checkoutUrl, '_blank');
          if (!stripeWindow) {
            this.setError('Popup blocked. Please allow popups and try again.');
            return;
          }

          this.openedInNewTab = true;
        },
        error: err => {
          this.isLoading = false;
          this.setError(err?.error?.message ?? 'Failed to start payment.');
        }
      });
  }

  goToCheckout(): void {
    this.router.navigate(['/public/checkout']);
  }

  private setError(message: string): void {
    this.hasError = true;
    this.errorMessage = message;
    this.toaster.error(message);
  }

  private handleSuccessPopupReturn(): void {
    const openerWindow = window.opener;
    if (!openerWindow || openerWindow.closed) {
      return;
    }

    try {
      openerWindow.localStorage.setItem('payment_success_notice', '1');
      openerWindow.focus();
      openerWindow.location.href = `${window.location.origin}/public/browse-games`;
    } catch {
      // Keep success screen visible if opener focus/redirect cannot be done.
    }

    setTimeout(() => {
      window.close();
    }, 300);
  }
}
