import { Component, OnInit, inject } from '@angular/core';
import { Router } from '@angular/router';
import { trigger, transition, style, animate } from '@angular/animations';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';

@Component({
  selector: 'app-logout',
  standalone: false,
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.scss',
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(10px)' }),
        animate('400ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class LogoutComponent implements OnInit {
  private router = inject(Router);
  private auth = inject(AuthFacadeService);

  countdownSeconds = 2;

  ngOnInit(): void {
    // Call logout (handles API call + clears state)
    this.auth.logout().subscribe({
      next: () => this.startCountdown(),
      error: () => this.startCountdown() // Even if API fails, clear local state
    });
  }

  private startCountdown(): void {
    const intervalId = setInterval(() => {
      this.countdownSeconds--;

      if (this.countdownSeconds <= 0) {
        clearInterval(intervalId);
        this.router.navigate(['/login']);
      }
    }, 1000);
  }
}
