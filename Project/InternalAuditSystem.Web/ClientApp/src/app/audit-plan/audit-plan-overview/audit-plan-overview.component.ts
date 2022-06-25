import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { AuditPlanAC, AuditPlansService, AuditPlanSectionType } from '../../swaggerapi/AngularFiles';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuditPlanSharedService } from '../audit-plan-shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-audit-plan-overview',
  templateUrl: './audit-plan-overview.component.html'
})
export class AuditPlanOverviewComponent implements OnInit, OnDestroy {
  backToolTipMsg: string;
  overviewTitle: string;
  overviewAndBackgroundLabel: string;
  totalBudgetHoursLabel: string;
  financialYearLabel: string;
  previousButton: string;
  saveNextButtonText: string;
  budgetLabel: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  minLengthRequiredMessage: string;
  invalidMessage: string;

  // boolean
  isFinalcialYearValid = true;
  isMinLenth = true;
  inMaxLength = true;
  isFromAddPage: string;

  // Objects
  auditPlanObj = {} as AuditPlanAC;
  selectedEntityId: string;
  auditPlanId: string;
  sectionType: AuditPlanSectionType;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditPlanService: AuditPlansService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private auditPlanSharedService: AuditPlanSharedService) {
    this.backToolTipMsg = this.stringConstants.backToListPageTooltipMessage;
    this.overviewTitle = this.stringConstants.overviewTitle;
    this.overviewAndBackgroundLabel = this.stringConstants.overviewAndBackgroundLabel;
    this.totalBudgetHoursLabel = this.stringConstants.totalBudgetHoursLabel;
    this.financialYearLabel = this.stringConstants.financialYearLabel;
    this.previousButton = this.stringConstants.previousButton;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.budgetLabel = this.stringConstants.budgetLabel;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.sectionType = AuditPlanSectionType.NUMBER_1;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.minLengthRequiredMessage = this.stringConstants.minYearRequiredMessage;
  }

  /***
   * Audit Plan audit cycle start date picker configurations
   */
  ngOnInit() {
    this.loaderService.open();
    // get the current selectedEntityId
    this.auditPlanObj.totalBudgetedHours = null;
    this.auditPlanObj.financialYear = null;
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.activeRoute.params.subscribe(params => {
          this.auditPlanId = params.id;
          this.isFromAddPage = params.isFromAdd;
        });
        this.getAuditPlanDetailsById();
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
    this.auditPlanService.auditPlansGetAuditPlanDetailsById(this.auditPlanId, this.selectedEntityId, AuditPlanSectionType.NUMBER_1).subscribe((result: AuditPlanAC) => {
      this.auditPlanObj = JSON.parse(JSON.stringify(result));
      if (this.auditPlanObj !== null) {
        // if no data entered/ from add page this page is opened then plan this fields
        this.auditPlanObj.financialYear = this.auditPlanObj.financialYear === 0 ? null : this.auditPlanObj.financialYear;
        this.auditPlanObj.totalBudgetedHours = this.auditPlanObj.totalBudgetedHours === 0 ? null : this.auditPlanObj.totalBudgetedHours;


        // if financial year is not valid then disable form
        if (this.auditPlanObj.financialYear === null) {
          this.isFinalcialYearValid = false;
        } else {
          // validate save data again
          this.validateFinancialYear();
        }
      }
      this.loaderService.close();
    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * Save audit plan general info
   */
  saveDataAndRediectToPlanProcesses() {
    if (this.auditPlanId !== undefined) {
      this.auditPlanObj.id = this.auditPlanId;
    }
    this.auditPlanSharedService.saveAndRediectToNextSection(this.auditPlanObj, this.selectedEntityId, this.sectionType, this.isFromAddPage);
  }

  /**
   * Redirect to general page
   */
  redirectToPreviousPage() {
    this.auditPlanSharedService.redirectToPreviousPageSectionWise(this.sectionType, this.auditPlanId, this.isFromAddPage);
  }

  /***
   * Validate financial year field
   */
  validateFinancialYear() {
    const validLength = 4;
    this.isFinalcialYearValid = this.auditPlanObj.financialYear.toString().length === validLength;
    // if validate then hide messages
    if (this.isFinalcialYearValid) {
      this.isMinLenth = this.inMaxLength = false;
    } else {
      this.isMinLenth = this.auditPlanObj.financialYear.toString().length < validLength;
      this.inMaxLength = this.auditPlanObj.financialYear.toString().length > validLength;
    }
  }

  /**
   * Back to list page of audit plan
   */
  backToListPage() {
    this.router.navigate([this.stringConstants.auditPlanListPath]);
  }
}
