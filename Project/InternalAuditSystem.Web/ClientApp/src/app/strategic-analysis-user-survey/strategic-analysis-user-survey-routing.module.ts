import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StrategicAnalysisUserSurveyComponent } from './strategic-analysis-user-survey/strategic-analysis-user-survey.component';
import { StrategicAnalysisUserSurveySecondComponent } from './strategic-analysis-user-survey-second/strategic-analysis-user-survey-second.component';
import { StrategicAnalysisUserEmailAttachmentComponent } from './strategic-analysis-user-email-attachment/strategic-analysis-user-email-attachment.component';
import { StrategicAnalysisUserComponent } from '../strategic-analysis-user/strategic-analysis-user/strategic-analysis-user.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: '', component: StrategicAnalysisUserComponent },
  { path: 'general', component: StrategicAnalysisUserSurveyComponent },
  { path: 'response', component: StrategicAnalysisUserSurveySecondComponent },
  { path: 'attachment', component: StrategicAnalysisUserEmailAttachmentComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StrategicAnalysisSurveyUserRoutingModule { }
