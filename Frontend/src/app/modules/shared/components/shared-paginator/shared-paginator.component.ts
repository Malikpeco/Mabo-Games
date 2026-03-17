import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-shared-paginator',
  standalone: false,
  templateUrl: './shared-paginator.component.html',
  styleUrl: './shared-paginator.component.scss',
})
export class SharedPaginatorComponent {
  @Input() currentPage = 1;
  @Input() totalPages = 1;
  @Input() disabled = false;

  @Output() pageChange = new EventEmitter<number>();

  get visiblePages(): number[] {
    const total = Math.max(1, this.totalPages);
    const page = Math.min(Math.max(1, this.currentPage), total);
    const start = Math.max(1, page - 3);
    const end = Math.min(total, start + 6);
    const pages: number[] = [];

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }

  goToPage(page: number): void {
    if (this.disabled) return;
    if (page < 1 || page > this.totalPages) return;
    if (page === this.currentPage) return;

    this.pageChange.emit(page);
  }
}
