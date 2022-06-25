import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { SharedModule } from '../shared/shared.module';
import { StrategicAnalysisAdminSurveyComponent } from './strategic-analysis-admin-survey/strategic-analysis-admin-survey.component';
import { StrategicAnalysisAdminRoutingModule } from './strategic-analysis-admin-routing.module';
import { StrategicAnalysisDragDropComponent } from './strategic-analysis-drag-drop/strategic-analysis-drag-drop.component';
import { DragDropModule } from '@angular/cdk/drag-drop';



@NgModule({
  declarations: [StrategicAnalysisAdminSurveyComponent, StrategicAnalysisDragDropComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    StrategicAnalysisAdminRoutingModule,
    DragDropModule
  ],
  exports: [StrategicAnalysisAdminSurveyComponent, StrategicAnalysisDragDropComponent ]
})
export class StrategicAnalysisAdminSurveyModule { }
