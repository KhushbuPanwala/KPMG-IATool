import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditGeneralComponent } from './audit-plan-general.component';
import { AuditPlanListComponent } from '../audit-plan-list/audit-plan-list.component';

const routes: Routes = [
  { path: '', component: AuditPlanListComponent },
  { path: 'general', component: AuditGeneralComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditGeneralRoutingModule { }
