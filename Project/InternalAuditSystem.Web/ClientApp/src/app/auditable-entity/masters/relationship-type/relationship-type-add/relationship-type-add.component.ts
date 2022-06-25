import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { RelationshipTypeAC, RelationshipTypesService } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-relationship-type-add',
  templateUrl: './relationship-type-add.component.html'
})
export class RelationshipTypeAddComponent implements OnInit {
  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message
  relationshipTypeLabel: string; // Variable relationship type
  requiredMessage: string;
  maxLengthExceedMessage: string;

  // objects
  relationshipTypeObject: RelationshipTypeAC;
  relationshipTypeId: string;
  selectedEntityId: string;

   // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private relationshipTypeService: RelationshipTypesService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.relationshipTypeLabel = this.stringConstants.relationshipTypeLabel;
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
   * Add and update relationship type
   */
  saveRelationshipTypes() {
    // remove extra spaces foreach field
    this.relationshipTypeObject.name = this.relationshipTypeObject.name.trim();
    this.loaderService.open();

    // add relationship type
    if (this.relationshipTypeId === undefined) {
      this.relationshipTypeService.relationshipTypesAddRelationshipType(this.relationshipTypeObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.relationshipTypeService.relationshipTypesUpdateRelationshipType(this.relationshipTypeObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.relationshipTypeObject);
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
