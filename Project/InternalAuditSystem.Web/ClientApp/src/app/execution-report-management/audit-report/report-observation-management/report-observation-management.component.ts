import { Component, OnInit, ViewChild, Inject, Optional } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { ReportObservationAC, ReportDetailAC, ObservationCategoryAC, RatingAC, KeyValuePairOfIntegerAndString, BASE_PATH } from '../../../swaggerapi/AngularFiles';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../../core/loader.service';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { ReportSharedService } from '../report-shared.service';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-observation',
  templateUrl: './report-observation-management.component.html',
  styleUrls: ['./report-observation-management.component.scss']
})

export class ReportObservationManagementComponent implements OnInit {
  observationManagementTitle: string; // Variable for observation main title
  backToolTip: string; // Vairable for back button tooltip
  powerPointToolTip: string; // Variable for Powerpoint alt text
  observationTabTitle: string; // Variable for observation tab title
  managementCommentsTitle: string; // Varibale for management comments title
  downloadToolTip: string; // Variable for download button
  reviewerCommentsTitle: string; // Variable for reviewer comments title

  reportObservation = {} as ReportObservationAC;
  reportObservationList = [] as Array<ReportObservationAC>;
  reportDetailAC = {} as ReportDetailAC;

  isShowManagementTab = false;
  isShowReviewerTab = false;
  isShowObservationTab = true;
  selectedObservationId: string;
  reportId;
  reportObservationId;
  operationType;
  isShowPaging = true;

  totalRecord: number; // set no. of count for pagination
  rotate = true; // pagination rotate
  maxSize = 5; // show no.of items in pagination
  page = 1; // set current page
  event = {} as PageChangedEvent;
  baseUrl: string;

  // for geting tab components
  @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;

  // messageToSendP: string = '';
  currentPage = 1;
  selectedTypeItems;
  selectedRating;
  selectedCategoryItems;

  ratingList: Array<RatingAC>;
  observationType: Array<KeyValuePairOfIntegerAndString>;
  observationCategoryList: Array<ObservationCategoryAC>;
  selectedEntityId;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private reportService: ReportSharedService,
    private router: Router,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.baseUrl = basePath;
    this.observationManagementTitle = this.stringConstants.observationManagementTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.managementCommentsTitle = this.stringConstants.managementCommentsTitle;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.reviewerCommentsTitle = this.stringConstants.reviewerCommentsTitle;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
    *  Initialization of properties.
    */
  ngOnInit() {
    this.loaderService.open();
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.reportId = params.id;
        });
        this.route.params.subscribe(params => {
          this.reportObservationId = params.reportObservationId;
          this.operationType = params.type;
        });
        if (this.reportObservationId !== undefined) {
          this.isShowPaging = false;
        }
        this.reportService.reportObservationSubject.subscribe((reportDetailAC) => {
          this.reportObservation = this.reportService.reportObservation;
          this.reportObservationList = this.reportService.reportObservationList;
          this.totalRecord = this.reportService.totalRecord * 10;
          this.loaderService.close();
        });
      }
    });
  }

  /**
   * set pagging afer save next
   */
  setPaging() {
    // set pagination
    this.event.itemsPerPage = 10;
    this.event.page = this.page + 1;
    this.pageChanged(this.event);
  }
  /**
   * Set selected reportobservation based on pagination
   * @param event: Page change event
   */

  pageChanged(event: PageChangedEvent) {
    this.loaderService.open();
    this.page = (event.page <= (this.totalRecord / 10)) ? event.page : 1;
    this.currentPage = this.page;
    const observationId = this.reportObservationList[this.page - 1].observationId;
    this.reportService.updateObservation(observationId);
    this.reportService.reportObservationSubject.subscribe((reportDetailAC) => {
      this.totalRecord = this.reportService.totalRecord * 10;
      this.loaderService.close();
    });

    this.reportService.selectedObservationSubject.subscribe((reportDetailAC) => {
      this.reportObservation = this.reportService.reportObservation;
      this.observationCategoryList = JSON.parse(JSON.stringify(this.reportService.observationCategoryList));
      this.ratingList = JSON.parse(JSON.stringify(this.reportService.ratingList));
      this.observationType = JSON.parse(JSON.stringify(this.reportService.observationTypeList));
      this.observationCategoryList = JSON.parse(JSON.stringify(this.reportService.observationCategoryList));
      this.loaderService.close();
    });
    // set observation tab as active
    this.isShowObservationTab = true;
    this.isShowManagementTab = false;
    this.isShowReviewerTab = false;
    this.selectTab(0);
  }

  /**
   * On back button click
   */
  onBackButton() {
    this.router.navigate(['report/observation-list', { id: this.reportId, type: this.stringConstants.editOperationText }]);
  }

  /**
   * Set tab as active tab
   * @param tabId: tab index
   */
  selectTab(tabId: number) {
    this.staticTabs.tabs[tabId].active = true;
  }
  /**
   * Load selected tab
   * @param data: tab directive
   */
  onSelect(data: TabDirective) {
    if (data.heading === this.managementCommentsTitle) {
      this.isShowManagementTab = true;
    } else if (data.heading === this.reviewerCommentsTitle) {
      this.isShowReviewerTab = true;
    } else {
      this.isShowObservationTab = true;
    }
  }

  /**
   * Create Report Observation PPT
   */
  createPPTReport() {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.downloadReportObservationPPTApi + this.reportObservation.id + this.stringConstants.entityParamString + this.selectedEntityId + this.stringConstants.timeOffSet + timeOffset;
    this.sharedService.createPPT(url);
  }
}
