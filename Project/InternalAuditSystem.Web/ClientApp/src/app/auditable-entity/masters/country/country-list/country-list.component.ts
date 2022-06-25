import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { CountryAddComponent } from '../country-add/country-add.component';
import { EntityCountryAC, CountriesService, BASE_PATH, UserAC } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { Pagination } from '../../../../models/pagination';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { async } from '@angular/core/testing';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-country-list',
  templateUrl: './country-list.component.html',
})
export class CountryListComponent implements OnInit, OnDestroy {

  // Creates an instance of documenter
  constructor(
    private modalService: BsModalService,
    private stringConstants: StringConstants, private sharedService: SharedService,
    private loaderService: LoaderService,
    private countryService: CountriesService,
    @Optional() @Inject(BASE_PATH) basePath: string) {
    this.regionText = this.stringConstants.regionText;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.relationshipTypeLabel = this.stringConstants.relationshipTypeLabel;
    this.countryText = this.stringConstants.countryText;
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.countryList = [];
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.baseUrl = basePath;
  }

  regionText: string; // Variable for region title
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  relationshipTypeLabel: string; // Variable for relationship type
  countryText: string; // Variable for country title text
  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows
  countryList = [] as Array<EntityCountryAC>;
  selectedEntityId: string;
  showNoDataText: string;
  baseUrl: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };



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
   * Load page data
   */
  loadPageData() {
    this.getAllCountry(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   *  Method for getting country list
   * @param pageNumber : Current Page no
   * @param itemsPerPage: No of items per page selected
   * @param searchValue: Search text
   * @param selectedEntityId: Current selected auditable entiity
   */
  getAllCountry(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.countryService.countriesGetAllCountry(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<EntityCountryAC>) => {
        this.countryList = result.items;
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
        this.handleError(error);
      });
  }

  /**
   * Search country basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchCountry(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.getAllCountry(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Open add country modalpopup
   */
  openCountryAdd() {

    const initialState = {
      keyboard: true,
      countryObject: {} as EntityCountryAC,
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
    };

    this.bsModalRef = this.modalService.show(CountryAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open country edit modal for updating details
   * @param countryId : Id of country
   * @param index  : Particular index of a list
   */
  openCountryEditModal(countryId: string, index: number) {
    this.countryService.countriesGetCountryById(countryId, this.selectedEntityId)
      .subscribe((response) => {
        const initialState = {
          keyboard: true,
          countryId,
          countryObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.countryList[index] = result;

              // success message
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
            }
          },
        };
        this.bsModalRef = this.modalService.show(CountryAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Open country delete confirmation modal
   * @param countryId: country Id that need to be deleted
   * @param index: Index of the entry you want to delete
   */
  openCountryDeleteModal(countryId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.countryService.countriesDeleteCountry(countryId, this.selectedEntityId)
            .subscribe(() => {
              this.countryList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.countryList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;
              }

              this.loadPageData();

              this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);
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
    this.getAllCountry(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem: Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.countryList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.countryList.length) {
      this.pageNumber = 1;
    }
  }

  /**
   * Method for export to excel of country
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelCountryApi + this.selectedEntityId + '&timeOffset=' + offset;

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
