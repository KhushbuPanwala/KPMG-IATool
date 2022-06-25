import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';
import { SamplingListComponent } from './sampling-list/sampling-list.component';
import { StrategicAnalysisAdminAddComponent } from '../strategic-analysis-admin/strategic-analysis-admin-add/strategic-analysis-admin-add.component';
import { StrategicAnalysisAdminSurveyComponent } from '../strategic-analysis-admin-survey/strategic-analysis-admin-survey/strategic-analysis-admin-survey.component';
import { StrategicAnalysisDragDropComponent } from '../strategic-analysis-admin-survey/strategic-analysis-drag-drop/strategic-analysis-drag-drop.component';
import { SamplingAdminResponseListComponent } from './sampling-response-admin-side/sampling-admin-response-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: SamplingListComponent },
  { path: 'add', component: StrategicAnalysisAdminAddComponent },
  { path: 'create-survey', component: StrategicAnalysisAdminSurveyComponent },
  { path: 'drag', component: StrategicAnalysisDragDropComponent },
  { path: 'response', component: SamplingAdminResponseListComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SamplingRoutingModule { }
