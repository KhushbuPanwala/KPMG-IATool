import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuditCategoryRoutingModule } from './audit-category-routing.module';
import { AuditCategoryListComponent } from './audit-category-list/audit-category-list.component';
import { AuditCategoryAddComponent } from './audit-category-add/audit-category-add.component';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';


@NgModule({
  declarations: [AuditCategoryListComponent, AuditCategoryAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    NgxPaginationModule,
    AuditCategoryRoutingModule,
    SharedModule
  ],
  providers: []
})
export class AuditCategoryModule { }
