import { NgModule } from '@angular/core';
import { RatingRoutingModule } from './rating-routing.module';
import { RatingListComponent } from './rating-list/rating-list.component';
import { RatingAddComponent } from './rating-add/rating-add.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../../../shared/shared.module';
import { RatingsService } from '../../../swaggerapi/AngularFiles';

@NgModule({
  declarations: [
    RatingListComponent,
    RatingAddComponent
  ],
  imports: [
    CommonModule,
    RatingRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    HttpClientModule,
    SharedModule
  ],
  providers: [RatingsService]
})
export class RatingModule { }
