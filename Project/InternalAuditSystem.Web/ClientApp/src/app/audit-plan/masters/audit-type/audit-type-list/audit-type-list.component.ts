import { Component, OnInit, ViewEncapsulation, Optional, Inject } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AuditTypeAddComponent } from '../audit-type-add/audit-type-add.component';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { AuditTypeAC } from '../../../../swaggerapi/AngularFiles/model/auditTypeAC';
import { AuditTypesService } from '../../../../swaggerapi/AngularFiles/api/auditTypes.service';
import { Pagination } from '../../../../models/pagination';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { BASE_PATH } from '../../../../swaggerapi/AngularFiles';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-audit-type-list',
  templateUrl: './audit-type-list.component.html',
  encapsulation: ViewEncapsulation.None
})
export class AuditTypeListComponent implements OnInit {


  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  // string variable
  searchText: string;
  excelToolTip: string;
  addToolTip: string;
  editToolTip: string;
  deleteToolTip: string;
  showingResults: string;
  showNoDataText: string;
  bsModalRef: BsModalRef;
  auditTypeLabel: string; // Variable for rcm process title


  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // Objects
  auditTypeList = [] as Array<AuditTypeAC>;
  selectedEntityId: string;
  startItem: number;
  baseUrl: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private auditTypeService: AuditTypesService,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    public router: Router,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.auditTypeLabel = this.stringConstants.auditTypeLabel;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.auditTypeList = [];
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
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getAllAuditTypes(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all audit types under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllAuditTypes(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.auditTypeService.auditTypesGetAllAuditTypes(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<AuditTypeAC>) => {
        this.loaderService.close();
        this.auditTypeList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
        this.handleError(error);
      });
  }

  /***
   * Search audit type basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchAuditType(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getAllAuditTypes(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }


  /**
   * Open audit type add modal for adding audit type
   */
  openAuditTypeAddModal() {
    const initialState = {
      auditTypeModalTitle: this.stringConstants.auditTypeLabel,
      keyboard: true,
      auditTypeObject: {} as AuditTypeAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };
    this.bsModalRef = this.modalService.show(AuditTypeAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open audit type edit modal for updating details
   * @param auditTypeId : Seleted audit type Id
   * @param index : Particular index of a list
   */
  openAuditTypeEditModal(auditTypeId: string, index: number) {
    this.loaderService.open();
    this.auditTypeService.auditTypesGetAuditTypeById(auditTypeId, this.selectedEntityId)
      .subscribe((response) => {

        this.loaderService.close();
        const initialState = {
          auditTypeModalTitle: this.stringConstants.auditTypeLabel,
          keyboard: true,
          auditTypeId,
          auditTypeObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.auditTypeList[index] = result;
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };

        this.bsModalRef = this.modalService.show(AuditTypeAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));

      }, error => {
          this.handleError(error);
        });
  }

  /**
   * Open audit type  delete confirmation modal
   * @param auditTypeId : Audit type Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  // Method that open delete confirmation modal
  openAuditTypeDeleteModal(auditTypeId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,

      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditTypeService.auditTypesDeleteAuditType(auditTypeId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();

              this.auditTypeList.splice(index, 1);

              if (this.auditTypeList.length === 0 && this.pageNumber > 1) {
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
    this.getAllAuditTypes(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.auditTypeList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.auditTypeList.length) {
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
   * Method for export to excel of audit Type
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditTypeApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
