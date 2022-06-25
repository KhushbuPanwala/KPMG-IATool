import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StrategicAnalysisAdminListComponent } from './strategic-analysis-admin-list/strategic-analysis-admin-list.component';
import { StrategicAnalysisAdminAddComponent } from './strategic-analysis-admin-add/strategic-analysis-admin-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: StrategicAnalysisAdminListComponent },
  { path: 'add', component: StrategicAnalysisAdminAddComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StrategicAnalysisAdminRoutingModule { }
