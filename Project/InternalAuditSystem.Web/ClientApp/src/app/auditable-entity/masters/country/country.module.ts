import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { CountryListComponent } from './country-list/country-list.component';
import { CountryAddComponent } from './country-add/country-add.component';
import { CountryRoutingModule } from './country-routing.module';
import { CountriesService } from '../../../swaggerapi/AngularFiles';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [CountryListComponent, CountryAddComponent],
  imports: [
    CommonModule,
    CountryRoutingModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    HttpClientModule
  ],
  providers: [CountriesService]
})
export class CountryModule { }
