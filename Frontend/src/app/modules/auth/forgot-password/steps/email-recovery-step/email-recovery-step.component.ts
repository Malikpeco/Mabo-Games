import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { UserApiService } from '../../../../../api-services/users/users-api.service';
import { VerifyResetCodeQuery } from '../../../../../api-services/users/users-api.model';

@Component({
  selector: 'app-email-recovery-step',
  standalone: false,
  templateUrl: './email-recovery-step.component.html',
  styleUrl: './email-recovery-step.component.scss',
  host: { style: 'width: 100%; height: 100%' }
})
export class EmailRecoveryStepComponent extends BaseComponent {
  private fb = inject(FormBuilder);
  private userApi = inject(UserApiService);

  @Input({ required: true }) userEmail!: string;

  @Output() codeVerified = new EventEmitter<string>();

  form = this.fb.group({
    recoveryCode: ['', [Validators.required, Validators.pattern('^[0-9]{6}$')]],
  });



  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload: VerifyResetCodeQuery = {
      resetCode: this.form.value.recoveryCode ?? '',
      userEmail: this.userEmail,
    };

    this.userApi.verifyResetCode(payload).subscribe({
      next: () => {

        this.stopLoading();
        this.codeVerified.emit(this.form.value.recoveryCode ?? '');
      },
      error: (err) => {

        this.stopLoading('Invalid recovery code. Please try again.');
        console.error('Verify reset code error:', err);
      },
    });
  }
}
