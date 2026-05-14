import { Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { UserApiService } from '../../../../../api-services/users/users-api.service';
import { ToasterService } from '../../../../../core/services/toaster.service';

export interface UploadProfilePictureDialogResult {
  updated: boolean;
}

@Component({
  selector: 'app-upload-profile-picture-dialog',
  standalone: false,
  templateUrl: './upload-profile-picture-dialog.component.html',
  styleUrl: './upload-profile-picture-dialog.component.scss',
})
export class UploadProfilePictureDialogComponent {
  private userApi = inject(UserApiService);
  private toaster = inject(ToasterService);

  constructor(
    private dialogRef: MatDialogRef<UploadProfilePictureDialogComponent, UploadProfilePictureDialogResult | null>,
    @Inject(MAT_DIALOG_DATA) public data: void,
  ) {}

  close(): void {
    this.dialogRef.close(null);
  }

  async handleFileSelected(file: File): Promise<void> {
    try {
      await firstValueFrom(this.userApi.uploadProfileImage(file));
      this.toaster.success('Profile picture updated.');
      this.dialogRef.close({ updated: true });
    } catch {
      this.toaster.error('Could not upload the selected image.');
    }
  }
}
