import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { MomListComponent } from './mom-list/mom-list.component';
import { MomAddComponent } from './mom-add/mom-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: MomListComponent },
  { path: 'add', component: MomAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MomRoutingModule { }
