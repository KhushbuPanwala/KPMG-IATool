import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuditAdvisoryComponent } from './audit-advisory.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditAdvisoryComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditAdvisoryRoutingModule { }
