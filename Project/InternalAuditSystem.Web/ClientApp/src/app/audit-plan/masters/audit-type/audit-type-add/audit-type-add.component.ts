import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { AuditTypeAC } from '../../../../swaggerapi/AngularFiles/model/auditTypeAC';
import { AuditTypesService } from '../../../../swaggerapi/AngularFiles/api/auditTypes.service';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-audit-type-add',
  templateUrl: './audit-type-add.component.html'
})

export class AuditTypeAddComponent implements OnInit {
  auditTypeModalTitle: string;
  typeLabel: string;
  saveButtonText: string;
  invalidMessage: string;
  requiredMessage: string;
  // objects
  auditTypeObject: AuditTypeAC;
  auditTypeId: string;
  selectedEntityId: string;
  maxLengthExceedMessage: string;


  // Creates an instance of documenter
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private auditTypeService: AuditTypesService,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService) {
    this.typeLabel = this.stringConstants.typeLabel;
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
   * Add and update audit type
   */
  saveAuditType() {
    // remove extra spaces foreach field
    this.auditTypeObject.name = this.auditTypeObject.name.trim();
    this.loaderService.open();

    // add audit type
    if (this.auditTypeId === undefined) {

      this.auditTypeService.auditTypesAddAuditType(this.auditTypeObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.auditTypeService.auditTypesUpdateAuditType(this.auditTypeObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.auditTypeObject);
        }, (error: HttpErrorResponse) => {
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
