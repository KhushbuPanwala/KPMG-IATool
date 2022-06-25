import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { ObservationAC, AuditPlanAC, ProcessAC, AuditProcessesService, ObservationsManagementService, ObservationCategoryAC, KeyValuePairOfIntegerAndString, AuditSubProcessesService, PlanProcessMappingAC, WorkProgramsService } from '../../swaggerapi/AngularFiles';
import { Subscription } from 'rxjs';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { EditorDialogComponent } from '../../shared/editor-dialog/editor-dialog.component';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { NgForm } from '@angular/forms';
import { AddTableDialogComponent } from '../../shared/add-table-dialog/add-table-dialog.component';
import { ObservationService } from '../observation.service';
import { ObservationType } from '../../swaggerapi/AngularFiles/model/observationType';
import { UploadFileDialogComponent } from '../../shared/upload-file-dialog/upload-file-dialog.component';
import { ObservationUploadFilesComponent } from '../observation-upload-files/observation-upload-files.component';
@Component({
  selector: 'app-observation-tab',
  templateUrl: './observation-tab.component.html',
})
export class ObservationTabComponent implements OnInit {
  headingLabel: string; // Variable for heading label
  backgroundLabel: string; // Variable for background label
  observationTabTitle: string; // Variable for Observation label
  rootCauseLabel: string; // Variable for rootcause
  auditPlanLabel: string;
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
  @Output() addedObservationId = new EventEmitter<string>();
  observationId: string;
  selectedPageItem: number;
  searchValue: string;
  observationAC = {} as ObservationAC;
  auditPlanList: AuditPlanAC[] = [];
  processList: ProcessAC[] = [];
  processAndSubProcessList: ProcessAC[] = [];
  isAuditplanChanged: boolean;
  auditableEntityId: string;
  dropdownDefaultValue: string;
  processDropdownDefaultValue: string;
  unsubscribe: Subscription;
  observationACList: Array<ObservationAC> = [];
  selectedCategoryItems: string;
  bsModalRef: BsModalRef;
  subProcessList: Array<PlanProcessMappingAC> = [];
  headingRequiredMessage: string;
  maxLengthExceedMessage: string;
  backgroundRequiredMessage: string;
  observationRequiredMessage: string;
  rootCauseRequiredMessage: string;
  implicationRequiredMessage: string;
  recommendationRequiredMessage: string;
  auditPlanVersion: string;
  isParentProcessChanged: boolean;
  observationFiles: File[] = [];
  filesAdded: string;
  rcmId: string;
  workProgramId: string;
  // Sub process items for observation
  observationType = [
    {
      value: ObservationType.NUMBER_0,
      label: 'Legal',
    },
    {
      value: ObservationType.NUMBER_1,
      label: 'Compliance',
    },
    {
      value: ObservationType.NUMBER_2,
      label: 'Process',
    },
    {
      value: ObservationType.NUMBER_3,
      label: 'Financial',
    }
  ];


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



  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, private staticTabs: TabsetComponent, private route: ActivatedRoute,
              public router: Router, private sharedService: SharedService,
              private auditProcessService: AuditProcessesService, private observationApiService: ObservationsManagementService, private modalService: BsModalService,
              private service: ObservationService, private workProgramService: WorkProgramsService) {
    this.headingLabel = this.stringConstants.headingLabel;
    this.backgroundLabel = this.stringConstants.backgroundLabel;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.rootCauseLabel = this.stringConstants.rootCauseLabel;
    this.auditPlanLabel = this.stringConstants.auditPlanLabel;
    this.processLabel = this.stringConstants.processLabel;
    this.subProcessLabel = this.stringConstants.subProcessLabel;
    this.typeLabel = this.stringConstants.typeLabel;
    this.categoryLabel = this.stringConstants.categoryLabel;
    this.repeatObservationLabel = this.stringConstants.repeatObservationLabel;
    this.implicationTitle = this.stringConstants.implicationTitle;
    this.recommendationTitle = this.stringConstants.recommendationTitle;
    this.headingRequiredMessage = this.stringConstants.headingRequiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.backgroundRequiredMessage = this.stringConstants.backgroundRequiredMessage;
    this.observationRequiredMessage = this.stringConstants.observationRequiredMessage;
    this.rootCauseRequiredMessage = this.stringConstants.rootCauseRequiredMessage;
    this.implicationRequiredMessage = this.stringConstants.implicationRequiredMessage;
    this.recommendationRequiredMessage = this.stringConstants.recommendationRequiredMessage;
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
    this.addedLabel = this.stringConstants.addedLabel;
    this.addToolTip = this.stringConstants.addToolTip;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.processDropdownDefaultValue = this.stringConstants.processDropdownDefaultValue;
    this.auditPlanVersion = this.stringConstants.auditPlanVersion;
    this.selectedCategoryItems = '';
    this.observationId = '';
    this.filesAdded = this.stringConstants.filesAdded;
    this.rcmId = '';
    this.workProgramId = '';
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
      this.route.params.subscribe(params => {
        this.auditableEntityId = params.entityId;
        this.observationId = params.observationId;
        this.selectedPageItem = params.pageItems;
        this.searchValue = params.searchValue;
        this.rcmId = params.rcmId;
        this.workProgramId = params.workProgramId;
        if (this.observationId !== '0' && this.observationId !== undefined) {
          this.getObservationDetailById(this.observationId, this.auditableEntityId);
        } else {
          this.observationId = null;
          this.getObservationDetailById(this.observationId, this.auditableEntityId);
        }
      });
      this.service.observationSubject.subscribe((observationResultAC) => {
        this.observationAC = this.service.observationAC;
      });
      this.service.observationFilesSubject.subscribe((observationFiles) => {
        this.observationFiles = observationFiles;
      });
  }

  /**
   * Method for getting observation details
   * @param id : Id of observation
   * @param auditableEntityId : Id of auditable entity
   */
  getObservationDetailById(id: string, auditableEntityId: string) {
    this.observationApiService.observationsManagementGetObservationDetailsById(id, auditableEntityId).subscribe((observationResult: ObservationAC) => {
      this.observationAC = observationResult;
      this.observationAC.id = id;
      this.observationAC.riskAndControlId = this.rcmId;
      this.service.setObservations(observationResult);
      this.auditPlanList = JSON.parse(JSON.stringify(observationResult.auditPlanList));
      this.processList = JSON.parse(JSON.stringify(observationResult.parentProcessList));
      this.subProcessList = JSON.parse(JSON.stringify(observationResult.processList));
      this.observationAC.targetDate = this.sharedService.convertLocalDateToUTCDate(observationResult.targetDate, false);

      if (this.observationId === null || this.observationId === undefined || this.observationId === '0') {
        this.observationAC.observationCategoryId = this.observationAC.observationCategoryList[0].id;
        this.observationAC.personResponsible = this.observationAC.personResponsibleList[0].id;
        if (this.rcmId === '' || this.rcmId === undefined || this.rcmId === '0') {
          this.observationAC.auditPlanId = '';
          this.observationAC.processId = '';
          this.observationAC.parentProcessId = '';
        } else {
          this.workProgramService.workProgramsGetWorkProgramDetailsById(this.auditableEntityId, this.workProgramId ).subscribe(result => {
            this.auditPlanList = JSON.parse(JSON.stringify(result.auditPlanACList));
            this.processList = JSON.parse(JSON.stringify(result.processACList));
            this.subProcessList = JSON.parse(JSON.stringify(result.subProcessACList));
            this.observationAC.auditPlanId = result.auditPlanId;
            this.observationAC.processId = result.processId;
            this.observationAC.parentProcessId =  result.parentProcessId;
          });

        }
      }

    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Change event for audit plan dropdown
   * @param auditPlanId : selected audit plan id
   */
  onAuditPlanChange(auditPlanId: string) {
    this.isAuditplanChanged = true;
    this.processList = [];
    const planDetail = this.auditPlanList.find(a => a.id === auditPlanId);
    this.observationAC.auditPlanId = auditPlanId;
    if (this.isAuditplanChanged) {
      this.observationAC.processId = '';
      this.observationAC.parentProcessId = '';
    }
    this.processList = JSON.parse(JSON.stringify(planDetail.parentProcessList));
    this.subProcessList = [] as Array<PlanProcessMappingAC>;
  }

  /**
   * Method for making subprocess field blank on selection of process
   */
  onProcessChange(processId: string) {
    // To clear subprocess when process is changed so no other subprocess will be selected that is not child of process
    this.isParentProcessChanged = true;
    const planIndex = this.auditPlanList.findIndex(a => a.id === this.observationAC.auditPlanId);
    this.subProcessList = this.auditPlanList[planIndex].planProcessList.filter(a => a.parentProcessId === processId);
    if (this.isParentProcessChanged) {
      this.observationAC.processId = '';
    }
  }


  /**
   * Method to open Background editor modal
   */
  openBackgroundModal() {
    const initialState = {
      title: this.backgroundLabel,
      keyboard: true,
      data: this.observationAC.background,
      callback: (result) => {
        this.observationAC.background = result;
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
      data: this.observationAC.observations,
      callback: (result) => {
        this.observationAC.observations = result;
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
      data: this.observationAC.rootCause,
      callback: (result) => {
        this.observationAC.rootCause = result;
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
      data: this.observationAC.recommendation,
      callback: (result) => {
        this.observationAC.recommendation = result;
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
      data: this.observationAC.implication,
      callback: (result) => {
        this.observationAC.implication = result;
      }
    };
    this.bsModalRef = this.modalService.show(EditorDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  openUploadModal() {
    // ToDo: This code is commented for designer reference
    const initialState = {
      title: this.stringConstants.uploadFileTitle,
      keyboard: true,
      observationId: this.observationAC.id,
      entityId: this.auditableEntityId,
      callback: (result) => {
        this.service.setObservationFiles(result);
      }
    };
    this.bsModalRef = this.modalService.show(ObservationUploadFilesComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add upload-file-dialog' }));
  }
}
