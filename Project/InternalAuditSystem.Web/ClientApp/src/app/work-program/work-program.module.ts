import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { WorkProgramRoutingModule } from './work-program-routing.module';
import { WorkProgramAddComponent } from './work-program-add/work-program-add.component';
import { WorkProgramAuditComponent } from './work-program-add/work-program-audit/work-program-audit.component';
import { WorkProgramListComponent } from './work-program-list/work-program-list.component';
import { WorkProgramsService } from '../swaggerapi/AngularFiles';
import { SharedModule } from '../shared/shared.module';
import { WorkProgramRcmComponent } from './work-program-add/work-program-rcm/work-program-rcm.component';
import { WorkProgramRcmEditComponent } from './work-program-add/work-program-rcm-edit/work-program-rcm-edit.component';
import { WorkProgramSamplingMethodologyComponent } from './work-program-add/work-program-sampling-methodology/work-program-sampling-methodology.component';
import { WorkProgramSurveyComponent } from './work-program-add/work-program-survey/work-program-survey.component';
import { RiskControlMatrixesService } from '../swaggerapi/AngularFiles/api/riskControlMatrixes.service';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { MomSharedModule } from '../shared/Mom/Mom-shared.module';

@NgModule({
  declarations: [WorkProgramAddComponent, WorkProgramAuditComponent,
    WorkProgramListComponent, WorkProgramRcmComponent, WorkProgramRcmEditComponent,
    WorkProgramSamplingMethodologyComponent,
    WorkProgramSurveyComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    WorkProgramRoutingModule,
    SharedModule,
    AngularEditorModule,
    MomSharedModule
  ],
  providers: [WorkProgramsService, RiskControlMatrixesService]
})
export class WorkProgramModule { }
