import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RelationshipComponent } from './relationship.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'relationship', component: RelationshipComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelationshipRoutingModule { }
