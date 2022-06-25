import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditableEntityCategoryListComponent } from './auditable-entity-category-list/auditable-entity-category-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditableEntityCategoryListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class AuditableEntityCategoryRoutingModule { }
