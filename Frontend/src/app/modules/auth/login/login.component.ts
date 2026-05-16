import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { AuthFacadeService } from '../../../core/services/auth/auth-facade.service';
import { LoginCommand } from '../../../api-services/auth/auth-api.model';
import { CurrentUserService } from '../../../core/services/auth/current-user.service';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private auth = inject(AuthFacadeService);
  private router = inject(Router);
  private currentUser = inject(CurrentUserService);
  hidePassword = true;

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
    rememberMe: [false],
  }, { updateOn: 'blur' });

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) {
      this.form.markAllAsTouched();
      return;
    }

    this.startLoading();

    const payload: LoginCommand = {
      email: this.form.value.email ?? '',
      password: this.form.value.password ?? '',
      fingerprint: null,
    };

    this.auth.login(payload).subscribe({
      next: () => {
        this.stopLoading();
        const target = this.currentUser.getDefaultRoute();
        this.router.navigate([target]);
      },
      error: (err) => {
        this.stopLoading(this.resolveLoginErrorMessage(err));
        console.error('Login error:', err);
      },
    });
  }

  private resolveLoginErrorMessage(error: unknown): string {
    const httpError = error as {
      status?: number;
      error?: { message?: string } | string;
    };

    if (httpError?.status === 401 || httpError?.status === 403) {
      return 'Incorrect email or password.';
    }

    if (typeof httpError?.error === 'object' && httpError.error?.message) {
      return httpError.error.message.includes('Pogrešni kredencijali')
        ? 'Incorrect email or password.'
        : httpError.error.message;
    }

    return 'Unable to sign in right now. Please try again.';
  }
}

