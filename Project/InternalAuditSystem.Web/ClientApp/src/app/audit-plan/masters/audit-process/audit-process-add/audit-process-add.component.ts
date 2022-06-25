import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { AuditProcessesService } from '../../../../swaggerapi/AngularFiles/api/auditProcesses.service';
import { ProcessAC } from '../../../../swaggerapi/AngularFiles';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-audit-process-add',
  templateUrl: './audit-process-add.component.html'
})
export class AuditProcessAddComponent implements OnInit {
  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message
  auditProcessLabel: string; // Variable audit processes
  processNameLabel: string; // Variable for processes name
  requiredMessage: string;
  maxLengthExceedMessage: string;

  // objects
  auditProcessObject: ProcessAC;
  auditProcessId: string;
  selectedEntityId: string;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditProcessService: AuditProcessesService
  ) {
    this.processNameLabel = this.stringConstants.processNameLabel;
    this.auditProcessLabel = this.stringConstants.auditProcessLabel;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.selectedEntityId = entityId;
    });
  }

  /**
   * Add and update audit process
   */
  saveAuditProcess() {
    // remove extra spaces foreach field
    this.auditProcessObject.name = this.auditProcessObject.name.trim();
    this.loaderService.open();

    // add audit type
    if (this.auditProcessId === undefined) {
      this.auditProcessService.auditProcessesAddAuditProcess(this.auditProcessObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.auditProcessService.auditProcessesUpdateAuditProcess(this.auditProcessObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.auditProcessObject);
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
