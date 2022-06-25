import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { EntityCategoryAC, EntityCategoriesService } from '../../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-auditable-entity-category-add',
  templateUrl: './auditable-entity-category-add.component.html'
})
export class AuditableEntityCategoryAddComponent implements OnInit {
  entityCategoryLabel: string; // Variable for auditable entity category
  categoryLabel: string; // Variable for category label
  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message

  requiredMessage: string;
  maxLengthExceedMessage: string;

  // objects
  entityCategoryObject: EntityCategoryAC;
  entityCategoryId: string;
  selectedEntityId: string;
  categoryTitle: string;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private entityCategoriesService: EntityCategoriesService) {
    this.entityCategoryLabel = this.stringConstants.auditableEntityCategoryLabel;
    this.categoryLabel = this.stringConstants.categoryLabel;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.categoryTitle = this.stringConstants.rcmSectorTitle;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.selectedEntityId = entityId;
    });
  }

  /**
   * Add and update entity category
   */
  saveEntityCategory() {
    // remove extra spaces foreach field
    this.entityCategoryObject.categoryName = this.entityCategoryObject.categoryName.trim();
    this.loaderService.open();

    // add audit type
    if (this.entityCategoryId === undefined) {
      this.entityCategoriesService.entityCategoriesAddEntityCategory(this.entityCategoryObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
        this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.entityCategoriesService.entityCategoriesUpdateEntityCategory(this.entityCategoryObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.entityCategoryObject);
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
