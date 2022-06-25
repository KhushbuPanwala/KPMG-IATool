import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { AuditableEntitiesService, AuditableEntityAC, AuditableEntityStatus, UserAC, UserRole } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-auditable-entity-detail',
  templateUrl: './auditable-entity-detail.component.html'
})
export class AuditableEntityDetailComponent implements OnInit, OnDestroy {
  detailsTitle: string;
  backToolTip: string;
  saveNextButtonText: string;
  nameLabel: string;
  statusTitle: string;
  versionTitle: string;
  descriptionTitle: string;
  statusButtonText: string;
  isStatusChanged: boolean;
  selectedPageItem: number;
  searchValue: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  invalidMessage: string;

  auditableEntityDetails: AuditableEntityAC;
  userSubscribe: Subscription;
  currentUserDetails: UserAC;

  // Point of discussion status array as swagger dont create enum properly
  statusList = [
    { value: AuditableEntityStatus.NUMBER_0, label: 'Active' },
    { value: AuditableEntityStatus.NUMBER_1, label: 'Update' },
    { value: AuditableEntityStatus.NUMBER_2, label: 'Closed' }
  ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditableEntityService: AuditableEntitiesService) {
    this.detailsTitle = this.stringConstants.detailsTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.nameLabel = this.stringConstants.nameLabel;
    this.statusTitle = this.stringConstants.statusTitle;
    this.descriptionTitle = this.stringConstants.descriptionTitle;
    this.versionTitle = this.stringConstants.versionTitle;
    this.statusButtonText = this.stringConstants.statusButtonCloseText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;

    this.isStatusChanged = false;

    this.auditableEntityDetails = {} as AuditableEntityAC;
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

            this.getAuditableEntityDetailById(params.id);
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
    this.auditableEntityService.auditableEntitiesGetAuditableEntityDetails(id, 1).subscribe((result: AuditableEntityAC) => {

      this.auditableEntityDetails = result;
      this.setStatus(this.auditableEntityDetails.status);

      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Set Status
   * @param status: AuditableEntityStatus
   */
  setStatus(status: AuditableEntityStatus) {
    if (AuditableEntityStatus.NUMBER_0 === status || AuditableEntityStatus.NUMBER_1 === status) {
      this.auditableEntityDetails.status = AuditableEntityStatus.NUMBER_1;
      this.auditableEntityDetails.statusString = this.statusList[AuditableEntityStatus.NUMBER_1].label;

      this.statusButtonText = this.stringConstants.statusButtonCloseText;
    } else {
      this.statusButtonText = this.statusList[AuditableEntityStatus.NUMBER_0].label;
    }
    this.isStatusChanged = false;
  }

  /**
   * Change AuditableEntity Status
   */
  changeAuditableEntityStatus() {
    if (this.auditableEntityDetails.status === AuditableEntityStatus.NUMBER_0 || this.auditableEntityDetails.status === AuditableEntityStatus.NUMBER_1) {
      this.auditableEntityDetails.status = AuditableEntityStatus.NUMBER_2;
      this.auditableEntityDetails.statusString = this.statusList[AuditableEntityStatus.NUMBER_2].label;
      this.isStatusChanged = true;
    } else {
      this.auditableEntityDetails.status = AuditableEntityStatus.NUMBER_1;
      this.auditableEntityDetails.statusString = this.statusList[AuditableEntityStatus.NUMBER_1].label;
      this.statusButtonText = this.stringConstants.statusButtonCloseText;
    }
  }

  /**
   * Add or update auditable entity
   */
  saveAuditableEntity() {
    this.loaderService.open();
    if (this.auditableEntityDetails.status === AuditableEntityStatus.NUMBER_1) {
      this.auditableEntityDetails.status = AuditableEntityStatus.NUMBER_0;
    }
    this.auditableEntityService.auditableEntitiesUpdateAuditableEntity(this.auditableEntityDetails).subscribe(() => {
      this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
      this.loaderService.close();
      this.router.navigate(['auditable-entity/classification', { id: this.auditableEntityDetails.id, pageItems: this.selectedPageItem, searchValue: this.searchValue}]);
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
}
