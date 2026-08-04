import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild } from '@angular/core';
import { environment } from '../../../../../environments/environment';

declare const grecaptcha: any;

@Component({
  selector: 'app-recaptcha',
  standalone: false,
  template: '<div #container></div>',
})
export class RecaptchaComponent implements AfterViewInit, OnDestroy {
  @Input() siteKey: string = environment.recaptchaSiteKey;
  @Output() resolved = new EventEmitter<string | null>();

  @ViewChild('container', { static: true }) container!: ElementRef<HTMLDivElement>;

  private widgetId: number | null = null;
  private pollHandle: ReturnType<typeof setTimeout> | null = null;

  ngAfterViewInit(): void {
    this.renderWhenReady();
  }

  ngOnDestroy(): void {
    if (this.pollHandle) {
      clearTimeout(this.pollHandle);
    }
  }

  reset(): void {
    if (this.widgetId !== null && typeof grecaptcha !== 'undefined') {
      grecaptcha.reset(this.widgetId);
    }
  }

  private renderWhenReady(): void {
    if (typeof grecaptcha !== 'undefined' && grecaptcha.render) {
      this.widgetId = grecaptcha.render(this.container.nativeElement, {
        sitekey: this.siteKey,
        callback: (token: string) => this.resolved.emit(token),
        'expired-callback': () => this.resolved.emit(null),
        'error-callback': () => this.resolved.emit(null),
      });
      return;
    }

    this.pollHandle = setTimeout(() => this.renderWhenReady(), 200);
  }
}
