import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';

import { SharedModule } from '../../../shared/shared.module';
import { AuditProcessRoutingModule } from './audit-process-routing.module';
import { AuditProcessListComponent } from './audit-process-list/audit-process-list.component';
import { AuditProcessAddComponent } from './audit-process-add/audit-process-add.component';
import { AuditProcessesService } from '../../../swaggerapi/AngularFiles/api/auditProcesses.service';


@NgModule({
  declarations: [AuditProcessListComponent, AuditProcessAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedModule,
    SharedBootstrapModule,
    AuditProcessRoutingModule
  ],
  providers : [AuditProcessesService]
})
export class AuditProcessModule { }
