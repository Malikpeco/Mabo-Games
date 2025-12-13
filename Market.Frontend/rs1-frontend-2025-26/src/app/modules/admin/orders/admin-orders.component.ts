import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import {Subject} from 'rxjs';
import {debounceTime, distinctUntilChanged, takeUntil} from 'rxjs/operators';
import {FormControl} from '@angular/forms';
import {BaseListPagedComponent} from '../../../core/components/base-classes/base-list-paged-component';
import {ListOrdersQueryDto, ListOrdersRequest, OrderStatusType} from '../../../api-services/orders/orders-api.models';
import {OrdersApiService} from '../../../api-services/orders/orders-api.service';
import {ToasterService} from '../../../core/services/toaster.service';
import {OrderStatusHelper} from '../../../api-services/orders/order-status.helper';
import {ChangeStatusDialogComponent} from './change-status-dialog/change-status-dialog.component';
import {OrderDetailsDialogComponent} from './admin-orders-details-dialog/order-details-dialog.component';


@Component({
  selector: 'app-admin-orders',
  standalone: false,
  templateUrl: './admin-orders.component.html',
  styleUrl: './admin-orders.component.scss',
})
export class AdminOrdersComponent
  extends BaseListPagedComponent<ListOrdersQueryDto, ListOrdersRequest>
  implements OnInit, OnDestroy
{
  private ordersApi = inject(OrdersApiService);
  private dialog = inject(MatDialog);
  private toaster = inject(ToasterService);
  private destroy$ = new Subject<void>();

  // Table columns
  displayedColumns: string[] = [
    'referenceNumber',
    'customer',
    'orderedAt',
    'totalAmount',
    'status',
    'actions'
  ];

  // Search control with debounce
  searchControl = new FormControl('');

  // Status filter
  statusFilter: OrderStatusType | null = null;
  statusOptions = OrderStatusHelper.getStatusOptions();

  // Expose OrderStatusType for template
  OrderStatusType = OrderStatusType;

  constructor() {
    super();
    this.request = new ListOrdersRequest();
    this.request.paging.pageSize = 20;
  }

  ngOnInit(): void {
    this.initList();
    this.setupSearchDebounce();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  /**
   * Setup search with debounce and minimum length
   */
  private setupSearchDebounce(): void {
    this.searchControl.valueChanges
      .pipe(
        debounceTime(400), // Wait 400ms after user stops typing
        distinctUntilChanged(), // Only if value actually changed
        takeUntil(this.destroy$)
      )
      .subscribe((searchTerm) => {
        // Only search if 3+ characters or empty (to clear)
        if (!searchTerm || searchTerm.length >= 3) {
          this.onSearchChange(searchTerm || '');
        }
      });
  }

  protected loadPagedData(): void {
    this.startLoading();

    this.ordersApi.list(this.request).subscribe({
      next: (response) => {
        this.handlePageResult(response);
        this.stopLoading();
      },
      error: (err) => {
        this.stopLoading('Failed to load orders');
        console.error('Load orders error:', err);
      },
    });
  }

  // === Filters ===

  onSearchChange(searchTerm: string): void {
    this.request.search = searchTerm;
    this.request.paging.page = 1; // Reset to first page
    this.loadPagedData();
  }

  onStatusFilterChange(status: OrderStatusType | null): void {
    this.statusFilter = status;
    // Note: Backend needs to support status filter
    // For now, we filter client-side or update backend
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  clearFilters(): void {
    this.searchControl.setValue('', { emitEvent: false });
    this.statusFilter = null;
    this.request.search = null;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  // === Actions ===

  onViewDetails(order: ListOrdersQueryDto, event?: MouseEvent): void {
    // spriječi da klik sa dugmeta ode na <tr> i ponovo otvori dialog
    event?.stopPropagation();

    const dialogRef = this.dialog.open(OrderDetailsDialogComponent, {
      width: '900px',
      maxWidth: '95vw',
      maxHeight: '90vh',
      data: {
        orderId: order.id
      },
      panelClass: 'order-details-dialog'
    });

    dialogRef.afterClosed().subscribe((changed: boolean) => {
      if (changed) {
        this.loadPagedData(); // Reload if status changed
      }
    });
  }

  onChangeStatus(order: ListOrdersQueryDto, event?: Event): void {
    // Prevent row click
    if (event) {
      event.stopPropagation();
    }

    const dialogRef = this.dialog.open(ChangeStatusDialogComponent, {
      width: '500px',
      maxWidth: '90vw',
      data: {
        order: order
      },
      panelClass: 'change-status-dialog'
    });

    dialogRef.afterClosed().subscribe((newStatus: OrderStatusType | undefined) => {
      if (newStatus !== undefined) {
        this.changeOrderStatus(order.id, newStatus);
      }
    });
  }

  private changeOrderStatus(orderId: number, newStatus: OrderStatusType): void {
    this.startLoading();

    this.ordersApi.changeStatus(orderId, newStatus).subscribe({
      next: () => {
        this.toaster.success('Order status updated successfully');
        this.loadPagedData(); // Reload list
      },
      error: (err) => {
        this.stopLoading();

        // Extract error message
        const errorMessage = this.extractErrorMessage(err);
        this.toaster.error(errorMessage || 'Failed to update order status');

        console.error('Change status error:', err);
      }
    });
  }

  // === Status Helpers (for template) ===

  getStatusLabel(status: OrderStatusType): string {
    return OrderStatusHelper.getLabel(status);
  }

  getStatusIcon(status: OrderStatusType): string {
    return OrderStatusHelper.getIcon(status);
  }

  getStatusClass(status: OrderStatusType): string {
    return OrderStatusHelper.getColorClass(status);
  }

  canChangeStatus(order: ListOrdersQueryDto): boolean {
    // Can change if not in final state
    return order.status !== OrderStatusType.Completed &&
      order.status !== OrderStatusType.Cancelled;
  }

  // === Display Helpers ===

  getCustomerName(order: ListOrdersQueryDto): string {
    return `${order.user.userFirstname} ${order.user.userLastname}`;
  }

  getCustomerAddress(order: ListOrdersQueryDto): string {
    return `${order.user.userAddress}, ${order.user.userCity}`;
  }

  /**
   * Extract user-friendly error message from HTTP error response
   */
  private extractErrorMessage(err: any): string | null {
    if (err?.error) {
      if (typeof err.error === 'string') {
        return err.error;
      }

      if (err.error.message) {
        return err.error.message;
      }

      if (err.error.title) {
        return err.error.title;
      }
    }

    if (err?.message) {
      return err.message;
    }

    return null;
  }
}
