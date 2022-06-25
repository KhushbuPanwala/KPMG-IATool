import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProcessListComponent } from './process-list/process-list.component';
import { ProcessAddComponent } from './process-add/process-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: ProcessListComponent },
  { path: 'add', component: ProcessAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class ProcessRoutingModule { }
