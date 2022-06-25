import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { SharedModule } from '../../shared/shared.module';
import { RiskControlMatrixesService } from '../../swaggerapi/AngularFiles';
import { RcmRoutingModule } from './rcm-routing.module';
import { RcmListComponent } from './rcm-list/rcm-list.component';
import { RcmAddComponent } from './rcm-add/rcm-add.component';
import { AngularEditorModule } from '@kolkov/angular-editor';

@NgModule({
  declarations: [
    RcmListComponent,
    RcmAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    RcmRoutingModule,
    NgxPaginationModule,
    AngularEditorModule,
    SharedModule
  ],
  providers: [RiskControlMatrixesService]

})
export class RcmModule { }
