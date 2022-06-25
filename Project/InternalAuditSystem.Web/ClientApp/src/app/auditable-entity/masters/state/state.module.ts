import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { StateListComponent } from './state-list/state-list.component';
import { StateAddComponent } from './state-add/state-add.component';
import { StateRoutingModule } from './state-routing.module';
import { StatesService } from '../../../swaggerapi/AngularFiles';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [StateListComponent, StateAddComponent],
  imports: [
    CommonModule,
    StateRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [StatesService]
})
export class StateModule { }
