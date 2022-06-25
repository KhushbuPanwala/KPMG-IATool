import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { SharedModule } from '../../../shared/shared.module';
import { RelationshipTypeRoutingModule } from './relationship-type-routing.module';
import { RelationshipTypeListComponent } from './relationship-type-list/relationship-type-list.component';
import { RelationshipTypeAddComponent } from './relationship-type-add/relationship-type-add.component';


@NgModule({
  declarations: [RelationshipTypeListComponent, RelationshipTypeAddComponent],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    SharedBootstrapModule,
    SharedModule,
    RelationshipTypeRoutingModule
  ]
})
export class RelationshipTypeModule { }
