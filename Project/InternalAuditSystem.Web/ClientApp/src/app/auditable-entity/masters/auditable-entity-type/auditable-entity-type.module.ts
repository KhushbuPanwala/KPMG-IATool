import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { AuditableEntityTypeRoutingModule } from './auditable-entity-type-routing.module';
import { AuditableEntityTypeListComponent } from './auditable-entity-type-list/auditable-entity-type-list.component';
import { AuditableEntityTypeAddComponent } from './auditable-entity-type-add/auditable-entity-type-add.component';
import { EntityTypesService } from '../../../swaggerapi/AngularFiles/api/entityTypes.service';


@NgModule({
  declarations: [AuditableEntityTypeListComponent, AuditableEntityTypeAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    AuditableEntityTypeRoutingModule
  ],
  providers: [EntityTypesService]
})
export class AuditableEntityTypeModule { }
