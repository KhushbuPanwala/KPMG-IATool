import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { GeographicalAreaAddComponent } from './geographical-area-add/geographical-area-add.component';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { Pagination } from '../../models/pagination';
import { PrimaryGeographicalAreaAC } from '../../swaggerapi/AngularFiles/model/primaryGeographicalAreaAC';
import { PrimaryGeographicalAreasService, UserAC, UserRole } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-geographical-area',
  templateUrl: './geographical-area.component.html'
})
export class GeographicalAreaComponent implements OnInit, OnDestroy {
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  primaryAreasTitle: string; // Variable for primary areas title
  pdfToolTip: string; // Variable for pdf tooltip
  backToolTip: string; // Variable for back tooltip
  regionText: string; // Variable for region text
  countryText: string; // Variable for country text
  stateText: string; // Variable for state
  locationLabel: string; // Variable for location label
  saveNextButtonText: string; // Varible for next button
  previousButton: string; // Varible for previous button
  selectedPageItem: number;
  searchValue: string;
  entityId: string;
  selectedEntityId: string;
  validationMessage: string;

  pageNumber: number = null;
  totalRecords: number;
  areaSearchValue = null;
  areaSelectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  geographicalAreasList: PrimaryGeographicalAreaAC[] = [];
  userSubscribe: Subscription;
  entitySubscribe: Subscription;
  currentUserDetails: UserAC;

  // Creates an instance of documenter
  constructor(
    private modalService: BsModalService,
    private stringConstants: StringConstants,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private primaryGeographicalAreasService: PrimaryGeographicalAreasService) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.primaryAreasTitle = this.stringConstants.primaryAreasTitle;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.backToolTip = this.stringConstants.backToolTip;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.previousButton = this.stringConstants.previousButton;
    this.regionText = this.stringConstants.regionText;
    this.countryText = this.stringConstants.countryText;
    this.stateText = this.stringConstants.stateText;
    this.locationLabel = this.stringConstants.locationLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.validationMessage = this.stringConstants.auditableEntityRequiredMessage;

    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.areaSelectedPageItem = this.pageItems[0].noOfItems;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // current logged in user details
    this.userSubscribe = this.sharedService.currentUserDetailsSubject.subscribe((currentUserDetails) => {
      if (currentUserDetails !== null) {
        this.currentUserDetails = currentUserDetails.userDetails;
        // if current user is team member restrict access
        if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
          this.router.navigate(['401']);
        } else {
          // get the current selectedEntityId
          this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
            if (entityId !== '') {
              this.selectedEntityId = entityId;
              this.route.params.subscribe(params => {
                this.selectedPageItem = params.pageItems;
                this.searchValue = params.searchValue;
                if (params.id !== undefined) {
                  this.entityId = params.id;
                  this.getPrimaryGeographicalAreaList(this.pageNumber, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
                }
              });
            }
          });
        }
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    if (this.entitySubscribe !== undefined) {
      this.entitySubscribe.unsubscribe();
    }
    this.userSubscribe.unsubscribe();
  }

  /**
   * Get PrimaryGeographicalArea list of list page
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   */
  getPrimaryGeographicalAreaList(pageNumber: number, selectedPageItem: number, searchValue: string, id: string) {
    this.loaderService.open();
    this.primaryGeographicalAreasService.primaryGeographicalAreasGetPrimaryGeographicalAreaList(pageNumber,
      selectedPageItem, searchValue, id).subscribe((result: Pagination<PrimaryGeographicalAreaAC>) => {

        this.geographicalAreasList = result.items;
        if (this.geographicalAreasList.length === 0) {
          this.validationMessage = this.stringConstants.auditableEntityRequiredMessage;
        } else {
          this.validationMessage = '';
        }
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);

        this.loaderService.close();
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
  }

  /**
   * Method that open PrimaryGeographicalArea add/update modal
   * @param id: id if in edit else blanl
   * @param index: index number
   */
  openPrimaryGeographicalAreaModal(id: string, index: number) {
    const initialState = {
      title: this.primaryAreasTitle,
      entityId: this.entityId,
      primaryGeographicalAreaId: id,
      keyboard: true,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          if (id === '') {
            // if selected item per page is less than the item per page then only push in current list
            if (this.geographicalAreasList.length < this.areaSelectedPageItem) {
              this.getPrimaryGeographicalAreaList(this.pageNumber, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
            }

            this.totalRecords = this.totalRecords + 1;
            // set footer showing message
            this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.areaSelectedPageItem, this.totalRecords);

            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          } else {
            // update particular value of the entry
            this.geographicalAreasList[index] = result;
            this.getPrimaryGeographicalAreaList(this.pageNumber, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
          }
        }

      }
    };
    this.bsModalRef = this.modalService.show(GeographicalAreaAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Method that open delete confirmation modal
   * @param id: primaryGeographicalAreaId
   */
  openDeleteModal(id: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        this.loaderService.open();
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.primaryGeographicalAreasService.primaryGeographicalAreasDeletePrimaryGeographicalArea(id).subscribe(() => {
            this.loaderService.close();
            this.getPrimaryGeographicalAreaList(null, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
          }, (error) => {
            this.loaderService.close();
            this.sharedService.showError(error.error);
          });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getPrimaryGeographicalAreaList(pageNumber, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getPrimaryGeographicalAreaList(null, this.areaSelectedPageItem, this.areaSearchValue, this.entityId);
  }

  /**
   * Search PrimaryGeographicalArea on grid
   * @param event: key event tab and enter
   */
  searchPrimaryGeographicalArea(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getPrimaryGeographicalAreaList(null, this.selectedPageItem, this.areaSearchValue, this.entityId);
    }
  }

  /**
   * Method for onSaveAndNextClick
   */
  onSaveAndNextClick() {
    this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
    this.router.navigate(['auditable-entity/risk-assesment', { id: this.entityId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);

  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * on previous button route to list page
   */
  onPreviousClick() {
    this.router.navigate(['auditable-entity/classification', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }
}
