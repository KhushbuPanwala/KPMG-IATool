import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkProgramAddComponent } from './work-program-add/work-program-add.component';
import { WorkProgramListComponent } from './work-program-list/work-program-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'add', component: WorkProgramAddComponent },
  { path: 'list', component: WorkProgramListComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkProgramRoutingModule { }
