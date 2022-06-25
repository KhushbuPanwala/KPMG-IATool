import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';

import { SharedModule } from '../../shared/shared.module';
import { PlanDocumentRoutingModule } from './plan-document-routing.module';
import { PlanDocumentComponent } from './plan-document.component';
import { PlanDocumentAddComponent } from './plan-document-add/plan-document-add.component';
import { AuditPlanSharedService } from '../audit-plan-shared.service';


@NgModule({
  declarations: [PlanDocumentComponent, PlanDocumentAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    PlanDocumentRoutingModule
  ],
  providers: [AuditPlanSharedService]
})
export class PlanDocumentModule { }
