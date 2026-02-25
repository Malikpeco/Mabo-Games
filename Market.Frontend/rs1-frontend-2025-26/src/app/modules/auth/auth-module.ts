import {NgModule} from '@angular/core';

import {AuthRoutingModule} from './auth-routing-module';
import {AuthLayoutComponent} from './auth-layout/auth-layout.component';
import {LoginComponent} from './login/login.component';
import {RegisterComponent} from './register/register.component';
import {ForgotPasswordComponent} from './forgot-password/forgot-password.component';
import {LogoutComponent} from './logout/logout.component';
import {SharedModule} from '../shared/shared-module';
import { EnterEmailStepComponent } from './forgot-password/steps/enter-email-step/enter-email-step.component';
import { ChooseMethodStepComponent } from './forgot-password/steps/choose-method-step/choose-method-step.component';
import { EmailRecoveryStepComponent } from './forgot-password/steps/email-recovery-step/email-recovery-step.component';
import { ResetPasswordStepComponent } from './forgot-password/steps/reset-password-step/reset-password-step.component';
import { DoneStepComponent } from './forgot-password/steps/done-step/done-step.component';
import { SecurityQuestionRecoveryStepComponent } from './forgot-password/steps/security-question-recovery-step/security-question-recovery-step.component';
import { TransitionLoadingComponent } from '../shared/components/transition-loading/transition-loading.component';



@NgModule({
  declarations: [
    AuthLayoutComponent,
    LoginComponent,
    RegisterComponent,
    ForgotPasswordComponent,
    LogoutComponent,
    EnterEmailStepComponent,
    ChooseMethodStepComponent,
    EmailRecoveryStepComponent,
    ResetPasswordStepComponent,
    DoneStepComponent,
    SecurityQuestionRecoveryStepComponent,
  
  ],
  imports: [
    AuthRoutingModule,
    SharedModule
  ]
})
export class AuthModule { }
