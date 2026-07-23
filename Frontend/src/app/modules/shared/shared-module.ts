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
import { MaboNotificationsDropdownComponent } from './components/mabo-notifications-dropdown/mabo-notifications-dropdown.component';
import { MaboNotificationPopupComponent } from './components/mabo-notification-popup/mabo-notification-popup.component';
import { MaboFooterComponent } from './components/mabo-footer/mabo-footer.component';
import { GameCardComponent } from './components/game-card/game-card.component';
import { LibraryGameCardComponent } from './components/library-game-card/library-game-card.component';
import { GenreFilterComponent } from './components/genre-filter/genre-filter.component';
import { GameListComponent } from './components/game-list/game-list.component';
import { SharedPaginatorComponent } from './components/shared-paginator/shared-paginator.component';
import { CountryDropdownComponent } from '../admin/games/admin-shared/country-dropdown/country-dropdown.component';



@NgModule({
  declarations: [
    FitConfirmDialogComponent,
    FitLoadingBarComponent,
    PasswordStrenghtMeterComponent,
    TransitionLoadingComponent,
    MaboNavbarComponent,
    MaboNotificationsDropdownComponent,
    MaboNotificationPopupComponent,
    MaboFooterComponent,
    GameCardComponent,
    LibraryGameCardComponent,
    GenreFilterComponent,
    GameListComponent,
    SharedPaginatorComponent,
    CountryDropdownComponent
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
    MaboNotificationsDropdownComponent,
    MaboNotificationPopupComponent,
    MaboFooterComponent,
    GameCardComponent,
    LibraryGameCardComponent,
    GenreFilterComponent,
    GameListComponent,
    SharedPaginatorComponent,
    CountryDropdownComponent
  ]
})
export class SharedModule { }
