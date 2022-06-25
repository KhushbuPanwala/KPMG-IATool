import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StrategicAnalysisAdminSurveyComponent } from './strategic-analysis-admin-survey/strategic-analysis-admin-survey.component';
import { StrategicAnalysisDragDropComponent } from './strategic-analysis-drag-drop/strategic-analysis-drag-drop.component';
import { StrategicAnalysisAdminListComponent } from '../strategic-analysis-admin/strategic-analysis-admin-list/strategic-analysis-admin-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: StrategicAnalysisAdminListComponent },
  { path: 'create', component: StrategicAnalysisAdminSurveyComponent },
  { path: 'drag', component: StrategicAnalysisDragDropComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StrategicAnalysisAdminRoutingModule { }
