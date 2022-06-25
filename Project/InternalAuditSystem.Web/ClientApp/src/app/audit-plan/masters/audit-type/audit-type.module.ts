import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuditTypeRoutingModule } from './audit-type-routing.module';
import { AuditTypeListComponent } from './audit-type-list/audit-type-list.component';
import { AuditTypeAddComponent } from './audit-type-add/audit-type-add.component';
import { SharedModule } from '../../../shared/shared.module';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';


@NgModule({
  declarations: [AuditTypeListComponent, AuditTypeAddComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    NgxPaginationModule,
    AuditTypeRoutingModule,
    SharedModule
  ],
  providers: []
})
export class AuditTypeModule { }
