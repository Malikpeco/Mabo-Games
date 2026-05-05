import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ListPublisherDto } from '../../../../api-services/publishers/publishers-api.models';

@Component({
  selector: 'app-publisher-list-item',
  standalone: false,
  templateUrl: './publisher-list-item.component.html',
  styleUrl: './publisher-list-item.component.scss',
})
export class PublisherListItemComponent {
  @Input({ required: true }) publisher!: ListPublisherDto;

  @Output() editPublisher = new EventEmitter<ListPublisherDto>();
  @Output() deletePublisher = new EventEmitter<ListPublisherDto>();

  get gameCount(): number {
    return this.publisher.games?.length ?? 0;
  }

  get gameCountLabel(): string {
    const count = this.gameCount;
    return count === 1 ? '1 game' : `${count} games`;
  }

  onEdit(): void {
    this.editPublisher.emit(this.publisher);
  }

  onDelete(): void {
    this.deletePublisher.emit(this.publisher);
  }
}
