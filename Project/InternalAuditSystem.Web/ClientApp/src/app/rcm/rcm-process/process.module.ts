import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { ProcessRoutingModule } from './process-routing.module';
import { ProcessListComponent } from './process-list/process-list.component';
import { ProcessAddComponent } from './process-add/process-add.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from '../../shared/shared.module';
import { RcmProcessService } from '../../swaggerapi/AngularFiles';

@NgModule({
  declarations: [
    ProcessListComponent,
    ProcessAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    ProcessRoutingModule,
    NgxPaginationModule,
    SharedModule
  ],
  providers: [RcmProcessService]

})
export class ProcessModule { }
