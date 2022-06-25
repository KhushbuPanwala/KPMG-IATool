import { Component, OnInit, EventEmitter, Output, Input, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { WorkProgramRcmTabs } from './work-program-rcm-model';
import { LoaderService } from '../../../core/loader.service';
import { RiskControlMatrixesService } from '../../../swaggerapi/AngularFiles/api/riskControlMatrixes.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../core/shared.service';
import { RiskControlMatrixAC } from '../../../swaggerapi/AngularFiles/model/riskControlMatrixAC';
import { RiskControlMatrixProcessAC } from '../../../swaggerapi/AngularFiles/model/riskControlMatrixProcessAC';
import { RiskControlMatrixSubProcessAC } from '../../../swaggerapi/AngularFiles/model/riskControlMatrixSubProcessAC';
import { ControlCategory } from '../../../swaggerapi/AngularFiles/model/controlCategory';
import { ControlType } from '../../../swaggerapi/AngularFiles/model/controlType';
import { NatureOfControl } from '../../../swaggerapi/AngularFiles/model/natureOfControl';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { EditorDialogComponent } from '../../../shared/editor-dialog/editor-dialog.component';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-work-program-rcm-edit',
  templateUrl: './work-program-rcm-edit.component.html',
  styleUrls: ['./work-program-rcm-edit.component.css']
})

export class WorkProgramRcmEditComponent implements OnInit, OnDestroy {

  rcmProcessTitle: string; // Variable for rcm process title
  rcmSubProcessTitle: string; // Variable for rcm sub process title
  addToolTip: string; // Variable for add tooltip
  riskCategoryLabel: string; // Variable for risk category
  riskDescriptionLabel: string; // Variable for risk description
  controlObjectivelabel: string; // Variable for control objective
  controlCategoryLabel: string; // Variable for congtrol category
  controlDescriptionLabel: string; // Variable for conytrol description
  testStepsLabel: string; // Variable for test steps
  controlTypeLabel: string; // Variable for control type
  natureOfControlLabel: string; // Variable for nature of control
  antiFraudControlLabel: string; // Variable for anti fraud control
  samplingMethodology: string; // Variable for sampling methodology
  testResultsLabel: string; // Vatriable for test results
  addedLabel: string; // Variable for added label
  createObservationLabel: string; // Variable for create observation label
  bsModalRef: BsModalRef; // Modal ref variable
  backgroundLabel: string; // Variable for background Label
  cancelButtonText: string;
  id: string;
  saveButtonText: string;
  requiredMessage: string;
  rcmDetailList: RiskControlMatrixAC[] = [];
  isShowSampling = false; // Variable for showing sampling methodology
  isOnPageLoad: boolean;
  isAddtabActive: boolean;
  dropdownDefaultValue: string;
  workProgramId: string;
  sectorId: string;
  auditableEntityId: string;
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
  @Output() samplingList = new EventEmitter<boolean>();
  @Output() rcmList = new EventEmitter<boolean>();
  @Input() rcmId: string;
  rcmDetail: RiskControlMatrixAC;

  // Process list array
  processList: RiskControlMatrixProcessAC[];

  tabs: WorkProgramRcmTabs[] = [];
  // Sub process list array
  subProcessList: RiskControlMatrixSubProcessAC[];

  rcmDetails = {} as RiskControlMatrixAC;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Per page items for control categories
  controlCategories = [
    {
      value: ControlCategory.NUMBER_0,
      label: 'Strategic',
    },
    {
      value: ControlCategory.NUMBER_1,
      label: 'Operation'
    },
    {
      value: ControlCategory.NUMBER_2,
      label: 'Financial',
    },
    {
      value: ControlCategory.NUMBER_3,
      label: 'Compliance',
    },
  ];

  // Per page items for control type list
  controlTypeList = [
    {
      value: ControlType.NUMBER_0,
      label: 'Manual',
    },
    {
      value: ControlType.NUMBER_1,
      label: 'Automated',
    },
    {
      value: ControlType.NUMBER_2,
      label: 'SemiAutomated',
    },
  ];

  // Per page items for nature of control list
  natureOfControlList = [
    {
      value: NatureOfControl.NUMBER_0,
      label: 'Preventive',
    },
    {
      value: NatureOfControl.NUMBER_1,
      label: 'Detective',
    }
  ];

  // Per page items for fraud control list
  fraudControlList = [
    {
      value: true,
      label: 'Yes',
    },
    {
      value: false,
      label: 'No',
    },
  ];

