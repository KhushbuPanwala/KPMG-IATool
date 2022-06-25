import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StrategicAnalysisAdminResponseComponent } from './strategic-analysis-admin-response/strategic-analysis-admin-response.component';
import { StrategicAnalysisAdminResponseModalComponent } from './strategic-analysis-admin-response-modal/strategic-analysis-admin-response-modal.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: StrategicAnalysisAdminResponseComponent },
  { path: 'response', component: StrategicAnalysisAdminResponseModalComponent},
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StrategicAnalysisAdminRoutingModule { }
