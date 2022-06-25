import { Component, OnInit } from '@angular/core';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { StringConstants } from '../stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-editor-dialog',
  templateUrl: './editor-dialog.component.html'
})
export class EditorDialogComponent implements OnInit {

  saveButtonText: string; // Variable for save button text
  title: string; // Title for background modal
  data = '';

  // HTML Editor
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'link',
        'unlink',
        'insertImage',
        'insertVideo',
        'toggleEditorMode',
        'undo',
        'redo',
        'removeFormat'
      ]
    ]
  };

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, public bsModalRef: BsModalRef) {
    this.saveButtonText = this.stringConstants.saveButtonText;
  }

  ngOnInit(): void {
  }

  /**
   * confirmation method
   */
  save() {
    if (this.bsModalRef.content.callback != null) {
      this.bsModalRef.content.callback(this.data);
      this.bsModalRef.hide();
    }
  }

}
