import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';

import { SharedModule } from '../../shared/shared.module';
import { EntityDocumentsRoutingModule } from './entity-documents-routing.module';
import { EntityDocumentsComponent } from './entity-documents.component';
import { EntityDocumentsAddComponent } from './entity-documents-add/entity-documents-add.component';


@NgModule({
  declarations: [EntityDocumentsComponent, EntityDocumentsAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    EntityDocumentsRoutingModule
  ]
})
export class EntityDocumentsModule { }
