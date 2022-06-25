import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { EntityTypeAC } from '../../../../swaggerapi/AngularFiles/model/entityTypeAC';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { EntityTypesService } from '../../../../swaggerapi/AngularFiles/api/entityTypes.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-auditable-entity-type-add',
  templateUrl: './auditable-entity-type-add.component.html'
})

export class AuditableEntityTypeAddComponent implements OnInit {
  entityTypeLabel: string; // Variable for auditable entity type add
  typeLabel: string; // Variable for type label
  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message
  requiredMessage: string;
  maxLengthExceedMessage: string;

  // objects
  entityTypeObject: EntityTypeAC;
  entityTypeId: string;
  selectedEntityId: string;


  // Creates an instance of documenter
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private entityTypesService: EntityTypesService) {
    this.entityTypeLabel = this.stringConstants.auditableEntityTypeLabel;
    this.typeLabel = this.stringConstants.typeLabel;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
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
   * Add and update entity type
   */
  saveEntityType() {
    // remove extra spaces foreach field
    this.entityTypeObject.typeName = this.entityTypeObject.typeName.trim();
    this.loaderService.open();

    // add audit type
    if (this.entityTypeId === undefined) {
      this.entityTypesService.entityTypesAddEntityType(this.entityTypeObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.entityTypesService.entityTypesUpdateEntityType(this.entityTypeObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.entityTypeObject);
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
