import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { SharedBootstrapModule } from '../../../shared/shared-bootstrap.module';
import { FormsModule } from '@angular/forms';
import { AuditTeamRoutingModule } from './audit-team-routing.module';
import { AuditTeamListComponent } from './audit-team-list/audit-team-list.component';
import { AuditTeamAddComponent } from './audit-team-add/audit-team-add.component';
import { SharedModule } from '../../../shared/shared.module';


@NgModule({
  declarations: [AuditTeamListComponent, AuditTeamAddComponent],
  imports: [
    CommonModule,
    NgSelectModule,
    FormsModule,
    AuditTeamRoutingModule,
    SharedBootstrapModule,
    SharedModule
  ],
  entryComponents: [AuditTeamAddComponent],

})
export class AuditTeamModule { }
