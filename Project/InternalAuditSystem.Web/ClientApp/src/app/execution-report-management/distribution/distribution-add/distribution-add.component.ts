import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { DistributorsService, EntityUserMappingAC } from '../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../core/shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-distribution-add',
  templateUrl: './distribution-add.component.html',
})
export class DistributionAddComponent implements OnInit, OnDestroy {
  updateDistributionList: string; // Variable for distribution
  backToolTip: string; // Variable for backtooltip
  selectDistributor: string; // Variable for select distributor
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation label
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  saveButtonText: string; // Variable for save button text
  usersList: EntityUserMappingAC[];
  selectedDistributor: string;
  selectedName: string; // selected name
  addDistributorsList: EntityUserMappingAC[] = [];
  showNoDataText: string;
  selectedPageItem: number;
  searchValue: string;
  selectedEntityId;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, private route: ActivatedRoute, private apiService: DistributorsService,
              public router: Router, private sharedService: SharedService) {
    this.updateDistributionList = this.stringConstants.updateDistributionList;
    this.backToolTip = this.stringConstants.backToolTip;
    this.selectDistributor = this.stringConstants.selectDistributor;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
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
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        this.getUsers();
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
   * Get Users detail by id for edit
   */
  getUsers() {
    this.apiService.distributorsGetUsers(this.selectedEntityId).subscribe(result => {
      this.usersList = result;
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
    const addedUser = this.addDistributorsList.find(a => a.userId === userId);
    if (addedUser !== undefined) {
      this.sharedService.showError(this.stringConstants.userExistMsg);
      return;
    }
    const userDetail = this.usersList.find(x => x.userId === userId);
    this.addDistributorsList.push(userDetail);
    this.selectedDistributor = '';
  }
  /**
   * Delete user from distributor list
   * @param userId: deleted user id
   */
  deleteDistributor(userId: string) {
    const userDetailIndex = this.addDistributorsList.findIndex(x => x.userId === userId);
    this.addDistributorsList.splice(userDetailIndex, 1);
    this.selectedDistributor = '';
  }

  /**
   * Add Distributor
   */
  saveDistributor() {
    this.apiService.distributorsAddDistributors(this.addDistributorsList, this.selectedEntityId).subscribe(result => {
      this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
      this.setListPageRoute();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.setListPageRoute();
  }

  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    this.router.navigate(['distribution/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
