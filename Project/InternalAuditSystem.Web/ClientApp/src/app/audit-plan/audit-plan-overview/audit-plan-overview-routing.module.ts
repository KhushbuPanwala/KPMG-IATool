import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuditPlanOverviewComponent } from './audit-plan-overview.component';
import { AuditPlanListComponent } from '../audit-plan-list/audit-plan-list.component';

const routes: Routes = [
  { path: '', component: AuditPlanListComponent },
  { path: 'overview', component: AuditPlanOverviewComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditPlanOverviewRoutingModule { }
