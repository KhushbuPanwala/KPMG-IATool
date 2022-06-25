import { Component, OnInit, Optional, Inject, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { Pagination } from '../../../models/pagination';
import { ReportAC, ReportsService, BASE_PATH, ReportUserMappingAC } from '../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';
import { ReportSharedService } from '../report-shared.service';
import { HttpUrlEncodingCodec } from '@angular/common/http';

@Component({
  selector: 'app-report-list',
  templateUrl: './report-list.component.html'
})
export class ReportListComponent implements OnInit, OnDestroy {

  reports: string; // Variable for Report title

  searchText: string; // Variable for search text
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  commentHistoryTooltip: string; // variable for CommentHistory tooltip
  showingResults: string; // Variable for showing results
  // List column name
  reportTitle: string; // Variable for report name
  ratingTitle: string; // Variable for report
  noOfObservationTitle: string; // Variable for no of observation
  statusTitle: string; // Variable for status
  stageTitle: string; // Variable for stage
  periodTitle: string; // Variable for period

  // list button tooltip
  downloadTooltip: string; // Variable for download tooltip
  powerPointToolTip: string; // Variable for powerpoint tooltip
  sendForReview: string; // Variable for send for review
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Varaible for delete title

  reportList = [] as Array<ReportAC>;
  selectedEntityId;
  baseUrl: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  toField: string;
  fromField: string;
  subject: string;
  tempUserEmailList: Array<string>;
  toEmailList: Array<string>;
  reviewerList: Array<ReportUserMappingAC>;
  report = {} as ReportAC;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private apiService: ReportsService,
    public router: Router,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private sharedService: SharedService,
    private reportSharedService: ReportSharedService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.baseUrl = basePath;
    this.reports = this.stringConstants.reports;
    this.searchText = this.stringConstants.searchText;
    this.reportTitle = this.stringConstants.reportTitle;
    this.ratingTitle = this.stringConstants.ratingTitle;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.noOfObservationTitle = this.stringConstants.noOfObservationTitle;
    this.statusTitle = this.stringConstants.statusTitle;
    this.stageTitle = this.stringConstants.stageTitle;
    this.periodTitle = this.stringConstants.periodTitle;
    this.downloadTooltip = this.stringConstants.downloadToolTip;
    this.sendForReview = this.stringConstants.sendForReview;
    this.deleteTitle = this.stringConstants.deleteTitle;

    this.addToolTip = this.stringConstants.addToolTip;
    this.showingResults = '';
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.showNoDataText = this.stringConstants.showNoDataText;

    this.commentHistoryTooltip = this.stringConstants.commentHistoryTitle;

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
        this.getReports(this.pageNumber, this.selectedPageItem, this.searchValue);
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
   * Search Reports Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchReport(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getReports(pageNumber, selectedPageItem, searchValue);
    }
  }

  /***
   * Get Reports Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  getReports(pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.apiService.reportsGetReports(pageNumber, selectedPageItem, searchValue, this.selectedEntityId, 0, 0).subscribe((result: Pagination<ReportAC>) => {
      this.reportList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
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
    this.getReports(pageNumber, this.selectedPageItem, this.searchValue);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getReports(null, this.selectedPageItem, this.searchValue);
  }

  /**
   * Delete report
   * @param reportId: id to delete report
   */
  deleteReport(reportId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.apiService.reportsDeleteReport(reportId, this.selectedEntityId).subscribe(data => {
              this.getReports(null, this.selectedPageItem, this.searchValue);
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
   * Ope observation list page
   * @param reportId: current report id
   */
  openObservationList(reportId: string) {
    this.router.navigate(['report/observation-list', { id: reportId, type: this.stringConstants.editOperationText }]);
  }

  /**
   * Open add report page
   */
  openAddReport() {
    this.router.navigate(['report/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue, type: this.stringConstants.addOperationText }]);
  }

  /**
   * Open Comment history
   * @param reportId: selected report id
   */
  openCommentHistory(reportId: string) {
    this.router.navigate(['report/comment-history', { id: reportId }]);
  }

  /**
   * Edit report
   * @param reportId: id to edit report
   */
  editReport(reportId: string) {
    this.router.navigate(['report/add', { id: reportId, pageItems: this.selectedPageItem, searchValue: this.searchValue, type: this.stringConstants.editOperationText }]);
  }


  /**
   * Export Report to excel
   */
  exportReport() {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.exportToExcelReportApi + this.selectedEntityId + this.stringConstants.timeOffSet + timeOffset;
    this.sharedService.exportToExcel(url);
  }


  /**
   * Create PPT Report
   * @param reportId: selected report id
   */
  createPPTReport(reportId: string) {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.downloadReportPPTApi + reportId + this.stringConstants.entityParamString + this.selectedEntityId + this.stringConstants.timeOffSet + timeOffset;
    this.sharedService.createPPT(url);
  }

  /**
   * View PPT Report
   * @param reportId: selected report id
   */
  viewPPTReport(reportId: string) {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    this.reportSharedService.viewReportPPT(reportId, this.selectedEntityId, timeOffset);
  }


  /**
   * Method for opening outlook
   * @param reportId: Id of report
   */
  openOutlook(reportId: string) {
    this.toEmailList = [];
    this.reviewerList = [];
    const timeOffset = new Date().getTimezoneOffset();
    this.apiService.reportsGetReportById(reportId, this.selectedEntityId, timeOffset).subscribe(result => {
      this.report = result;
      this.fromField = this.stringConstants.defaultEmailId;
      this.report.reviewerList.forEach(y => {
        this.toEmailList.push(y.email);
      });

      const reportDetail = this.reportList.find(a => a.id === reportId);
      const emailCommaSeperatedString = this.toEmailList.join(', ');
      this.toField = emailCommaSeperatedString;
      this.subject = this.report.reportTitle.replace('&', '');
      const reportURL = this.baseUrl + '/report/add;id=' + reportId + ';' + this.stringConstants.pageItemsText + '=' + this.selectedPageItem + ';' + this.stringConstants.searchValueText + '=' + this.searchValue + ';type=' + this.stringConstants.editOperationText;
      const emailBody = this.stringConstants.reportTitle + ': ' + this.report.reportTitle.replace('&', '') + '\n'
        + this.stringConstants.noOfObservationTitle + ': ' + reportDetail.noOfObservation + '\n'
        + this.stringConstants.ratingLabel + ': ' + reportDetail.ratings + '\n'
        + this.stringConstants.statusTitle + ': ' + reportDetail.status + '\n'
        + this.stringConstants.stageTitle + ': ' + reportDetail.stageName + '\n'
        + this.stringConstants.periodTitle + ': ' + this.report.auditStartDate + ' ' + this.stringConstants.toText + ' ' + this.report.auditEndDate + '\n'
        + this.stringConstants.linkText + ': ' + reportURL;
      const href = this.stringConstants.mailtoString + this.toField + '?' + this.stringConstants.subjectString + '=' + this.subject + '&' + this.stringConstants.bodyString + '=' + encodeURIComponent(emailBody);
      window.location.href = href;
    });
  }
}
