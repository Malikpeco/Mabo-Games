import {Component, inject, Inject} from '@angular/core';
import {GetOrderByIdQueryDto, OrderStatusType} from '../../../../api-services/orders/orders-api.models';
import { OrderStatusHelper } from '../../../../api-services/orders/order-status.helper';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {OrdersApiService} from '../../../../api-services/orders/orders-api.service';

export interface OrderDetailsDialogData {
  orderId: number;
}

@Component({
  selector: 'app-admin-orders-details-dialog',
  standalone: false,
  templateUrl: './order-details-dialog.component.html',
  styleUrl: './order-details-dialog.component.scss',
})
export class OrderDetailsDialogComponent {
  private ordersApi = inject(OrdersApiService);
  private dialogRef = inject(MatDialogRef<OrderDetailsDialogComponent>);

  order?: GetOrderByIdQueryDto;
  isLoading = false;
  errorMessage: string | null = null;

  constructor(@Inject(MAT_DIALOG_DATA) public data: OrderDetailsDialogData) {}

  ngOnInit(): void {
    this.loadOrderDetails();
  }

  private loadOrderDetails(): void {
    this.isLoading = true;
    this.errorMessage = null;

    this.ordersApi.getById(this.data.orderId).subscribe({
      next: (order) => {
        this.order = order;
        this.isLoading = false;
      },
      error: (err) => {
        this.errorMessage = 'Failed to load order details';
        this.isLoading = false;
        console.error('Load order details error:', err);
      }
    });
  }

  // === Status Helpers ===

  getStatusLabel(status: OrderStatusType): string {
    return OrderStatusHelper.getLabel(status);
  }

  getStatusIcon(status: OrderStatusType): string {
    return OrderStatusHelper.getIcon(status);
  }

  getStatusClass(status: OrderStatusType): string {
    return OrderStatusHelper.getColorClass(status);
  }

  // === Display Helpers ===

  getCustomerName(): string {
    if (!this.order) return '';
    return `${this.order.user.userFirstname} ${this.order.user.userLastname}`;
  }

  getCustomerAddress(): string {
    if (!this.order) return '';
    return `${this.order.user.userAddress}, ${this.order.user.userCity}`;
  }

  // === Actions ===

  onClose(): void {
    this.dialogRef.close(false);
  }

  retry(): void {
    this.loadOrderDetails();
  }
}
