import { Component, OnInit, OnDestroy, Inject, Optional } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { RcmProcessAC } from '../../../swaggerapi/AngularFiles/model/rcmProcessAC';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { RcmProcessService } from '../../../swaggerapi/AngularFiles/api/rcmProcess.service';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../../../models/pagination';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { Subscription } from 'rxjs';
import { BASE_PATH } from '../../../swaggerapi/AngularFiles';

@Component({
  selector: 'app-process-list',
  templateUrl: './process-list.component.html'
})
export class ProcessListComponent implements OnInit, OnDestroy {

  rcmProcessList = [] as Array<RcmProcessAC>;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = '';
  id: string; // Variable for id
  rcmProcessTitle: string; // Variable for rcm process title
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  ofText: string; // Variable for of tooltip
  showingText: string; // Variable for showing tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  showNoDataText: string; // Variable for showing text for no data

  // only to subscripe for the current component
  entitySubscribe: Subscription;

  selectedEntityId;
  baseUrl: string;
  constructor(private stringConstants: StringConstants,
              private apiService: RcmProcessService,
              public router: Router,
              private modalService: BsModalService,
              private loaderService: LoaderService,
              private route: ActivatedRoute,
              private sharedService: SharedService, @Optional() @Inject(BASE_PATH) basePath: string) {

    this.id = this.stringConstants.id;
    this.rcmProcessTitle = this.stringConstants.rcmProcessTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingText = this.stringConstants.showingText;
    this.ofText = this.stringConstants.ofText;
    this.showingResults = '';
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.baseUrl = basePath;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getRcmProcess(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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

  /***
   * Search RCM Process Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchRcmProcess(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getRcmProcess(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /***
   * Get RCM Process Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  async getRcmProcess(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.loaderService.open();
    this.apiService.rcmProcessGetRcmProcess(pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<RcmProcessAC>) => {
      this.rcmProcessList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Set showing result data
   * @param pageNumber:  current page no.
   * @param selectedPageItem: items per page
   * @param totalRecords: total no. of reecords
   */
  setShowingResult(pageNumber: number, selectedPageItem: number, totalRecords: number) {
    const startItem = (pageNumber - 1) * selectedPageItem + 1;
    const endItem = (totalRecords < (pageNumber * selectedPageItem)) ? totalRecords : (pageNumber * selectedPageItem);
    this.showingResults = this.showingText + ' ' + startItem + ' - ' + endItem + ' ' + this.ofText + ' ' + totalRecords;
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getRcmProcess(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRcmProcess(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Open add - RCM Process page
   */
  openAddRcmProcess() {
    this.router.navigate(['process/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Delete RCM Process
   * @param processId: id to delete RCM Process
   */
  deleteRcmProcess(processId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.apiService.rcmProcessDeleteRcmProcess(processId, this.selectedEntityId).subscribe(data => {
              this.getRcmProcess(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, (error) => {
              this.loaderService.close();
              this.sharedService.handleError(error);
            });
          }
        }
      }
    });
  }

  /**
   * Edit RCM Process
   * @param processId: id to edit RCM Process
   */
  editRcmProcess(processId: string) {
    this.router.navigate(['process/add', { id: processId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method for export to excel of process
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelRcmProcessApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
