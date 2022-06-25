import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { SectorRoutingModule } from './sector-routing.module';
import { SectorAddComponent } from './sector-add/sector-add.component';
import { SectorListComponent } from './sector-list/sector-list.component';
import { SharedModule } from '../../shared/shared.module';
import { RcmSectorService } from '../../swaggerapi/AngularFiles/api/rcmSector.service';
import { NgxPaginationModule } from 'ngx-pagination';

@NgModule({
  declarations: [
    SectorAddComponent,
    SectorListComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SectorRoutingModule,
    NgxPaginationModule,
    SharedModule
    ],
  providers: [RcmSectorService]
})
export class SectorModule { }
