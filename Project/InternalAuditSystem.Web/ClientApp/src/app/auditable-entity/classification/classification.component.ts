import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { EntityCategoryAC, EntityTypeAC, AuditableEntityAC, AuditableEntitiesService, UserAC, UserRole } from '../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-classification',
  templateUrl: './classification.component.html',
})
export class ClassificationComponent implements OnInit, OnDestroy {
  classification: string; // Variable for classification title
  backToolTip: string; // Variable for backtooltip
  saveNextButtonText: string; // Variable for next button
  previousButton: string; // Variable for previous button
  categoryLabel: string; // Variable for category label
  typeLabel: string; // Variable for type label
  dropdownDefaultValue: string;
  selectedEntityId: string;
  selectedPageItem: number;
  searchValue: string;
  entityId: string;

  auditableEntityDetails: AuditableEntityAC;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;
  categoryList: EntityCategoryAC[] = [];
  typeList: EntityTypeAC[] = [];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditableEntityService: AuditableEntitiesService) {
    this.classification = this.stringConstants.classification;
    this.backToolTip = this.stringConstants.backToolTip;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.previousButton = this.stringConstants.previousButton;
    this.categoryLabel = this.stringConstants.auditableEntityCategoryLabel;
    this.typeLabel = this.stringConstants.auditableEntityTypeLabel;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.auditableEntityDetails = {} as AuditableEntityAC;

    this.categoryList = [];
    this.typeList = [];
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
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
              // decode base 64 to string
              this.getAuditableEntityDetailById(params.id);
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
   * Get auditable entity details for edit page
   * @param id : auditableEntity id will come from query string
   */
  getAuditableEntityDetailById(id: string) {
    this.loaderService.open();
    this.auditableEntityService.auditableEntitiesGetAuditableEntityDetails(id, 2).subscribe((result: AuditableEntityAC) => {

      this.categoryList = result.entityCategoryACList;
      this.typeList = result.entityTypeACList;
      this.auditableEntityDetails = result;

      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Update auditable entity Classification
   */
  saveAuditableEntity() {
    this.loaderService.open();
    this.auditableEntityService.auditableEntitiesUpdateAuditableEntity(this.auditableEntityDetails).subscribe(() => {
      this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
      this.loaderService.close();
      this.router.navigate(['auditable-entity/geographical-area', { id: this.auditableEntityDetails.id, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * on previous button route to list page
   */
  onPreviousClick() {
    this.router.navigate(['auditable-entity/details', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }
}
