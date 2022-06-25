import { Component, OnInit, TemplateRef, OnDestroy, Inject, Optional } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

import { AuditTeamAddComponent } from '../audit-team-add/audit-team-add.component';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { LoaderService } from '../../../../core/loader.service';
import { AuditTeamsService, UserAC, EntityUserMappingAC, BASE_PATH, PaginationOfEntityUserMappingAC } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { Pagination } from '../../../../models/pagination';
import { Subscription, Observable } from 'rxjs';
import { HttpErrorResponse, HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-audit-team-list',
  templateUrl: './audit-team-list.component.html',
})
export class AuditTeamListComponent implements OnInit, OnDestroy {
  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  auditTeamTitle: string; // Variable for audit team title
  searchText: string; // Variable for search text
  excelToolTip: string; // Variable for excel tooltip
  srNo: string; // Variable for srno field
  nameLabel: string; // Varaible for name
  designationLabel: string; // Variable for designation
  departmentLabel: string; // Variable for department
  emailLabel: string; // Variable for email
  showingResults: string; // Variable for showing results
  deleteToolTip: string; // Variable for delete tooltip
  editToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  syncToolTip: string; // Variable for sync tooltip
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for title
  showNoDataText: string;

  // Objects
  auditTeamList = [] as Array<EntityUserMappingAC>;
  selectedEntityId: string;
  startItem: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;
  isToLoadMainLoader;

  baseUrl: string;
  isLoading = false;
  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private auditTeamService: AuditTeamsService,
    private loaderService: LoaderService,
    private sharedService: SharedService, @Optional() @Inject(BASE_PATH) basePath: string) {
    this.auditTeamTitle = this.stringConstants.auditTeamLabel;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.srNo = this.stringConstants.srNo;
    this.nameLabel = this.stringConstants.nameLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.departmentLabel = this.stringConstants.departmentLabel;
    this.showingResults = this.stringConstants.showingResults;
    this.syncToolTip = this.stringConstants.syncToolTip;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.auditTeamList = [];
    this.baseUrl = basePath;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.loadPageData();
      }
    });
    // current logged in user details
    this.userSubscribe = this.sharedService.currentUserDetailsSubject.subscribe((currentUserDetails) => {
      if (currentUserDetails !== null) {
        this.currentUserDetails = currentUserDetails.userDetails;
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
    this.userSubscribe.unsubscribe();
  }

  /**
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getAllAuditTeamMemeber(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }

  /**
   * Get all audit team memebers under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllAuditTeamMemeber(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.auditTeamService.auditTeamsGetAllAuditTeamMembers(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<EntityUserMappingAC>) => {
        this.loaderService.close();
        this.auditTeamList = result.items;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);
      }, error => {
        this.handleError(error);
      });
  }

  /**
   * Sync audit team members data with ad
   */
  syncTeamDataWithAd() {
    this.isLoading = true;
    this.syncToolTip = this.stringConstants.syncInProgressToolTip;
    this.auditTeamService.auditTeamsSyncAuditTeamData(this.pageNumber, this.itemsPerPage, this.selectedEntityId).subscribe((result: PaginationOfEntityUserMappingAC) => {
      this.isLoading = false;
      this.syncToolTip = this.stringConstants.syncToolTip;
      this.auditTeamList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);
      this.sharedService.showSuccess(this.stringConstants.syncSucessfullMessage);
    }, error => {
      this.handleError(error);
    });
  }

  /***
   * Search audit team members basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchAuditTeamMembers(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getAllAuditTeamMemeber(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Open audit team member add modal for adding team members
   */
  openAuditTeamModal() {
    const initialState = {
      keyboard: true,
      teamUserObject: {} as UserAC,
      callback: (result) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      }
    };

    this.bsModalRef = this.modalService.show(AuditTeamAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open delete audit team member modal
   * @param auditTeamMemberId : Member Id
   * @param index : Index for the row
   */
  openAuditTeamMemberDeleteModal(auditTeamMemberId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditTeamService.auditTeamsDeleteAuditTeamMeberFromAuditableEntity(auditTeamMemberId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.auditTeamList.splice(index, 1);

              // check if it is this the last item of the current page then go back to previous page
              if (this.auditTeamList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;
              }

              this.loadPageData();
              this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);

              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, error => {
              this.handleError(error);
            });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loadPageData();
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.auditTeamList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.auditTeamList.length) {
      this.pageNumber = 1;
    }

    this.loadPageData();
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
  /**
   * Method for export to excel of audit team
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditTeamApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }


}
