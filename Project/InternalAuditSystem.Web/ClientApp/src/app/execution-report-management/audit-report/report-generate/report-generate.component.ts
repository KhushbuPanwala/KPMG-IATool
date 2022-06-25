import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ReportAC, ReportsService, RatingAC, EntityUserMappingAC, KeyValuePairOfIntegerAndString, ReviewStatus, ReviewerDocumentAC } from '../../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { EditorDialogComponent } from '../../../shared/editor-dialog/editor-dialog.component';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ReportUserMappingAC } from '../../../swaggerapi/AngularFiles/model/reportUserMappingAC';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { UploadReportFileDialogComponent } from '../upload-report-file-dialog/upload-report-file-dialog.component';
import { ReportSharedService } from '../report-shared.service';
import { UploadService } from '../../../core/upload.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-generate',
  templateUrl: './report-generate.component.html'
})
export class ReportGenerateComponent implements OnInit, OnDestroy {

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private apiService: ReportsService,
    private router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private reportSharedService: ReportSharedService,
    private uploadService: UploadService
  ) {
    this.generateReportTitle = this.stringConstants.generateReportTitle;
    this.reportTitleLabel = this.stringConstants.reportTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.auditPeriodEndDateLabel = this.stringConstants.auditPeriodEndDate;
    this.auditPeriodStartDateLabel = this.stringConstants.auditPeriodStartDate;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.statusLabel = this.stringConstants.statusTitle;
    this.stageLabel = this.stringConstants.stageTitle;
    this.commentsLabel = this.stringConstants.commentsTitle;
    this.textAreaPlaceHolder = this.stringConstants.textAreaPlaceHolder;
    this.selectReviewerLabel = this.stringConstants.selectReviewer;
    this.addToolTip = this.stringConstants.addToolTip;
    this.attachmentText = this.stringConstants.attachmentText;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.uploadFileLabel = this.stringConstants.uploadFileTitle;
    this.backgroundLabel = this.stringConstants.backgroundLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.teamMasterLabel = this.stringConstants.teamMasterLabel;
  }
  generateReportTitle: string; // Variable for generate report title
  backToolTip: string; // Vairable for back button tooltip
  powerPointToolTip: string; // Variable for Powerpoint alt text
  downloadToolTip: string; // Variable for download tooltip
  invalidMessage: string; // Variable for invalid message
  requiredMessage: string; // Variable for required message
  auditPeriodStartDateLabel: string; // Variable for audit period start date
  auditPeriodEndDateLabel: string; // Variable for audit period end date
  reportTitleLabel: string; // Variable for report title
  ratingLabel: string; // Variable for rating label
  statusLabel: string; // Variable for status
  stageLabel: string; // Varaible for stage title
  commentsLabel: string; // Variable for comments title
  textAreaPlaceHolder: string; // Variable for text area place holder
  selectReviewerLabel: string; // Variable for select reviewer
  addToolTip: string; // Variable for add tooltip
  nameLabel: string; // Variable for name title
  designationLabel: string; // Variable for name designation
  removeToolTip: string; // Variable for remove tooltip
  attachmentText: string; // Variable for attachment text
  saveNextButtonText: string; // Varible for save button next
  uploadFileLabel: string; // Variable for upload file title
  bsModalRef: BsModalRef; // Modal ref variable
  backgroundLabel: string; // Variable for background Label
  showNoDataText: string;
  report: ReportAC;
  reviewer = {} as ReportUserMappingAC;
  ratingList = [] as Array<RatingAC>;
  reportId: string;
  selectedPageItem: number;
  searchValue: string;
  operationType: string;
  statusList = [] as Array<KeyValuePairOfIntegerAndString>;
  stageList = [] as Array<KeyValuePairOfIntegerAndString>;
  reviewerStatusList = [] as Array<KeyValuePairOfIntegerAndString>;
  userList = [] as Array<EntityUserMappingAC>;
  reviewerList = [] as Array<ReportUserMappingAC>;
  selectedReviewer: string;
  selectedStatus: number;
  maxLengthExceedMessage: string;
  teamMasterLabel: string;
  selectedEntityId;

  // HTML Editor
  config: AngularEditorConfig = {
    editable: true,
    spellcheck: true,
    height: '15rem',
    minHeight: '5rem',
    placeholder: 'Enter text here...',
    translate: 'no',
    defaultParagraphSeparator: 'p',
    defaultFontName: 'Arial',
    toolbarHiddenButtons: [
      [
        'link',
        'unlink',
        'insertImage',
        'insertVideo',
        'toggleEditorMode',
        'undo',
        'redo',
        'removeFormat'
      ]
    ]
  };

  /**
   * Add and update Report data
   */
  formData: FormData = new FormData();
  reviewerDocumentList = [] as Array<ReviewerDocumentAC>;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
          this.operationType = params.type;
        });
        this.report = {} as ReportAC;
        this.getReportById();
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
   * Get Report detail by id for edit
   */
  getReportById() {
    const timeOffset = new Date().getTimezoneOffset();
    this.apiService.reportsGetReportById(this.reportId, this.selectedEntityId, timeOffset).subscribe(result => {
      this.report = result;
      this.ratingList = this.report.ratingsList;
      this.stageList = this.report.stageList;
      this.statusList = this.report.statusList;
      this.userList = this.report.userList;
      this.reviewerStatusList = this.report.reviewerStatus;
      this.reviewerList = this.report.reviewerList;
      if (this.reportId === '0') {
        // set default value for add report
        this.report.auditPeriodStartDate = new Date();
        this.report.auditPeriodEndDate = new Date();
        this.report.ratingId = this.ratingList[0].id;
      } else {
        this.report.auditPeriodStartDate = this.sharedService.convertLocalDateToUTCDate(result.auditPeriodStartDate, false);
        this.report.auditPeriodEndDate = this.sharedService.convertLocalDateToUTCDate(result.auditPeriodEndDate, false);
        // set reviewer list
        this.reportSharedService.setReviewerDocumentList(this.report.reviewerList);
      }
    }, (error) => {
      this.sharedService.handleError(error);
    });
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
    this.reviewer = {} as ReportUserMappingAC;
    this.reviewer.userId = userDetail.userId;
    this.reviewer.name = userDetail.name;
    this.reviewer.designation = userDetail.designation;
    this.reviewer.status = ReviewStatus.NUMBER_0;
    this.reviewerList.push(this.reviewer);
    this.selectedReviewer = '';
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
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['report/list']);
  }


  /**
   * Open upload document modal
   * @param userId: selected reviewer user id
   */
  openUploadFileModal(userId: string) {
    const reviewerData = this.reportSharedService.allReviewerDocumentList.find(a => a.userId === userId);
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
      reviewerDocument: {} as ReviewerDocumentAC,
      reviewerDocumentObjList: reviewerDocumentData
    };
    this.bsModalRef = this.modalService.show(UploadReportFileDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add upload-file-dialog' }));
  }

  /**
   * Method to comment editor modal
   */
  openCommentModal() {
    const initialState = {
      title: this.commentsLabel,
      keyboard: true,
      data: this.report.comment,
      callback: (result) => {
        this.report.comment = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Save report
   */
  saveReport() {
    this.report.ratingsList = null;
    this.report.statusList = null;
    this.report.stageList = null;
    this.report.reviewerList = this.reviewerList;
    this.report.entityId = this.selectedEntityId;
    if (this.reportId === '0') {
      this.apiService.reportsAddReport(this.report, this.selectedEntityId).subscribe(result => {
        this.report = result;
        // call upload file
        this.uploadReviewerDocument(this.stringConstants.recordAddedMsg);
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      this.apiService.reportsUpdateReport(this.report, this.selectedEntityId).subscribe(result => {
        this.report = result;
        // call upload file
        this.uploadReviewerDocument(this.stringConstants.recordUpdatedMsg);
      }, (error) => {
        this.sharedService.handleError(error);
      });
    }
  }

  /**
   * Upload reviewer document
   * @param successMsg: toaster message
   */

  uploadReviewerDocument(successMsg: string) {
    // call upload file
    for (const row of this.report.reviewerList) {
      const userId = row.userId;
      const reportUserMappingId = row.id;
      const uploadedFiles = [] as Array<File>;
      const reviewerDocuments = this.reportSharedService.reviewerDocumentList.filter(a => a.userId === userId);
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
    this.uploadService.uploadFileOnReport(this.formData, '/api/Reports/add-reviewer-documents')
      .subscribe(() => {
        // clear reviewer document list after save and next
        this.reportSharedService.uploadReviewerDocuments([], '');
        this.sharedService.showSuccess(successMsg);
        this.router.navigate(['report/distribution-add', { id: this.report.id, type: this.operationType }]);
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
    this.router.navigate(['audit-team/list']);
    this.loaderService.close();
  }
}
