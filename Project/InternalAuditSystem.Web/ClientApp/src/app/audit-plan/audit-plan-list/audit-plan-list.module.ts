import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../../shared/shared.module';
import { AuditPlanRoutingModule } from './audit-plan-list-routing.module';
import { AuditPlanListComponent } from './audit-plan-list.component';
import { AuditPlanSharedService } from '../audit-plan-shared.service';


@NgModule({
  declarations: [AuditPlanListComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    SharedModule,
    AuditPlanRoutingModule
  ],
  providers: [AuditPlanSharedService]
})
export class AuditPlanModule { }
