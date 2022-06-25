import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { EntityDocumentsAddComponent } from './entity-documents-add/entity-documents-add.component';
import { StringConstants } from '../../shared/stringConstants';
import { EntityDocumentAC, EntityDocumentsService, UserRole, UserAC } from '../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { UploadService } from '../../core/upload.service';
import { Pagination } from '../../models/pagination';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-entity-documents',
  templateUrl: './entity-documents.component.html'
})
export class EntityDocumentsComponent implements OnInit, OnDestroy {
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  documentsTitle: string; // Variable for documents title
  purposeTitle: string; // Variable for purpose title
  downloadToolTip: string; // Variable for tooltip
  viewToolTip: string; // Variable for view tooltip
  saveButtonText: string;   // Variable for savaAs text
  previousButton: string; // Variable for previous button
  existingVersionLabel: string; // VAriable for exsting Version Label
  newVersionLabel: string; // Variable for new version label

  selectedPageItem: number;
  searchValue: string;
  entityId: string;
  selectedEntityId: string;
  saveNextButtonText: string;

  pageNumber: number = null;
  totalRecords: number;
  documentSearchValue = null;
  documentSelectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  entityDocuemntList: EntityDocumentAC[] = [];
  userSubscribe: Subscription;
  currentUserDetails: UserAC;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private entityDocumentsService: EntityDocumentsService) {
    this.documentsTitle = this.stringConstants.documentsTitle;
    this.purposeTitle = this.stringConstants.purposeTitle;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.viewToolTip = this.stringConstants.viewToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.previousButton = this.stringConstants.previousButton;
    this.existingVersionLabel = this.stringConstants.existingVersionLabel;
    this.newVersionLabel = this.stringConstants.newVersionLabel;
    this.showNoDataText = this.stringConstants.showNoDataText;

    this.pageItems = this.stringConstants.pageItems;
    this.documentSelectedPageItem = this.pageItems[0].noOfItems;
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
    // current logged in user details
    this.userSubscribe = this.sharedService.currentUserDetailsSubject.subscribe((currentUserDetails) => {
      if (currentUserDetails !== null) {
        this.currentUserDetails = currentUserDetails.userDetails;
        // if current user is team member restrict access
        if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
          this.router.navigate([this.stringConstants.unauthorizedPath]);
        } else {
          this.route.params.subscribe(params => {
            this.selectedPageItem = params.pageItems;
            this.searchValue = params.searchValue;
            if (params.id !== undefined) {
              this.entityId = params.id;
              this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);
            }
          });
        }
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.userSubscribe.unsubscribe();
  }

  /**
   * Get EntityDoucment list for list page
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   * @param id: entityid
   */
  getEntityDocumentList(pageNumber: number, selectedPageItem: number, searchValue: string, id: string) {
    this.loaderService.open();
    this.entityDocumentsService.entityDocumentsGetEntityDocumentList(id, pageNumber,
      selectedPageItem, searchValue).subscribe((result: Pagination<EntityDocumentAC>) => {

        this.entityDocuemntList = result.items;
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);

        this.loaderService.close();
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
  }

  /**
   * Method that open EntityDocument add/update modal
   * @param id: id if in edit else blank
   */
  openEntityDocumentAddModal(id: string) {
      const initialState = {
        entityId: this.entityId,
        entityDocumentId: id,
        backdrop: 'static',
        keyboard: false,
        callback: (result: EntityDocumentAC) => {
          if (result !== undefined) {
            this.bsModalRef.hide();
            if (id === '') {

              this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);

              this.totalRecords = this.totalRecords + 1;
              // set footer showing message
              this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.documentSelectedPageItem, this.totalRecords);

              this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
            } else {
              // update particular value of the entry
              this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);

            }
          }
        }
      };
      this.bsModalRef = this.modalService.show(EntityDocumentsAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  // Method that open delete confirmation modal
  openDeleteModal(id: string) {
    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.stringConstants.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (id !== '') {

              this.loaderService.open();
              this.entityDocumentsService.entityDocumentsDeleteEntityDocumentAync(id).subscribe(() => {
                this.loaderService.close();
                this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);

                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
              }, (error) => {
                this.sharedService.showError(this.stringConstants.somethingWentWrong);
                this.loaderService.close();
              });
            }
          }
        }
      }
    });
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);
  }

  /**
   * Search PrimaryGeographicalArea on grid
   * @param event: key event tab and enter
   */
  searchEntityDocument(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.documentSearchValue = this.documentSearchValue.trim();
      this.getEntityDocumentList(this.pageNumber, this.documentSelectedPageItem, this.documentSearchValue, this.entityId);
    }
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * on save button route to list page
   */
  onSaveClick() {
    this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method to open document
   * @param documnetPath: documnetPath
   */
  openDocument(entityDocumentId: string) {
    this.loaderService.open();
    this.entityDocumentsService.entityDocumentsGetEntityDocumentDownloadUrl(entityDocumentId).subscribe((result) => {
      this.loaderService.close();
      // Open document in new tab
      const url = this.router.serializeUrl(
        // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
        this.router.createUrlTree(['document-preview/view', { path: btoa(result) }])
      );
      window.open(url, '_blank');
    });

  }

  /**
   * Method to download entityDocument
   * @param entityDocumentId: entityDocumentId
   */
  downloadEntityDocumentDoc(entityDocumentId: string) {
    this.loaderService.open();
    this.entityDocumentsService.entityDocumentsGetEntityDocumentDownloadUrl(entityDocumentId).subscribe((result) => {
      this.loaderService.close();
      const a = document.createElement('a');
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      a.download = '';
      a.href = result;
      a.target = '_blank';
      a.click();
      document.body.removeChild(a);
    });
  }
  /**
   * On previous button route to list page
   */
  onPreviousClick() {
    this.router.navigate(['auditable-entity/relationship', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }
}
