import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManagementListComponent } from './management-list/management-list.component';
import { ObservationComponent } from './observation-management.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: ManagementListComponent },
  { path: 'add', component: ObservationComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ObservationManagementRoutingModule { }