  // Per page items for test Result list
  testResultList = [
    {
      value: true,
      label: 'Pass',
    },
    {
      value: false,
      label: 'Fail',
    },
  ];


  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private loaderService: LoaderService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private riskControlMatrixService: RiskControlMatrixesService,
    private modalService: BsModalService
  ) {
    this.rcmProcessTitle = this.stringConstants.rcmProcessTitle;
    this.rcmSubProcessTitle = this.stringConstants.rcmSubProcessTitle;
    this.addToolTip = this.stringConstants.addToolTip;
    this.riskCategoryLabel = this.stringConstants.riskCategory;
    this.riskDescriptionLabel = this.stringConstants.riskDescription;
    this.controlObjectivelabel = this.stringConstants.controlObjective;
    this.controlCategoryLabel = this.stringConstants.controlCategory;
    this.controlDescriptionLabel = this.stringConstants.controlDescription;
    this.testStepsLabel = this.stringConstants.testSteps;
    this.controlTypeLabel = this.stringConstants.controlType;
    this.natureOfControlLabel = this.stringConstants.natureOfControl;
    this.antiFraudControlLabel = this.stringConstants.antiFraudControl;
    this.samplingMethodology = this.stringConstants.samplingMethodology;
    this.testResultsLabel = this.stringConstants.testResults;
    this.addedLabel = this.stringConstants.addedLabel;
    this.createObservationLabel = this.stringConstants.createObservationLabel;
    this.cancelButtonText = this.stringConstants.cancelButtonText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.isShowSampling = false;
    this.processList = [];
    this.auditableEntityId = '';
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.auditableEntityId = entityId;
        // Show by default one tab.
        this.getRcmDetails(this.rcmId);
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
   * Method for add new rcm Tab
   * @param isOnPageLoad: Is loaded first time
   */
  addRCMTab(isOnPageLoad: boolean): void {
    this.loaderService.open();

    const newTabIndex = this.tabs.length + 1;

    if (isOnPageLoad) {
      this.tabs.push({
        id: this.rcmId,
        title: `Risk ${newTabIndex}`,
        content: ``,
        disabled: false,
        removable: true,
        active: true,
        rcmDetails: this.rcmDetails
      });
      this.loaderService.close();

    } else {
      this.tabs.push({
        id: '',
        title: `Risk ${newTabIndex}`,
        content: ``,
        disabled: false,
        removable: true,
        active: true,
        rcmDetails: {} as RiskControlMatrixAC,
      });
      setTimeout(() => {
        this.tabs[this.tabs.length - 1].active = true;
        this.loaderService.close();
      }, 10);
    }
  }

  /**
   * Method on save click
   * @param rcmDetails: rcm details
   */
  onSaveClick(rcmDetails: RiskControlMatrixAC) {
    this.loaderService.open();
    rcmDetails.rcmProcessId = this.rcmDetails.rcmProcessId;
    rcmDetails.rcmSubProcessId = this.rcmDetails.rcmSubProcessId;
    rcmDetails.riskControlMatrixSubProcessACList = [];
    rcmDetails.riskControlMatrixProcessACList = [];
    rcmDetails.entityId = this.auditableEntityId;
    if (rcmDetails.id !== undefined) {
      this.rcmDetailList = [];
      this.rcmDetailList.push(rcmDetails);
      this.riskControlMatrixService.riskControlMatrixesUpdateRiskControlMatrix(this.rcmDetailList, this.auditableEntityId).subscribe(() => {
        this.loaderService.close();
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.rcmList.emit(false);
      },
        (error) => {
          this.sharedService.handleError(error);
        });
    } else {
      rcmDetails.sectorId = this.sectorId;
      rcmDetails.workProgramId = this.workProgramId;
      this.riskControlMatrixService.riskControlMatrixesAddRiskControlMatrix(rcmDetails, this.auditableEntityId).subscribe(() => {
        this.loaderService.close();
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.rcmList.emit(false);
      },
        (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * Method on cancel click
   */
  onCancelClick() {
    this.rcmList.emit(false);
  }
  /**
   * Open Sampling Methodology content
   */
  openSampling() {
    this.isShowSampling = true;
  }

  /**
   * Method to add sampling method
   * @param id: rcm Id
   * @param samplingId: sampling Id
   */
  onSamplingAddClick(id: string, rcmDetail: RiskControlMatrixAC) {
    if (id !== undefined) {
      this.rcmDetail = rcmDetail;
      this.isShowSampling = true;
      this.samplingList.emit(true);
    } else {
      this.sharedService.showError(this.stringConstants.samplingAddMessage);
    }
  }

  /**
   * Passing boolean to sampling page to show edit page on previous click
   * @param isShowSample: is show sample boolean
   */
  isShowSample(isShowSample: boolean) {
    this.isShowSampling = isShowSample;

  }

  /**
   * Method to get rcm details
   * @param id: Rcm id
   */
  async getRcmDetails(id: string) {
    this.loaderService.open();
    this.riskControlMatrixService.riskControlMatrixesGetRiskControlMatrixDetailsById(id, this.auditableEntityId).subscribe(result => {

      this.processList = result.riskControlMatrixProcessACList;
      this.subProcessList = result.riskControlMatrixSubProcessACList;

      this.rcmDetails = result;
      this.sectorId = result.sectorId;
      this.workProgramId = result.workProgramId;
      this.addRCMTab(true);
      this.loaderService.close();
    },
      (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Method to comment editor modal
   * @param text: Text of editor
   * @param label: Label of text editor
   */
  openEditorModal(text: string, label: string, tabIndex: number) {
    this.modalService.config.class = 'page-modal audit-team-add upload-file-dialog';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.backgroundLabel,
        keyboard: true,
        data: text,
        callback: (result) => {
          if (label === this.riskDescriptionLabel) {
            this.tabs[tabIndex].rcmDetails.riskDescription = result;
          } else if (label === this.controlDescriptionLabel) {
            this.tabs[tabIndex].rcmDetails.controlDescription = result;
          } else if (label === this.controlObjectivelabel) {
            this.tabs[tabIndex].rcmDetails.controlObjective = result;
          } else if (label === this.riskCategoryLabel) {
            this.tabs[tabIndex].rcmDetails.riskCategory = result;
          } else if (label === this.testStepsLabel) {
            this.tabs[tabIndex].rcmDetails.testSteps = result;
          }
        }
      }
    });
  }

  /**
   * Method for opening observation add page
   * @param id:rcmId
   */
  openObservationAddPage(id: string) {
    if (id !== undefined) {
      this.router.navigate(['observation-management/add', { observationId: 0, pageItems: 0, searchValue: '', entityId: this.auditableEntityId, rcmId: id, workProgramId: this.workProgramId }]);
    } else {
      this.sharedService.showError(this.stringConstants.observationAddMessage);
    }
  }
}
