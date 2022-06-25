import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { SharedModule } from '../../shared/shared.module';
import { GeographicalAreaRoutingModule } from './geographical-area-routing.module';
import { GeographicalAreaComponent } from './geographical-area.component';
import { GeographicalAreaAddComponent } from './geographical-area-add/geographical-area-add.component';


@NgModule({
  declarations: [GeographicalAreaComponent, GeographicalAreaAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    GeographicalAreaRoutingModule
  ]
})
export class GeographicalAreaModule { }
