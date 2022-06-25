import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';

import { SharedModule } from '../../shared/shared.module';
import { PlanAuditProcessRoutingModule } from './plan-audit-process-routing.module';
import { PlanAuditProcessComponent } from './plan-audit-process.component';
import { PlanAuditProcessAddComponent } from './plan-audit-process-add/plan-audit-process-add.component';
import { AuditPlanSharedService } from '../audit-plan-shared.service';

@NgModule({
  declarations: [PlanAuditProcessComponent, PlanAuditProcessAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    PlanAuditProcessRoutingModule
  ],
  providers: [AuditPlanSharedService]
})
export class PlanAuditProcessModule { }
