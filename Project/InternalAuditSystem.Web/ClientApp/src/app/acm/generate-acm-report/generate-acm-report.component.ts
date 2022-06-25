import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ReportAC, EntityUserMappingAC, ReviewStatus, KeyValuePairOfIntegerAndString, RatingAC, ReviewerDocumentAC, AcmService, ACMPresentationAC, ACMReportStatus, ReportStage, ReportStatus } from '../../swaggerapi/AngularFiles';
import { Router, ActivatedRoute } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { Pagination } from '../../models/pagination';
import { LoaderService } from '../../core/loader.service';
import { UploadReportFileDialogComponent } from '../../execution-report-management/audit-report/upload-report-file-dialog/upload-report-file-dialog.component';
import { ACMReportMappingAC } from '../../swaggerapi/AngularFiles/model/aCMReportMappingAC';
import { AgGridEvent } from 'ag-grid-community';
import { Subscription } from 'rxjs';
import { ACMReviewerDocumentAC } from '../../swaggerapi/AngularFiles/model/aCMReviewerDocumentAC';
import { AcmReportSharedService } from '../acm-report-shared.service';
import { UploadService } from '../../core/upload.service';
import { ACMReportService } from '../../swaggerapi/AngularFiles/api/aCMReport.service';
import { ACMReportsService } from '../../swaggerapi/AngularFiles/api/aCMReports.service';
import { ACMReportDetailAC } from '../../swaggerapi/AngularFiles/model/aCMReportDetailAC';
import { ACMReportUserMappingAC } from '../../swaggerapi/AngularFiles/model/aCMReportUserMappingAC';
import { ACMReviewerAC } from '../../swaggerapi/AngularFiles/model/aCMReviewerAC';
import { AcmReportUploadFilesComponent } from '../acm-report-upload-files/acm-report-upload-files.component';

@Component({
  selector: 'app-generate-acm-report',
  templateUrl: './generate-acm-report.component.html'
})

export class GenerateAcmReportComponent implements OnInit, OnDestroy {

  reports: string; // Variable for Report title

  searchText: string; // Variable for search text
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;

  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Varaible for delete title

  generateAcmReportTitle: string; // Variable for generate acm report title
  wordToolTip: string; // Variable for wordtooltip
  powerPointToolTip: string; // Variable for powerpoint
  pdfToolTip: string; //  Variable for pdf tooltip
  downloadToolTip: string; // Variable for download tooltip
  backToolTip: string; // Variable for backtooltip
  reportTitle: string; // Variable for report title
  statusTitle: string; // Variable for status title
  versionLabel: string; // Variable for version label
  stageTitle: string; // Variable for stage title
  reportListTitle: string; // Variable for report list title
  showingResults: string; // Variable for showing result
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  processLabel: string; // Variable for process
  noOfObservationTitle: string; // Variable for no of observation
  ratingTitle: string; // Variable for rating title
  periodTitle: string; // Variable for period title
  selectReviewer: string; // Variable for select reviewer
  addToolTip: string; // Variable for add tooltip
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation
  emailLabel: string; // Variable for email label
  removeToolTip: string; // Variable for remove tooltip
  attachmentText: string; // Variable for attachment Text
  saveButtonText: string; // Variable for save button
  requiredMessage: string;
  maxLengthExceedMessage: string;
  reportTitleLabel: string;
  operationType: string;
  flag: boolean;
  acmId: string;

  teamMasterLabel: string; // Variable for team master label

  event: AgGridEvent;
  public gridApi;
  public gridColumnApi;
  public rowData = [] as Array<ACMPresentationAC>;

  reportId: string;

  acmReportDetails = {} as ACMReportDetailAC;
  statusList = [] as Array<KeyValuePairOfIntegerAndString>;
  stageList = [] as Array<KeyValuePairOfIntegerAndString>;
  userList = [] as Array<EntityUserMappingAC>;
  reviewerList = [] as Array<ACMReviewerAC>;
  reviewer = {} as ACMReviewerAC;
  ratingList = [] as Array<RatingAC>;

  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  acmReportList = [] as Array<ACMPresentationAC>;
  acmReportDetailsList = [] as Array<ACMReportDetailAC>;
  newACMReportList = [] as Array<ACMPresentationAC>;

  selectedacmReportList = [] as Array<ReportAC>;
  getAllAcmReportList = [] as Array<ReportAC>;
  selectedEntityId;
  baseUrl: string;
  selectedReviewer: string;

