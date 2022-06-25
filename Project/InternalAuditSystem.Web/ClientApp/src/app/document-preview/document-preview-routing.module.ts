import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DocumentPreviewComponent } from './document-preview.component';

const routes: Routes = [
  { path: 'view', component: DocumentPreviewComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DocumentPreviewRoutingModule { }
