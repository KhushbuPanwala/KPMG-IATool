import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { CommonModule } from '@angular/common';
import { DistributionRoutingModule } from './distribution-routing.module';
import { DistributionListComponent } from './distribution-list/distribution-list.component';
import { DistributionAddComponent } from './distribution-add/distribution-add.component';
import { SharedModule } from '../../shared/shared.module';
import { DistributorsService } from '../../swaggerapi/AngularFiles';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [DistributionListComponent, DistributionAddComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    DistributionRoutingModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [DistributorsService]
})
export class DistributionModule { }
