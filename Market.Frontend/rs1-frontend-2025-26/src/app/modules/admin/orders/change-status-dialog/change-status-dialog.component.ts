import { Component, inject, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import {ListOrdersQueryDto, OrderStatusType} from '../../../../api-services/orders/orders-api.models';
import {OrderStatusHelper} from '../../../../api-services/orders/order-status.helper';

export interface ChangeStatusDialogData {
  order: ListOrdersQueryDto;
}

@Component({
  selector: 'app-change-status-dialog',
  standalone: false,
  templateUrl: './change-status-dialog.component.html',
  styleUrl: './change-status-dialog.component.scss'
})
export class ChangeStatusDialogComponent {
  private dialogRef = inject(MatDialogRef<ChangeStatusDialogComponent>);

  selectedStatus?: OrderStatusType;
  availableStatuses: OrderStatusType[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: ChangeStatusDialogData) {
    this.availableStatuses = OrderStatusHelper.getNextStatuses(data.order.status);

    // Pre-select first available status
    if (this.availableStatuses.length > 0) {
      this.selectedStatus = this.availableStatuses[0];
    }
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

  getCurrentStatusLabel(): string {
    return OrderStatusHelper.getLabel(this.data.order.status);
  }

  getCurrentStatusIcon(): string {
    return OrderStatusHelper.getIcon(this.data.order.status);
  }

  getCurrentStatusClass(): string {
    return OrderStatusHelper.getColorClass(this.data.order.status);
  }

  // === Actions ===

  onConfirm(): void {
    if (this.selectedStatus !== undefined) {
      this.dialogRef.close(this.selectedStatus);
    }
  }

  onCancel(): void {
    this.dialogRef.close(undefined);
  }

  canConfirm(): boolean {
    return this.selectedStatus !== undefined &&
      this.selectedStatus !== this.data.order.status;
  }

  protected readonly OrderStatusType = OrderStatusType;
}
