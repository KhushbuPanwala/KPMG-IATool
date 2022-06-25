import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { AuditSubProcessAddComponent } from '../audit-sub-process-add/audit-sub-process-add.component';
import { ProcessAC } from '../../../../swaggerapi/AngularFiles/model/processAC';
import { AuditSubProcessesService } from '../../../../swaggerapi/AngularFiles/api/auditSubProcesses.service';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../../../../models/pagination';
import { Subscription } from 'rxjs/internal/Subscription';
import { BASE_PATH } from '../../../../swaggerapi/AngularFiles';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-audit-process-list',
  templateUrl: './audit-sub-process-list.component.html'
})
export class AuditSubProcessListComponent implements OnInit, OnDestroy {

  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // Objects
  auditSubProcessList = [] as Array<ProcessAC>;
  selectedEntityId: string;
  startItem: number;
  showNoDataText: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  auditSubProcessLabel: string; // Variable for audit sub process title
  auditProcessLabel: string; // Variable audit sub process add
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  subProcessesName: string; // Variable for processes name
  parentProcessName: string; // Variable for parent process
  baseUrl: string;
  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    public router: Router,
    private auditSubProcessService: AuditSubProcessesService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.auditSubProcessLabel = this.stringConstants.auditSubProcessLabel;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.subProcessesName = this.stringConstants.subProcessesName;
    this.parentProcessName = this.stringConstants.parentProcessName;
    this.auditProcessLabel = this.stringConstants.auditProcessLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.auditSubProcessList = [];
    this.baseUrl = basePath;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.loadPageData();
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
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getAllAuditSubProcesses(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }


  /**
   * Get all audit sub-processes under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllAuditSubProcesses(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.auditSubProcessService.auditSubProcessesGetAllAuditSubProcesses(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<ProcessAC>) => {
        this.loaderService.close();
        this.auditSubProcessList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.handleError(error);
      });
  }

  /***
   * Search audit sub-processes basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchAuditSubProcess(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;

      this.loaderService.open();
      this.getAllAuditSubProcesses(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Open audit sub-process add modal for adding audit category
   */
  openAuditSubProcessAddModal() {
    const initialState = {
      keyboard: true,
      auditSubProcessObject: {} as ProcessAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };
    this.bsModalRef = this.modalService.show(AuditSubProcessAddComponent,
      Object.assign({ initialState }, {class: 'page-modal audit-team-add'}));
  }

  /**
   * Open audit sub-process edit modal for updating details
   * @param auditSubProcessId : Seleted audit sub-process Id
   * @param index : Particular index of a list
   */
  openAuditSubProcessEditModal(auditSubProcessId: string, index: number) {
    this.loaderService.open();
    this.auditSubProcessService.auditSubProcessesGetAuditSubProcessById(auditSubProcessId, this.selectedEntityId)
      .subscribe((response) => {
        this.loaderService.close();
        const initialState = {
          keyboard: true,
          auditSubProcessId,
          auditSubProcessObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.auditSubProcessList[index] = result;
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };
        this.bsModalRef = this.modalService.show(AuditSubProcessAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));

      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Open audit sub-process delete confirmation modal
   * @param auditSubProcessId : Audit sub-process Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  openAuditSubProcessDeleteModal(auditSubProcessId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditSubProcessService.auditSubProcessesDeleteAuditSubProcess(auditSubProcessId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.auditSubProcessList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.auditSubProcessList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;
              }

              this.loadPageData();
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, error => {
                this.handleError(error);
            });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loaderService.open();
    this.getAllAuditSubProcesses(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.auditSubProcessList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.auditSubProcessList.length) {
      this.pageNumber = 1;
    }
    this.loadPageData();
  }

  /**
   * Handle error scenario in case of add/ update
   * @param error : http error
   */
  handleError(error: HttpErrorResponse) {
    this.loaderService.close();
    if (error.status === 403) {
      // if entity close then show info of close status
      this.sharedService.showInfo(this.stringConstants.closedEntityRestrictionMessage);

    } else if (error.status === 405) {
      // if entity is deleted then show warning for the action
      this.sharedService.showWarning(this.stringConstants.deletedEntityRestrictionMessage);

    } else if (error.status === 404) {
      // for any unknow page request (page not found)
      this.router.navigate([this.stringConstants.pageNotFoundPath]);

    } else if (error.status === 401) {
      // for any unauthorized access
      this.router.navigate([this.stringConstants.unauthorizedPath]);

    } else {
      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.sharedService.showError(errorMessage);
    }
  }

  /**
   * Method for export to excel of audit subprocess
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditSubProcessApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
