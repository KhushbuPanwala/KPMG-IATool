import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditableEntityListComponent } from './auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditableEntityListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditableEntityListTypeRoutingModule { }
