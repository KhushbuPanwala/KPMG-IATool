import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RelationshipTypeListComponent } from './relationship-type-list/relationship-type-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: RelationshipTypeListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RelationshipTypeRoutingModule { }
