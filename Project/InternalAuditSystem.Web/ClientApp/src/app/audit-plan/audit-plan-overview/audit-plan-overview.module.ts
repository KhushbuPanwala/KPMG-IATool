import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { AuditPlanOverviewRoutingModule } from './audit-plan-overview-routing.module';
import { AuditPlanOverviewComponent } from './audit-plan-overview.component';
import { AuditPlanSharedService } from '../audit-plan-shared.service';


@NgModule({
  declarations: [AuditPlanOverviewComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    SharedModule,
    AuditPlanOverviewRoutingModule
  ],
  providers: [AuditPlanSharedService]
})
export class AuditPlanOverviewModule { }
