import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';

@Component({
  selector: 'app-carousel-row',
  standalone: false,
  templateUrl: './carousel-row.component.html',
  styleUrl: './carousel-row.component.scss',
})
export class CarouselRowComponent implements OnChanges {
  @Input() itemCount = 0;
  @Input() itemsPerPage = 4;
  @Input() rows = 1;
  @Input() showDots = true;

  @Input() page = 0;
  @Output() pageChange = new EventEmitter<number>();

  get pageCount(): number {
    return Math.max(1, Math.ceil(this.itemCount / this.itemsPerPage));
  }

  get pages(): number[] {
    return Array.from({ length: this.pageCount }, (_, i) => i);
  }

  ngOnChanges(): void {
    const maxPage = this.pageCount - 1;
    if (this.page > maxPage) {
      this.setPage(maxPage);
    }
  }

  prev(): void {
    this.setPage(Math.max(0, this.page - 1));
  }

  next(): void {
    this.setPage(Math.min(this.pageCount - 1, this.page + 1));
  }

  goToPage(index: number): void {
    this.setPage(index);
  }

  private setPage(index: number): void {
    if (index === this.page) return;
    this.page = index;
    this.pageChange.emit(index);
  }
}
