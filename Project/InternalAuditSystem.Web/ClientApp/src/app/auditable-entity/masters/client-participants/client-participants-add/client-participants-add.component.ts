import { Component, OnInit, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { UserAC, ClientParticipantsService } from '../../../../swaggerapi/AngularFiles';
import { NgForm } from '@angular/forms';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-client-participants-add',
  templateUrl: './client-participants-add.component.html'
})


export class ClientParticipantsAddComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('clientAddEditForm') ngForm: NgForm;

  selected: string; // Variable for selected names
  nameLabel: string; // Variable for name label
  title: string; // Variable for title
  emailLabel: string; // Variable for email
  saveButtonText: string; // Variable form save button text
  invalidMessage: string; // Variable for invalid message
  designationLabel: string; // Variable for designation label
  clientParticipantsText: string; // Variable for client participants text
  requiredMessage: string;
  userId: string;
  selectedEntityId: string;
  maxLengthExceedMessage: string;

  // objects
  clientObject: UserAC;
  // formChangesSubscription: any;
  isChanged = false;
  initialData: string;


  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private clientParticipantService: ClientParticipantsService,
    public router: Router,
    private toaster: ToastrService,
    private sharedService: SharedService,
    private loaderService: LoaderService) {
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.clientParticipantsText = this.stringConstants.clientParticipantsText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
  }

  /**
   * Lifecycle hook that is called after data-bound properties of a directive are initialized.
   * Initialization of properties.
   */
  ngOnInit() {

    // get the current selectedEntityId
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
      }
    });

    const tempUserObj = {} as UserAC;
    tempUserObj.name = this.clientObject.name;
    tempUserObj.designation = this.clientObject.designation;
    tempUserObj.emailId = this.clientObject.emailId;

    this.initialData = JSON.stringify(tempUserObj);
  }

  /**
   * Check if any form data is changed or not
   * NOTE : Incomplete implementation
   */
  ngAfterViewInit() {
    this.ngForm.form.valueChanges.subscribe(x => {
      const changedData = JSON.stringify(x);

      if (this.initialData !== changedData) {
        this.isChanged = true;
      }
    });
  }

  /**
   * Destroy the subscribed object
   * NOTE : Incomplete implementation
   */
  ngOnDestroy() {
    // this.formChangesSubscription.unsubscribe();
  }


  /**
   * Add and update Rating
   */
  saveClientParticipant() {
    // remove extra spaces foreach field
    this.clientObject.name = this.clientObject.name.trim();
    this.clientObject.designation = this.clientObject.designation.trim();
    this.clientObject.emailId = this.clientObject.emailId.trim();
    this.loaderService.open();
    // add client participant
    if (this.userId === undefined) {

      this.clientParticipantService.clientParticipantsAddClientParticipant(this.clientObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.hide();
        this.bsModalRef.content.callback(result);
      }, error => {
          this.handleError(error);
      });
    } else {
      // NOTE : only on value change related implementaion pending
      // if form value changed then only update
      if (this.isChanged) {
        this.clientParticipantService.clientParticipantsUpdateClientParticipant(this.clientObject, this.selectedEntityId)
          .subscribe(() => {
            this.loaderService.close();
            this.bsModalRef.hide();
            this.bsModalRef.content.callback(this.clientObject);
          }, error => {
              this.handleError(error);
          });
      } else {
        this.bsModalRef.hide();
      }
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
