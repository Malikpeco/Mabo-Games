import { LOCALE_ID, NgModule, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideAnimations} from '@angular/platform-browser/animations';
import {HttpClient, provideHttpClient, withInterceptors} from '@angular/common/http';
import { registerLocaleData } from '@angular/common';
import localeEnGb from '@angular/common/locales/en-GB';

import { AppRoutingModule } from './app-routing-module';
import { AppComponent } from './app.component';
import {authInterceptor} from './core/interceptors/auth-interceptor.service';
import {loadingBarInterceptor} from './core/interceptors/loading-bar-interceptor.service';
import {errorLoggingInterceptor} from './core/interceptors/error-logging-interceptor.service';
import {TranslateLoader, TranslateModule} from '@ngx-translate/core';
import {CustomTranslateLoader} from './core/services/custom-translate-loader';
import {materialModules} from './modules/shared/material-modules';
import {SharedModule} from './modules/shared/shared-module';
import { APP_LOCALE } from './core/constants/locale';

// Registers the British (en-GB) locale data so Angular's date/currency/number
// pipes can actually format using it (Angular only ships en-US data by default).
registerLocaleData(localeEnGb);

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (http: HttpClient) => new CustomTranslateLoader(http),
        deps: [HttpClient]
      }
    }),
    SharedModule,
    materialModules,
  ],
  providers: [
    provideAnimations(),
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection(),
    { provide: LOCALE_ID, useValue: APP_LOCALE },
    provideHttpClient(
      withInterceptors([
        loadingBarInterceptor,
        authInterceptor,
        errorLoggingInterceptor
      ])
    )
  ],
  exports: [
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
