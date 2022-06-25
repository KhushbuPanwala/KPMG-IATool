import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { DivisionRoutingModule } from './division-routing.module';
import { DivisionListComponent } from './division-list/division-list.component';
import { DivisionAddComponent } from './division-add/division-add.component';


@NgModule({
  declarations: [DivisionListComponent, DivisionAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    DivisionRoutingModule
  ]
})
export class DivisionModule { }
