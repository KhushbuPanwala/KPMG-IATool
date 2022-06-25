import { Component, OnInit, AfterViewInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { FormControl, FormGroup } from '@angular/forms';
import { Time } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ObservationsManagementService, ObservationAC, EntityUserMappingAC, UserAC } from '../../swaggerapi/AngularFiles';
import { EditorDialogComponent } from '../../shared/editor-dialog/editor-dialog.component';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ObservationService } from '../observation.service';
import { SharedService } from '../../core/shared.service';
import { Disposition } from '../../swaggerapi/AngularFiles/model/disposition';
import { ObservationStatus } from '../../swaggerapi/AngularFiles/model/observationStatus';
import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../../app/core/loader.service';
import { UploadService } from '../../core/upload.service';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-management-comments',
  templateUrl: './management-comments.component.html',
  styleUrls: ['./management-comments.component.scss']

})
export class ManagementCommentsComponent implements OnInit, OnDestroy {
  managementResponseTitle: string; // Variable for management response section
  conclusionTitle: string; // Variable for conclusion title
  personResponsibleLabel: string; // Variable for person responsible title
  targetDateTitle: string; // Variable for target date title
  linkedObservationTitle: string; // Variable for linked observation title
  statusTitle: string; // Variable for status
  dispositionTitle: string; // Variable for disposition title
  auditorTitle: string; // Variable for auditor title
  nameLabel: string; // Variable for name
  emailLabel: string; // Variable for email
  designationTitle: string; // Variable for designation
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  personLabel: string; // Variable for person responsible
  reviewerCommentsTitle: string; // Variable reviewer comments title
  saveDraft: string; // VAriable for save as draft button
  saveButtonText: string; // Variable for save button
  bsModalRef: BsModalRef; // Modal ref variable
  observationAC = {} as ObservationAC;
  responsiblePersonList: Array<UserAC>;
  linkedObservations = [] as Array<ObservationAC>;
  observationACList: Array<ObservationAC> = [];
  dropdownDefaultValue: string;
  selectedResponsiblePerson: string;
  auditableEntityId: string;
  observationId: string;
  isDraft: boolean;
  selectedPageItem: number;
  searchValue: string;
  isValid: boolean;
  observationFiles = [] as Array<File>;
  observationWithFiles = {} as ObservationAC;

  // Per page items for status list
  observationStatusList = [
    {
      value: ObservationStatus.NUMBER_0, label: 'Open',
    },
    {
      value: ObservationStatus.NUMBER_1, label: 'Closed',
    },
    {
      value: ObservationStatus.NUMBER_2, label: 'Pending',
    }

  ];

  dispositionList = [
    { value: Disposition.NUMBER_0, label: 'Reportable' },
    { value: Disposition.NUMBER_1, label: 'NonReportable' },
  ];



