import { Component, OnInit, OnDestroy, Inject, Optional } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { PlanAuditProcessAddComponent } from './plan-audit-process-add/plan-audit-process-add.component';
import { AuditPlanSectionType, AuditPlanAC, AuditPlansService, PlanProcessMappingAC, PlanProcessStatus, BASE_PATH } from '../../swaggerapi/AngularFiles';
import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { Pagination } from '../../models/pagination';
import { AuditPlanSharedService } from '../audit-plan-shared.service';
import { Route } from '@angular/compiler/src/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-plan-audit-process',
  templateUrl: './plan-audit-process.component.html'
})
export class PlanAuditProcessComponent implements OnInit, OnDestroy {
  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  planProcessesTitle: string;
  selectAuditProcessLabel: string;
  searchText: string;
  excelToolTip: string;
  addToolTip: string;
  editToolTip: string;
  deleteToolTip: string;
  showingResults: string;
  bsModalRef: BsModalRef;
  previousButton: string;
  deleteTitle: string;
  processNameLabel: string;
  subProcessNameLabel: string;
  timeLineLabel: string; // Variable for timeline label
  statusTitle: string; // Variable for status
  showNoDataText: string;
  saveAndNextToolTipMsg: string;
  backToolTipMsg: string;
  saveNextButtonText: string;
  toText: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // Objects
  auditPlanObj = {} as AuditPlanAC;
  selectedEntityId: string;
  auditPlanId: string;
  sectionType: AuditPlanSectionType;
  planProcessList = [] as Array<PlanProcessMappingAC>;
  isFromAddPage: string;
  isPreviousClicked: boolean;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  baseUrl: string;
  // Point of discussion status array as swagger dont create enum properly
  planProcessStatusList = [
    { value: PlanProcessStatus.NUMBER_0, label: 'Inprogress' },
    { value: PlanProcessStatus.NUMBER_1, label: 'Closed' }
  ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private loaderService: LoaderService,
    private auditPlanService: AuditPlansService,
    private auditPlanSharedService: AuditPlanSharedService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    private sharedService: SharedService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.backToolTipMsg = this.stringConstants.backToListPageTooltipMessage;
    this.planProcessesTitle = this.stringConstants.planProcessesTitle;
    this.selectAuditProcessLabel = this.stringConstants.selectAuditProcessModalTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.processNameLabel = this.stringConstants.auditProcessLabel;
    this.subProcessNameLabel = this.stringConstants.auditSubProcessLabel;
    this.timeLineLabel = this.stringConstants.timeLineLabel;
    this.statusTitle = this.stringConstants.statusTitle;
    this.previousButton = this.stringConstants.previousButton;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.sectionType = AuditPlanSectionType.NUMBER_2;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.toText = ' To ';
    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.planProcessList = [];
    this.baseUrl = basePath;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.activeRoute.params.subscribe(params => {
          this.auditPlanId = params.id;
          this.isFromAddPage = params.isFromAdd;
        });
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
    this.loaderService.open();
    this.getPlanProcessesePageAndSerachWise(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId, this.auditPlanId);
  }

