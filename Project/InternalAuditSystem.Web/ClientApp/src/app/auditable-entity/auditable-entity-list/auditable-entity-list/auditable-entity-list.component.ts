import { Component, OnInit, Optional, Inject } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { SharedService } from '../../../core/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { AuditableEntitiesService, AuditableEntityAC, AuditableEntityStatus, BASE_PATH, PaginationOfAuditableEntityAC, UserAC, UserType, UserRole } from '../../../swaggerapi/AngularFiles';
import { Pagination } from '../../../models/pagination';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { LoaderService } from '../../../core/loader.service';

@Component({
  selector: 'app-auditable-entity-list',
  templateUrl: './auditable-entity-list.component.html'
})
export class AuditableEntityListComponent implements OnInit {
  auditableEntityTitle: string; // Variable for strategic analysis title
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  nameLabel: string; // Variable for name  title
  typeLabel: string; // Variable for type title
  levelText: string; // Variable for level title
  versionsText: string; // Variable for versions title
  engagementManager: string; // Vriable for engagement manager
  statusTitle: string; // Variable for status title
  markClosed: string; // Variable for mark closed button
  activeText: string; // Variable for active button text
  editToolTip: string; // Variable for edit tool tip
  createNewVersionToolTip: string;
  deleteToolTip: string; // Variable for delete tool tip
  showingResults: string; // Variable of showing results
  newVersionToolTip: string;
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for title
  baseUrl: string;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  auditableEntityList: AuditableEntityAC[] = [];
  currentUserDetails: UserAC;

  statusList = [
    { value: AuditableEntityStatus.NUMBER_0, label: 'Active' },
    { value: AuditableEntityStatus.NUMBER_1, label: 'Update' },
    { value: AuditableEntityStatus.NUMBER_2, label: 'Closed' }
  ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private sharedService: SharedService,
    public router: Router,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private modalService: BsModalService,
    private auditableEntityService: AuditableEntitiesService,
    @Optional() @Inject(BASE_PATH) basePath: string) {
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.nameLabel = this.stringConstants.nameLabel;
    this.typeLabel = this.stringConstants.typeLabel;
    this.engagementManager = this.stringConstants.engagementManager;
    this.levelText = this.stringConstants.levelText;
    this.versionsText = this.stringConstants.versionsText;
    this.statusTitle = this.stringConstants.statusTitle;
    this.markClosed = this.stringConstants.markClosed;
    this.activeText = this.stringConstants.activeText;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.createNewVersionToolTip = this.stringConstants.createNewVersion;
    this.baseUrl = basePath;

    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.newVersionToolTip = this.stringConstants.newVersionToolTip;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params.pageItems !== undefined) {
        this.selectedPageItem = Number(params.pageItems);
      }
      this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
    });
    this.getAuditableEntityList(this.pageNumber, this.selectedPageItem, this.searchValue);
  }

  /**
   * Get Auditable Entity list for list page with pagination
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   */
  getAuditableEntityList(pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.loaderService.open();
    this.auditableEntityService.auditableEntitiesGetAuditableEntityList(pageNumber,
      selectedPageItem, searchValue).subscribe((result: PaginationOfAuditableEntityAC) => {
        this.auditableEntityList = result.items;

        // update current user details
        this.sharedService.updateOnEntityListUpdates(result.currentUserDetails);
        this.currentUserDetails = result.currentUserDetails.userDetails;

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
        this.loaderService.close();
      }, (error) => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }
  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getAuditableEntityList(pageNumber, this.selectedPageItem, this.searchValue);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getAuditableEntityList(null, this.selectedPageItem, this.searchValue);
  }

  /**
   * Delete AuditableEntity Id from List
   * @param auditableEntityId: auditableEntityId
   */
  deleteAuditableEntity(auditableEntityId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.auditableEntityService.auditableEntitiesDeleteAuditableEntity(auditableEntityId).subscribe(() => {
              this.loaderService.close();
              this.getAuditableEntityList(null, this.selectedPageItem, this.searchValue);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, (error) => {
              this.loaderService.close();
              this.sharedService.showError(error.error);
            });
          }
        },
      }
    });
  }

  /**
   * Redirect to Add page
   */
  redirectToDetailsPage() {
    this.router.navigate(['auditable-entity/details', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);

  }
  /**
   * Method for export auditable entity to excel
   */
  onExportClick() {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.auditableEntityExportApi + timeOffset;
    this.sharedService.exportToExcel(url);
  }
  /**
   * On edit click in list will redirect to edit page
   * @param auditableEntityId : auditableEntity Id to be edited
   */
  editAuditableEntity(auditableEntityId: string) {
    this.router.navigate(['auditable-entity/details', { id: auditableEntityId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * On create version click in list will redirect to edit page
   * @param auditableEntityId : auditableEntity Id to be edited
   */
  createVersionAuditableEntity(auditableEntityId: string) {
    this.loaderService.open();
    this.auditableEntityService.auditableEntitiesCreateNewVersion(auditableEntityId).subscribe(() => {
      this.loaderService.close();
      this.getAuditableEntityList(null, this.selectedPageItem, this.searchValue);
      this.sharedService.showSuccess(this.stringConstants.newVersionMsg);
    }, (error) => {
      this.loaderService.close();
      this.sharedService.showError(error.error);
    });
  }

  /**
   * Search AuditableEntity on grid
   * @param event: key event tab and enter
   */
  searchAuditableEntity(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getAuditableEntityList(null, this.selectedPageItem, this.searchValue);
    }
  }

  /**
   * Set Status
   * @param status: AuditableEntityStatus
   */
  setStatus(status: AuditableEntityStatus) {
    if (AuditableEntityStatus.NUMBER_0 === status || AuditableEntityStatus.NUMBER_1 === status) {
      return this.stringConstants.statusButtonCloseText;
    } else {
      return this.stringConstants.statusButtonReopenText;
    }
  }

  /**
   * Update closed & active status for the audit plan
   * @param auditableEntity : AuditableEntity object
   */
  updatedStatus(auditableEntity: AuditableEntityAC) {

    let entityStatus = '';
    let successMessage = '';
    if (auditableEntity.status === AuditableEntityStatus.NUMBER_0 || auditableEntity.status === AuditableEntityStatus.NUMBER_1) {
      auditableEntity.status = this.statusList[2].value;
      entityStatus = this.statusList[2].label;
      successMessage = this.stringConstants.entityClosedMessage;
    } else {
      auditableEntity.status = this.statusList[0].value;
      entityStatus = this.statusList[0].label;
      successMessage = this.stringConstants.entityActivatedMessage;
    }

    this.loaderService.open();
    this.auditableEntityService.auditableEntitiesUpdateAuditableEntity(auditableEntity).subscribe(() => {
      this.auditableEntityList.find(x => x.id === auditableEntity.id).statusString = entityStatus;
      this.sharedService.showSuccess(successMessage);
      this.loaderService.close();

    }, error => {
      this.loaderService.close();
      this.sharedService.showError(error.error);
    });
  }
}
