import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { RcmSubProcessListComponent } from './rcm-sub-process-list/rcm-sub-process-list.component';
import { RcmSubProcessAddComponent } from './rcm-sub-process-add/rcm-sub-process-add.component';
import { HttpClientModule } from '@angular/common/http';
import { RcmSubProcessService } from '../../../swaggerapi/AngularFiles';
import { RcmSubProcessRoutingModule } from './rcm-sub-process-routing.module';
import { SharedModule } from '../../../shared/shared.module';

@NgModule({
  declarations: [
    RcmSubProcessListComponent,
    RcmSubProcessAddComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    RcmSubProcessRoutingModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [ RcmSubProcessService ]
})
export class RcmSubProcessModule { }
