import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditProcessListComponent } from './audit-process-list/audit-process-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditProcessListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditProcessRoutingModule { }
