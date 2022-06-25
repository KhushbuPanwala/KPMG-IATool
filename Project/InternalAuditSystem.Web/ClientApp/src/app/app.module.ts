
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AppComponent } from './app.component';
import { SidemenuComponent } from './sidemenu/sidemenu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from './shared/shared-bootstrap.module';
import { StringConstants } from './shared/stringConstants';
import { AppRoutingModule } from './app-routing.module';
import { BreadcrumbComponent } from './breadcrumb/breadcrumb.component';
import { APP_BASE_HREF } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { BASE_PATH } from '../app/swaggerapi/AngularFiles/variables';
import { ApiModule } from '../app/swaggerapi/AngularFiles/api.module';
import { APIS, AuditableEntitiesService } from './swaggerapi/AngularFiles';
import { CoreModule } from './core/core.module';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { AgGridModule } from 'ag-grid-angular';
import { SharedService } from './core/shared.service';
import { UploadService } from './core/upload.service';
import { SharedModule } from './shared/shared.module';

@NgModule({
  declarations: [
    AppComponent,
    SidemenuComponent,
    BreadcrumbComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    NgSelectModule,
    FormsModule,
    CommonModule,
    SharedBootstrapModule,
    AppRoutingModule,
    HttpClientModule,
    ApiModule,
    SharedModule,
    CoreModule,
    ToastrModule.forRoot(),
    AgGridModule.withComponents([])
  ],
  bootstrap: [AppComponent],
  providers: [StringConstants, SharedService, UploadService, ToastrService, APIS, AuditableEntitiesService, { provide: APP_BASE_HREF, useValue: '/' },
    { provide: BASE_PATH, useValue: window.location.protocol + '//' + window.location.host }
  ]
})
export class AppModule { }
