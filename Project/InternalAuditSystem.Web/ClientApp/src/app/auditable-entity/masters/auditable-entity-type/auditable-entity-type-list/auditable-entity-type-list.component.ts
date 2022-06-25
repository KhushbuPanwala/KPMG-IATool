import { Component, OnInit, Inject, Optional, OnDestroy} from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AuditableEntityTypeAddComponent } from '../auditable-entity-type-add/auditable-entity-type-add.component';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { EntityTypeAC } from '../../../../swaggerapi/AngularFiles/model/entityTypeAC';
import { Pagination } from '../../../../models/pagination';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { EntityTypesService } from '../../../../swaggerapi/AngularFiles/api/entityTypes.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { BASE_PATH, UserAC } from '../../../../swaggerapi/AngularFiles';

@Component({
  selector: 'app-auditable-entity-type-list',
  templateUrl: './auditable-entity-type-list.component.html'
})
export class AuditableEntityTypeListComponent implements OnInit, OnDestroy {

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
  entityTypeList = [] as Array<EntityTypeAC>;
  selectedEntityId: string;
  startItem: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;

  entityTypeLabel: string; // Variable for auditable entity type
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  showNoDataText: string;
  baseUrl: string;
  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private entityTypesService: EntityTypesService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.entityTypeLabel = this.stringConstants.auditableEntityTypeLabel;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.entityTypeList = [];
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
    this.getAllEntitTypes(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all entity types under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllEntitTypes(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {

    this.entityTypesService.entityTypesGetAllEntityTypes(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<EntityTypeAC>) => {
        this.loaderService.close();
        this.entityTypeList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.handleError(error);
      });
  }

  /***
   * Search entity types basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchEntityType(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;

      this.loaderService.open();
      this.getAllEntitTypes(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Open entity type add modal for adding entity type
   */
  openEntityTypeAddModal() {
    const initialState = {
      keyboard: true,
      entityTypeObject: {} as EntityTypeAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };

    this.bsModalRef = this.modalService.show(AuditableEntityTypeAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open entity type edit modal for updating details
   * @param entityTypeId : Seleted entity type  Id
   * @param index : Particular index of a list
   */
  openEntityTypeEditModal(entityTypeId: string, index: number) {
    this.loaderService.open();
    this.entityTypesService.entityTypesGetEntityTypById(entityTypeId, this.selectedEntityId)
      .subscribe((response) => {

        this.loaderService.close();
        const initialState = {
          keyboard: true,
          entityTypeId,
          entityTypeObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.entityTypeList[index] = result;

              // success message
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };

        this.bsModalRef = this.modalService.show(AuditableEntityTypeAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Open entity type delete confirmation modal
   * @param entityTypeId : Entity type Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  openEntityTypeDeleteModal(entityTypeId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.entityTypesService.entityTypesDeleteEntityType(entityTypeId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.entityTypeList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.entityTypeList.length === 0 && this.pageNumber > 1) {
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
    if (this.entityTypeList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.entityTypeList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Method for export to excel of auditable entity type
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditableEntityTypeApi + this.selectedEntityId + '&timeOffset=' + offset;

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
