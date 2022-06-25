import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RcmSubProcessListComponent } from './rcm-sub-process-list/rcm-sub-process-list.component';
import { RcmSubProcessAddComponent } from './rcm-sub-process-add/rcm-sub-process-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: RcmSubProcessListComponent },
  { path: 'add', component: RcmSubProcessAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RcmSubProcessRoutingModule { }
