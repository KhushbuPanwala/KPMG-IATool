import { NgModule } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PersonNameValidatorDirective } from './directives/person-name-validator.directive';
import { NaturalNumberValidatorDirective } from './directives/natural-number-validator.directive';
import { EmailValidatorDirective } from './directives/email-validator.directive';
import { DecimalNumberValidatorDirective } from './directives/decimal-number-validator.directive';
import { CountryStateNameValidatorDirective } from './directives/country-state-name-validator.directive';
import { CommonInputValidatorDirective } from './directives/common-input-validator.directive';
import { AlphanumericValidatorDirective } from './directives/alphanumeric-validator.directive';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { NgxPaginationModule } from 'ngx-pagination';
import { LoaderService } from '../core/loader.service';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { LoaderInterceptor } from '../core/loader-interceptor.service';
import { LoaderComponent } from './loader/loader.component';
import { MaxLengthValidatorDirective } from './directives/maxlength-validator.directive';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { MinLengthValidatorDirective } from './directives/minlength-validator.directive';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { UnAuthorizedComponent } from './unauthorized-page/unauthorized.component';

@NgModule({

  exports: [
    PersonNameValidatorDirective,
    NaturalNumberValidatorDirective,
    EmailValidatorDirective,
    DecimalNumberValidatorDirective,
    CommonInputValidatorDirective,
    CountryStateNameValidatorDirective,
    AlphanumericValidatorDirective,
    NgxPaginationModule,
    LoaderComponent,
    MaxLengthValidatorDirective,
    MinLengthValidatorDirective,
    PageNotFoundComponent,
    UnAuthorizedComponent
  ],
  declarations: [
    PersonNameValidatorDirective,
    NaturalNumberValidatorDirective,
    EmailValidatorDirective,
    DecimalNumberValidatorDirective,
    CountryStateNameValidatorDirective,
    CommonInputValidatorDirective,
    AlphanumericValidatorDirective,
    MaxLengthValidatorDirective,
    MinLengthValidatorDirective,
    LoaderComponent,
    PageNotFoundComponent,
    UnAuthorizedComponent

  ],
  imports: [
    ModalModule.forRoot(),
    ToastrModule.forRoot(),
    NgxPaginationModule,
    NgxDocViewerModule
  ],

  providers: [LoaderService, ToastrService,
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true }
  ]

})

export class SharedModule {

}
