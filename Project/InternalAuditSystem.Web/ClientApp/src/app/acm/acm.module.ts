import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { AcmRoutingModule } from './acm-routing.module';
import { AcmComponent } from './acm.component';
import { AcmAddComponent } from './acm-add/acm-add.component';
import { GenerateAcmReportComponent } from './generate-acm-report/generate-acm-report.component';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { NgxPaginationModule } from 'ngx-pagination';
import { AcmService } from '../swaggerapi/AngularFiles';
import { ACMUploadFilesComponent } from './acm-upload-files/acm-upload-files.component';
import { HttpClientModule } from '@angular/common/http';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AcmReportSharedService } from './acm-report-shared.service';
import { ACMReportsService } from '../swaggerapi/AngularFiles/api/aCMReports.service';
import { AcmReportUploadFilesComponent } from './acm-report-upload-files/acm-report-upload-files.component';


@NgModule({
  declarations: [AcmComponent, AcmAddComponent, GenerateAcmReportComponent, ACMUploadFilesComponent, AcmReportUploadFilesComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    SharedModule,
    AcmRoutingModule,
    NgxPaginationModule,
    HttpClientModule,
    AngularEditorModule
  ],
  providers: [TabsetComponent, AcmReportSharedService, AcmService, ACMReportsService]
})
export class AcmModule { }
