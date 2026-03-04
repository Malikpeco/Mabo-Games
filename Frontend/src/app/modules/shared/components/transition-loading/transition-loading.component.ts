import { Component, inject, Input } from '@angular/core';
import { BaseComponent } from '../../../../core/components/base-classes/base-component';
import { animate, style, transition, trigger } from '@angular/animations';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transition-loading',
  standalone: false,
  templateUrl: './transition-loading.component.html',
  styleUrl: './transition-loading.component.scss',
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('400ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class TransitionLoadingComponent extends BaseComponent {

  private router = inject(Router);

  @Input({ required: true }) title!: string;
  @Input({ required: true }) message!: string;
  @Input() redirectRoute?: string;
  @Input() countdownSeconds: number = 2;

  private intervalId?: number;
  currentCountdown: number = this.countdownSeconds;
  isVisible: boolean = true;  // ← Add this

  ngOnInit(): void {
    this.currentCountdown = this.countdownSeconds;
    this.startCountdown();  // ← Always start countdown
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  private startCountdown(): void {
    this.intervalId = window.setInterval(() => {
      this.currentCountdown--;

      if (this.currentCountdown <= 0) {
        clearInterval(this.intervalId);
        
        if (this.redirectRoute) {
          this.router.navigate([this.redirectRoute]);
        } else {
          this.isVisible = false;  // ← Just hide itself
        }
      }
    }, 1000);
  }
}