import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EntityRelationMappingsService, AuditableEntityAC, RelationshipTypeAC, UserAC, UserRole } from '../../swaggerapi/AngularFiles';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { Pagination } from '../../models/pagination';
import { EntityRelationMappingAC } from '../../swaggerapi/AngularFiles/model/entityRelationMappingAC';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-relationship',
  templateUrl: './relationship.component.html'
})
export class RelationshipComponent implements OnInit, OnDestroy {
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  relationshipTitle: string; // Varible for relationship
  backToolTip: string; // Variable for back tooltip
  pdfToolTip: string; // Variable for pdf tooltip
  selectRelationshipType: string; // Variable for select relationship type
  selectEntityTitle: string; // Variable for select entity title
  applyButton: string; // Variable for apply button text
  relationshipTypeLabel: string; // Variable for relationship type
  entityTitle: string; // Variable for entity title
  saveNextButtonText: string; // Variable for next button
  previousButton: string; // Variable for previous button
  selectedPageItem: number;
  searchValue: string;
  entityId: string;
  selectedEntityId: string;

  pageNumber: number = null;
  totalRecords: number;
  relationSelectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;
  relationSearchValue: string;
  dropdownDefaultValue: string;
  entityRelationMappingId: string;
  version: string;

  entityRelationMappingDetails: EntityRelationMappingAC;
  entityRelationMappingList: EntityRelationMappingAC[] = [];
  auditableEntityList: AuditableEntityAC[] = [];
  relationShipTypeList: RelationshipTypeAC[] = [];
  userSubscribe: Subscription;
  entitySubscribe: Subscription;
  currentUserDetails: UserAC;

  constructor(
    private stringConstants: StringConstants,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private entityRelationMappingsService: EntityRelationMappingsService,
    private modalService: BsModalService) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.relationshipTitle = this.stringConstants.relationshipTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.selectEntityTitle = this.stringConstants.selectEntityTitle;
    this.selectRelationshipType = this.stringConstants.selectRelationshipType;
    this.applyButton = this.stringConstants.applyButton;
    this.entityTitle = this.stringConstants.entityTitle;
    this.relationshipTypeLabel = this.stringConstants.relationshipTypeLabel;
    this.previousButton = this.stringConstants.previousButton;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.pageItems = this.stringConstants.pageItems;
    this.relationSelectedPageItem = this.pageItems[0].noOfItems;
    this.entityRelationMappingDetails = {} as EntityRelationMappingAC;
    this.entityRelationMappingId = '';
    this.version = this.stringConstants.auditPlanVersion;

  }
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
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
          // get the current selectedEntityId
          this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
            if (entityId !== '') {
              this.selectedEntityId = entityId;
              this.route.params.subscribe(params => {
                this.selectedPageItem = params.pageItems;
                this.searchValue = params.searchValue;
                if (params.id !== undefined) {
                  this.entityId = params.id;
                  this.getEntityRelationMappingList(this.pageNumber, this.relationSelectedPageItem, this.relationSearchValue, this.entityId);
                }
              });
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
    if (this.entitySubscribe !== undefined) {
      this.entitySubscribe.unsubscribe();
    }
    this.userSubscribe.unsubscribe();
  }

  /**
   * Get PrimaryGeographicalArea list of list page
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   * @param id: entityid
   */
  getEntityRelationMappingList(pageNumber: number, selectedPageItem: number, searchValue: string, id: string) {
    this.loaderService.open();
    this.entityRelationMappingsService.entityRelationMappingsGetEntityRelationMappingList(pageNumber,
      selectedPageItem, searchValue, id).subscribe((result: Pagination<EntityRelationMappingAC>) => {

        this.auditableEntityList = JSON.parse(JSON.stringify(result.items[0].auditableEntityACList));
        this.relationShipTypeList = JSON.parse(JSON.stringify(result.items[0].relationshipTypeACList));

        if (result.items[0].relatedEntityName === null) {
          this.entityRelationMappingList = [];
        } else {
          this.entityRelationMappingList = result.items;
        }

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
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getEntityRelationMappingList(pageNumber, this.relationSelectedPageItem, this.relationSearchValue, this.entityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getEntityRelationMappingList(null, this.relationSelectedPageItem, this.relationSearchValue, this.entityId);
  }

  /**
   * Search PrimaryGeographicalArea on grid
   * @param event: key event tab and enter
   */
  searchEntityRelationMapping(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.relationSearchValue = this.relationSearchValue.trim();
      this.getEntityRelationMappingList(null, this.selectedPageItem, this.relationSearchValue, this.entityId);
    }
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Delete EntityRelationMapping from List
   * @param entityRelationMappingId: entityRelationMapping Id
   */
  deleteEntityRelationMapping(entityRelationMappingId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.entityRelationMappingsService.entityRelationMappingsDeleteEntityRelationMapping(entityRelationMappingId).subscribe(() => {
              this.loaderService.close();
              this.getEntityRelationMappingList(null, this.relationSelectedPageItem, this.relationSearchValue, this.entityId);
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
   * On save click
   */
  onSaveClick() {
    if ((this.entityRelationMappingDetails.relatedEntityId === undefined || this.entityRelationMappingDetails.relatedEntityId === null) && (this.entityRelationMappingDetails.relationTypeId === undefined || this.entityRelationMappingDetails.relationTypeId === null)) {
      this.sharedService.showError(this.stringConstants.relationErrorOnAdd);
      return;
    }
    if (this.entityRelationMappingDetails.relatedEntityId === undefined || this.entityRelationMappingDetails.relatedEntityId === null) {
      this.sharedService.showError(this.stringConstants.noEntityErrorOnAdd);
      return;
    }
    if (this.entityRelationMappingDetails.relationTypeId === undefined || this.entityRelationMappingDetails.relationTypeId === null) {
      this.sharedService.showError(this.stringConstants.noRelationErrorOnAdd);
      return;
    }

    this.loaderService.open();
    this.entityRelationMappingDetails.entityId = this.entityId;
    if (this.entityRelationMappingId === '') {
      this.entityRelationMappingsService.entityRelationMappingsAddEntityRelationMapping(this.entityRelationMappingDetails).subscribe(() => {
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.entityRelationMappingDetails = {} as EntityRelationMappingAC;
        this.entityRelationMappingId = '';
        this.loaderService.close();
        this.getEntityRelationMappingList(null, this.selectedPageItem, this.relationSearchValue, this.entityId);
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    } else {
      this.entityRelationMappingsService.entityRelationMappingsUpdateEntityRelationMapping(this.entityRelationMappingDetails).subscribe(() => {
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.loaderService.close();
        this.entityRelationMappingId = '';
        this.entityRelationMappingDetails = {} as EntityRelationMappingAC;
        this.getEntityRelationMappingList(null, this.selectedPageItem, this.relationSearchValue, this.entityId);
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    }
  }

  /**
   * On edit click
   * @param id: entity relation type mapping id
   */
  onEditClick(id: string) {
    this.entityRelationMappingDetails = {} as EntityRelationMappingAC;
    this.entityRelationMappingId = id;
    this.entityRelationMappingDetails = this.entityRelationMappingList.filter(x => x.id === id)[0];
  }

  /**
   * On save and next click
   */
  onSaveAndNextClick() {
    this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
    this.router.navigate(['auditable-entity/documents', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }

  /**
   * On previous button route to list page
   */
  onPreviousClick() {
    this.router.navigate(['auditable-entity/risk-assesment', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }
}
