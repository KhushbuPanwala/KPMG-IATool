import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { RatingAC, RatingsService, BASE_PATH } from '../../../../swaggerapi/AngularFiles';
import { Pagination } from '../../../../models/pagination';
import { SharedService } from '../../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rating-list',
  templateUrl: './rating-list.component.html',
  styleUrls: ['./rating-list.component.scss']
})

export class RatingListComponent implements OnInit, OnDestroy {

  ratingList = [] as Array<RatingAC>;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  ratingLabel: string; // Variable for rating list title
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variabale for excel placeholder
  addToolTip: string; // Variable for add tooltip
  id: string; // Variable for id
  legendLabel: string; // Variable for legend
  scoreTitle: string; // Variable for score
  qualitativeFactors: string; // Variable for qualitative factors
  quantitativeFactors: string; // Variable for quantitative factors
  showingResults: string; // Variable for showing results
  editToolTip: string; // Variable for edit tool
  deleteToolTip: string; // Variable for delete tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  redColor: string;
  yellowColor: string;
  greenColor: string;
  selectedEntityId;

  showNoDataText: string;
  baseUrl: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private apiService: RatingsService,
    public router: Router,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.baseUrl = basePath;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.searchText = this.stringConstants.searchText;
    this.legendLabel = this.stringConstants.legendLabel;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.id = this.stringConstants.id;
    this.scoreTitle = this.stringConstants.scoreTitle;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.qualitativeFactors = this.stringConstants.qualitativeFactors;
    this.quantitativeFactors = this.stringConstants.quantitativeFactors;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.redColor = this.stringConstants.redColor;
    this.yellowColor = this.stringConstants.yellowColor;
    this.greenColor = this.stringConstants.greenColor;
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
  ngOnInit() {

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
        this.getRatings(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * Search Ratings Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchRatings(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getRatings(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /***
   * Get Ratings Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   * @param selectedEntityId : Current selected auditable entiity
   */
  getRatings(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.apiService.ratingsGetRatings(pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<RatingAC>) => {
      this.ratingList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getRatings(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRatings(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Delete rating
   * @param ratingId: id to delete rating
   */
  deleteRating(ratingId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.apiService.ratingsDeleteRating(ratingId, this.selectedEntityId).subscribe(data => {
              this.getRatings(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * Export Ratings to excel
   */
  exportRatings() {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + '/api/Ratings/exportRatings?entityId=' + this.selectedEntityId + '&timeOffset=' + timeOffset;
    this.sharedService.exportToExcel(url);
  }

  /**
   * Open add rating page
   */
  openAddRating() {
    this.router.navigate(['rating/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
  /**
   * Edit rating
   * @param ratingId: id to edit rating
   */
  editRating(ratingId: string) {
    this.router.navigate(['rating/add', { id: ratingId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
