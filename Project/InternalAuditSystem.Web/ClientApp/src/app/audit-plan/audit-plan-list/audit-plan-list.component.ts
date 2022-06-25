import { Component, OnInit, OnDestroy, Optional, Inject } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { AuditCategoriesService, AuditPlanAC, AuditPlansService, AuditPlanStatus, BASE_PATH } from '../../swaggerapi/AngularFiles';
import { Pagination } from '../../models/pagination';
import { Router } from '@angular/router';
import { AuditPlanSharedService } from '../audit-plan-shared.service';
import { audit } from 'rxjs/operators';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-audit-plan-list',
  templateUrl: './audit-plan-list.component.html'
})
export class AuditPlanListComponent implements OnInit, OnDestroy {
  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  auditPlanTitle: string;
  searchText: string;
  excelToolTip: string;
  addToolTip: string;
  statusTitle: string;
  activeText: string;
  editToolTip: string;
  deleteToolTip: string;
  showingResults: string;
  versionLabel: string;
  auditTypeLabel: string;
  totalBudgetedHours: string;
  markClosed: string;
  showNoDataText: string;

  newVersionToolTip: string;

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  // Objects
  auditPlanList = [] as Array<AuditPlanAC>;
  selectedEntityId: string;
  startItem: number;
  hoursLabel: string;
  bsModalRef: BsModalRef;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  planStatusList = [];
  baseUrl: string;
  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditPlanSharedService: AuditPlanSharedService,
    private router: Router,
    private modalService: BsModalService,
    private auditPlanService: AuditPlansService, @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.auditPlanTitle = this.stringConstants.auditPlanTitleLabel;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.activeText = this.stringConstants.activeText;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.statusTitle = this.stringConstants.statusTitle;
    this.versionLabel = this.stringConstants.versionLabel;
    this.auditTypeLabel = this.stringConstants.auditTypeLabel;
    this.totalBudgetedHours = this.stringConstants.totalBudgetedHours;
    this.markClosed = this.stringConstants.markClosed;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.hoursLabel = this.stringConstants.hoursLabel;
    this.newVersionToolTip = this.stringConstants.newVersionToolTip;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.auditPlanList = [];
    this.planStatusList = this.auditPlanSharedService.planStatus;
    this.baseUrl = basePath;
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
        this.loadPageData();
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
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.getAllAuditPlans(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all plans under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllAuditPlans(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.auditPlanService.auditPlansGetAllAuditPlansPageWiseAndSearchWise(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<AuditPlanAC>) => {
        this.loaderService.close();
        this.auditPlanList = result.items;
        this.auditPlanList.forEach((plan) => {
          plan.statusString = this.setStatus(plan.status);
        });
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.auditPlanSharedService.handleError(error);
      });
  }

  /***
   * Search audit plan basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchAuditPlan(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getAllAuditPlans(pageNumber, itemsPerPage, searchValue.trim(), this.selectedEntityId);
    }
  }

  /**
   * Open add page
   */
  redirectToGeneralPage() {
    this.router.navigate([this.stringConstants.auditPlanGeneralPath, { isFromAdd: true }]);
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loaderService.open();
    this.getAllAuditPlans(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.auditPlanList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.auditPlanList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Set status display string according to it's value
   * @param status: WorkProgramStatus
   */
  setStatus(status: AuditPlanStatus) {
    let statusString = '';
    switch (status) {
      // active
      case AuditPlanStatus.NUMBER_0:
        statusString = this.planStatusList[0].label;
        break;

      // even if status is update show update
      case AuditPlanStatus.NUMBER_1:
        statusString = this.planStatusList[0].label;
        break;

      // close
      default:
        statusString = this.planStatusList[2].label;
        break;
    }
    return statusString;
  }

  /**
   * Redirect to General page
   * @param auditPlanId : Audit Plan Id
   */
  openEditAuditPlanPage(auditPlanId: string) {
    this.router.navigate([this.stringConstants.auditPlanGeneralPath, { id: auditPlanId, isFromAdd: false }]);
  }

  /**
   * Update closed & active status for the audit plan
   * @param auditPlanObj : Audit plan object
   */
  updatedStatus(auditPlanObj: AuditPlanAC) {

    let planStatus = '';
    let successMessage = '';
    if (auditPlanObj.status === AuditPlanStatus.NUMBER_0 || auditPlanObj.status === AuditPlanStatus.NUMBER_1) {
      auditPlanObj.status = this.planStatusList[2].value;
      planStatus = this.planStatusList[2].label;
      successMessage = this.stringConstants.planClosedMessage;
    } else {
      auditPlanObj.status = this.planStatusList[0].value;
      planStatus = this.planStatusList[0].label;
      successMessage = this.stringConstants.planActivatedMessage;
    }

    this.loaderService.open();
    this.auditPlanService.auditPlansUpdateAuditPlanStatus(auditPlanObj, this.selectedEntityId).subscribe(() => {
      this.auditPlanList.find(x => x.id === auditPlanObj.id).statusString = planStatus;
      this.sharedService.showSuccess(successMessage);
      this.loaderService.close();

    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * New version create for audit plan
   * @param auditPlanId : selected plan id
   */
  createNewVersion(auditPlanId: string) {
    this.loaderService.open();
    this.auditPlanService.auditPlansCreateNewVersionOfAuditPlan(auditPlanId, this.selectedEntityId).subscribe(() => {
      this.loaderService.close();
      this.loadPageData();
      this.sharedService.showSuccess(this.stringConstants.newVersionMsg);
    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * Delete audit plan after confirmation
   * @param auditPlanId : Id of the plan
   */
  deleteAuditPlan(auditPlanId: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditPlanService.auditPlansDeleteAuditPlan(auditPlanId, this.selectedEntityId).subscribe(() => {
            this.loaderService.close();
            this.loadPageData();
            this.sharedService.showSuccess(this.stringConstants.planDeleteMsg);
          }, error => {
            this.auditPlanSharedService.handleError(error);
          });
        }
      }
    };

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * Method for export to excel of audit plan
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditPlanApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
