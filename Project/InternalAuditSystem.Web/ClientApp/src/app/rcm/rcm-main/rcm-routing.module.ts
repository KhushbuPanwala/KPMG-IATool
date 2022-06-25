import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RcmListComponent } from './rcm-list/rcm-list.component';
import { RcmAddComponent } from './rcm-add/rcm-add.component';


const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: RcmListComponent },
  { path: 'add', component: RcmAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class RcmRoutingModule { }
