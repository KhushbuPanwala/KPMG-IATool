import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditSubProcessListComponent } from './audit-sub-process-list/audit-sub-process-list.component';



const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditSubProcessListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditSubProcessRoutingModule { }
