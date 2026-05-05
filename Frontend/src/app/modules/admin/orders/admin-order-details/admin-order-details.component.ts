import { Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { OrdersApiService } from '../../../../api-services/orders/orders-api.service';
import { OrderDetailsDto } from '../../../../api-services/orders/orders-api.models';

@Component({
  selector: 'app-admin-order-details',
  standalone: false,
  templateUrl: './admin-order-details.component.html',
  styleUrl: './admin-order-details.component.scss',
})
export class AdminOrderDetailsComponent implements OnInit {
  private ordersApi = inject(OrdersApiService);
  private route = inject(ActivatedRoute);
  private location = inject(Location);

  order?: OrderDetailsDto;
  isLoading = false;
  error = '';

  ngOnInit(): void {
    this.loadOrder();
  }


  loadOrder(): void {
    const rawId = this.route.snapshot.paramMap.get('id');
    const orderId = Number(rawId);

    if (!Number.isFinite(orderId) || orderId <= 0) {
      this.error = 'Invalid order id.';
      return;
    }

    this.isLoading = true;
    this.error = '';

    this.ordersApi.getById(orderId).subscribe({
      next: (res) => {

        this.order = res;
        this.isLoading = false;
      },

      error: (err) => {
        this.isLoading = false;
        this.order = undefined;


        this.error = err?.status === 404 ? 'Order not found.' : 'Failed to load order details.';
      },
    });
  }

  goBack(): void {
    this.location.back();
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'EUR' }).format(value);
  }

  get statusClass(): string {
    return `status-${this.order?.status?.toLowerCase()}`;
  }
}
