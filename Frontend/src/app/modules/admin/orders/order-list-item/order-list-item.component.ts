import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ListOrdersQueryDto } from '../../../../api-services/orders/orders-api.models';

@Component({
  selector: 'app-order-list-item',
  standalone: false,
  templateUrl: './order-list-item.component.html',
  styleUrl: './order-list-item.component.scss',
})
export class OrderListItemComponent {
  @Input({ required: true }) order!: ListOrdersQueryDto;

  @Output() viewDetails = new EventEmitter<ListOrdersQueryDto>();

  get formattedDate(): string {
    const date = new Date(this.order.orderDate);
    return date.toLocaleDateString('en-US', { year: 'numeric', month: 'short', day: 'numeric' });
  }

  get formattedTotal(): string {
    return new Intl.NumberFormat('en-US', { style: 'currency', currency: 'EUR' }).format(this.order.totalAmount);
  }

  get statusBadgeClass(): string {
    return `status-${this.order.status.toLowerCase()}`;
  }

  onViewDetails(): void {
    this.viewDetails.emit(this.order);
  }
}
