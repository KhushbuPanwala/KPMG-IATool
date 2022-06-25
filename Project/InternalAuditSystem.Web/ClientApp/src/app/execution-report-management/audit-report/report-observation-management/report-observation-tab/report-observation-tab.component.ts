import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ActivatedRoute } from '@angular/router';
import { SharedService } from '../../../../core/shared.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { EditorDialogComponent } from '../../../../shared/editor-dialog/editor-dialog.component';
import { ReportDetailAC, ReportObservationAC, RatingAC, KeyValuePairOfIntegerAndString, ObservationCategoryAC, ReportObservationsService } from '../../../../swaggerapi/AngularFiles';
import { NgForm } from '@angular/forms';
import { LoaderService } from '../../../../core/loader.service';
import { ReportSharedService } from '../../report-shared.service';
import { ReportAddTableDialogComponent } from '../report-add-table-dialog/report-add-table-dialog.component';
import { ReportObservationFileDialogComponent } from '../report-observation-file-dialog/report-observation-file-dialog.component';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-observation-tab',
  templateUrl: './report-observation-tab.component.html',
})

export class ReportObservationTabComponent implements OnInit, OnDestroy {
  @ViewChild('observationTabForm') ngForm: NgForm;
  headingLabel: string; // Variable for heading label
  backgroundLabel: string; // Variable for background label
  observationTabTitle: string; // Variable for Observation label
  rootCauseLabel: string; // Variable for rootcause label
  processLabel: string; // Variable for process label
  subProcessLabel: string; // Variable for sub process label
  typeLabel: string; // Vairable for type label
  categoryLabel: string; // Variable for category
  repeatObservationLabel: string; // Variable for repeat observation
  implicationTitle: string; // Variable for Implication
  recommendationTitle: string; // Variable for Recommendation
  yes: string; // Variable for Yes
  no: string; // Variable for No
  addToolTip: string; // Variable for add label
  addedLabel: string; // Variable for added label
  ratingLabel: string; // VAriable for rrating  label
  bsModalRef: BsModalRef; // Modal ref variable
  addTableTitle: string; // Variable for add table title
  redColor: string; // Variable for red color
  yellowColor: string; // Variable for yellow color
  greenColor: string; // Varibale for greenColor

  reportDetailAC = {} as ReportDetailAC;
  reportObservation = {} as ReportObservationAC;
  selectedEntityId: string;
  reportId: string;
  reportObservationId: string;
  operationType: string;
  selectedTypeItems;
  selectedRating;
  selectedCategoryItems;

