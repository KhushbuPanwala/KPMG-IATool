import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AcmComponent } from './acm.component';
import { AcmAddComponent } from './acm-add/acm-add.component';
import { GenerateAcmReportComponent } from './generate-acm-report/generate-acm-report.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: AcmComponent },
  { path: 'add', component: AcmAddComponent },
  { path: 'generate-acm-report', component: GenerateAcmReportComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AcmRoutingModule { }
