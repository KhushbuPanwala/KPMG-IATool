import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { RatingListComponent } from './rating-list/rating-list.component';
import { RatingAddComponent } from './rating-add/rating-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: RatingListComponent },
  { path: 'add', component: RatingAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RatingRoutingModule { }
