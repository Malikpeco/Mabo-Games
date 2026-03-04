import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {materialModules} from './material-modules';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TranslatePipe} from '@ngx-translate/core';
import { FitConfirmDialogComponent } from './components/fit-confirm-dialog/fit-confirm-dialog.component';
import {DialogHelperService} from './services/dialog-helper.service';
import { FitLoadingBarComponent } from './components/fit-loading-bar/fit-loading-bar.component';
import { PasswordStrenghtMeterComponent } from './components/password-strenght-meter/password-strenght-meter/password-strenght-meter.component';
import { TransitionLoadingComponent } from './components/transition-loading/transition-loading.component';
import { MaboNavbarComponent } from './components/mabo-navbar/mabo-navbar.component';
import { MaboFooterComponent } from './components/mabo-footer/mabo-footer.component';
import { GameCardComponent } from './components/game-card/game-card.component';



@NgModule({
  declarations: [
    FitConfirmDialogComponent,
    FitLoadingBarComponent,
    PasswordStrenghtMeterComponent,
    TransitionLoadingComponent,
    MaboNavbarComponent,
    MaboFooterComponent,
    GameCardComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    TranslatePipe,
    ...materialModules
  ],
  providers: [
    DialogHelperService
  ],
  exports:[
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    TranslatePipe,
    FormsModule,
    FitLoadingBarComponent,
    materialModules,
    PasswordStrenghtMeterComponent,
    TransitionLoadingComponent,
    MaboNavbarComponent,
    MaboFooterComponent,
    GameCardComponent
  ]
})
export class SharedModule { }
