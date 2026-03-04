import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';

import 'zone.js'; // Razvoj softvera 1 setup, prethodno instalirati "npm install zone.js"

platformBrowser().bootstrapModule(AppModule, {

})
  .catch(err => console.error(err));
