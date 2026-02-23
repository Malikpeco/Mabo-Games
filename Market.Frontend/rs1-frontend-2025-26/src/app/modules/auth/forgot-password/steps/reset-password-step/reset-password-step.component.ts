import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { UserApiService } from '../../../../../api-services/user/user-api.service';
import { PasswordResetCommand } from '../../../../../api-services/user/user-api.model';
import { passwordMatchValidator } from '../../../../../core/validators/password-match.validator';
import { PasswordStrenghtMeterComponent } from '../../../../shared/components/password-strenght-meter/password-strenght-meter/password-strenght-meter.component';
@Component({
  selector: 'app-reset-password-step',
  standalone: false,
  templateUrl: './reset-password-step.component.html',
  styleUrl: './reset-password-step.component.scss',
  host: { style: 'width: 100%; height: 100%' },
})
export class ResetPasswordStepComponent extends BaseComponent {

  private fb = inject(FormBuilder);
  private userApi = inject(UserApiService);

  @Input({ required: true }) recoveryCode!: string;

  @Output() passwordReset = new EventEmitter<void>();

  form = this.fb.group(
    {
      password: ['', [
        Validators.required,
        Validators.minLength(8),
        Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$'),
      ]],
      confirmPassword: ['', [Validators.required]],
    },
    {
      validators: passwordMatchValidator(),
    }
  );

  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload: PasswordResetCommand = {
      recoveryCode: this.recoveryCode,
      newPassword: this.form.value.password ?? '',
      confirmNewPassword: this.form.value.confirmPassword ?? '',
    };

    this.userApi.resetPassword(payload).subscribe({
      next: () => {
        this.stopLoading();
        this.passwordReset.emit();
      },
      error: (err) => {
        this.stopLoading('Failed to reset password. Please try again.');
        console.error('Reset password error:', err);
      },
    });
  }
}