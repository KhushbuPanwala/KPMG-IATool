import { Component, OnInit, EventEmitter, Output, Input, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
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
import { RcmSectorAC } from '../../../swaggerapi/AngularFiles';
import { RcmTabs } from './rcm-model';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rcm-add',
  templateUrl: './rcm-add.component.html'
})

export class RcmAddComponent implements OnInit, OnDestroy {

  rcmId: string;
  rcmSectorTitle: string; // Variable for rcm sector title
  rcmProcessTitle: string; // Variable for rcm process title
  rcmSubProcessTitle: string; // Variable for rcm sub process title

  riskCategoryLabel: string; // Variable for risk category
  riskDescriptionLabel: string; // Variable for risk description
  controlObjectivelabel: string; // Variable for control objective
  controlDescriptionLabel: string; // Variable for conytrol description

  controlCategoryLabel: string; // Variable for congtrol category
  controlTypeLabel: string; // Variable for control type
  natureOfControlLabel: string; // Variable for nature of control
  antiFraudControlLabel: string; // Variable for anti fraud control

  excelToolTip: string; //  Variable for excel tooltip
  backToolTip: string; // Variable for back tooltip
  addToolTip: string; // Variable for add tooltip
  bsModalRef: BsModalRef; // Modal ref variable
  backgroundLabel: string; // Variable for background Label
  saveButtonText: string;
  requiredMessage: string;

  // only to subscripe for the current component
  entitySubscribe: Subscription;

  isOnPageLoad: boolean;
  isAddtabActive: boolean;
  dropdownDefaultValue: string;
  selectedEntityId;
  selectedPageItem: number;
  searchValue: string;
  sectorId: string;
  rcmTitle: string;

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

  tabs: RcmTabs[] = [];
  rcmDetailList: RiskControlMatrixAC[] = [];
  rcmDetails = {} as RiskControlMatrixAC;

  // Sector list array
  sectorList: RcmSectorAC[];
  // Process list array
  processList: RiskControlMatrixProcessAC[];
  // Sub process list array
  subProcessList: RiskControlMatrixSubProcessAC[];

  // Per page items for control categories
  controlCategories = [
    {
      value: ControlCategory.NUMBER_0,
      label: 'Strategic',
    },
    {
      value: ControlCategory.NUMBER_1,
      label: 'Operational'
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
    this.rcmSectorTitle = this.stringConstants.rcmSectorTitle;
    this.rcmProcessTitle = this.stringConstants.rcmProcessTitle;
    this.rcmSubProcessTitle = this.stringConstants.rcmSubProcessTitle;

    this.riskCategoryLabel = this.stringConstants.riskCategory;
    this.riskDescriptionLabel = this.stringConstants.riskDescription;
    this.controlObjectivelabel = this.stringConstants.controlObjective;
    this.controlDescriptionLabel = this.stringConstants.controlDescription;

    this.controlCategoryLabel = this.stringConstants.controlCategory;
    this.controlTypeLabel = this.stringConstants.controlType;
    this.natureOfControlLabel = this.stringConstants.natureOfControl;
    this.antiFraudControlLabel = this.stringConstants.antiFraudControl;

    this.backToolTip = this.stringConstants.backToolTip;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.rcmTitle = this.stringConstants.rcmTitle;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          this.rcmId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        this.getRcmById();
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
   * Get RCM Process detail by id for edit
   */
  getRcmById() {
    this.loaderService.open();
    this.riskControlMatrixService.riskControlMatrixesGetRiskControlMatrixDetailsById(this.rcmId, this.selectedEntityId).subscribe(result => {
      this.sectorList = result.riskControlMatrixSectorACList;
      this.processList = result.riskControlMatrixProcessACList;
      this.subProcessList = result.riskControlMatrixSubProcessACList;
      this.rcmDetails = result;
      this.addRCMTab(true);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Method for add new rcm Tab
   * @param isOnPageLoad: Is loaded first time
   */
  addRCMTab(isOnPageLoad: boolean): void {

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
      }, 10);
    }
  }

  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    this.router.navigate(['rcm/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method on save click
   */
  onSaveClick(rcmDetails: RiskControlMatrixAC) {
    this.loaderService.open();
    rcmDetails.sectorId = this.rcmDetails.sectorId;
    rcmDetails.rcmProcessId = this.rcmDetails.rcmProcessId;
    rcmDetails.rcmSubProcessId = this.rcmDetails.rcmSubProcessId;
    rcmDetails.riskControlMatrixSectorACList = [];
    rcmDetails.riskControlMatrixSubProcessACList = [];
    rcmDetails.riskControlMatrixProcessACList = [];

    this.rcmDetails.entityId = this.selectedEntityId;
    rcmDetails.entityId = this.selectedEntityId;
    if (rcmDetails.id === undefined || rcmDetails.id === null) {
      this.riskControlMatrixService.riskControlMatrixesAddRiskControlMatrix(rcmDetails, this.selectedEntityId).subscribe(() => {
        this.loaderService.close();
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.loaderService.close();
        this.sharedService.handleError(error);
      });
    } else {
      this.rcmDetailList = [];
      this.rcmDetailList.push(rcmDetails);
      this.riskControlMatrixService.riskControlMatrixesUpdateRiskControlMatrix(this.rcmDetailList, this.selectedEntityId).subscribe(() => {
        this.loaderService.close();
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.loaderService.close();
        this.sharedService.handleError(error);
      });
    }
  }

  /**
   * Method to get rcm details
   * @param id: Rcm id
   */
  async getRcmDetails(id: string) {
    this.loaderService.open();
    this.riskControlMatrixService.riskControlMatrixesGetRiskControlMatrixDetailsById(id).subscribe(result => {
      this.sectorList = result.riskControlMatrixSectorACList;
      this.processList = result.riskControlMatrixProcessACList;
      this.subProcessList = result.riskControlMatrixSubProcessACList;

      this.rcmDetails = result;
      this.rcmId = result.id;
      this.addRCMTab(true);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.setListPageRoute();
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
          }
        }
      }
    });
  }
}
