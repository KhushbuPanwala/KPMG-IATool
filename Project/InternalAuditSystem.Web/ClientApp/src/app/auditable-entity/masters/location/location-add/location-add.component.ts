import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { LocationsService, LocationAC } from '../../../../swaggerapi/AngularFiles';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-location-add',
  templateUrl: './location-add.component.html'
})
export class LocationAddComponent implements OnInit {

  saveButtonText: string; // Variable for save button text
  invalidMessage: string; // Variable for invalid message
  locationModalTitle: string; // Variable for division title
  requiredMessage: string;
  maxLengthExceedMessage: string;

  // objects
  locationObject: LocationAC;
  locationId: string;
  selectedEntityId: string;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private locationService: LocationsService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.locationModalTitle = this.stringConstants.locationLabel;
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
   * Add and update location
   */
  saveLocation() {
    // remove extra spaces foreach field
    this.locationObject.name = this.locationObject.name.trim();
    this.loaderService.open();

    // add relationship type
    if (this.locationId === undefined) {
      this.locationService.locationsAddLocation(this.locationObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.locationService.locationsUpdateLocation(this.locationObject, this.selectedEntityId)
        .subscribe(() => {
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.locationObject);
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
