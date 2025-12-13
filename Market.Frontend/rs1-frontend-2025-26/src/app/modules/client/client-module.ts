import {NgModule} from '@angular/core';

import {ClientRoutingModule} from './client-routing-module';
import {SharedModule} from '../shared/shared-module';


@NgModule({
  declarations: [],
  imports: [
    SharedModule,
    ClientRoutingModule
  ]
})
export class ClientModule { }
