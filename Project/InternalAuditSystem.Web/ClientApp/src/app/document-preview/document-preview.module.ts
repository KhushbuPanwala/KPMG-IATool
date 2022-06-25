import { NgModule } from '@angular/core';
import { DocumentPreviewComponent } from './document-preview.component';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { DocumentPreviewRoutingModule } from './document-preview-routing.module';

@NgModule({
  declarations: [
    DocumentPreviewComponent
  ],
  imports: [
    NgxDocViewerModule,
    DocumentPreviewRoutingModule
  ],
  providers: []
})
export class DocumentPreviewModule { }
