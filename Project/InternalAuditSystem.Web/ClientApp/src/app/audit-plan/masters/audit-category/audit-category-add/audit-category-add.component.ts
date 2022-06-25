import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { AuditCategoryAC } from '../../../../swaggerapi/AngularFiles/model/auditCategoryAC';
import { ToastrService } from 'ngx-toastr';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { AuditCategoriesService } from '../../../../swaggerapi/AngularFiles';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-audit-category-add',
  templateUrl: './audit-category-add.component.html'
})

export class AuditCategoryAddComponent implements OnInit {
  auditCategoryModalTitle: string; // Variable for audit category title
  categoryLabel: string; // Variable for category label
  saveButtonText: string; // Variable for save button text
  invalidMessage: string;
  requiredMessage: string;

  // objects
  auditCategoryObject: AuditCategoryAC;
  auditCategoryId: string;
  selectedEntityId: string;
  maxLengthExceedMessage: string;


  // Creates an instance of documenter
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditCategoryService: AuditCategoriesService) {
    this.categoryLabel = this.stringConstants.categoryLabel;
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
   * Add and update audit category
   */
  saveAuditCategory() {
    // remove extra spaces foreach field
    this.auditCategoryObject.name = this.auditCategoryObject.name.trim();
    this.loaderService.open();

    // add audit type
    if (this.auditCategoryId === undefined) {

      this.auditCategoryService.auditCategoriesAddAuditCategory(this.auditCategoryObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.auditCategoryService.auditCategoriesUpdateAuditCategory(this.auditCategoryObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.auditCategoryObject);
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
