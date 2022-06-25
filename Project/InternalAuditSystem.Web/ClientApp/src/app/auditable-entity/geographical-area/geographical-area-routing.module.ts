import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { GeographicalAreaComponent } from './geographical-area.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'geographical-area', component: GeographicalAreaComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GeographicalAreaRoutingModule { }
