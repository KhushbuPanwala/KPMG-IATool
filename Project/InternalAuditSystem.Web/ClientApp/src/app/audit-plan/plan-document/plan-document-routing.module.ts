import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { PlanDocumentComponent } from './plan-document.component';
import { AuditPlanListComponent } from '../audit-plan-list/audit-plan-list.component';

const routes: Routes = [
  { path: '', component: AuditPlanListComponent },
  { path: 'documents', component: PlanDocumentComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PlanDocumentRoutingModule { }
