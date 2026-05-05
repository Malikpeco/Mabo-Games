import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { OrdersApiService } from '../../../../api-services/orders/orders-api.service';
import { ListOrdersQueryDto, ListOrdersRequest } from '../../../../api-services/orders/orders-api.models';
import { BaseListPagedComponent } from '../../../../core/components/base-classes/base-list-paged-component';

@Component({
  selector: 'app-admin-orders',
  standalone: false,
  templateUrl: './admin-orders.component.html',
  styleUrl: './admin-orders.component.scss',
})
export class AdminOrdersComponent extends BaseListPagedComponent<ListOrdersQueryDto, ListOrdersRequest> implements OnInit, OnDestroy {
  private ordersApi = inject(OrdersApiService);
  private router = inject(Router);
  private searchDebounceTimer?: ReturnType<typeof setTimeout>;

  searchTerm = '';
  statusFilter = '';
  dateFromFilter = '';
  dateToFilter = '';
  readonly statusOptions: Array<{ value: string; label: string }> = [
    { value: '', label: 'All Statuses' },
    { value: 'Pending', label: 'Pending' },
    { value: 'Paid', label: 'Paid' },
    { value: 'Cancelled', label: 'Cancelled' },
  ];
  pageSizeOptions: number[] = [10, 25, 50, 100];

  constructor() {
    super();
    this.request = new ListOrdersRequest();
    this.request.paging.pageSize = 10;
  }

  get hasSearchTerm(): boolean {
    return this.searchTerm.trim().length > 0;
  }

  get page(): number {
    return this.request.paging.page;
  }

  get pageSize(): number {
    return this.request.paging.pageSize;
  }

  ngOnInit(): void {
    this.initList();
  }

  ngOnDestroy(): void {
    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }
  }

  protected loadPagedData(): void {
    this.startLoading();
    this.syncRequestFilters();

    this.ordersApi
      .getOrders(this.request)
      .subscribe({
        next: (res) => {
          this.handlePageResult(res);
          this.stopLoading();
        },
        error: () => {
          this.items = [];
          this.totalItems = 0;
          this.totalPages = 1;
          this.stopLoading('Failed to load orders.');
        },
      });
  }

  onSearchChange(term: string): void {
    this.searchTerm = term;
    this.request.paging.page = 1;

    if (this.searchDebounceTimer) {
      clearTimeout(this.searchDebounceTimer);
    }

    this.searchDebounceTimer = setTimeout(() => {
      this.loadPagedData();
    }, 300);
  }

  onStatusFilterChange(status: string): void {
    this.statusFilter = status;
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onDateFilterChange(): void {
    this.request.paging.page = 1;
    this.loadPagedData();
  }

  onPageChange(newPage: number): void {
    this.goToPage(newPage);
  }

  onPageSizeChange(size: number | string): void {
    const parsedSize = Number(size);
    if (!Number.isFinite(parsedSize) || parsedSize <= 0) {
      return;
    }

    this.changePageSize(parsedSize);
  }

  onViewDetails(order: ListOrdersQueryDto): void {
    this.router.navigate(['/admin/orders', order.id, 'details']);
  }

  private syncRequestFilters(): void {
    this.request.search = this.searchTerm.trim() || null;
    this.request.statusFilter = this.statusFilter || null;
    this.request.dateFrom = this.dateFromFilter || null;
    this.request.dateTo = this.dateToFilter || null;
  }
}
