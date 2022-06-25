import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RiskAssesmentComponent } from './risk-assesment.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'risk-assesment', component: RiskAssesmentComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RiskAssesmentRoutingModule { }
