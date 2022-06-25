import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { ObservationCategoryRoutingModule } from './observation-category-routing.module';
import { ObservationCategoryComponent } from './observation-category.component';
import { ObservationCategoryAddComponent } from './observation-category-add/observation-category-add.component';


@NgModule({
  declarations: [ObservationCategoryComponent, ObservationCategoryAddComponent],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    ObservationCategoryRoutingModule
  ]
})
export class ObservationCategoryModule { }
