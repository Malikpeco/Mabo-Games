import { Component, ElementRef, EventEmitter, Inject, Output, ViewChild, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';

interface FileUploadDialogData {
  managedUploading?: boolean;
  title?: string;
  subtitle?: string;
  emptyStateTitle?: string;
  emptyStateSubtitle?: string;
  uploadButtonText?: string;
  uploadingText?: string;
  uploadIcon?: string;
  acceptedFileTypes?: string[];
  maxFileSizeBytes?: number;
  showImagePreview?: boolean;
}

@Component({
  selector: 'app-file-upload-dialog',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatDialogModule],
  templateUrl: './file-upload-dialog.component.html',
  styleUrl: './file-upload-dialog.component.scss',
})
export class FileUploadDialogComponent implements OnDestroy {
  @ViewChild('fileInput') fileInput?: ElementRef<HTMLInputElement>;

  @Output() fileSelected = new EventEmitter<File>();
  @Output() uploadComplete = new EventEmitter<boolean>();

  private readonly defaultTitle = 'Upload a new image';
  private readonly defaultSubtitle = 'Drag and drop an image here, or browse for a file on your device.';
  private readonly defaultEmptyStateTitle = 'Drop your image here';
  private readonly defaultEmptyStateSubtitle = 'PNG or JPG up to 10 MB.';
  private readonly defaultUploadButtonText = 'Upload picture';
  private readonly defaultUploadingText = 'Uploading...';
  private readonly defaultUploadIcon = 'cloud_upload';
  private readonly defaultAcceptedFileTypes: string[] = ['image/png', 'image/jpeg'];
  private readonly defaultMaxFileSizeBytes = 10 * 1024 * 1024;

  selectedFile: File | null = null;
  previewUrl: string | null = null;
  isDragging = false;
  isUploading = false;
  errorMessage = '';

  get title(): string {
    return this.data?.title?.trim() || this.defaultTitle;
  }

  get subtitle(): string {
    return this.data?.subtitle?.trim() || this.defaultSubtitle;
  }

  get emptyStateTitle(): string {
    return this.data?.emptyStateTitle?.trim() || this.defaultEmptyStateTitle;
  }

  get emptyStateSubtitle(): string {
    return this.data?.emptyStateSubtitle?.trim() || this.defaultEmptyStateSubtitle;
  }

  get uploadButtonText(): string {
    return this.data?.uploadButtonText?.trim() || this.defaultUploadButtonText;
  }

  get uploadingText(): string {
    return this.data?.uploadingText?.trim() || this.defaultUploadingText;
  }

  get uploadIcon(): string {
    return this.data?.uploadIcon?.trim() || this.defaultUploadIcon;
  }

  get acceptedFileTypes(): string[] {
    if (!this.data?.acceptedFileTypes?.length) {
      return this.defaultAcceptedFileTypes;
    }

    return this.data.acceptedFileTypes;
  }

  get maxFileSizeBytes(): number {
    return this.data?.maxFileSizeBytes && this.data.maxFileSizeBytes > 0
      ? this.data.maxFileSizeBytes
      : this.defaultMaxFileSizeBytes;
  }

  get showImagePreview(): boolean {
    return this.data?.showImagePreview ?? true;
  }

  get acceptedMimeTypes(): string {
    return this.acceptedFileTypes.join(',');
  }

  get canUpload(): boolean {
    return !!this.selectedFile && !this.isUploading;
  }

  constructor(
    private dialogRef: MatDialogRef<FileUploadDialogComponent, any>,
    @Inject(MAT_DIALOG_DATA) public data: FileUploadDialogData | null,
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
      this.fileSelected.emit(this.selectedFile);
      // Dialog can be closed by parent component after handling the file
    } catch {
      this.errorMessage = 'Could not upload the selected file.';
    }

    if (!this.data?.managedUploading) {
      this.isUploading = false;
    }
  }

  private setSelectedFile(file: File | null): void {
    this.errorMessage = '';

    if (!file) {
      this.selectedFile = null;
      this.revokePreviewUrl();
      return;
    }

    const isAcceptedType = this.acceptedFileTypes.some(
      type => type === '*/*' || file.type === type || this.matchesWildcard(file.type, type)
    );

    if (!isAcceptedType) {
      this.selectedFile = null;
      this.revokePreviewUrl();
      const typesList = this.acceptedFileTypes.join(' or ');
      this.errorMessage = `Please choose a ${typesList} file.`;
      return;
    }

    if (file.size > this.maxFileSizeBytes) {
      this.selectedFile = null;
      this.revokePreviewUrl();
      const maxSizeMB = Math.round(this.maxFileSizeBytes / (1024 * 1024));
      this.errorMessage = `Please choose a file smaller than ${maxSizeMB} MB.`;
      return;
    }

    this.selectedFile = file;
    this.revokePreviewUrl();

    // Preview is optional so this dialog can be reused for generic file uploads.
    if (this.showImagePreview && file.type.startsWith('image/')) {
      this.previewUrl = URL.createObjectURL(file);
    }
  }

  private matchesWildcard(mimeType: string, pattern: string): boolean {
    if (pattern === '*/*') {
      return true;
    }
    if (pattern.endsWith('/*')) {
      return mimeType.startsWith(pattern.slice(0, -2));
    }
    return mimeType === pattern;
  }

  private revokePreviewUrl(): void {
    if (this.previewUrl) {
      URL.revokeObjectURL(this.previewUrl);
      this.previewUrl = null;
    }
  }
}
