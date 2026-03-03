import {NgModule} from '@angular/core';

import {AdminRoutingModule} from './admin-routing-module';
import { AdminPanelComponent } from './admin-panel/admin-panel.component';
import { SharedModule } from '../shared/shared-module';


@NgModule({
  declarations: [
    AdminPanelComponent
  ],
  imports: [
    AdminRoutingModule,
    SharedModule
  ]
})
export class AdminModule { }
