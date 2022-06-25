import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PlanAuditProcessComponent } from './plan-audit-process.component';
import { AuditPlanListComponent } from '../audit-plan-list/audit-plan-list.component';

const routes: Routes = [
  { path: '', component: AuditPlanListComponent },
  { path: 'plan-process', component: PlanAuditProcessComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlanAuditProcessRoutingModule { }
