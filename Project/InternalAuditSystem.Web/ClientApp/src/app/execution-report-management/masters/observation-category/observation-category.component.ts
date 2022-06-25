import { Component, OnInit, OnDestroy, Optional, Inject } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../shared/stringConstants';
import { ObservationCategoryAddComponent } from './observation-category-add/observation-category-add.component';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { SharedService } from '../../../core/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Pagination } from '../../../models/pagination';
import { ObservationCategoryAC, ObservationCategoriesService, BASE_PATH } from '../../../swaggerapi/AngularFiles';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-observation-category',
  templateUrl: './observation-category.component.html'
})
export class ObservationCategoryComponent implements OnInit, OnDestroy {
  observationCategoryText: string;
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
  selectedEntityId: string;
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  observationCategoryList = [] as Array<ObservationCategoryAC>;
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  itemsPerPage: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  baseUrl: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  // Creates an instance of documenter
  constructor(
    private modalService: BsModalService,
    private stringConstants: StringConstants, private sharedService: SharedService, public router: Router,
    private route: ActivatedRoute, private observationCategoryService: ObservationCategoriesService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.observationCategoryText = this.stringConstants.observationCategoryText;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.relationshipTypeLabel = this.stringConstants.relationshipTypeLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.baseUrl = basePath;

  }


  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
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
   * Method for loading data
   */
  loadPageData() {
    this.getAllObservationCategories(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Method for getting all observation categories
   * @param pageNumber : current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   * @param selectedEntityId: selected entityId
   */
  getAllObservationCategories(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.observationCategoryService.observationCategoriesGetAllObservationCategories(pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<ObservationCategoryAC>) => {
      this.observationCategoryList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /* Open add observation category modalpopup*/
  openObservationCategoryAdd() {
    this.modalService.config.class = 'page-modal audit-team-add';

    this.bsModalRef = this.modalService.show(ObservationCategoryAddComponent, {
      initialState: {
        keyboard: true,
        observationCategoryObject: {} as ObservationCategoryAC,
        callback: (result) => {
          if (result !== undefined) {
            this.bsModalRef.hide();
            this.loadPageData();
            this.totalRecords = this.totalRecords + 1;
            // set footer showing message
            this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);

            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          }
        }
      }
    });
  }

  /**
   * Method for editing observation category
   * @param observationCategoryId : Selected observation categoryId
   * @param index: Particular index of a list
   */
  openObservationCategoryEditModal(observationCategoryId: string, index: number) {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.observationCategoryService.observationCategoriesGetObservationCategoryDetailsById(observationCategoryId, this.selectedEntityId)
      .subscribe((response) => {
        this.bsModalRef = this.modalService.show(ObservationCategoryAddComponent, {
          initialState: {
            keyboard: true,
            observationCategoryId,
            observationCategoryObject: response,
            callback: (result) => {
              if (result !== undefined) {
                // update particular value of the entry
                this.observationCategoryList[index] = result;

                // success message
                this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
              }
            },
          }
        });
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Open ObservationCategory delete confirmation modal
   * @param observationCategoryId: Selected observation categoryId
   * @param index: Particular index of a list
   */
  openObservationCategoryDeleteModal(observationCategoryId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.observationCategoryService.observationCategoriesDeleteObservationCategory(observationCategoryId, this.selectedEntityId)
            .subscribe(() => {
              this.observationCategoryList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.observationCategoryList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;

              }
              this.loadPageData();

              this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);

              // success message
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);

            }, (error) => {
              this.sharedService.handleError(error);
            });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * Search ObservationCategory basis of name
   * @param event : key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchObservationCategory(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getAllObservationCategories(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.getAllObservationCategories(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem: Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.observationCategoryList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.observationCategoryList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Method for export to excel of observation category
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelObservationCategoryApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
