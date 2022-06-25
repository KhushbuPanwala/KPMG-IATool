import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { SharedModule } from '../../shared/shared.module';
import { ClassificationRoutingModule } from './classification-routing.module';
import { ClassificationComponent } from './classification.component';


@NgModule({
  declarations: [ClassificationComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    ClassificationRoutingModule
  ]
})
export class ClassificationModule { }
