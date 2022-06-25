import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { AuditAdvisoryRoutingModule } from './audit-advisory-routing.module';
import { AuditAdvisoryComponent } from './audit-advisory.component';


@NgModule({
  declarations: [AuditAdvisoryComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    SharedModule,
    AuditAdvisoryRoutingModule
  ]
})
export class AuditAdvisoryModule { }
