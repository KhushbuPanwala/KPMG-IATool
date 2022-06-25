import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { StateListComponent } from './state-list/state-list.component';
import {  StateAddComponent} from './state-add/state-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: StateListComponent },
  { path: 'add', component: StateAddComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StateRoutingModule { }
