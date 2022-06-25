import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CountryListComponent } from './country-list/country-list.component';
import {  CountryAddComponent} from './country-add/country-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: CountryListComponent },
  { path: 'add', component: CountryAddComponent },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CountryRoutingModule { }