  selectedReportIds = [] as Array<string>;
  selectedStatus: string;
  selectedStage: string;
  selectedReviewerName: string;
  reviewerNameList: ACMReportUserMappingAC[] = [];
  isChecked: boolean;
  acmReportMappingData = [] as Array<ACMReportMappingAC>;
  showingText: string;
  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              private modalService: BsModalService,
              private acmSharedService: AcmReportSharedService,
              private uploadService: UploadService,
              private acmService: AcmService,
              private acmReportService: ACMReportsService,
              public router: Router,
              private route: ActivatedRoute,
              private loaderService: LoaderService,
              private sharedService: SharedService) {
    this.generateAcmReportTitle = this.stringConstants.generateAcmReportTitle;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.backToolTip = this.stringConstants.backToolTip;
    this.statusTitle = this.stringConstants.statusTitle;
    this.versionLabel = this.stringConstants.versionLabel;
    this.stageTitle = this.stringConstants.stageTitle;
    this.reportTitle = this.stringConstants.reportTitle;
    this.reportListTitle = this.stringConstants.reportListTitle;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.processLabel = this.stringConstants.processLabel;
    this.noOfObservationTitle = this.stringConstants.noOfObservationTitle;
    this.ratingTitle = this.stringConstants.ratingTitle;
    this.periodTitle = this.stringConstants.periodTitle;
    this.selectReviewer = this.stringConstants.selectReviewer;
    this.addToolTip = this.stringConstants.addToolTip;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.attachmentText = this.stringConstants.attachmentText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.reportTitleLabel = this.stringConstants.reportTitle;

    this.showingResults = this.stringConstants.showingResults;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.showingText = this.stringConstants.showingText;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.teamMasterLabel = this.stringConstants.teamMasterLabel;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.isChecked = false;
    this.acmReportDetails.acmReportTitle = '';
    this.deleteTitle = this.stringConstants.deleteTitle;
  }

  /**
   * Add and update Report data
   */
  formData: FormData = new FormData();
  reviewerDocumentList = [] as Array<ReviewerDocumentAC>;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  statusItems = [{
    value: ACMReportStatus.NUMBER_0, label: 'Initial'
  },
    { value: ACMReportStatus.NUMBER_1, label: 'Pending' },
    { value: ACMReportStatus.NUMBER_2, label: 'Complete' }
  ];

  stageItems = [
    { value: ACMReportStatus.NUMBER_0, label: 'Draft' },
    { value: ACMReportStatus.NUMBER_1, label: 'Final' },
  ];
  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.selectedEntityId = entityId;

      this.route.params.subscribe(params => {
        if (params.pageItems !== undefined) {
          this.acmId = params.id;
          this.selectedPageItem = Number(params.pageItems);
        }
        this.operationType = params.type;
        this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
      });
      if (this.acmId !== '0') {
        this.getACMReportById(this.acmId, this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
      } else {

        this.getACMReportById(this.acmId, this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber: number, isPagination: boolean) {
    this.getReports(pageNumber, this.selectedPageItem, this.searchValue, this.acmReportDetails.acmReportStatusToString, this.acmReportDetails.acmReportStageToString, isPagination);
  }

  /**
   * On per page items change
   */
  onPageItemChange(isPagination: boolean) {
    this.getReports(null, this.selectedPageItem, this.searchValue, this.acmReportDetails.acmReportStatusToString, this.acmReportDetails.acmReportStageToString, isPagination);
  }

  /**
   * Method for opening report edit page
   * @param reportId:Id of report
   */
  editReport(reportId: string) {
    this.router.navigate(['report/add', { id: reportId, pageItems: this.selectedPageItem, searchValue: this.searchValue, type: this.stringConstants.editOperationText }]);
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
            this.getAllAcmReportList.splice(this.getAllAcmReportList.indexOf(this.getAllAcmReportList.filter(x => x.id === reportId)[0]), 1);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
          }
        }
      }
    });
  }

  /**
   * Open observation list page
   * @param reportId: current report id
   */
  openObservationList(reportId: string) {
    this.router.navigate(['report/observation-list', { id: reportId, type: this.stringConstants.editOperationText }]);
  }

  /**
   * On checkbox event of selected reports
   * @param reportId: report id
   */
  onSelectedRowData(event, reportId: string) {
    if (event.target.checked) {
      this.selectedReportIds.push(reportId);
    } else {
      this.selectedReportIds = this.selectedReportIds.filter(k => k !== reportId);
      const getUnselectedReportList = this.getAllAcmReportList.filter(x => x.id === reportId);
      for (const report of getUnselectedReportList) {
        report.isChecked = false;
      }
    }
  }
  /***
   * Get Reports Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   * @param selectedStatus: selected acm report status
   * @param selectedStage:selected acm stage
   */
  getReports(pagenumber: number, selectedPageItem: number, searchvalue: string, selectedStatus: string, selectedStage: string, isPagination: boolean) {
    if (this.stageList === null || this.statusList === null) {
      this.sharedService.showError(this.stringConstants.showNoDataText);
    } else {
      this.acmReportService.aCMReportsGetACMAllReportsByStatusAndStag(pagenumber, selectedPageItem, searchvalue, this.selectedEntityId, selectedStatus, selectedStage).subscribe((result: Pagination<ReportAC>) => {
        if (this.getAllAcmReportList.length === 0 || this.getAllAcmReportList === undefined) {
          for (const row of result.items) {
            this.getAllAcmReportList.push(row);
          }
        } else {
          // compare two list and remove repeated data
          const newAcmReportList = result.items;
          const AcmReportListInGrid = this.getAllAcmReportList;
          const notMatchedReports = result.items !== null ? newAcmReportList.filter(function(o1) {
            // filter out (!) items in result2
            return !AcmReportListInGrid.some(function(o2) {
              return o1.id === o2.id; // assumes unique id
            });
          }).map(function(o) {
            return o;
          }) : [] as Array<ReportAC>;
          if (notMatchedReports.length === 0 && result.items.length > 0 && !isPagination) {
            this.sharedService.showError(this.stringConstants.recordExistMsg);
          } else if ((result.items === null || result.items.length === 0) && !isPagination ) {
            this.sharedService.showError(this.stringConstants.noRecordFoundMessage);
          } else {
            for (const row of notMatchedReports) {
              this.getAllAcmReportList.push(row);
            }
          }
        }

        this.pageNumber = result.pageIndex;
        this.totalRecords = this.getAllAcmReportList.length;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
        this.acmReportDetails.acmReportStageToString = '';
        this.acmReportDetails.acmReportStatusToString = '';
      });
    }
  }

  /**
   * Method for setting status
   */
  setStatus() {
    for (const status of this.getAllAcmReportList) {
      if (this.statusItems[ACMReportStatus.NUMBER_0].label === this.acmReportDetails.acmReportStatusToString) {
      this.acmReportDetails.acmReportStatus = ACMReportStatus.NUMBER_0;
      status.status = this.statusItems[ACMReportStatus.NUMBER_0].label;
      } else if (this.statusItems[ACMReportStatus.NUMBER_1].label === this.acmReportDetails.acmReportStatusToString) {
      this.acmReportDetails.acmReportStatus = ACMReportStatus.NUMBER_1;
      status.status = this.statusItems[ACMReportStatus.NUMBER_1].label;
        } else {
      this.acmReportDetails.acmReportStatus = ACMReportStatus.NUMBER_2;
      status.status = this.statusItems[ACMReportStatus.NUMBER_2].label;
      }
    }
  }

  /**
   * Method for setting stage
   */
  setStage() {
    for (const stage of this.getAllAcmReportList) {
      if (this.stageItems[ReportStage.NUMBER_0].label === this.acmReportDetails.acmReportStageToString) {
        this.acmReportDetails.acmReportStage = ReportStage.NUMBER_0;
        stage.stageName = this.stageItems[ReportStage.NUMBER_0].label;
      } else if (this.statusItems[ReportStage.NUMBER_1].label === this.acmReportDetails.acmReportStageToString) {
        this.acmReportDetails.acmReportStage = ReportStage.NUMBER_1;
        stage.stageName = this.stageItems[ReportStage.NUMBER_1].label;
      }
    }
  }

  /**
   * Get Report detail by id for edit
   * @param acmId : Id of acm
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   * @param entityId :selected entityId
   */
  getACMReportById(acmId: string, pageNumber: number, selectedPageItem: number, searchValue: string, entityId: string) {
    if (this.stageList === null || this.statusList === null) {
      this.sharedService.showError(this.stringConstants.showNoDataText);
    } else {
      this.acmReportService.aCMReportsGetACMReportsAndReviewers(acmId, pageNumber, selectedPageItem, searchValue, entityId).subscribe(result => {
        this.acmReportDetails = result;

        this.getAllAcmReportList = result.linkedACMReportList !== null ? result.linkedACMReportList : [] as Array<ReportAC>;

        this.pageNumber = result.pageIndex;
        this.totalRecords = this.getAllAcmReportList.length;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.selectedPageItem, this.totalRecords);
        this.stageList = this.acmReportDetails.stageList;
        this.statusList = this.acmReportDetails.statusList;
        this.userList = this.acmReportDetails.userList;
        this.reviewerList = this.acmReportDetails.acmReportReviewerList;
        // set reviewer list
        this.acmSharedService.setReviewerDocumentList(this.reviewerList);
      });
    }
  }

  /**
   * Add user in reviewer list
   * @param userId: selected user id
   */
  addReviewer(userId: string) {
    if (userId === undefined || userId === '') {
      return;
    }
    const addedUser = this.reviewerList.find(a => a.userId === userId);
    if (addedUser !== undefined) {
      this.sharedService.showError(this.stringConstants.userExistMsg);
      return;
    }
    const userDetail = this.userList.find(x => x.userId === userId);
    this.reviewer = {} as ACMReviewerAC;
    this.reviewer.userId = userDetail.userId;
    this.reviewer.name = userDetail.name;
    this.reviewer.designation = userDetail.designation;
    this.reviewer.status = ReviewStatus.NUMBER_0;
    this.reviewerList.push(this.reviewer);
    this.reviewerList = [...this.reviewerList];
    this.selectedReviewer = '';
    // set reviewer list
    this.acmSharedService.setReviewerDocumentList(this.reviewerList);
  }

  /**
   * Delete user from reviewer list
   * @param userId: deleted user id
   */
  deleteReviewer(userId: string) {
    const userDetailIndex = this.reviewerList.findIndex(x => x.userId === userId);
    this.reviewerList.splice(userDetailIndex, 1);
    this.selectedReviewer = '';
  }

  /**
   * Open upload document modal
   * @param userId: selected reviewer user id
   */ // needs work
  openUploadFileModal(userId: string) {
    const reviewerData = this.acmSharedService.allReviewerDocumentList.find(a => a.userId === userId);
    let reviewerDocumentData;

    if (reviewerData.reviewerDocumentList !== undefined && reviewerData.reviewerDocumentList.length !== 0) {
      reviewerDocumentData = JSON.parse(JSON.stringify(reviewerData.reviewerDocumentList));
    }
    if (reviewerDocumentData === undefined) {
      reviewerDocumentData = [];
    }
    const initialState = {
      title: this.stringConstants.uploadFile,
      keyboard: true,
      userId,
      reviewerDocument: {} as ACMReviewerDocumentAC,
      reviewerDocumentObjList: reviewerDocumentData
    };
    this.bsModalRef = this.modalService.show(AcmReportUploadFilesComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add upload-file-dialog' }));
  }

  /**
   * Save report list and reviewers
   */
  saveReportListAndReviewer() {

    this.acmReportDetails.acmReportReviewerList = this.reviewerList;
    this.acmReportDetails.entityId = this.selectedEntityId;
    this.acmReportDetails.acmId = this.acmId;
    const selectedReportList = this.getAllAcmReportList.filter(x => x.isChecked === true);
    this.acmReportDetails.linkedACMReportList = this.getAllAcmReportList.filter(x => this.selectedReportIds.includes(x.id));
    for (const selectedReport of selectedReportList) {
      this.acmReportDetails.linkedACMReportList.push(selectedReport);
      selectedReport.isChecked = true;
    }

    this.acmReportService.aCMReportsAddAcmReport(this.acmReportDetails, this.selectedEntityId).subscribe(result => {
        this.acmReportDetails = result;
        // call upload file
        if (this.acmReportDetails.id === null || this.acmReportDetails.id === undefined || this.acmReportDetails.id === '0') {
          this.uploadReviewerDocument(this.stringConstants.recordAddedMsg);
        } else {
          this.uploadReviewerDocument(this.stringConstants.recordUpdatedMsg);
        }
        this.router.navigate(['acm/list']);

      }, (error) => {
        this.sharedService.handleError(error);
      });



  }

  /**
   * Upload reviewer document
   * @param successMsg: toaster message
   */

  uploadReviewerDocument(successMsg: string) {
    // call upload file
    for (const row of this.acmReportDetails.acmReportReviewerList) {
      const userId = row.userId;
      const reportUserMappingId = row.id;
      const uploadedFiles = [] as Array<File>;
      const reviewerDocuments = this.acmSharedService.reviewerDocumentList.filter(a => a.userId === userId);
      if (reviewerDocuments !== null && reviewerDocuments !== undefined) {
        for (const rowFiles of reviewerDocuments) {
          uploadedFiles.push(rowFiles.uploadedFile as File);
        }
      }
      for (const rowFiles of uploadedFiles) {
        this.formData.append(reportUserMappingId, rowFiles);
        this.formData.append(this.stringConstants.selectedEntityKey, this.selectedEntityId);

      }
    }
    this.uploadService.uploadFileOnReport(this.formData, '/api/ACMReports/add-acm-reviewer-documents')
      .subscribe(() => {
        // clear reviewer document list after save and next
        this.acmSharedService.uploadReviewerDocuments([], '');
        this.sharedService.showSuccess(successMsg);
      },
        (error) => {
          this.sharedService.handleError(error);
        });
  }

  /**
   * Open team master
   */
  openTeamMaster() {
    this.loaderService.open();
    this.router.navigate(['audit-team/list', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.selectedEntityId }]);
    this.loaderService.close();
  }

  /**
   * Method for back button
   */
  backToAcmList() {
    this.loaderService.open();
    this.router.navigate(['acm/list', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.selectedEntityId }]);
    this.loaderService.close();
  }
}
