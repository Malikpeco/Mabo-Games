import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog';
import { FileUploadDialogComponent } from './components/file-upload-dialog/file-upload-dialog.component';

@NgModule({
  imports: [
    CommonModule,
    MatIconModule,
    MatDialogModule,
    FileUploadDialogComponent,
  ],
  exports: [
    FileUploadDialogComponent,
  ]
})
export class SharedModule { }
