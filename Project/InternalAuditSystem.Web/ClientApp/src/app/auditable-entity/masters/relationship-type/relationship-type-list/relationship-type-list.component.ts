import { Component, OnInit, Optional, Inject, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { RelationshipTypeAddComponent } from '../relationship-type-add/relationship-type-add.component';
import { RelationshipTypeAC, RelationshipTypesService, BASE_PATH, UserAC } from '../../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { Pagination } from '../../../../models/pagination';
import { Subscription } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-relationship-type-list',
  templateUrl: './relationship-type-list.component.html'
})
export class RelationshipTypeListComponent implements OnInit, OnDestroy {

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
  relationshipTypeList = [] as Array<RelationshipTypeAC>;
  selectedEntityId: string;
  startItem: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;

  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  relationshipTypeLabel: string; // Variable for relationship type
  showNoDataText: string;
  baseUrl: string;
  // Creates an instance of documenter
  constructor(
    private modalService: BsModalService,
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private relationshipTypeService: RelationshipTypesService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.relationshipTypeLabel = this.stringConstants.relationshipTypeLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.relationshipTypeList = [];
    this.baseUrl = basePath;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
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
    // current logged in user details
    this.userSubscribe = this.sharedService.currentUserDetailsSubject.subscribe((currentUserDetails) => {
      if (currentUserDetails !== null) {
        this.currentUserDetails = currentUserDetails.userDetails;
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
    this.userSubscribe.unsubscribe();
  }

  /**
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getAllRelationshipTypes(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all relationship types under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllRelationshipTypes(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {

    this.relationshipTypeService.relationshipTypesGetAllRelationshipTypes(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<RelationshipTypeAC>) => {
        this.loaderService.close();
        this.relationshipTypeList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.handleError(error);
      });
  }

  /***
   * Search relationship types basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchRelationshipType(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;

      this.loaderService.open();
      this.getAllRelationshipTypes(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Open relationship type add modal for adding relationship type
   */
  openRelationshipTypeAddModal() {
    const initialState = {
      keyboard: true,
      relationshipTypeObject: {} as RelationshipTypeAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };

    this.bsModalRef = this.modalService.show(RelationshipTypeAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open relationship type edit modal for updating details
   * @param relationshipTypeId : relationship type Id that need to be edited
   * @param index : Particular index of a list
   */
  openRelationshipTypeEditModal(relationshipTypeId: string, index: number) {
    this.loaderService.open();
    this.relationshipTypeService.relationshipTypesGetRelationshipTypeById(relationshipTypeId, this.selectedEntityId)
      .subscribe((response) => {
        this.loaderService.close();
        const initialState = {
          keyboard: true,
          relationshipTypeId,
          relationshipTypeObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.relationshipTypeList[index] = result;

              // success message
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };

        this.bsModalRef = this.modalService.show(RelationshipTypeAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));

      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Open relationship type delete confirmation modal
   * @param relationshipTypeId : relationship type Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  openRelationshipTypeDeleteModal(relationshipTypeId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.relationshipTypeService.relationshipTypesDeleteRelationshipType(relationshipTypeId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.relationshipTypeList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.relationshipTypeList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;
              }

              this.loadPageData();

              // success message
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
    this.loadPageData();
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.relationshipTypeList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.relationshipTypeList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Method for export to excel of auditable entity relationship type
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditableEntityRelationShipTypeApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
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
    } else {

      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.sharedService.showError(errorMessage);
    }
  }
}
