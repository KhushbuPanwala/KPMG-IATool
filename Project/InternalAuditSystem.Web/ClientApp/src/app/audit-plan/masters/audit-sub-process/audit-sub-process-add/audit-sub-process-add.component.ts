import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { AuditSubProcessesService } from '../../../../swaggerapi/AngularFiles/api/auditSubProcesses.service';
import { AuditProcessesService } from '../../../../swaggerapi/AngularFiles/api/auditProcesses.service';
import { ProcessAC } from '../../../../swaggerapi/AngularFiles/model/processAC';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-audit-process-add',
  templateUrl: './audit-sub-process-add.component.html'
})
export class AuditSubProcessAddComponent implements OnInit {
  auditSubAddProcesses: string; // Variable for audit sub process title
  auditAddProcesses: string; // Variable for audit sub process add  title
  subProcessesName: string; // Variable for sub processes name
  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message
  auditProcesses: string; // Variable audit processes
  scopeBasedOnLabel: string; // Variable for scope based on
  scopeLabel: string; // Variable for scope
  selectedEntityId: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  scopeValue: string;
  auditSubProcessId: string;
  dropdownDefaultValue: string;
  // Creates an instance of documenter
  auditProcessList = [] as Array<ProcessAC>;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  auditSubProcessObject: ProcessAC;


  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditSubProcessService: AuditSubProcessesService,
    private auditProcessService: AuditProcessesService) {
    this.auditSubAddProcesses = this.stringConstants.auditSubAddProcesses;
    this.subProcessesName = this.stringConstants.subProcessesName;
    this.auditProcesses = this.stringConstants.auditProcessLabel;
    this.scopeBasedOnLabel = this.stringConstants.scopeBasedOnLabel;
    this.scopeLabel = this.stringConstants.scopeLabel;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.auditSubAddProcesses = this.stringConstants.auditSubAddProcesses;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        // get the name of the entity
        this.sharedService.selectedEntityObjSubject.subscribe(async (entity) => {
          this.scopeValue = entity.name;

          this.loaderService.open();
          await this.getAllProcessesUnderAuditableEntity();
        });
      }
    });
  }

  /**
   * Get all the process under an auditable entity
   */
  getAllProcessesUnderAuditableEntity() {
    this.auditProcessService.auditProcessesGetOnlyProcessesByEntityId(this.selectedEntityId)
      .subscribe((result: Array<ProcessAC>) => {
        this.auditProcessList = result;
        this.loaderService.close();
      }, error => {
        this.handleError(error);
      });
  }

  /**
   * Add and update audit category
   */
  saveAuditSubProcess() {
    // remove extra spaces foreach field
    this.auditSubProcessObject.name = this.auditSubProcessObject.name.trim();
    this.auditSubProcessObject.scopeBasedOn = this.auditSubProcessObject.scopeBasedOn.trim();
    // make null to update changed process properly
    this.auditSubProcessObject.parentProcess = null;
    this.loaderService.open();

    // add audit type
    if (this.auditSubProcessId === undefined) {
      this.auditSubProcessService.auditSubProcessesAddAuditSubProcess(this.auditSubProcessObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();

        // bind parent process name
        result.parentProcess = {} as ProcessAC;
        result.parentProcess.name = this.auditProcessList.find(x => x.id === this.auditSubProcessObject.parentId).name;

        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.auditSubProcessService.auditSubProcessesUpdateAuditSubProcess(this.auditSubProcessObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();

          this.auditSubProcessObject.parentProcess = {} as ProcessAC;
          // bind parent process name
          this.auditSubProcessObject.parentProcess.name = this.auditProcessList.find(x => x.id === this.auditSubProcessObject.parentId).name;
          this.bsModalRef.content.callback(this.auditSubProcessObject);
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
