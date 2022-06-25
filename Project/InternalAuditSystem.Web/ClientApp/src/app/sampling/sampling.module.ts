import { NgModule } from '@angular/core';
import { SamplingListComponent } from './sampling-list/sampling-list.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { NgSelectModule } from '@ng-select/ng-select';
import { SamplingRoutingModule } from './sampling-routing.module';
import { StrategicAnalysisAdminModule } from '../strategic-analysis-admin/strategic-analysis-admin.module';
import { StrategicAnalysisAdminSurveyModule } from '../strategic-analysis-admin-survey/strategic-analysis-admin-survey.module';
import { SamplingAdminResponseListComponent } from './sampling-response-admin-side/sampling-admin-response-list.component';

@NgModule({
  declarations: [SamplingListComponent, SamplingAdminResponseListComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    HttpClientModule,
    SharedModule,
    SamplingRoutingModule,
    StrategicAnalysisAdminModule,
    StrategicAnalysisAdminSurveyModule
  ]
})
export class SamplingModule { }
