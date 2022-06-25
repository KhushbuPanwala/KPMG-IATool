import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ReportGenerateComponent } from './report-generate/report-generate.component';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportDistributionComponent } from './report-distribution/report-distribution.component';
import { ObservationListComponent } from './observation-list/observation-list.component';
import { ReportObservationManagementComponent } from './report-observation-management/report-observation-management.component';
import { CommentHistoryComponent } from './comment-history/comment-history.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: ReportListComponent },
  { path: 'add', component: ReportGenerateComponent, },
  { path: 'distribution-add', component: ReportDistributionComponent },
  { path: 'observation-list', component: ObservationListComponent },
  { path: 'observation-add', component: ReportObservationManagementComponent },
  { path: 'comment-history', component: CommentHistoryComponent }
];


@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
