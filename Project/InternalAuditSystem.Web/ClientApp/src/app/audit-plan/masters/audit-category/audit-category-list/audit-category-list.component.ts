import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { AuditCategoryAddComponent } from '../audit-category-add/audit-category-add.component';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { AuditCategoryAC } from '../../../../swaggerapi/AngularFiles/model/auditCategoryAC';
import { AuditCategoriesService, BASE_PATH } from '../../../../swaggerapi/AngularFiles';
import { Pagination } from '../../../../models/pagination';
import { Subscription } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-audit-category-list',
  templateUrl: './audit-category-list.component.html'
})
export class AuditCategoryListComponent implements OnInit, OnDestroy {

  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows


  categoryLabel: string; // Variable for category label
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  showNoDataText: string;
  auditCategoryLabel: string;
  baseUrl: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // Objects
  auditCategoryList = [] as Array<AuditCategoryAC>;
  selectedEntityId: string;
  startItem: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    public router: Router,
    private auditCategoryService: AuditCategoriesService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.categoryLabel = this.stringConstants.categoryLabel;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.auditCategoryLabel = this.stringConstants.auditCategoryLabel;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.auditCategoryList = [];
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
    this.getAllAuditCategories(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all audit categories under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllAuditCategories(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.loaderService.close();
    this.auditCategoryService.auditCategoriesGetAllAuditCategories(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<AuditCategoryAC>) => {
        this.loaderService.close();
        this.auditCategoryList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.handleError(error);
      });
  }

  /***
   * Search audit categories basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchAuditCategory(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;

      this.loaderService.open();
      this.getAllAuditCategories(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }


  /**
   * Open audit category add modal for adding audit category
   */
  openAuditCategoryAddModal() {
    const initialState = {
      auditCategoryModalTitle: this.stringConstants.auditCategoryLabel,
      keyboard: true,
      auditCategoryObject: {} as AuditCategoryAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };

    this.bsModalRef = this.modalService.show(AuditCategoryAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open audit category edit modal for updating details
   * @param auditCategoryId : Seleted audit category Id
   * @param index : Particular index of a list
   */
  openAuditCategoryEditModal(auditCategoryId: string, index: number) {
    this.loaderService.open();
    this.auditCategoryService.auditCategoriesGetAuditCategoryById(auditCategoryId, this.selectedEntityId)
      .subscribe((response) => {

        this.loaderService.close();
        const initialState = {
          auditCategoryModalTitle: this.stringConstants.auditCategoryLabel,
          keyboard: true,
          auditCategoryId,
          auditCategoryObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.auditCategoryList[index] = result;
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };

        this.bsModalRef = this.modalService.show(AuditCategoryAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Open audit category delete confirmation modal
   * @param auditCategoryId : Audit category Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  openAuditCategoryDeleteModal(auditCategoryId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditCategoryService.auditCategoriesDeleteAuditCategory(auditCategoryId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.auditCategoryList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.auditCategoryList.length === 0 && this.pageNumber > 1) {
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
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loadPageData();
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.auditCategoryList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.auditCategoryList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Method for export to excel of audit category
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditCategoryApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
