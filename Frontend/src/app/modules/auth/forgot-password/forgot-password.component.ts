import { Component } from '@angular/core';
import { CheckRecoveryOptionsResultDto } from '../../../api-services/users/users-api.model';

@Component({
  selector: 'app-forgot-password',
  standalone: false,
  templateUrl: './forgot-password.component.html',
  styleUrl: './forgot-password.component.scss',
})
export class ForgotPasswordComponent {

  forgotPasswordStep = ForgotPasswordStep;
  currentStep: ForgotPasswordStep = ForgotPasswordStep.EnterEmail;

  // Shared state passed between steps
  userEmail: string = '';
  recoveryCode: string = '';
  recoveryOptions: CheckRecoveryOptionsResultDto | null = null;

  // Step 1 → Step 2: Enter Email completed
  onEmailSubmitted(data: { email: string; recoveryOptions: CheckRecoveryOptionsResultDto }): void {
    this.userEmail = data.email;
    this.recoveryOptions = data.recoveryOptions;
    this.currentStep = ForgotPasswordStep.ChooseMethod;
  }

  // Step 2 → Step 3a or 3b: Method chosen
  onMethodChosen(method: 'email' | 'security-question'): void {
    if (method === 'security-question') {
      this.currentStep = ForgotPasswordStep.SecurityQuestion;
    } else {
      this.currentStep = ForgotPasswordStep.EmailCode;
    }
  }

  // Step 3a → Step 4: Email code verified
  onCodeVerified(code: string): void {
    this.recoveryCode = code;
    this.currentStep = ForgotPasswordStep.ResetPassword;
  }

  // Step 3b → Step 4: Security question answered (code returned from backend)
  onSecurityQuestionAnswered(code: string): void {
    this.recoveryCode = code;
    this.currentStep = ForgotPasswordStep.ResetPassword;
  }

  // Step 4 → Step 5: Password reset completed
  onPasswordReset(): void {
    this.currentStep = ForgotPasswordStep.Done;
  }
}

export enum ForgotPasswordStep {
  EnterEmail,
  ChooseMethod,
  SecurityQuestion,
  EmailCode,
  ResetPassword,
  Done,
}