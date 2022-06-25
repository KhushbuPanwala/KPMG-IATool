import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditTypeListComponent } from './audit-type-list/audit-type-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditTypeListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditTypeRoutingModule { }
