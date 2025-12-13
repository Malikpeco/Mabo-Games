export enum DialogType {
  INFO = 'info',
  SUCCESS = 'success',
  WARNING = 'warning',
  ERROR = 'error',
  QUESTION = 'question',
  CUSTOM = 'custom'
}

export enum DialogButton {
  OK = 'ok',
  CANCEL = 'cancel',
  YES = 'yes',
  NO = 'no',
  CLOSE = 'close',
  DELETE = 'delete',
  SAVE = 'save'
}

export interface DialogButtonConfig {
  type: DialogButton;
  label?: string;
  translationKey?: string;
  color?: 'primary' | 'accent' | 'warn';
  result?: any;
}

export interface DialogConfig {
  type: DialogType;
  title?: string;
  titleKey?: string;
  titleParams?: any;
  message?: string;
  messageKey?: string;
  messageParams?: any;
  icon?: string;
  buttons?: DialogButtonConfig[];
  width?: string;
  disableClose?: boolean;
  data?: any;
}

export interface DialogResult {
  button: DialogButton;
  data?: any;
}
