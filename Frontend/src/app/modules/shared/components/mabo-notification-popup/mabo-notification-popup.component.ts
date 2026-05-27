import { Component, EventEmitter, Input, Output } from '@angular/core';
import { NotificationItem } from '../notification-item.model';

@Component({
  selector: 'app-mabo-notification-popup',
  standalone: false,
  templateUrl: './mabo-notification-popup.component.html',
  styleUrl: './mabo-notification-popup.component.scss',
})
export class MaboNotificationPopupComponent {
  @Input() notification: NotificationItem | null = null;
  @Output() closePopup = new EventEmitter<void>();

  close(): void {
    this.closePopup.emit();
  }
}