import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuditCategoryListComponent } from './audit-category-list/audit-category-list.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AuditCategoryListComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditCategoryRoutingModule { }
