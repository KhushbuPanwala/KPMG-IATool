import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DistributionAddComponent } from './distribution-add/distribution-add.component';
import { DistributionListComponent } from './distribution-list/distribution-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: DistributionListComponent },
  { path: 'add', component: DistributionAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DistributionRoutingModule { }
