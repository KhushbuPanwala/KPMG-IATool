import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedModule } from '../../shared/shared.module';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { AuditableEntityListComponent } from './auditable-entity-list/auditable-entity-list.component';
import { AuditableEntityListTypeRoutingModule } from './auditable-entity-list-routing.module';


@NgModule({
  declarations: [AuditableEntityListComponent],
  imports: [
    CommonModule,
    AuditableEntityListTypeRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedModule,
    SharedBootstrapModule
  ]
})
export class AuditableEntityListModule { }
