import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { DialogButton, DialogType } from '../../models/dialog-config.model';
import { DialogHelperService } from '../../services/dialog-helper.service';
import { NotificationItem } from '../notification-item.model';

@Component({
  selector: 'app-mabo-notifications-dropdown',
  standalone: false,
  templateUrl: './mabo-notifications-dropdown.component.html',
  styleUrl: './mabo-notifications-dropdown.component.scss',
})
export class MaboNotificationsDropdownComponent {
  @Input() notifications: NotificationItem[] = [];
  @Output() notificationSelected = new EventEmitter<NotificationItem>();
  @Output() clearReadRequested = new EventEmitter<void>();
  private dialog = inject(DialogHelperService);

  get hasReadNotifications(): boolean {
    return this.notifications.some((notification) => !notification.unread);
  }

  selectNotification(notification: NotificationItem): void {
    this.notificationSelected.emit(notification);
  }

  clearReadNotifications(): void {
    this.dialog
      .open({
        type: DialogType.QUESTION,
        title: 'Clear read notifications?',
        message: 'This will remove all notifications that have already been read.',
        icon: 'delete_forever',
        buttons: [
          { type: DialogButton.NO },
          { type: DialogButton.YES, color: 'primary' },
        ],
      })
      .subscribe((result) => {
        if (result?.button === DialogButton.YES) {
          this.clearReadRequested.emit();
        }
      });
  }
}