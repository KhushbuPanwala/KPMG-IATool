import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { ReportObservationAC, ReportDetailAC, ReportUserMappingAC, ReportObservationsService, ReportObservationReviewerAC, ReportObservationsDocumentAC, ReportObservationTableAC } from '../../../../swaggerapi/AngularFiles';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { EditorDialogComponent } from '../../../../shared/editor-dialog/editor-dialog.component';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { SharedService } from '../../../../core/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../../../core/loader.service';
import { ReportSharedService } from '../../report-shared.service';
import { BulkUpload } from '../../../../shared/bulk-upload';
import { UploadService } from '../../../../core/upload.service';
import { DocumentUpload } from '../../../../shared/document-upload';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-reveiwer-comments',
  templateUrl: './report-reveiwer-comments.component.html'
})
export class ReportReveiwerCommentsComponent implements OnInit, OnDestroy {

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private sharedService: SharedService,
    private reportService: ReportSharedService,
    private apiService: ReportObservationsService,
    private router: Router,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private uploadService: UploadService
  ) {
    this.selectReviewerTitle = this.stringConstants.selectReviewer;
    this.commentsTitle = this.stringConstants.commentsTitle;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.saveCloseButtonText = this.stringConstants.saveCloseButtonText;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
  }
  selectReviewerTitle: string; // Variable for select reveiwer
  commentsTitle: string; // Variable for comments title
  bsModalRef: BsModalRef; // Modal ref variable

  reportDetailAC = {} as ReportDetailAC;
  reportObservation = {} as ReportObservationAC;
  reportReviewerList = [] as Array<ReportUserMappingAC>;
  reportObservationsReviewerList = [] as Array<ReportObservationReviewerAC>;
  reportObservationsReviewer = {} as ReportObservationReviewerAC;
  observationComment = '';
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  reportId: string;

  selectedReviewer;
  saveNextButtonText: string;
  saveCloseButtonText: string;
  isViewObservation = false;
  reportObservationId;
  isShowSaveNext;
  isDisableSaveButton = false;
  dropdownDefaultValue: string;

  // input output for paging
  @Output() pagingToEmit = new EventEmitter<string>();

  // HTML Editor
  config: AngularEditorConfig = {};

  /**
   * Upload report observation document
   * * @param callFrom: is call from save and next or save and close
   */
  uploadDocument = {} as DocumentUpload;

  /**
   * Set report observation data for save
   */
  addedTableList = [] as Array<ReportObservationTableAC>;
  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.loaderService.open();
        this.route.params.subscribe(params => {
          this.reportObservationId = params.reportObservationId;
        });
        if (this.reportObservationId === undefined) {
          this.isShowSaveNext = true;
        }
        this.reportService.reportObservationSubject.subscribe((reportDetailAC) => {
          this.reportObservation = this.reportService.reportObservation;
          this.reportReviewerList = JSON.parse(JSON.stringify(this.reportService.observationReviewerList));
          this.loaderService.close();
        });
        this.reportService.selectedObservationSubject.subscribe((reportDetailAC) => {
          this.reportObservation = this.reportService.reportObservation;
          this.loaderService.close();
        });
        this.reportService.selectedOperationTypeSubject.subscribe((operationType) => {
          this.isViewObservation = this.reportService.isViewObservation;
          this.loaderService.close();
        });
      }
    });

    // HTML Editor
    this.config = {
      editable: !this.isViewObservation,
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
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /**
   * Set reviewer comment
   * @param userId: selected reviewer
   */
  showReviewerComment(userId: string) {
    if (this.reportObservation.observationReviewerList.length === 0) {
      this.reportObservationsReviewer.reportObservationId = this.reportObservation.id;
      this.reportObservationsReviewer.userId = userId;
      this.reportObservationsReviewer.comment = '';
      this.reportObservation.observationReviewerList.push(this.reportObservationsReviewer);
    } else {
      if (this.reportObservationsReviewer.userId !== userId) {
        const reportObservation = this.reportObservation.observationReviewerList.find(a => a.userId === this.reportObservationsReviewer.userId);
        if (reportObservation !== undefined) {
          this.reportObservation.observationReviewerList.find(a => a.userId === this.reportObservationsReviewer.userId).comment = this.reportObservationsReviewer.comment;
        }
        const reviewer = this.reportObservation.observationReviewerList.find(a => a.userId === userId);
        if (reviewer !== undefined) {
          this.reportObservationsReviewer = JSON.parse(JSON.stringify(reviewer));
        } else {
          this.reportObservationsReviewer = {} as ReportObservationReviewerAC;
          this.reportObservationsReviewer.reportObservationId = this.reportObservation.id;
          this.reportObservationsReviewer.userId = userId;
          this.reportObservationsReviewer.comment = '';
          this.reportObservation.observationReviewerList.push(this.reportObservationsReviewer);
        }
      } else {
        const reviewer = this.reportObservation.observationReviewerList.find(a => a.userId === userId);
        if (reviewer !== undefined) {
          this.reportObservationsReviewer = JSON.parse(JSON.stringify(reviewer));
        }
      }
    }
  }

  /**
   * Open Observation Comment editor dialog
   */
  openObservationCommentModal() {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.commentsTitle,
        keyboard: true,
        data: this.reportObservationsReviewer.comment,
        callback: (result) => {
          this.reportObservationsReviewer.comment = result;
        }
      }
    });
  }

  /**
   * Set report observation data
   */
  setReportObservationData() {
    this.reportDetailAC.entityId = this.selectedEntityId;
    this.reportDetailAC.reportObservationList = [] as Array<ReportObservationAC>;
    this.addedTableList = this.reportObservation.reportObservationTableList;
    this.reportObservation.reportObservationTableList = [];
    this.reportDetailAC.reportObservationList.push(this.reportObservation);
    if (this.reportReviewerList.length !== 0 && this.reportObservation.observationReviewerList.length !== 0 && this.reportObservationsReviewer.userId !== undefined) {
      this.reportObservation.observationReviewerList.find(a => a.userId === this.reportObservationsReviewer.userId).comment = this.reportObservationsReviewer.comment;
      for (const row of this.reportObservation.observationReviewerList) {
        if (row.comment === '') {
          const index = this.reportObservation.observationReviewerList.findIndex(a => a.userId === row.userId);
          this.reportObservation.observationReviewerList.splice(index, 1);
        }
      }
    }
    this.checkRequiredValidation();
  }


  /**
   * Check reuired validation and disable save button
   */
  checkRequiredValidation() {
    this.isDisableSaveButton = false;
    if (this.reportObservation.heading === null || this.reportObservation.heading === '' ||
      this.reportObservation.heading.length > Number(this.stringConstants.maxCharecterAllowed)) {
      this.isDisableSaveButton = true;
    }
    if (this.reportObservation.targetDate === null || this.reportObservation.targetDate.toString() === this.stringConstants.invalidDate) {
      this.isDisableSaveButton = true;
    }
    if (this.reportObservation.ratingId === null) {
      this.isDisableSaveButton = true;
    }
  }

  /**
   * Set report observation data for save
   */
  saveNextObservation() {
    this.setReportObservationData();
    if (!this.isDisableSaveButton) {
      this.apiService.reportObservationsUpdateReportObservation(this.reportDetailAC, this.selectedEntityId).subscribe(result => {
        this.loaderService.open();
        // update observation list
        this.uploadReportObservationDocument(true);
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.invalidDataMsg);
    }
  }
  /**
   * Save report observation
   */
  saveCloseObservation() {
    this.setReportObservationData();
    if (!this.isDisableSaveButton) {
      this.apiService.reportObservationsUpdateReportObservation(this.reportDetailAC, this.selectedEntityId).subscribe(result => {
        this.loaderService.open();
        this.uploadReportObservationDocument(false);

      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      this.sharedService.showError(this.stringConstants.invalidDataMsg);
    }
  }
  /**
   * Upload report observation document
   * @param callFrom: define call from save and next or from save and close
   */
  uploadReportObservationDocument(callFrom: boolean) {
    // call upload file
    this.uploadDocument.reportObservationId = this.reportObservation.id;
    const observationFiles = this.reportService.observationFiles;
    this.uploadService.uploadFileOnReportObservationDocument<DocumentUpload>(this.uploadDocument, observationFiles, 'Files', '/api/ReportObservations/add-report-Observation-documents')
      .subscribe((result) => {
        if (callFrom) {
          const reportObservation = this.reportService.reportObservation;
          reportObservation.reportObservationDocumentList = result;

          this.reportObservation.reportObservationTableList = this.addedTableList;
          this.reportService.updateObservationList(this.reportObservation);

          this.reportService.uploadObservationDocumentsAfterSave(this.reportObservation.reportObservationDocumentList, this.reportObservation.id);

          // update observation data and linked observation data
          this.reportService.updateLinkedObservationList(this.reportObservation);

          // set next observation selected
          const currentIndex = this.reportService.reportObservationList.indexOf(this.reportObservation);
          const nextIndex = (currentIndex + 1) % this.reportService.reportObservationList.length;
          this.reportObservation = JSON.parse(JSON.stringify(this.reportService.reportObservationList[nextIndex]));
          this.reportService.updateObservation(this.reportObservation.observationId);

          this.reportService.selectedObservationSubject.subscribe((reportDetailAC) => {
            this.reportObservation = JSON.parse(JSON.stringify(this.reportService.reportObservation));
            this.loaderService.close();
          });
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          // call parent element paging method
          this.pagingToEmit.emit();
          this.loaderService.close();
        } else {
          this.loaderService.close();
          this.reportService.uploadObservationDocumentsAfterSave(this.reportObservation.reportObservationDocumentList, this.reportObservation.id);
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          this.router.navigate(['report/list']);
        }
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

}
