import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';

import { SharedModule } from '../../shared/shared.module';
import { RiskAssesmentRoutingModule } from './risk-assesment-routing.module';
import { RiskAssesmentComponent } from './risk-assesment.component';
import { RiskAssesmentAddComponent } from './risk-assesment-add/risk-assesment-add.component';


@NgModule({
  declarations: [RiskAssesmentComponent, RiskAssesmentAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    RiskAssesmentRoutingModule
  ]
})
export class RiskAssesmentModule { }
