import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SectorAddComponent } from './sector-add/sector-add.component';
import { SectorListComponent } from './sector-list/sector-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: SectorListComponent },
  { path: 'add', component: SectorAddComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class SectorRoutingModule { }
