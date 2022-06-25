import { Component, OnInit, OnDestroy , Optional, Inject } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ClientParticipantsAddComponent } from '../client-participants-add/client-participants-add.component';
import { ConfirmationDialogComponent } from '../../../../shared/confirmation-dialog/confirmation-dialog.component';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { ClientParticipantsService, EntityUserMappingAC, BASE_PATH } from '../../../../swaggerapi/AngularFiles';
import { Pagination } from '../../../../models/pagination';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-client-participants-list',
  templateUrl: './client-participants-list.component.html'
})
export class ClientParticipantsListComponent implements OnInit, OnDestroy {

  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  // string variables
  clientParticipantsText: string;
  searchText: string;
  excelToolTip: string;
  srNo: string;
  nameLabel: string;
  designationLabel: string; // Variable for designation
  emailLabel: string; // Variable for email
  showingResults: string; // Variable for showing results
  deleteToolTip: string; // Variable for delete tooltip
  editToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for title
  showNoDataText: string;

  clientParticipantsList = [] as Array<EntityUserMappingAC>;
  selectedEntityId;
  startItem: number;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  baseUrl: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    public router: Router,
    private toaster: ToastrService,
    private clientParticipantService: ClientParticipantsService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.clientParticipantsText = this.stringConstants.clientParticipantsText;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.srNo = this.stringConstants.srNo;
    this.nameLabel = this.stringConstants.nameLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;
    this.clientParticipantsList = [];
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
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /**
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getAllClientParticipants(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId);
  }


  /***
   * Search client participant basis of name
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchClientParticipant(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.loaderService.open();
      pageNumber = 1;
      this.getAllClientParticipants(pageNumber, itemsPerPage, searchValue, this.selectedEntityId);
    }
  }

  /**
   * Get all client participant under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   */
  getAllClientParticipants(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string) {
    this.clientParticipantService.clientParticipantsGetAllClientParticipants(pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<EntityUserMappingAC>) => {
        this.loaderService.close();
        this.clientParticipantsList = result.items;

        // calculate serial no
        this.setSerialNo();

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.itemsPerPage, this.totalRecords);
      }, error => {
          this.handleError(error);
      });
  }

  /**
   * Open client participant add modal for adding client participant
   */
  openClientParticipantAddModal() {
    const initialState = {
      title: this.clientParticipantsText,
      keyboard: true,
      clientObject: {} as EntityUserMappingAC,
      callback: (result) => {
        if (result !== undefined) {
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        }
      },
    };

    this.bsModalRef = this.modalService.show(ClientParticipantsAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Open Client participant edit modal for updateing details
   * @param clientParticipantId : Seleted client participant Id
   * @param index : Particular index of a list
   */
  openClientParticipantEditodal(clientParticipantId: string, index: number) {
    this.loaderService.open();
    this.clientParticipantService.clientParticipantsGetClientParticipantById(clientParticipantId, this.selectedEntityId)
      .subscribe((response) => {
        this.loaderService.close();
        const initialState = {
          title: this.clientParticipantsText,
          keyboard: true,
          userId: response.id,
          clientObject: response,
          callback: (result) => {
            if (result !== undefined) {
              // update particular value of the entry
              this.clientParticipantsList.find(x => x.id === clientParticipantId).user = JSON.parse(JSON.stringify(result));

              this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);

            }
          },
        };
        this.bsModalRef = this.modalService.show(ClientParticipantsAddComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));

      },
        error => {
          this.handleError(error);
        });
  }

  /**
   * Set serial no of a list
   */
  setSerialNo() {
    // calculate serial no
    const startindex = (this.pageNumber !== 1) ? (this.itemsPerPage * (this.pageNumber - 1)) + 1 : 1;
    for (let i = 0; i < this.clientParticipantsList.length; i++) {
      this.clientParticipantsList[i].srNo = startindex + i;
    }
  }


  /**
   * Open client participant delete confirmation modal
   * @param clientParticipantId : Client participant Id that need to be deleted
   * @param index : Index of the entry you want to delete
   */
  // Method that open delete confirmation modal
  openDeleteModal(clientParticipantId: string, index: number) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,

      callback: (result) => {
        if (result === this.stringConstants.yes) {

          this.loaderService.open();
          this.clientParticipantService.clientParticipantsDeleteClientParticipant(clientParticipantId, this.selectedEntityId)
            .subscribe(() => {
              this.loaderService.close();
              this.clientParticipantsList.splice(index, 1);
              this.setSerialNo();

              // check if it is this the last item of the current page then go back to previous page
              if (this.clientParticipantsList.length === 0 && this.pageNumber > 1) {
                this.pageNumber = this.pageNumber - 1;
              }

              this.loadPageData();

              // success message
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
    if (this.clientParticipantsList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.clientParticipantsList.length) {
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
   * Method for export to excel of client participant
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAuditClientParticipantApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}
