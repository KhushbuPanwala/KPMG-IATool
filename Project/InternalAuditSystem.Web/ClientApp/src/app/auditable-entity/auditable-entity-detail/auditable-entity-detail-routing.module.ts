import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuditableEntityDetailComponent } from './auditable-entity-detail.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'details', component: AuditableEntityDetailComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditableEntityDetailRoutingModule { }
