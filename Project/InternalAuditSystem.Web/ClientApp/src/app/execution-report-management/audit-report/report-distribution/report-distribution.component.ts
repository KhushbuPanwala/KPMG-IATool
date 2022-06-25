import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { DistributorsAC, ReportUserMappingAC, ReportDistributorsService } from '../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-report-distribution',
  templateUrl: './report-distribution.component.html'
})
export class ReportDistributionComponent implements OnInit, OnDestroy {

  distributorListTitle: string; // Variable for distribution
  backToolTip: string; // Variable for backtooltip
  selectDistributor: string; // Variable for select distributor
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation label
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  saveNextButtonText: string; // Variable for save button text
  selectedDistributor: string;
  selectedName: string; // selected name
  showNoDataText: string;
  selectedPageItem: number;
  searchValue: string;
  usersList = [] as Array<DistributorsAC>;
  distributorsList = [] as Array<ReportUserMappingAC>;

  pageNumber: number = null;
  reportDistributor = {} as ReportUserMappingAC;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  reportId: string;
  selectedEntityId;
  operationType: string;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              private route: ActivatedRoute,
              private apiService: ReportDistributorsService,
              private router: Router,
              private sharedService: SharedService) {
    this.distributorListTitle = this.stringConstants.distributorListPageTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.selectDistributor = this.stringConstants.selectDistributor;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.showNoDataText = this.stringConstants.showNoDataText;
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.reportId = params.id;
          this.operationType = params.type;
        });
        this.getDistributors();
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

  /***
   * Get Distributors Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  getDistributors() {
    this.apiService.reportDistributorsGetDistributorsByReportId(this.selectedEntityId, this.reportId)
      .subscribe(result => {
        this.usersList = result.distributorsList;
        this.distributorsList = result.reportDistributorsList;
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Add user in distributor list
   * @param userId: selected user id
   */
  addDistributor(userId: string) {
    if (userId === undefined || userId === '') {
      return;
    }
    const distributor = this.distributorsList.find(a => a.userId === userId);
    if (distributor !== undefined) {
      this.sharedService.showError(this.stringConstants.userExistMsg);
      return;
    }
    const userDetail = this.usersList.find(x => x.userId === userId);
    this.reportDistributor.userId = userDetail.userId;
    this.reportDistributor.name = userDetail.name;
    this.reportDistributor.designation = userDetail.designation;
    this.reportDistributor.reportId = this.reportId;
    this.distributorsList.push(this.reportDistributor);
    this.reportDistributor = {} as ReportUserMappingAC;
    this.selectedDistributor = '';
  }

  /**
   * Delete user from distributor list
   * @param userId: deleted user id
   */
  deleteDistributor(userId: string) {
    const userDetailIndex = this.distributorsList.findIndex(x => x.userId === userId);
    this.distributorsList.splice(userDetailIndex, 1);
    this.selectedDistributor = '';
  }

  /**
   * Add Distributor
   */
  saveDistributor() {
    this.apiService.reportDistributorsAddReportDistributors(this.distributorsList, this.selectedEntityId).subscribe(result => {
      if (this.operationType === this.stringConstants.addOperationText) {
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
      } else {
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
      }
      this.router.navigate(['report/observation-list', { id: this.reportId, type: this.operationType }]);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['report/add', { id: this.reportId, type: this.stringConstants.editOperationText }]);
  }
}
