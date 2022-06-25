import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { AuditTeamsService, UserAC } from '../../../../swaggerapi/AngularFiles';
import { Subject, Observable } from 'rxjs';
import { map, debounceTime, switchMap } from 'rxjs/operators';
import { LoaderService } from '../../../../core/loader.service';
import { SharedService } from '../../../../core/shared.service';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-audit-team-add',
  templateUrl: './audit-team-add.component.html',
})
export class AuditTeamAddComponent implements OnInit {
  searchValue: string; // Variable for selected names
  nameLabel: string; // Variable for name label
  title: string; // Variable for title
  auditTeamModalTitle: string;
  emailLabel: string;
  departmentLabel: string;
  designationLabel: string;
  saveButtonText: string;
  invalidMessage: string;
  requiredMessage: string;
  noRecordFound: string;

  // objects
  userAcList = [] as Array<UserAC>;
  entityTypeId: string;
  selectedEntityId: string;
  displayNameList = [] as Array<string>;
  dropdownList: Observable<UserAC[]>;
  subject = new Subject<string>();
  isLoading = false;
  teamUserObject: UserAC;
  isToHideList = true;
  isDataPopulated: boolean;
  isNoRecordMatched: boolean;

  // Creates an instance of documenter.
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private auditTeamService: AuditTeamsService,
    private loaderService: LoaderService,
    private sharedService: SharedService) {
    this.nameLabel = this.stringConstants.nameLabel;
    this.auditTeamModalTitle = this.stringConstants.auditTeamLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.departmentLabel = this.stringConstants.departmentLabel;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.designationLabel = this.stringConstants.designationLabel;
    this.teamUserObject = {} as UserAC;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.noRecordFound = this.stringConstants.noRecordFoundMessage;
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        // get all users from ad based on seraching
        this.getUsersFromAdBasedOnSearch();
      }
    });
  }

  /**
   * Only on enter press get all the users from ad based on the search term
   * @param event : Key board event that is pressed
   * @param searchValue : Search string
   */
  passValueOnEnterEvent(event: KeyboardEvent, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText && searchValue.trim() !== '') {
      this.isLoading = true;
      this.isToHideList = true;
      this.subject.next(searchValue);
    }
    this.isToHideList = true;
    if (searchValue.trim() === '') {
      this.isDataPopulated = false;
      // if no serach string entered or blanked then refresh form data
      this.teamUserObject = {} as UserAC;
      this.isNoRecordMatched = false;
    }
  }

  /**
   * Get all users from ad based on seraching
   */
  getUsersFromAdBasedOnSearch() {
    this.dropdownList = this.subject.pipe(
      switchMap((searchText: string) => {
        return this.auditTeamService.auditTeamsGetAllUserFromAdBasedOnSearch(searchText).pipe(map((result: Array<UserAC>) => {
          this.isLoading = false;
          this.userAcList = JSON.parse(JSON.stringify(result));
          // if no record found so error message
          this.isNoRecordMatched = this.isToHideList = this.userAcList.length === 0;
          return result.map(x => x);
        }));
      })
    );
  }

  /**
   * Populate other data according to the selection of names
   * @param email : Email
   */
  populateSelectedUserDetails(email: string) {
    this.isToHideList = true;
    this.isDataPopulated = true;
    this.teamUserObject = this.userAcList.find(x => x.emailId === email);
  }

  /**
   * Add and update entity type
   */
  saveAuditTeamMember() {
    // remove extra spaces foreach field
    this.teamUserObject.name = this.teamUserObject.name.trim();
    if (this.isDataPopulated) {
      this.loaderService.open();

      // separate team email id
      this.teamUserObject.auditMemberEmailId = this.teamUserObject.emailId;
      // handle scenario of extra text added after name
      this.teamUserObject.name = this.userAcList.find(x => x.emailId === this.teamUserObject.emailId).name;
      this.teamUserObject.emailId = '';
      this.auditTeamService.auditTeamsAddNewAuditTeamMember(this.teamUserObject, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.bsModalRef.content.callback(result);
      }, error => {
        // disable save button on error
        this.isDataPopulated = false;
        this.teamUserObject.emailId = this.teamUserObject.auditMemberEmailId;
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
