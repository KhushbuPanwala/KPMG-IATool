import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MomRoutingModule } from '../../mom/mom-routing.module';
import { MomListComponent } from '../../mom/mom-list/mom-list.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../shared-bootstrap.module';
import { MomAddComponent } from '../../mom/mom-add/mom-add.component';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { SharedModule } from '../../shared/shared.module';
import { MomsService } from '../../swaggerapi/AngularFiles/api/moms.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
@NgModule({
  declarations: [MomListComponent, MomAddComponent],

  imports: [FormsModule,
    CommonModule,
    MomRoutingModule,
    NgSelectModule,
    SharedBootstrapModule,
    HttpClientModule,
    SharedModule, ReactiveFormsModule],
  providers: [HttpClient, MomsService, DatePipe, TabsetComponent],
  exports: [MomAddComponent]
})
export class MomSharedModule { }
