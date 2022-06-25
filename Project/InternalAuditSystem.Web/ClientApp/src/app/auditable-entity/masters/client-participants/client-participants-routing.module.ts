import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ClientParticipantsListComponent } from './client-participants-list/client-participants-list.component';
import { ClientParticipantsAddComponent } from './client-participants-add/client-participants-add.component';

const routes: Routes = [
  { path: '', redirectTo: 'list' },
  { path: 'list', component: ClientParticipantsListComponent },
  { path: 'add', component: ClientParticipantsAddComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ClientParticipantsRoutingModule { }
