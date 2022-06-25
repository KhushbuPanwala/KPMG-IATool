import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { RegionListComponent } from './region-list/region-list.component';
import { RegionAddComponent } from './region-add/region-add.component';
import { RegionRoutingModule } from './region-routing.module';
import { RegionsService } from '../../../swaggerapi/AngularFiles';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [RegionListComponent, RegionAddComponent],
  imports: [
    CommonModule,
    RegionRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [RegionsService]
})
export class RegionModule { }
