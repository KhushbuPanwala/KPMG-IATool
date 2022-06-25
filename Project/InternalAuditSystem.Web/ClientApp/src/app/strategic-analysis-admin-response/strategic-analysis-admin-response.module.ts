import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { StrategicAnalysisAdminResponseComponent } from './strategic-analysis-admin-response/strategic-analysis-admin-response.component';
import { StrategicAnalysisAdminRoutingModule} from '../strategic-analysis-admin-response/strategic-analysis-admin-response-routing.module';
import { SharedModule } from '../shared/shared.module';
import { StrategicAnalysisAdminResponseModalComponent } from './strategic-analysis-admin-response-modal/strategic-analysis-admin-response-modal.component';

@NgModule({
  declarations: [StrategicAnalysisAdminResponseComponent, StrategicAnalysisAdminResponseModalComponent],
  imports: [
    CommonModule,
    StrategicAnalysisAdminRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule
  ]
})
export class StrategicAnalysisAdminResponseModule { }
