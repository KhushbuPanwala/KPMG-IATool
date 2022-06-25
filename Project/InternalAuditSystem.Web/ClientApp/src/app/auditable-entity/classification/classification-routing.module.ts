import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ClassificationComponent } from './classification.component';
import { AuditableEntityListComponent } from '../auditable-entity-list/auditable-entity-list/auditable-entity-list.component';

const routes: Routes = [
  { path: '', component: AuditableEntityListComponent },
  { path: 'classification', component: ClassificationComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClassificationRoutingModule { }
