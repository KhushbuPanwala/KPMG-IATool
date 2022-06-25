import { NgModule } from '@angular/core';
// import { ObservationSharedModule} from '../shared/Observation/observation-shared.module';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { SharedModule } from '../shared/shared.module';
import { ObservationManagementRoutingModule } from '../observation/observation-management-routing.module';
import { ManagementListComponent } from '../observation/management-list/management-list.component';
import { ObservationComponent } from '../observation/observation-management.component';
import { ObservationTabComponent } from '../observation/observation-tab/observation-tab.component';
import { ManagementCommentsComponent } from '../observation/management-comments/management-comments.component';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AngularEditorModule } from '@kolkov/angular-editor';
import { HttpClientModule } from '@angular/common/http';
import { EditorDialogComponent } from '../shared/editor-dialog/editor-dialog.component';
import { ObservationsManagementService } from '../swaggerapi/AngularFiles';
import { AddTableDialogComponent } from '../shared/add-table-dialog/add-table-dialog.component';
import { ObservationUploadFilesComponent } from '../observation/observation-upload-files/observation-upload-files.component';
@NgModule({
  declarations: [ManagementListComponent, ObservationComponent, ObservationTabComponent, ManagementCommentsComponent, AddTableDialogComponent, ObservationUploadFilesComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    HttpClientModule,
    ObservationManagementRoutingModule,
    AngularEditorModule

  ],
  providers: [TabsetComponent, ObservationsManagementService],
})
export class ObservationModule { }