  /***
   * Search audit plan process basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchPlanProcess(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getPlanProcessesePageAndSerachWise(pageNumber, itemsPerPage, searchValue, this.selectedEntityId, this.auditPlanId);
    }
  }

  /**
   * Get all plans under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   * @param auditPlanId : Current selected audit plan Id
   */
  getPlanProcessesePageAndSerachWise(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string, auditPlanId: string) {
    this.auditPlanService.auditPlansGetPlanProcessesPageWiseAndSearchWiseByPlanId(auditPlanId, pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<PlanProcessMappingAC>) => {
        this.loaderService.close();
        this.planProcessList = JSON.parse(JSON.stringify(result.items));

        // set tooltip message accordigly
        this.saveAndNextToolTipMsg = this.planProcessList.length === 0 ? this.stringConstants.planProcessListRequiredToolTipMessage : '';
        this.planProcessList.forEach((process) => {
          process.statusString = this.planProcessStatusList[process.status].label;
          process.startDateTime = this.sharedService.convertLocalDateToUTCDate(process.startDateTime, false);
          process.endDateTime = this.sharedService.convertLocalDateToUTCDate(process.endDateTime, false);
        });


        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.auditPlanSharedService.handleError(error);
      });
  }

  /**
   * Save audit plan process info
   */
  saveDataAndRediectToDocuments() {
    const message = this.isFromAddPage === this.stringConstants.trueString ? this.stringConstants.recordAddedMsg : this.stringConstants.recordUpdatedMsg;
    this.sharedService.showSuccess(message);
    this.router.navigate([this.stringConstants.auditPlanDocumentsPath, { id: this.auditPlanId , isFromAdd: this.isFromAddPage }]);
  }

  /**
   * Open plan process add modal
   */
  openPlanProcessAddModal() {
    this.loaderService.open();
    const initialState = {
      auditCategoryModalTitle: this.stringConstants.auditCategoryLabel,
      keyboard: true,
      isEdit: false,
      auditPlanId: this.auditPlanId,
      planProcessObj: {} as PlanProcessMappingAC,
      callback: (result: PlanProcessMappingAC) => {
        if (result !== undefined) {
          this.loadPageData();
        }
      },
    };

    this.bsModalRef = this.modalService.show(PlanAuditProcessAddComponent,
        Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open plan process edit  modal
   * @param planProcessId : plan process id
   * @param index : data index id
   */
  openPlanProcessEditModal(planProcessId: string, index: number) {
    this.loaderService.open();
    const planProcessData = JSON.parse(JSON.stringify(this.planProcessList[index]));
    const initialState = {
      auditCategoryModalTitle: this.stringConstants.auditCategoryLabel,
      keyboard: true,
      isEdit: true,
      auditPlanId: this.auditPlanId,
      planProcessObj: planProcessData,
      callback: (result: PlanProcessMappingAC) => {
        if (result !== undefined) {
          this.loadPageData();
        }
      },
    };

    this.bsModalRef = this.modalService.show(PlanAuditProcessAddComponent,
        Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open plan process delete modal
   * @param planProcessId : plan process id
   * @param index : data index id
   */
  openPlanProcessDeleteModal(planProcessId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {

          if (planProcessId === undefined) {
            this.planProcessList.splice(index, 1);
            this.saveAndNextToolTipMsg = this.planProcessList.length === 0 ? this.stringConstants.planProcessListRequiredToolTipMessage : '';
            this.sharedService.showSuccess(this.stringConstants.auditProcessDeleteMsg);
          } else {
            this.loaderService.open();
            this.auditPlanService.auditPlansDeletePlanProcess(planProcessId, this.selectedEntityId)
              .subscribe(() => {
                this.loaderService.close();

                this.planProcessList.splice(index, 1);
                this.saveAndNextToolTipMsg = this.planProcessList.length === 0 ? this.stringConstants.planProcessListRequiredToolTipMessage : '';
                // check if it is this the last item of the current page then go back to previous page
                if (this.planProcessList.length === 0 && this.pageNumber > 1) {
                  this.pageNumber = this.pageNumber - 1;
                  this.loadPageData();
                } else {
                  this.totalRecords = this.totalRecords - 1;

                  // after delete from current page if total record is same item per page then refresh the record
                  if (this.totalRecords === this.itemsPerPage) {
                    this.loadPageData();
                  }
                  this.sharedService.showSuccess(this.stringConstants.auditProcessDeleteMsg);
                  this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);
                }
              }, error => {
                  this.auditPlanSharedService.handleError(error);
              });

          }
        }
      }
    };

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loaderService.open();
    this.getPlanProcessesePageAndSerachWise(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId, this.auditPlanId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.planProcessList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.planProcessList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
  }

  /**
   * Redirect to overview page
   */
  redirectToPreviousPage() {
    this.auditPlanSharedService.redirectToPreviousPageSectionWise(this.sectionType, this.auditPlanId, this.isFromAddPage);
  }

  /**
   * Back to list page of audit plan
   */
  backToListPage() {
    this.router.navigate([this.stringConstants.auditPlanListPath]);
  }

  /**
   * Method for export to excel of audit process and subprocess
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditPlanProcessApi + this.selectedEntityId + '&auditPlanId=' + this.auditPlanId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
