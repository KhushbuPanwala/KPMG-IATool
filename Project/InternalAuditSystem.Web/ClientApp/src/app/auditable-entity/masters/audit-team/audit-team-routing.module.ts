import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditTeamListComponent } from './audit-team-list/audit-team-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditTeamListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditTeamRoutingModule { }
