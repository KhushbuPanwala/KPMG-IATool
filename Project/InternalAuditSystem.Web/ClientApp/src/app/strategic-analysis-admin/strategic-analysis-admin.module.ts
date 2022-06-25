import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared/shared-bootstrap.module';
import { StrategicAnalysisAdminListComponent } from './strategic-analysis-admin-list/strategic-analysis-admin-list.component';
import { StrategicAnalysisAdminRoutingModule } from './strategic-analysis-admin-routing.module';
import { HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { StrategicAnalysisAdminAddComponent } from './strategic-analysis-admin-add/strategic-analysis-admin-add.component';


@NgModule({
  declarations: [StrategicAnalysisAdminListComponent, StrategicAnalysisAdminAddComponent],
  imports: [
    CommonModule,
    StrategicAnalysisAdminRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    HttpClientModule,
    SharedModule
  ],
  exports: [StrategicAnalysisAdminAddComponent]
})
export class StrategicAnalysisAdminModule { }
