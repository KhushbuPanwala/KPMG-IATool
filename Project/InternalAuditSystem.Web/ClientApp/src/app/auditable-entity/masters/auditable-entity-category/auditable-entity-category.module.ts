import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { AuditableEntityCategoryRoutingModule } from './auditable-entity-category-routing.module';
import { AuditableEntityCategoryListComponent } from './auditable-entity-category-list/auditable-entity-category-list.component';
import { AuditableEntityCategoryAddComponent } from './auditable-entity-category-add/auditable-entity-category-add.component';
import { EntityCategoriesService } from '../../../swaggerapi/AngularFiles/api/entityCategories.service';


@NgModule({
  declarations: [AuditableEntityCategoryListComponent, AuditableEntityCategoryAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    AuditableEntityCategoryRoutingModule
  ],
  providers: [EntityCategoriesService]
})
export class AuditableEntityCategoryModule { }
