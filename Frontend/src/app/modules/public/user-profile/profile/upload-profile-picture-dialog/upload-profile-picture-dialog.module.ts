import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogModule } from '@angular/material/dialog';
import { FileUploadDialogComponent } from '../../../../../shared/components/file-upload-dialog/file-upload-dialog.component';
import { UploadProfilePictureDialogComponent } from './upload-profile-picture-dialog.component';

@NgModule({
  declarations: [
    UploadProfilePictureDialogComponent,
  ],
  imports: [
    CommonModule,
    MatDialogModule,
    FileUploadDialogComponent,
  ],
})
export class UploadProfilePictureDialogModule { }