  // Auditable entity date picker configurations
  currentDate = new Date();
  form = new FormGroup({
    dateYMD: new FormControl(new Date()),
    dateFull: new FormControl(new Date()),
    dateMDY: new FormControl(new Date()),
    dateRange: new FormControl([
      new Date(),
      new Date(this.currentDate.setDate(this.currentDate.getDate() + 7))
    ])
  });
  timepickerVisible = false; // Condition that hide timepicker
  mytime: Time; // Assign time
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  selectedEntityId: string;

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
  constructor(private stringConstants: StringConstants,
              private modalService: BsModalService,
              private observationApiService: ObservationsManagementService,
              private observationService: ObservationService,
              private sharedService: SharedService,
              private route: ActivatedRoute,
              private router: Router,
              private uploadSevice: UploadService) {
    this.managementResponseTitle = this.stringConstants.managementResponseTitle;
    this.conclusionTitle = this.stringConstants.conclusionTitle;
    this.personResponsibleLabel = this.stringConstants.personResponsibleLabel;
    this.targetDateTitle = this.stringConstants.targetDateTitle;
    this.linkedObservationTitle = this.stringConstants.linkedObservationTitle;
    this.statusTitle = this.stringConstants.statusTitle;
    this.dispositionTitle = this.stringConstants.dispositionTitle;
    this.auditorTitle = this.stringConstants.auditorTitle;
    this.nameLabel = this.stringConstants.nameLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.designationTitle = this.stringConstants.designationLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.personLabel = this.stringConstants.personLabel;
    this.reviewerCommentsTitle = this.stringConstants.reviewerCommentsTitle;
    this.saveDraft = this.stringConstants.saveDraft;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.linkedObservations = [];
    this.observationId = '';
    this.isValid = true;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(entityId => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          this.auditableEntityId = params.entityId;
          this.observationId = params.observationId;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        this.observationService.observationSubject.subscribe((observationResultAC) => {

          this.observationAC = this.observationService.observationAC;

          if (this.observationAC.id === null || this.observationAC.id === undefined || this.observationAC.id === '0') {
            this.observationAC.linkedObservation = this.observationAC.linkedObservationACList.length > 0 ? this.observationAC.linkedObservationACList[0].id : '';
            this.observationAC.targetDate = new Date();
            this.observationAC.dispositionToString = this.dispositionList[0].label;
            this.observationAC.observationStatusToString = this.observationStatusList[0].label;
          } else {
            this.setStatus();
            this.setDisposition();
          }

        });

        this.observationService.observationFilesSubject.subscribe(files => {
          this.observationFiles = files;
        });
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
   * Method to open Management Response editor modal
   */
  openManagementResponseModal() {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.conclusionTitle,
        keyboard: true,
        data: this.observationAC.managementResponse,
        callback: (result) => {
          this.observationAC.managementResponse = result;
        }
      }
    });
  }


  /**
   * Method to open Conclusion editor modal
   */
  openConclusionModal() {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.conclusionTitle,
        keyboard: true,
        data: this.observationAC.conclusion,
        callback: (result) => {
          this.observationAC.conclusion = result;
        }
      }
    });
  }

  /**
   * Method for setting status of observation
   */
  setStatus() {
    if (this.observationAC.observationStatusToString === this.observationStatusList[ObservationStatus.NUMBER_0].label) {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_0;
    } else if (this.observationAC.observationStatusToString === this.observationStatusList[ObservationStatus.NUMBER_1].label) {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_1;
    } else {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_2;
    }
  }

  /**
   * Method for setting disposition
   */
  setDisposition() {
    if (this.observationAC.dispositionToString === this.dispositionList[Disposition.NUMBER_0].label) {
      this.observationAC.disposition = Disposition.NUMBER_0;
    } else {
      this.observationAC.disposition = Disposition.NUMBER_1;
    }
  }
  /**
   * Method for saving observation
   * @param isDraft : Checks if observation is saved as draft or not
   */
  saveObservation(isDraft: boolean) {
    const isValidData = this.checkRequiredValidation();

    if (isValidData) {
      if (isDraft) {
        this.isDraft = isDraft;
        // show pending status if user click on draft  button
        this.observationAC.statusString = this.observationStatusList[ObservationStatus.NUMBER_2].label;
        this.observationAC.observationListStatus = ObservationStatus.NUMBER_2;
      } else {
        // show completed status if user click save button
        this.observationAC.statusString = this.stringConstants.completedStatusString;
        this.observationAC.observationListStatus = ObservationStatus.NUMBER_1;
      }
      this.setStatus();
      this.setDisposition();
      this.observationAC.entityId = this.auditableEntityId;
      if (this.observationId === undefined || this.observationId === '0' || this.observationId === null) {
        this.observationApiService.observationsManagementAddObservation(this.observationAC, this.selectedEntityId).subscribe(addedObservation => {
          this.observationAC.auditPlanId = addedObservation.auditPlanId;
          this.observationAC = addedObservation;
          this.uploadObservationFile(true);
          if (this.observationFiles.length === 0) {
            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
            this.router.navigate(['observation-management/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
          }
        }, (error) => {
          this.sharedService.handleError(error);
        });
      } else {
        this.observationApiService.observationsManagementUpdateObservation(this.observationAC, this.selectedEntityId).subscribe(updatedObservation => {
          this.uploadObservationFile(false);
          if (this.observationFiles.length === 0) {
            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
            this.router.navigate(['observation-management/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
          }
        }, (error) => {
          this.sharedService.handleError(error);
        });
      }
    }
  }

  /**
   * Method for checking validation
   */
  checkRequiredValidation() {
    this.isValid = false;
    if (this.observationAC.heading === null || this.observationAC.heading === '') {
      this.sharedService.showError(this.stringConstants.headingRequiredMessage);
    } else if (this.observationAC.background === null || this.observationAC.background === '') {
      this.sharedService.showError(this.stringConstants.backgroundRequiredMessage);
    } else if (this.observationAC.observations === null || this.observationAC.observations === '') {
      this.sharedService.showError(this.stringConstants.observationRequiredMessage);
    } else if (this.observationAC.rootCause === null || this.observationAC.rootCause === '') {
      this.sharedService.showError(this.stringConstants.rootCauseRequiredMessage);
    } else if (this.observationAC.implication === null || this.observationAC.implication === '') {
      this.sharedService.showError(this.stringConstants.implicationRequiredMessage);
    } else if (this.observationAC.recommendation === null || this.observationAC.recommendation === '') {
      this.sharedService.showError(this.stringConstants.recommendationRequiredMessage);
    } else if (this.observationAC.auditPlanId === null || this.observationAC.auditPlanId === '') {
      this.sharedService.showError(this.stringConstants.auditPlanRequiredMessage);
    } else if (this.observationAC.parentProcessId === null || this.observationAC.parentProcessId === '') {
      this.sharedService.showError(this.stringConstants.processRequiredMessage);
    } else if (this.observationAC.processId === null || this.observationAC.processId === '') {
      this.sharedService.showError(this.stringConstants.subProcessRequiredMessage);
    } else if (this.observationAC.observationStatusToString === null || this.observationAC.observationStatusToString === '') {
      this.sharedService.showError(this.stringConstants.observationStatusRequiredMessage);
    } else if (this.observationAC.dispositionToString === null || this.observationAC.dispositionToString === '') {
      this.sharedService.showError(this.stringConstants.observationDispositionRequiredMessage);
    } else if (this.observationAC.heading !== '' && this.observationAC.heading.length > Number(this.stringConstants.maxCharecterAllowed)) {
      this.sharedService.showError(this.stringConstants.headingmaxLengthExceedMessage);
    } else {
      this.isValid = true;
    }
    return this.isValid;
  }

  /**
   * Upload observation file
   */
  uploadObservationFile(isAdd: boolean) {
    if (this.observationFiles.length > 0) {
      this.observationWithFiles.id = this.observationAC.id;
      this.observationWithFiles.observationFiles = this.observationFiles;

      this.uploadSevice.uploadFileOnAdd<ObservationAC>(this.observationWithFiles, this.observationFiles, this.stringConstants.observationFiles, this.stringConstants.observationApiPath).subscribe(
        (result: ObservationAC) => {
          this.observationService.setObservationFiles(result.observationFiles as File[]);

          if (isAdd) {
            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
            this.router.navigate(['observation-management/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
          } else {
            this.observationApiService.observationsManagementUpdateObservation(this.observationAC, this.selectedEntityId).subscribe(updatedObservation => {
              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
              this.router.navigate(['observation-management/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
            }, (error) => {
              this.sharedService.handleError(error);
            });
          }
        },
        error => {
          this.sharedService.showError(this.stringConstants.somethingWentWrong);
        }
      );
    }
  }

}
