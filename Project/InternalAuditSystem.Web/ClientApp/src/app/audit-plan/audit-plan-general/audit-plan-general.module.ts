import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { AuditGeneralComponent } from './audit-plan-general.component';
import { AuditGeneralRoutingModule } from './audit-plan-general-routing.module';
import { AuditPlanSharedService } from '../audit-plan-shared.service';


@NgModule({
  declarations: [AuditGeneralComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    SharedModule,
    AuditGeneralRoutingModule,
  ],
  providers: [AuditPlanSharedService]
})
export class AuditGeneralModule { }
