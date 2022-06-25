import { Component, OnInit, OnDestroy } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { ICellRendererParams, IAfterGuiAttachedParams } from 'ag-grid-community';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ReportObservationsService } from '../../../swaggerapi/AngularFiles';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { SharedService } from '../../../core/shared.service';
import { ActivatedRoute } from '@angular/router';
import { ReportSharedService } from '../report-shared.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-ag-grid-delete-button',
  templateUrl: './ag-grid-delete-button.component.html',
  styleUrls: ['./ag-grid.component.scss']
})
export class AgGridDeleteButtonComponent implements ICellRendererAngularComp, OnInit, OnDestroy {

  params; // no type define as aggrid took any type of params
  bsModalRef: BsModalRef; // Modal ref variable
  reportId;
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  operationType;
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private apiService: ReportObservationsService,
    private sharedService: SharedService,
    private reportService: ReportSharedService,
    private route: ActivatedRoute
  ) { }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.operationType = params.type;
        });
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /**
   * AG Grid life cycle init method
   * @param params: inital value
   */
  // no type define as aggrid took any type of params
  agInit(params): void {
    this.params = params;
  }

  /**
   * AG render interface implemenation
   * @param params: deleted observation value
   */
  // no type define as aggrid took any type of params
  refresh(params): boolean {
    params.api.refreshCells(params);
    this.deleteReport(params.value);
    return false;
  }

  /**
   * Delete report
   * @param reportObservationId: id to delete report observation
   */
  deleteReport(reportObservationId: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.apiService.reportObservationsDeleteReportObservation(this.reportId, reportObservationId, this.selectedEntityId).subscribe(data => {
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            this.reportService.updateDeletedObservationList(true);
          }, (error) => {
            this.sharedService.handleError(error);
          });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }
}
