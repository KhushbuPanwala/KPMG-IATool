import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { EntityDocumentsComponent } from './entity-documents.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'documents', component: EntityDocumentsComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EntityDocumentsRoutingModule { }
