import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditableEntityTypeListComponent } from './auditable-entity-type-list/auditable-entity-type-list.component';


const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditableEntityTypeListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditableEntityTypeRoutingModule { }
