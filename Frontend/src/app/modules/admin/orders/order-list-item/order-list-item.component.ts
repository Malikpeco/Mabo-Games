import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ListOrdersQueryDto } from '../../../../api-services/orders/orders-api.models';
import { APP_LOCALE } from '../../../../core/constants/locale';

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
    return date.toLocaleDateString(APP_LOCALE, { year: 'numeric', month: 'short', day: 'numeric' });
  }

  get formattedTotal(): string {
    return new Intl.NumberFormat(APP_LOCALE, { style: 'currency', currency: 'EUR' }).format(this.order.totalAmount);
  }

  get statusBadgeClass(): string {
    return `status-${this.order.status.toLowerCase()}`;
  }

  onViewDetails(): void {
    this.viewDetails.emit(this.order);
  }
}
