import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StrategicAnalysisUserComponent } from './strategic-analysis-user/strategic-analysis-user.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: StrategicAnalysisUserComponent },
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class StrategicAnalysisUserRoutingModule { }