  ratingList = [] as Array<RatingAC>;
  observationTypeList = [] as Array<KeyValuePairOfIntegerAndString>;
  observationCategoryList = [] as Array<ObservationCategoryAC>;
  requiredMessage: string;
  isViewObservation = false;
  maxLengthExceedMessage: string;
  dropdownDefaultValue: string;
  tableCount: number;
  fileCount: number;
  // HTML Editor
  config: AngularEditorConfig = {};
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private sharedService: SharedService, private reportService: ReportSharedService,
    private apiService: ReportObservationsService,
    private loaderService: LoaderService) {
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.headingLabel = this.stringConstants.headingLabel;
    this.backgroundLabel = this.stringConstants.backgroundLabel;
    this.rootCauseLabel = this.stringConstants.rootCauseLabel;
    this.processLabel = this.stringConstants.processLabel;
    this.subProcessLabel = this.stringConstants.subProcessLabel;
    this.typeLabel = this.stringConstants.typeLabel;
    this.categoryLabel = this.stringConstants.categoryLabel;
    this.repeatObservationLabel = this.stringConstants.repeatObservationLabel;
    this.implicationTitle = this.stringConstants.implicationTitle;
    this.recommendationTitle = this.stringConstants.recommendationTitle;
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
    this.addedLabel = this.stringConstants.addedLabel;
    this.addToolTip = this.stringConstants.addToolTip;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.addTableTitle = this.stringConstants.addTableTitle;
    this.redColor = this.stringConstants.redColor;
    this.yellowColor = this.stringConstants.yellowColor; // Variable for yellow color
    this.greenColor = this.stringConstants.greenColor; // Varibale for greenColor
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.reportObservationId = params.reportObservationId;
          this.operationType = params.type;
        });
        // get report observations
        this.getReportObservations();
        if (this.operationType === this.stringConstants.viewOperationText) {
          this.reportService.updateOperationType(true);
          this.isViewObservation = true;
        } else {
          this.reportService.updateOperationType(false);
          this.isViewObservation = false;
        }
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

    // set selected observation detail on save and next, pagination
    this.reportService.selectedObservationSubject.subscribe((reportDetailAC) => {
      this.reportObservation = this.reportService.reportObservation;
      if (this.reportObservation !== undefined) {
        this.tableCount = this.reportObservation.tableCount;
        this.fileCount = this.reportObservation.reportObservationDocumentList !== null ? this.reportObservation.reportObservationDocumentList.length : 0;
      }
      this.ratingList = JSON.parse(JSON.stringify(this.reportService.ratingList));
    });
    // set updated table count
    this.reportService.updateObservationTableCount.subscribe(() => {
      this.reportObservation = this.reportService.reportObservation;
      this.tableCount = this.reportObservation !== undefined ? this.reportObservation.tableCount : 0;
    });
    // set updated document count
    this.reportService.updateObservationFileCount.subscribe(() => {
      this.reportObservation = this.reportService.reportObservation;
      this.fileCount = this.reportObservation !== undefined ? this.reportObservation.fileCount : 0;
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
   * get report observation data
   */
  getReportObservations() {
    this.apiService.reportObservationsGetReportObservations(this.reportId, this.reportObservationId, this.selectedEntityId)
      .subscribe((reportDetailAC: ReportDetailAC) => {
        this.loaderService.open();
        this.reportService.setReportObservations(reportDetailAC);
        this.reportDetailAC = reportDetailAC;
        this.reportObservation = this.reportService.reportObservation;
        this.observationCategoryList = JSON.parse(JSON.stringify(this.reportService.observationCategoryList));
        this.ratingList = JSON.parse(JSON.stringify(this.reportService.ratingList));
        this.observationTypeList = JSON.parse(JSON.stringify(this.reportService.observationTypeList));
        this.tableCount = this.reportObservation.tableCount;
        this.fileCount = this.reportObservation.reportObservationDocumentList !== null ? this.reportObservation.reportObservationDocumentList.length : 0;
        this.loaderService.close();
      }, (error) => {
        this.sharedService.handleError(error);
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

  /**
   * Method to open Background editor modal
   */
  openBackgroundModal() {
    const initialState = {
      title: this.backgroundLabel,
      keyboard: true,
      data: this.reportObservation.background,
      callback: (result) => {
        this.reportObservation.background = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }


  /**
   * Method to open rootCause editor modal
   */
  openRootCauseModal() {
    const initialState = {
      title: this.rootCauseLabel,
      keyboard: true,
      data: this.reportObservation.rootCause,
      callback: (result) => {
        this.reportObservation.rootCause = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }


  /**
   * Method to open recommendation editor modal
   */
  openRecommendationModal() {
    const initialState = {
      title: this.recommendationTitle,
      keyboard: true,
      data: this.reportObservation.recommendation,
      callback: (result) => {
        this.reportObservation.recommendation = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }


  /**
   * Method to open implication editor modal
   */
  openImplicationModal() {
    const initialState = {
      title: this.implicationTitle,
      keyboard: true,
      data: this.reportObservation.implication,
      callback: (result) => {
        this.reportObservation.implication = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }


  /**
   * Method to open observation editor modal
   */
  openObservationsModal() {
    const initialState = {
      title: this.observationTabTitle,
      keyboard: true,
      data: this.reportObservation.observations,
      callback: (result) => {
        this.reportObservation.observations = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }


  /**
   * Method to open add table dialog
   */
  openAddTableDialog() {
    this.apiService.reportObservationsGetReportObservationTable(this.reportObservation.id, this.selectedEntityId).subscribe(
      result => {
        const initialState = {
          title: this.addTableTitle,
          keyboard: true,
          tableList: result,
          reportObservationId: this.reportObservation.id
        };
        this.bsModalRef = this.modalService.show(ReportAddTableDialogComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }



  /**
   * Open upload document modal
   */
  openAddFileDialog() {
    const initialState = {
      title: this.stringConstants.uploadFile,
      keyboard: true,
      reportObservationId: this.reportObservation.id
    };
    this.bsModalRef = this.modalService.show(ReportObservationFileDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add upload-file-dialog' }));
  }
}
