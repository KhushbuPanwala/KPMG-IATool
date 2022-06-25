import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { SharedService } from '../../../../core/shared.service';
import { ObservationCategoriesService, ObservationCategoryAC } from '../../../../swaggerapi/AngularFiles';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-observation-category-add',
  templateUrl: './observation-category-add.component.html'
})
export class ObservationCategoryAddComponent implements OnInit, OnDestroy {

  observationCategoryText: string; // Variable for observation category text
  saveButtonText: string; // Variable for save button text
  observationCategoryObject = {} as ObservationCategoryAC;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectedEntityId: string;
  observationCategoryId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(private stringConstants: StringConstants,
              public bsModalRef: BsModalRef, private sharedService: SharedService, private observationCategoryService: ObservationCategoriesService) {
    this.observationCategoryText = this.stringConstants.observationCategoryText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
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
   * Method to save/update observation category
   */
  saveObservationCategory() {
    // remove extra spaces foreach field
    this.observationCategoryObject.categoryName = this.observationCategoryObject.categoryName.trim();
    this.observationCategoryObject.entityId = this.selectedEntityId;
    // add ObservationCategory
    if (this.observationCategoryId === undefined) {
      this.observationCategoryService.observationCategoriesAddObservationCategory(this.observationCategoryObject, this.selectedEntityId).subscribe(result => {
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      this.observationCategoryService.observationCategoriesUpdateObservationCategory(this.observationCategoryObject, this.selectedEntityId)
        .subscribe(() => {
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(this.observationCategoryObject);
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }
}
