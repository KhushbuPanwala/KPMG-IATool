import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { SharedModule } from '../../shared/shared.module';
import { AuditableEntityDetailRoutingModule } from './auditable-entity-detail-routing.module';
import { AuditableEntityDetailComponent } from './auditable-entity-detail.component';


@NgModule({
  declarations: [AuditableEntityDetailComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    AuditableEntityDetailRoutingModule
  ]
})
export class AuditableEntityDetailModule { }
