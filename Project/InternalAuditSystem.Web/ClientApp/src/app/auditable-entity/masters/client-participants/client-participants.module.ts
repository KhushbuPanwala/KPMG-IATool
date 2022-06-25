import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ClientParticipantsListComponent } from './client-participants-list/client-participants-list.component';
import { ClientParticipantsAddComponent } from './client-participants-add/client-participants-add.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { ClientParticipantsRoutingModule } from './client-participants-routing.module';
import { SharedModule } from '../../../shared/shared.module';
import { ClientParticipantsService } from '../../../swaggerapi/AngularFiles/api/clientParticipants.service';

@NgModule({
  declarations: [ClientParticipantsListComponent, ClientParticipantsAddComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    SharedBootstrapModule,
    FormsModule,
    ClientParticipantsRoutingModule,
    SharedModule
  ],
  providers: [ClientParticipantsService]
})
export class ClientParticipantsModule { }
