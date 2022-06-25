import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { AuditSubProcessRoutingModule } from './audit-sub-process-routing.module';
import { AuditSubProcessListComponent } from './audit-sub-process-list/audit-sub-process-list.component';
import { AuditSubProcessAddComponent } from './audit-sub-process-add/audit-sub-process-add.component';
import { AuditSubProcessesService } from '../../../swaggerapi/AngularFiles/api/auditSubProcesses.service';
import { AuditProcessesService } from '../../../swaggerapi/AngularFiles/api/auditProcesses.service';


@NgModule({
  declarations: [AuditSubProcessListComponent, AuditSubProcessAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    AuditSubProcessRoutingModule
  ],
  providers: [AuditSubProcessesService, AuditProcessesService]

})
export class AuditSubProcessModule { }
