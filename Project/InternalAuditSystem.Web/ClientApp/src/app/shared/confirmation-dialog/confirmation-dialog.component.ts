import { Component, OnInit, } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
})
export class ConfirmationDialogComponent implements OnInit {
  title: string;
  id: string;

  yes: string; // Variable for yes
  no: string; // Variable for no
  confirmationMessage: string; // Variable for confirmation message

  pageNo: number;
  selectedPageItem: number;
  searchValue: string;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, public bsModalRef: BsModalRef) {
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
    this.confirmationMessage = this.stringConstants.confirmationMessage;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
  }

  /**
   * confirmation method
   */
  confirm() {
    if (this.bsModalRef.content.callback != null) {
      this.bsModalRef.content.callback(this.stringConstants.yes);
      this.bsModalRef.hide();
    }
  }
  /**
   * close dialog
   */
  close() {
    if (this.bsModalRef.content.callback != null) {
      this.bsModalRef.content.callback(this.stringConstants.no);
      this.bsModalRef.hide();
    }
  }
}
