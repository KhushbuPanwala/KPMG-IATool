import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditPlanListComponent } from './audit-plan-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditPlanListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditPlanRoutingModule { }
