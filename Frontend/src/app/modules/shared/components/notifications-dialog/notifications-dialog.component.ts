import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { NotificationsApiService } from '../../../../api-services/notifications/notifications-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';

@Component({
  selector: 'app-notifications-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MatDialogModule],
  templateUrl: './notifications-dialog.component.html',
  styleUrls: ['./notifications-dialog.component.scss']
})
export class NotificationsDialogComponent {
  private fb = inject(FormBuilder);
  private notificationsApi = inject(NotificationsApiService);
  private toaster = inject(ToasterService);
  isSending = false;

  constructor(private dialogRef: MatDialogRef<NotificationsDialogComponent>) {}

  form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    content: ['', [Validators.required, Validators.maxLength(500)]]
  });

  onCancel(): void {
    this.dialogRef.close();
  }

  onSend(): void {
    if (this.isSending || this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const { title, content } = this.form.getRawValue();
    const trimmedTitle = (title ?? '').trim();
    const trimmedContent = (content ?? '').trim();

    if (!trimmedTitle || !trimmedContent) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSending = true;

    this.notificationsApi.sendNotification({
      title: trimmedTitle,
      content: trimmedContent
    }).pipe(
      finalize(() => {
        this.isSending = false;
      })
    ).subscribe({
      next: () => {
        this.toaster.success('Notification sent successfully.');
        this.dialogRef.close();
      },
      error: (error) => {
        const message = error?.error?.message || error?.error?.title || 'Could not send notification. Please try again.';
        this.toaster.error(message);
      }
    });
  }

  get canSend(): boolean {
    return this.form.valid && !this.isSending;
  }

  get titleText(): string {
    return 'Send Notification';
  }

  get submitText(): string {
    return 'Send';
  }
}
