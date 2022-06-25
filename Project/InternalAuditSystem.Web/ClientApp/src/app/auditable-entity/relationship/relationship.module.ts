import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../shared/shared-bootstrap.module';
import { SharedModule } from '../../shared/shared.module';
import { RelationshipRoutingModule } from './relationship-routing.module';
import { RelationshipComponent } from './relationship.component';


@NgModule({
  declarations: [RelationshipComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    RelationshipRoutingModule
  ]
})
export class RelationshipModule { }
