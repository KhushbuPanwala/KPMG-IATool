import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegionListComponent } from './region-list/region-list.component';
import { RegionAddComponent } from './region-add/region-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: RegionListComponent },
  { path: 'add', component: RegionAddComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RegionRoutingModule { }
