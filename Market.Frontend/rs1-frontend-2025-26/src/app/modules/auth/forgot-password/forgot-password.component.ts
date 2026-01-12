import { Component, inject } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BaseComponent } from '../../../core/components/base-classes/base-component';
import { CheckRecoveryOptionsQuery, CheckRecoveryOptionsResultDto, RequestPasswordResetByEmailCommand, PasswordResetCommand, VerifyResetCodeQuery } from '../../../api-services/user/user-api.model';
import { UserApiService } from '../../../api-services/user/user-api.service';
import { passwordMatchValidator } from '../../../core/validators/password-match.validator';


@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent extends BaseComponent {

  private fb = inject(FormBuilder);
  private router = inject(Router);
  private userApi = inject(UserApiService);

  forgotPasswordStep = ForgotPasswordStep;
  currentStep: ForgotPasswordStep = ForgotPasswordStep.EnterEmail;

  checkRecoveryOptionsResult: CheckRecoveryOptionsResultDto | null = null;

  choseSecurityQuestionRecovery: boolean = false;

  userEmail: string = '';

  userCode: string = '';

  form = this.fb.group({
    email: ['sec@market.com', [Validators.required, Validators.email]],
    recoveryCode: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(6), Validators.maxLength(6),]],
    password: [{ value: '', disabled: true }, [Validators.required, Validators.minLength(8), Validators.pattern("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")]],
    confirmPassword: [{ value: '', disabled: true }, [Validators.required]],
    updateOn: 'blur'
  },
    {
      validators: passwordMatchValidator()
    }

  );




  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    switch (this.currentStep) {


      //Initial step where user enters his email to start the recovery process
      case ForgotPasswordStep.EnterEmail:
        this.startLoading();
        const recoveryPayload: CheckRecoveryOptionsQuery = {
          recoveryEmail: this.form.value.email ?? ''
        };

        this.userApi.checkRecoveryOptions(recoveryPayload).subscribe({
          next: (result) => {
            this.checkRecoveryOptionsResult = result;
            this.userEmail = this.form.value.email ?? '';
            this.form.get('email')?.disable();
            this.currentStep = ForgotPasswordStep.ChooseMethod;
            this.stopLoading();
          },
          error: (err) => {
            this.stopLoading('Failed to retrieve recovery options. Please try again.');
            console.error('Check recovery options error:', err);
          }
        })
        break;

      //Choosing between recovery via security questions or email (if user doesnt have security questions he just can do email recovery)

      case ForgotPasswordStep.ChooseMethod:
        this.startLoading();

        if (this.choseSecurityQuestionRecovery)
          this.currentStep = ForgotPasswordStep.SecurityQuestion;
        else {
          const emailResetPayload: RequestPasswordResetByEmailCommand = {
            userEmail: this.userEmail
          };

          this.userApi.requestPasswordResetByEmail(emailResetPayload).subscribe({
            next: () => {
              this.form.get('recoveryCode')?.enable();
              this.currentStep = ForgotPasswordStep.EmailCode;
              this.stopLoading();
            },
            error: (err) => {
              this.stopLoading('Failed to send recovery code. Please try again.');
              console.error('Request password reset by email error:', err);
            }
          });
        }
        break;

      //Reset process via email (sending a recovery code to the user's email)

      case ForgotPasswordStep.EmailCode:
        this.startLoading();

        const verifyCodePayload: VerifyResetCodeQuery = {
          resetCode: this.form.value.recoveryCode ?? ''
          , userEmail: this.userEmail ?? ''
        };

        this.userApi.verifyResetCode(verifyCodePayload).subscribe({
          next: () => {
            this.userCode = this.form.value.recoveryCode ?? '';
            this.form.get('recoveryCode')?.disable();
            this.form.get('password')?.enable();
            this.form.get('confirmPassword')?.enable();
            this.currentStep = ForgotPasswordStep.ResetPassword;
            this.stopLoading();
          },
          error: (err) => {
            this.stopLoading('Invalid recovery code. Please try again.');
            console.error('Verify reset code error:', err);
          }
        });
        break;

      //Reset process via security questions (user answers his security questions and then silently gets his recovery code)

      case ForgotPasswordStep.SecurityQuestion:
        // Logic to verify security question
        this.currentStep = ForgotPasswordStep.ResetPassword;
        break;


      case ForgotPasswordStep.ResetPassword:
        this.startLoading();

        const resetPasswordPayload: PasswordResetCommand = {
          recoveryCode: this.userCode ?? '',
          newPassword: this.form.value.password ?? '',
          confirmNewPassword: this.form.value.confirmPassword ?? ''
        };

        this.userApi.resetPassword(resetPasswordPayload).subscribe({
          next: () => {
            this.form.get('password')?.disable();
            this.form.get('confirmPassword')?.disable();
            this.stopLoading();
            this.currentStep = ForgotPasswordStep.Done;
          },
          error: (err) => {
            this.stopLoading('Failed to reset password. Please try again.');
            console.error('Reset password error:', err);
          }
        });
        break;


      default:
        this.router.navigate(['/auth/login']);
        break;
    }

  }


  startEmailRecovery(): void {
    this.choseSecurityQuestionRecovery = false;
    this.onSubmit();
  }

  startSecurityQuestionRecovery(): void {
    this.choseSecurityQuestionRecovery = true;
    this.onSubmit();
  }

}



export enum ForgotPasswordStep {
  EnterEmail,
  ChooseMethod,
  SecurityQuestion,
  EmailCode,
  ResetPassword,
  Done
}

