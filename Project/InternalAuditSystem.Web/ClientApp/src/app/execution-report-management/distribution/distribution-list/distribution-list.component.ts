import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { Pagination } from '../../../models/pagination';
import { DistributorsAC } from '../../../swaggerapi/AngularFiles/model/distributorsAC';
import { DistributorsService, BASE_PATH } from '../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-distribution-list',
  templateUrl: './distribution-list.component.html'
})
export class DistributionListComponent implements OnInit, OnDestroy {

  distributionList = [] as Array<DistributorsAC>;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  searchText: string; // Variable for search placeholder
  distributionLabel: string; // Variable for distributor list title
  excelToolTip: string; // Variabale for excel placeholder
  addToolTip: string; // Variable for add tooltip
  id: string; // Variable for id
  nameLabel: string; // Variable for name
  designationLabel: string; // Variable for Designation
  showingResults: string; // Variable for showing results
  editToolTip: string; // Variable for edit tool
  deleteToolTip: string; // Variable for delete tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  showNoDataText: string;
  selectedEntityId;
  baseUrl: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private apiService: DistributorsService,
    public router: Router,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.baseUrl = basePath;
    this.searchText = this.stringConstants.searchText;
    this.distributionLabel = this.stringConstants.distributorListPageTitle;
    this.addToolTip = this.stringConstants.addToolTip;
    this.id = this.stringConstants.id;
    this.nameLabel = this.stringConstants.nameLabel; // Variable for name
    this.designationLabel = this.stringConstants.designationLabel;
    this.deleteToolTip = this.stringConstants.deleteToolTip;

    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;
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
  async ngOnInit() {

    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        await this.getDistributors(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * Get Distributors Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   * @param selectedEntityId: selected Entity Id
   */
  async getDistributors(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.apiService.distributorsGetDistributors(pageNumber, selectedPageItem, searchValue, selectedEntityId)
      .subscribe((result: Pagination<DistributorsAC>) => {
        this.distributionList = result.items;
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

  /***
   * Search Distributor Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchDistributor(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getDistributors(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getDistributors(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getDistributors(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Delete Distributor
   * @param distributorId: id to delete distributor
   */
  deleteDistributor(distributorId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.apiService.distributorsDeleteDistributor(distributorId, this.selectedEntityId).subscribe(data => {
              this.getDistributors(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, (error) => {
              this.sharedService.handleError(error);
            });
          }
        }
      }
    });
  }

  /**
   * Export Distributor to excel
   */
  exportDistributors() {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + '/api/Distributors/exportDistributors?entityId=' + this.selectedEntityId + '&timeOffset=' + timeOffset;
    this.sharedService.exportToExcel(url);
  }

  /**
   * Open add distributor page
   */
  openAddDistributor() {
    this.router.navigate(['distribution/add', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
