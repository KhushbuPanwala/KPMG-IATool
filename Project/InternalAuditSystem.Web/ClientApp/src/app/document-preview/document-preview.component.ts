import { Component, OnInit, } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-document-preview-dialog',
  templateUrl: './document-preview.component.html',
})
export class DocumentPreviewComponent implements OnInit {
  documentPath: string;

  // Creates an instance of documenter.
  constructor(private route: ActivatedRoute) {
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      // decode base 64 to string
      this.documentPath = atob(params.path);
    });
  }
}
