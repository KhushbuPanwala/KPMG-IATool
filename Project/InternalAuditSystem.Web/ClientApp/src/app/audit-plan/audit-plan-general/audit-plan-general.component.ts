import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { FormGroup, FormControl } from '@angular/forms';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { AuditPlansService, AuditPlanAC, AuditTypeAC, AuditCategoryAC, AuditPlanStatus, AuditPlanSectionType } from '../../swaggerapi/AngularFiles';
import { Router, ActivatedRoute } from '@angular/router';
import { AuditPlanSharedService } from '../audit-plan-shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-audit-general',
  templateUrl: './audit-plan-general.component.html',
})
export class AuditGeneralComponent implements OnInit, OnDestroy {
  backToolTipMsg: string;
  generalTitle: string;
  auditPlanTitleLabel: string;
  auditPlanPlaceholder: string;
  auditTypeLabel: string;
  auditCycleLabel: string;
  dateLabel: string;
  dueDateLabel: string;
  identifiedOnLabel: string;
  statusLabel: string;
  versionLabel: string;
  statusButtonCloseText: string;
  auditCategoryLabel: string;
  saveNextButtonText: string;
  dropdownDefaultValue: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;

  // boolean
  isToDisableCloseButton = false;
  isFromAddPage: string;

  // Objects
  auditPlanObj = {} as AuditPlanAC;
  auditTypeList = [] as Array<AuditTypeAC>;
  auditCategoryList = [] as Array<AuditCategoryAC>;
  selectedEntityId: string;
  auditPlanId: string;
  sectionType: AuditPlanSectionType;
  planStatusList = [];
  // only to subscripe for the current component
  entitySubscribe: Subscription;


  /***
   * Audit Plan audit cycle start date picker configurations
   */
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
  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditPlanService: AuditPlansService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private auditPlanSharedService: AuditPlanSharedService) {
    this.generalTitle = this.stringConstants.generalTitle;
    this.auditPlanTitleLabel = this.stringConstants.auditPlanTitleLabel;
    this.auditTypeLabel = this.stringConstants.auditTypeLabel;
    this.auditCycleLabel = this.stringConstants.auditCycleLabel;
    this.dateLabel = this.stringConstants.dateLabel;
    this.dueDateLabel = this.stringConstants.dueDateLabel;
    this.identifiedOnLabel = this.stringConstants.identifiedOnLabel;
    this.statusLabel = this.stringConstants.statusTitle;
    this.versionLabel = this.stringConstants.versionLabel;
    this.statusButtonCloseText = this.stringConstants.statusButtonCloseText;
    this.auditCategoryLabel = this.stringConstants.auditCategoryLabel;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.dropdownDefaultValue = this.stringConstants.notSpecifiedLabel;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.sectionType = AuditPlanSectionType.NUMBER_0;
    this.planStatusList = this.auditPlanSharedService.planStatus;
    this.backToolTipMsg = this.stringConstants.backToListPageTooltipMessage;
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
        this.activeRoute.params.subscribe(params => {
          this.auditPlanId = params.id;
          this.isFromAddPage = params.isFromAdd;
        });
        if (this.auditPlanId === undefined) {
          this.getInitialData();
        } else {
          this.getAuditPlanDetailsById();
        }
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
   * Get audit plan details by it's id
   */
  getAuditPlanDetailsById() {
    this.auditPlanService.auditPlansGetAuditPlanDetailsById(this.auditPlanId, this.selectedEntityId, AuditPlanSectionType.NUMBER_0).subscribe((result: AuditPlanAC) => {
      this.auditPlanObj = JSON.parse(JSON.stringify(result));

      if (this.auditPlanObj !== null) {
        // update status field according to it's value set
        this.updateStatusField();

        // convert dates to local time
        this.auditPlanObj.startDateTime = this.sharedService.convertLocalDateToUTCDate(this.auditPlanObj.startDateTime, false);
        this.auditPlanObj.endDateTime = this.sharedService.convertLocalDateToUTCDate(this.auditPlanObj.endDateTime, false);

        // bind selection dropdown data
        this.auditCategoryList = JSON.parse(JSON.stringify(result.auditCategorySelectionDisaplyList));
        this.auditTypeList = JSON.parse(JSON.stringify(result.auditTypeSelectionDisaplyList));
      }
      this.loaderService.close();
    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * Get initial data required for audit plan
   */
  getInitialData() {
    this.auditPlanService.auditPlansGetInitialDataForAuditPlanAdd(this.selectedEntityId).subscribe((result: AuditPlanAC) => {
      this.loaderService.close();

      // bind selection dropdown data
      this.auditCategoryList = JSON.parse(JSON.stringify(result.auditCategorySelectionDisaplyList));
      this.auditTypeList = JSON.parse(JSON.stringify(result.auditTypeSelectionDisaplyList));

      // set status to update
      this.auditPlanObj.status = this.planStatusList[0].value;
      this.auditPlanObj.statusString = this.planStatusList[0].label;
      this.auditPlanObj.version = 1.0;
    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * Update status according to status button
   */
  updateCurrentStatusToclose() {
    if (this.auditPlanObj.status === AuditPlanStatus.NUMBER_0 || this.auditPlanObj.status === AuditPlanStatus.NUMBER_1) {
      this.auditPlanObj.status = this.planStatusList[2].value;
      this.auditPlanObj.statusString = this.planStatusList[2].label;
      this.isToDisableCloseButton = true;
      this.auditPlanSharedService.isStatusChangedToClose = true;
    } else {
      this.auditPlanObj.status = this.planStatusList[1].value;
      this.auditPlanObj.statusString = this.planStatusList[1].label;
      this.statusButtonCloseText = this.stringConstants.statusButtonCloseText;
      this.auditPlanSharedService.isStatusChangedToClose = false;
    }
  }

  /**
   * Save audit plan general info
   */
  saveDataAndRediectToOverview() {
    if (this.auditPlanId !== undefined) {
      this.auditPlanObj.id = this.auditPlanId;
    } else {
      this.auditPlanObj.totalBudgetedHours = 0;
      this.auditPlanObj.financialYear = 0;
    }
    this.auditPlanSharedService.saveAndRediectToNextSection(this.auditPlanObj, this.selectedEntityId, this.sectionType, this.isFromAddPage);
  }

  /**
   * Back to list page of audit plan
   */
  backToListPage() {
    this.router.navigate([this.stringConstants.auditPlanListPath]);
  }

  /**
   * Set display status and button text according to saved status code
   */
  updateStatusField() {
    if (this.auditPlanObj.status === AuditPlanStatus.NUMBER_0 || this.auditPlanObj.status === AuditPlanStatus.NUMBER_1) {
      this.auditPlanObj.statusString = this.planStatusList[1].label;
      this.statusButtonCloseText = this.stringConstants.statusButtonCloseText;
    } else {
      this.auditPlanObj.statusString = this.planStatusList[2].label;
      this.statusButtonCloseText = this.stringConstants.activeText;
    }
  }
}
