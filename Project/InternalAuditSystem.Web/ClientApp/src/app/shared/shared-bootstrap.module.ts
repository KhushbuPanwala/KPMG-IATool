import { NgModule } from '@angular/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { ModalModule, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { UploadFileDialogComponent } from './upload-file-dialog/upload-file-dialog.component';

@NgModule({
  imports: [
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot(),
    PopoverModule.forRoot(),
    TypeaheadModule.forRoot(),
    TooltipModule.forRoot(),
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ButtonsModule.forRoot(),
    ModalModule.forRoot()
  ],
  exports: [
    BsDatepickerModule,
    TimepickerModule,
    PopoverModule,
    TooltipModule,
    PaginationModule,
    PaginationModule,
    TabsModule,
    ButtonsModule,
    ModalModule,
    TypeaheadModule,
    BsDropdownModule

  ],
  declarations: [ConfirmationDialogComponent, UploadFileDialogComponent],
  providers: [PopoverModule, BsModalRef],
  entryComponents: [ConfirmationDialogComponent, UploadFileDialogComponent]
})

export class SharedBootstrapModule {

}
