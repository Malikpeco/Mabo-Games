import { platformBrowser } from '@angular/platform-browser';
import { AppModule } from './app/app-module';

import 'zone.js'; // Software Development 1 setup, install "npm install zone.js" beforehand

platformBrowser().bootstrapModule(AppModule, {

})
  .catch(err => console.error(err));
