import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { FormGroup, FormControl } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AuditPlansService, PlanProcessMappingAC, AuditProcessesService, ProcessAC, AuditPlanAC, PlanProcessStatus, AuditPlanSectionType } from '../../../swaggerapi/AngularFiles';
import { AuditPlanSharedService } from '../../audit-plan-shared.service';
import { LoaderService } from '../../../core/loader.service';
import { SharedService } from '../../../core/shared.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-plan-audit-process-add',
  templateUrl: './plan-audit-process-add.component.html'
})
export class PlanAuditProcessAddComponent implements OnInit, OnDestroy {
  selectAuditProcessModalTitle: string;
  buttonText: string;
  auditProcessLabel: string;
  auditSubProcessLabel: string;
  statusTitle: string;
  timeLineLabel: string;
  startDate: string;
  endDate: string;
  toText: string;
  auditPlanId: string;
  selectedEntityId: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectProcessPlaceholder: string;
  selectSubProcessPlaceholder: string;

  // boolean
  isEdit: boolean;

  // objects
  auditPlanObj = {} as AuditPlanAC;
  planProcessObj: PlanProcessMappingAC;
  processStatusList = [];
  allProcessList = [] as Array<ProcessAC>;
  processList = [] as Array<ProcessAC>;
  subProcessList = [] as Array<ProcessAC>;
  planProcessList = [] as Array<PlanProcessMappingAC>;
  sectionType: AuditPlanSectionType;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditPlanSharedService: AuditPlanSharedService,
    private router: Router,
    private auditProcessService: AuditProcessesService,
    private auditPlanService: AuditPlansService) {
    this.selectAuditProcessModalTitle = this.stringConstants.selectAuditProcessModalTitle;
    this.auditProcessLabel = this.stringConstants.auditProcessLabel;
    this.auditSubProcessLabel = this.stringConstants.auditSubProcessLabel;
    this.statusTitle = this.stringConstants.statusTitle;
    this.timeLineLabel = this.stringConstants.timeLineLabel;
    this.startDate = this.stringConstants.startDate;
    this.endDate = this.stringConstants.endDate;
    this.toText = this.stringConstants.toText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.selectProcessPlaceholder = this.stringConstants.selectProcessPlaceholder;
    this.selectSubProcessPlaceholder = this.stringConstants.selectSubProcessPlaceholder;
    this.sectionType = AuditPlanSectionType.NUMBER_2;

    // assign status enum
    this.processStatusList = this.auditPlanSharedService.planProcessStatus;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.planProcessObj.status = this.planProcessObj.status === undefined ? PlanProcessStatus.NUMBER_0 : this.planProcessObj.status;
        this.buttonText = this.stringConstants.updateButtonText;
        if (this.planProcessObj.processId === undefined && this.planProcessObj.parentProcessId === undefined) {
          this.buttonText = this.stringConstants.addButtonText;
        }
        this.getInitial1Data();
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
   * Get initial data for the dropdown
   */
  getInitial1Data() {
    this.auditProcessService.auditProcessesGetEntityWiseAllProcessesByEntityId(this.selectedEntityId).subscribe((result: Array<ProcessAC>) => {
      this.loaderService.close();

      this.allProcessList = JSON.parse(JSON.stringify(result));
      this.processList = JSON.parse(JSON.stringify(this.allProcessList.filter(x => x.parentId === null)));

      // if edit form then populate
      if (this.planProcessObj.processId !== undefined) {
        const processData = this.allProcessList.find(x => x.id === this.planProcessObj.processId);

        this.planProcessObj.startDateTime = new Date(this.planProcessObj.startDateTime);
        this.planProcessObj.endDateTime = new Date(this.planProcessObj.endDateTime);
        // if added data is main process then populate subprocess list only
        if (processData.parentId === undefined || processData.parentId === null) {
          this.planProcessObj.parentProcessId = this.planProcessObj.processId;
          this.subProcessList = JSON.parse(JSON.stringify(this.allProcessList.filter(x => x.parentId === this.planProcessObj.parentProcessId)));
          this.planProcessObj.processId = null;
        } else {
          // populate subprocess list
          this.subProcessList = JSON.parse(JSON.stringify(this.allProcessList.filter(x => x.parentId === processData.parentId)));
          this.planProcessObj.parentProcessId = processData.parentId;
        }
      }
    }, error => {
      this.handleError(error);
    });
  }

  /**
   * Populate subprocess list on selection of process
   * @param processId : Id of process selected
   */
  populateSubprocessList(processId: string) {
    this.planProcessObj.processId = null;
    this.subProcessList = JSON.parse(JSON.stringify(this.allProcessList.filter(x => x.parentId === processId)));
  }

  /**
   * Add/Edit data in plan process list temporarily
   */
  savePlanProcess() {
    // set process and subporcess name according to selection
    if (this.planProcessObj.processId === undefined) {
      this.planProcessObj.parentProcessName = this.processList.find(x => x.id === this.planProcessObj.parentProcessId).name;
      this.planProcessObj.processId = this.planProcessObj.parentProcessId;
    } else {
      const processName = this.processList.find(x => x.id === this.planProcessObj.parentProcessId).name;
      this.planProcessObj.parentProcessName = processName;

      // if subprocess selected then set name like this
      if (this.planProcessObj.processId !== null) {
        this.planProcessObj.processName = this.subProcessList.find(x => x.id === this.planProcessObj.processId).name;
      }
    }

    // set status string
    this.planProcessObj.statusString = this.auditPlanSharedService.planProcessStatus[this.planProcessObj.status].label;
    this.planProcessObj.planId = this.auditPlanId;

    // make server call for saving
    if (this.planProcessObj.id === undefined) {
      this.auditPlanService.auditPlansAddPlanProcess(this.planProcessObj, this.selectedEntityId).subscribe((result: string) => {
        this.bsModalRef.hide();
        this.sharedService.showSuccess(this.stringConstants.auditProcessAddMsg);
        this.bsModalRef.content.callback(result);
      }, error => {
        this.handleError(error);
      });
    } else {
      this.auditPlanService.auditPlansUpdatePlanProcess(this.planProcessObj, this.selectedEntityId).subscribe((result: string) => {
        this.bsModalRef.hide();
        this.sharedService.showSuccess(this.stringConstants.auditProcessUpdateMsg);
        this.bsModalRef.content.callback(this.planProcessObj);
      }, error => {
        this.handleError(error);
      });
    }
  }

  /**
   * Handle error scenario in case of add/ update
   * @param error : http error
   */
  handleError(error: HttpErrorResponse) {
    this.loaderService.close();
    if (error.status === 403) {
      // if entity close then show info of close status
      this.sharedService.showInfo(this.stringConstants.closedEntityRestrictionMessage);

    } else if (error.status === 405) {
      // if entity is deleted then show warning for the action
      this.sharedService.showWarning(this.stringConstants.deletedEntityRestrictionMessage);
    } else {

      // check if duplicate entry exception then show error message otherwise show something went wrong message
      const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
      this.sharedService.showError(errorMessage);
    }
  }
}
