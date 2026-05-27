import { Component, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { NotificationsDialogComponent } from '../../shared/components/notifications-dialog/notifications-dialog.component';


@Component({
  selector: 'app-admin-panel',
  standalone: false,
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.scss',
})
export class AdminPanelComponent {
  private matDialog = inject(MatDialog);

  isGamesOpen = false;

  toggleGames() {
    this.isGamesOpen = !this.isGamesOpen;
  }

  openNotificationsDialog(): void {
    this.matDialog.open(NotificationsDialogComponent, {
      width: '760px',
      maxWidth: 'calc(100vw - 24px)',
      disableClose: false,
      panelClass: ['custom-dialog-container', 'site-dialog-panel'],
      backdropClass: 'site-dialog-backdrop',
    });
  }

}
