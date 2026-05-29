import { Component, inject, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { CurrentUserService } from '../../../../core/services/auth/current-user.service';
import { ToasterService } from '../../../../core/services/toaster.service';
import { NotificationItem } from '../notification-item.model';
import { UserNotificationsApiService } from '../../../../api-services/user-notifications/user-notifications-api.service';
import { UserNotificationDto } from '../../../../api-services/user-notifications/user-notifications-api.models';

@Component({
  selector: 'app-mabo-navbar',
  standalone: false,
  templateUrl: './mabo-navbar.component.html',
  styleUrl: './mabo-navbar.component.scss',
})
export class MaboNavbarComponent implements OnInit {
  router = inject(Router);
  route = inject(ActivatedRoute);
  private currentUserService = inject(CurrentUserService);
  private userNotificationsApi = inject(UserNotificationsApiService);
  isAdmin = this.currentUserService.isAdmin;
  isAuthenticated = this.currentUserService.isAuthenticated;
  toaster = inject(ToasterService);
  notificationsOpen = false;
  selectedNotification: NotificationItem | null = null;

  notifications: NotificationItem[] = [];

  get unreadCount(): number {
    return this.notifications.filter((notification) => notification.unread).length;
  }

  ngOnInit(): void {
    if (this.isAuthenticated()) {
      this.loadNotifications();
    }
  }
  
  logout():void{
    this.router.navigate(['/auth/logout']);
  }

  toggleNotifications(): void {
    if (!this.isAuthenticated()) {
      this.toaster.error('You need to be logged in to view notifications.');
      return;
    }

    this.notificationsOpen = !this.notificationsOpen;

    if (this.notificationsOpen) {
      this.loadNotifications();
    }
  }

  closeNotifications(): void {
    this.notificationsOpen = false;
  }

  openNotification(notification: NotificationItem): void {
    this.userNotificationsApi.getById(notification.id).subscribe({
      next: (result) => {
        this.selectedNotification = this.mapNotification(result);
        this.notifications = this.notifications.map((currentNotification) =>
          currentNotification.id === result.id
            ? {
                ...currentNotification,
                unread: !result.isRead,
                content: result.content,
                message: this.buildPreview(result.content),
                sentAt: result.sentAt,
                time: this.formatSentAt(result.sentAt),
              }
            : currentNotification
        );
        this.notificationsOpen = false;
      },
      error: () => this.toaster.error('Unable to open the notification. Please try again.'),
    });
  }

  clearReadNotifications(): void {
    this.userNotificationsApi.clearRead().subscribe({
      next: () => {
        this.notifications = this.notifications.filter((notification) => notification.unread);

        if (this.selectedNotification && !this.selectedNotification.unread) {
          this.selectedNotification = null;
        }
      },
      error: () => this.toaster.error('Unable to clear read notifications right now.'),
    });
  }

  closeNotificationPopup(): void {
    this.selectedNotification = null;
  }

  private loadNotifications(): void {
    this.userNotificationsApi.list().subscribe({
      next: (results) => {
        this.notifications = results.map((notification) => this.mapNotification(notification));
        if (this.selectedNotification) {
          const refreshedSelection = this.notifications.find((notification) => notification.id === this.selectedNotification?.id);
          this.selectedNotification = refreshedSelection ?? null;
        }
      },
      error: () => {
        this.notifications = [];
      },
    });
  }

  private mapNotification(notification: UserNotificationDto): NotificationItem {
    return {
      id: notification.id,
      title: notification.title,
      message: this.buildPreview(notification.content),
      content: notification.content,
      time: this.formatSentAt(notification.sentAt),
      unread: !notification.isRead,
      sentAt: notification.sentAt,
    };
  }

  private buildPreview(content: string): string {
    const trimmedContent = content.trim();

    if (trimmedContent.length <= 80) {
      return trimmedContent;
    }

    return `${trimmedContent.slice(0, 77)}...`;
  }

  private formatSentAt(sentAt: string): string {
    const date = new Date(sentAt);

    if (Number.isNaN(date.getTime())) {
      return sentAt;
    }

    return new Intl.DateTimeFormat('en', {
      dateStyle: 'medium',
      timeStyle: 'short',
    }).format(date);
  }
  
  cartbtnclick():void{
    if(!this.isAuthenticated()){
      this.toaster.error("You need to be logged in to access the cart.");
    }
    else{
      this.router.navigate(['/public/cart']);
    }
  }

  isNavActive(key: 'cart'): boolean {
    const url = this.router.url;
    return url.startsWith('/public/cart') || url.startsWith('/public/checkout') || url.startsWith('/public/payment');
  }

  adminPanelBtnClick():void{
    if(!this.isAuthenticated()){
      this.toaster.error("You need to be logged in to access the admin panel.");
    }
    else if(!this.isAdmin()){
      this.toaster.error("You do not have permission to access the admin panel.");
    }
    else{
      this.router.navigate(['/admin']);
    }
  }

}
