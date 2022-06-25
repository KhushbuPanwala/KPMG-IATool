import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { SharedModule } from '../shared/shared.module';
import { StrategicAnalysisUserComponent } from './strategic-analysis-user/strategic-analysis-user.component';
import { StrategicAnalysisUserRoutingModule } from './strategic-analysis-user-routing.module';


@NgModule({
  declarations: [StrategicAnalysisUserComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    StrategicAnalysisUserRoutingModule
  ]
})
export class StrategicAnalysisUserModule { }
