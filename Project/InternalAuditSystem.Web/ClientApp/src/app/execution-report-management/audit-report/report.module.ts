import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { NgxPaginationModule } from 'ngx-pagination';
import { CommonModule } from '@angular/common';
import { ReportRoutingModule } from './report-routing.module';
import { ReportListComponent } from './report-list/report-list.component';
import { ReportGenerateComponent } from './report-generate/report-generate.component';
import { SharedModule } from '../../shared/shared.module';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { HttpClientModule } from '@angular/common/http';
import { EditorDialogComponent } from '../../shared/editor-dialog/editor-dialog.component';
import { ReportDistributionComponent } from './report-distribution/report-distribution.component';
import { ObservationListComponent } from './observation-list/observation-list.component';
import { AgGridModule } from 'ag-grid-angular';
import { ReportObservationManagementComponent } from './report-observation-management/report-observation-management.component';
import { ReportObservationTabComponent } from './report-observation-management/report-observation-tab/report-observation-tab.component';
import { ReportManagementCommentsComponent } from './report-observation-management/report-management-comments/report-management-comments.component';
import { ReportReveiwerCommentsComponent } from './report-observation-management/report-reveiwer-comments/report-reveiwer-comments.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { AgGridCheckboxComponent } from './ag-grid-component/ag-grid-checkbox.component';
import { AgGridEditButtonComponent } from './ag-grid-component/ag-grid-edit-button.component';
import { AgGridDeleteButtonComponent } from './ag-grid-component/ag-grid-delete-button.component';
import { AgGridViewButtonComponent } from './ag-grid-component/ag-grid-view-button.component';
import { CommentHistoryComponent } from './comment-history/comment-history.component';
import { ReportsService, ReportObservationsService } from '../../swaggerapi/AngularFiles';
import { ReportSharedService } from './report-shared.service';
import { UploadReportFileDialogComponent } from './upload-report-file-dialog/upload-report-file-dialog.component';
import { ReportAddTableDialogComponent } from './report-observation-management/report-add-table-dialog/report-add-table-dialog.component';
import { ReportObservationFileDialogComponent } from './report-observation-management/report-observation-file-dialog/report-observation-file-dialog.component';

@NgModule({
  declarations: [ReportListComponent, ReportGenerateComponent, EditorDialogComponent,
    ReportDistributionComponent, ReportObservationManagementComponent,
    UploadReportFileDialogComponent, ReportAddTableDialogComponent,
    ReportObservationFileDialogComponent,
    ReportObservationTabComponent,
    ReportManagementCommentsComponent,
    ReportReveiwerCommentsComponent,
    ObservationListComponent,
    CommentHistoryComponent,
    AgGridCheckboxComponent,
    AgGridEditButtonComponent,
    AgGridDeleteButtonComponent,
    AgGridViewButtonComponent

  ],
  imports: [
    TabsModule.forRoot(),
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    NgxPaginationModule,
    ReportRoutingModule,
    SharedModule,
    HttpClientModule,
    AngularEditorModule,
    AgGridModule.withComponents([])
  ],

  providers: [ReportSharedService, ReportsService, ReportObservationsService]
})
export class ReportModule { }
