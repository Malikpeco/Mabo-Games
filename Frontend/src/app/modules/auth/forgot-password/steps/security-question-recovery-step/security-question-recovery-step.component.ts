import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { BaseComponent } from '../../../../../core/components/base-classes/base-component';
import { FormBuilder, Validators } from '@angular/forms';
import { UserApiService } from '../../../../../api-services/users/users-api.service';
import { UserSecurityQuestionsApiService } from '../../../../../api-services/user-security-questions/user-security-questions-api.service';
import { GetUserSecurityQuestionsListByEmailQuery, GetUserSecurityQuestionsListByEmailQueryDto } from '../../../../../api-services/user-security-questions/user-security-questions-api.mode';
import { RequestPasswordResetBySecurityQuestionCommand } from '../../../../../api-services/users/users-api.model';

@Component({
  selector: 'app-security-question-recovery-step',
  standalone: false,
  templateUrl: './security-question-recovery-step.component.html',
  styleUrl: './security-question-recovery-step.component.scss',
  host: { style: 'width: 100%; height: 100%' }
})
export class SecurityQuestionRecoveryStepComponent extends BaseComponent {


  private fb = inject(FormBuilder);
  private userApi = inject(UserApiService);
  private userSecurityQuestionsApi = inject(UserSecurityQuestionsApiService);

  @Input({ required: true }) userEmail!: string;

  @Output() codeVerified = new EventEmitter<string>();

  userQuestions: GetUserSecurityQuestionsListByEmailQueryDto[] = [];





  ngOnInit(): void {
    
    const payload: GetUserSecurityQuestionsListByEmailQuery = {
      userEmail: this.userEmail
    }
  
    this.userSecurityQuestionsApi.getUserSecurityQuestionsListByEmail(payload).subscribe(res => {
      this.userQuestions = res;
    });
  }



  form = this.fb.group({
    selectedQuestionId: ['', [Validators.required]],
    securityQuestionAnswer: ['', [Validators.required, Validators.minLength(3)]],
  });


  onSubmit(): void {
    if (this.form.invalid || this.isLoading) return;

    this.startLoading();

    const payload: RequestPasswordResetBySecurityQuestionCommand = {
      userSecurityQuestionId: Number(this.form.get('selectedQuestionId')!.value),
      securityQuestionAnswer: String(this.form.get('securityQuestionAnswer')!.value)
    };

    this.userApi.requestPasswordResetBySecurityQuestion(payload).subscribe({
      next: (response) => {
        this.codeVerified.emit(String(response.resetCode));
        this.stopLoading();
      },
      error: (error) => {
        console.error('Incorrect answer, please try again', error);
        this.stopLoading('Incorrect answer, please try again');
      }
    });

  }



}
