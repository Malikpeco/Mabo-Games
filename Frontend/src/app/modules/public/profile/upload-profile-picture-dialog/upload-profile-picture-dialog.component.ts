import { Component, ElementRef, Inject, OnDestroy, ViewChild, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { firstValueFrom } from 'rxjs';
import { UserApiService } from '../../../../api-services/users/users-api.service';
import { ToasterService } from '../../../../core/services/toaster.service';

export interface UploadProfilePictureDialogResult {
  updated: boolean;
}

@Component({
  selector: 'app-upload-profile-picture-dialog',
  standalone: false,
  templateUrl: './upload-profile-picture-dialog.component.html',
  styleUrl: './upload-profile-picture-dialog.component.scss',
})
export class UploadProfilePictureDialogComponent implements OnDestroy {
  @ViewChild('fileInput') fileInput?: ElementRef<HTMLInputElement>;

  private userApi = inject(UserApiService);
  private toaster = inject(ToasterService);

  selectedFile: File | null = null;
  previewUrl: string | null = null;
  isDragging = false;
  isUploading = false;
  errorMessage = '';

  constructor(
    private dialogRef: MatDialogRef<UploadProfilePictureDialogComponent, UploadProfilePictureDialogResult | null>,
    @Inject(MAT_DIALOG_DATA) public data: void,
  ) {}

  ngOnDestroy(): void {
    this.revokePreviewUrl();
  }

  close(): void {
    this.dialogRef.close(null);
  }

  openFilePicker(): void {
    this.fileInput?.nativeElement.click();
  }

  onFileInputChange(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.setSelectedFile(input.files?.[0] ?? null);
    input.value = '';
  }

  onDragEnter(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.isDragging = false;
    this.setSelectedFile(event.dataTransfer?.files?.[0] ?? null);
  }

  async upload(): Promise<void> {
    if (!this.selectedFile || this.isUploading) {
      return;
    }

    this.isUploading = true;
    this.errorMessage = '';

    try {
      await firstValueFrom(this.userApi.uploadProfileImage(this.selectedFile));
      this.toaster.success('Profile picture updated.');
      this.dialogRef.close({ updated: true });
    } catch {
      this.errorMessage = 'Could not upload the selected image.';
    } finally {
      this.isUploading = false;
    }
  }

  get canUpload(): boolean {
    return !!this.selectedFile && !this.isUploading;
  }

  private setSelectedFile(file: File | null): void {
    this.errorMessage = '';

    if (!file) {
      this.selectedFile = null;
      this.revokePreviewUrl();
      return;
    }

    if (file.type !== 'image/png' && file.type !== 'image/jpeg') {
      this.selectedFile = null;
      this.revokePreviewUrl();
      this.errorMessage = 'Please choose a PNG or JPEG image.';
      return;
    }

    if (file.size > 10 * 1024 * 1024) {
      this.selectedFile = null;
      this.revokePreviewUrl();
      this.errorMessage = 'Please choose an image smaller than 10 MB.';
      return;
    }

    this.selectedFile = file;
    this.revokePreviewUrl();
    this.previewUrl = URL.createObjectURL(file);
  }

  private revokePreviewUrl(): void {
    if (this.previewUrl) {
      URL.revokeObjectURL(this.previewUrl);
      this.previewUrl = null;
    }
  }
}