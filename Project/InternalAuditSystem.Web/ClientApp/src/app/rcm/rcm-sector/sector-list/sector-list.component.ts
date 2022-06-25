import { Component, OnInit, OnDestroy, Inject, Optional } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Pagination } from '../../../models/pagination';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { RcmSectorService } from '../../../swaggerapi/AngularFiles/api/rcmSector.service';
import { RcmSectorAC } from '../../../swaggerapi/AngularFiles/model/rcmSectorAC';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { Subscription } from 'rxjs';
import { BASE_PATH } from '../../../swaggerapi/AngularFiles';


@Component({
  selector: 'app-sector-list',
  templateUrl: './sector-list.component.html',
})
export class SectorListComponent implements OnInit, OnDestroy {

  rcmSectorList = [] as Array<RcmSectorAC>;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = '';
  id: string; // Variable for id
  rcmSectorTitle: string; // Variable for rcm sector title
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  showingText: string; // Variable for showing tooltip
  ofText: string; // Variable for of tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  showNoDataText: string; // Variable for showing text for no data
  selectedEntityId;

  // only to subscripe for the current component
  entitySubscribe: Subscription;
  baseUrl: string;
  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private apiService: RcmSectorService,
    public router: Router,
    private modalService: BsModalService,
    private loaderService: LoaderService,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    @Optional() @Inject(BASE_PATH) basePath: string) {

    this.id = this.stringConstants.id;
    this.rcmSectorTitle = this.stringConstants.rcmSectorTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
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
        this.getRcmSector(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * Search RCM Sector Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchRcmSector(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getRcmSector(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /***
   * Get RCM Sector Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  async getRcmSector(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.loaderService.open();
    this.apiService.rcmSectorGetRcmSector(pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<RcmSectorAC>) => {
      this.rcmSectorList = result.items;
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
    this.getRcmSector(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRcmSector(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Open add RCM Sector page
   */
  openAddRcmSector() {
    this.router.navigate(['sector/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Delete RCM Sector
   * @param sectorId: id to delete RCM Sector
   */
  deleteRcmSector(sectorId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.apiService.rcmSectorDeleteRcmSector(sectorId, this.selectedEntityId).subscribe(data => {
              this.loaderService.close();
              this.getRcmSector(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * Edit RCM Sector
   * @param sectorId: id to edit RCM Sector
   */
  editRcmSector(sectorId: string) {
    this.router.navigate(['sector/add', { id: sectorId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

 /**
  * Method for export to excel of rcm sector
  */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelRcmSectorApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
