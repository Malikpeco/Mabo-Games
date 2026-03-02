import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {FitPaginatorBarComponent} from './components/fit-paginator-bar/fit-paginator-bar.component';
import {materialModules} from './material-modules';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TranslatePipe} from '@ngx-translate/core';
import { FitConfirmDialogComponent } from './components/fit-confirm-dialog/fit-confirm-dialog.component';
import {DialogHelperService} from './services/dialog-helper.service';
import { FitLoadingBarComponent } from './components/fit-loading-bar/fit-loading-bar.component';
import { FitTableSkeletonComponent } from './components/fit-table-skeleton/fit-table-skeleton.component';
import { PasswordStrenghtMeterComponent } from './components/password-strenght-meter/password-strenght-meter/password-strenght-meter.component';
import { TransitionLoadingComponent } from './components/transition-loading/transition-loading.component';
import { MaboNavbarComponent } from './components/mabo-navbar/mabo-navbar.component';
import { MaboFooterComponent } from './components/mabo-footer/mabo-footer.component';



@NgModule({
  declarations: [
    FitPaginatorBarComponent,
    FitConfirmDialogComponent,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    PasswordStrenghtMeterComponent,
    TransitionLoadingComponent,
    MaboNavbarComponent,
    MaboFooterComponent
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
    FitPaginatorBarComponent,
    CommonModule,
    ReactiveFormsModule,
    TranslatePipe,
    FormsModule,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    materialModules,
    PasswordStrenghtMeterComponent,
    TransitionLoadingComponent,
    MaboNavbarComponent,
    MaboFooterComponent
  ]
})
export class SharedModule { }
