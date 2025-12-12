// src/app/modules/shared/components/fit-confirm-dialog/fit-fit-confirm-dialog.component.ts

import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { TranslateService } from '@ngx-translate/core';
import { DialogConfig, DialogButton, DialogType, DialogResult } from '../../models/dialog-config.model';

@Component({
  selector: 'app-fit-confirm-dialog',
  standalone: false,
  templateUrl: './fit-confirm-dialog.component.html',
  styleUrls: ['./fit-confirm-dialog.component.scss']
})
export class FitConfirmDialogComponent {
  DialogType = DialogType;

  constructor(
    public dialogRef: MatDialogRef<FitConfirmDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public config: DialogConfig,
    private translate: TranslateService
  ) {}

  onButtonClick(button: DialogButton, result?: any): void {
    const dialogResult: DialogResult = {
      button,
      data: result || this.config.data
    };
    this.dialogRef.close(dialogResult);
  }

  getIconClass(): string {
    switch (this.config.type) {
      case DialogType.SUCCESS:
        return 'icon-success';
      case DialogType.ERROR:
        return 'icon-error';
      case DialogType.WARNING:
        return 'icon-warning';
      case DialogType.QUESTION:
        return 'icon-question';
      case DialogType.INFO:
      default:
        return 'icon-info';
    }
  }

  getDefaultIcon(): string {
    if (this.config.icon) {
      return this.config.icon;
    }

    switch (this.config.type) {
      case DialogType.SUCCESS:
        return 'check_circle';
      case DialogType.ERROR:
        return 'error';
      case DialogType.WARNING:
        return 'warning';
      case DialogType.QUESTION:
        return 'help';
      case DialogType.INFO:
      default:
        return 'info';
    }
  }

  getButtonIcon(buttonType: DialogButton): string {
    switch (buttonType) {
      case DialogButton.OK:
        return 'check';
      case DialogButton.YES:
        return 'check';
      case DialogButton.NO:
        return 'close';
      case DialogButton.CANCEL:
        return 'close';
      case DialogButton.DELETE:
        return 'delete';
      case DialogButton.SAVE:
        return 'save';
      case DialogButton.CLOSE:
        return 'close';
      default:
        return 'check';
    }
  }

  getButtonLabel(button: any): string {
    // Ako ima custom label, koristi ga
    if (button.label) {
      return button.label;
    }

    // Ako ima translation key, koristi ga
    if (button.translationKey) {
      return this.translate.instant(button.translationKey);
    }

    // Inače koristi default translation
    return this.translate.instant(`DIALOGS.BUTTONS.${button.type.toUpperCase()}`);
  }

  getTitle(): string {
    return this.config.titleKey
      ? this.translate.instant(this.config.titleKey, this.config.titleParams)
      : this.config.title ?? '';
  }

  getMessage(): string {
    return this.config.messageKey
      ? this.translate.instant(this.config.messageKey, this.config.messageParams)
      : this.config.message ?? '';
  }
}
