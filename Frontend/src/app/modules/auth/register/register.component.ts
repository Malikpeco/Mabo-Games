import { Component, inject, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { passwordMatchValidator } from '../../../core/validators/password-match.validator';
import { RegisterUserCommand } from '../../../api-services/users/users-api.model';
import { UserApiService } from '../../../api-services/users/users-api.service';
import { RecaptchaComponent } from '../../shared/components/recaptcha/recaptcha.component';



@Component({
  selector: 'app-register',
  standalone: false,
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent extends BaseComponent {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private userApi = inject(UserApiService);

  @ViewChild(RecaptchaComponent) recaptcha!: RecaptchaComponent;
  recaptchaToken: string | null = null;

  onCaptchaResolved(token: string | null): void {
    this.recaptchaToken = token;
  }

  form = this.fb.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
    confirmPassword: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    phone: [null, [Validators.pattern("^\\+?(\\d{1,3})?[-.\\s]?(\\(?\\d{3}\\)?[-.\\s]?)?(\\d[-.\\s]?){6,9}\\d$")]],
    updateOn: 'blur'
  },
    {
      validators: passwordMatchValidator()
    }

  );



  onSubmit(): void {
  if (this.form.invalid || this.isLoading) return;

  if (!this.recaptchaToken) {
    this.stopLoading('Please complete the captcha challenge.');
    return;
  }

  this.startLoading();

  const payload: RegisterUserCommand = {
    firstName: 'testFirstName',
    lastName: 'testLastName',
    username: this.form.value.username ?? '',
    email: this.form.value.email ?? '',
    password: this.form.value.password ?? '',
    confirmPassword: this.form.value.confirmPassword ?? '',
    phoneNumber: this.form.value.phone || null,
    recaptchaToken: this.recaptchaToken,
  };

  this.userApi.register(payload).subscribe({
    next: () => {
      this.stopLoading();
      this.router.navigate(['/auth/login']);
    },
    error: (err) => {
      this.stopLoading('Registration failed. Please try again.');
      console.error('Registration error:', err);
      this.recaptchaToken = null;
      this.recaptcha?.reset();
    },
  });
}

}

