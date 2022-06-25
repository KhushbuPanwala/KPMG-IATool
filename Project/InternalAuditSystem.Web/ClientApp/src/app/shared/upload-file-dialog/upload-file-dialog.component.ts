import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-upload-file-dialog',
  templateUrl: './upload-file-dialog.component.html'
})
export class UploadFileDialogComponent implements OnInit, AfterViewInit {
  saveButtonText: string; // Variable for save button text
  title: string; // Variable for modal title
  noFileChosen: string; // Variable for no file choose
  chooseFileText: string; // Variable for choose file text
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for fileNameText
  searchText: string; // Variable for search text
  @ViewChild('planDocument', { static: false }) myDialog: ElementRef;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, public bsModalRef: BsModalRef) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.searchText = this.stringConstants.searchText;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
  }

  /**
   * Called after the view is initially rendered
   */
  ngAfterViewInit() {
    this.myDialog.nativeElement.parentElement.parentElement.parentElement.parentElement.classList.add('remove-click');
  }
}
