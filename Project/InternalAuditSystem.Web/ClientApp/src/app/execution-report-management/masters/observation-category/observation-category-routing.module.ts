import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ObservationCategoryComponent } from './observation-category.component';
import { ObservationCategoryAddComponent } from './observation-category-add/observation-category-add.component';
const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: ObservationCategoryComponent },
  { path: 'add', component: ObservationCategoryAddComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ObservationCategoryRoutingModule { }
