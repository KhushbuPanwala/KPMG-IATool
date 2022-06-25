import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { RcmSubProcessAC } from '../../../../swaggerapi/AngularFiles/model/rcmSubProcessAC';
import { ActivatedRoute, Router } from '@angular/router';
import { RcmSubProcessService } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rcm-sub-process-add',
  templateUrl: './rcm-sub-process-add.component.html'
})
export class RcmSubProcessAddComponent implements OnInit, OnDestroy {

  rcmSubProcessTitle: string; // Variable for rcm process title
  textAreaPlaceHolder: string; // Variable for textarea placeholder
  saveButtonText: string; // Variable for save button
  backToolTip: string; // Variable for back tooltip

  subProcessId: string;
  rcmSubProcess: RcmSubProcessAC;
  selectedPageItem: number;
  searchValue: string;

  invalidMessage: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectedEntityId;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              private route: ActivatedRoute,
              private apiService: RcmSubProcessService,
              public router: Router,
              private loaderService: LoaderService,
              private sharedService: SharedService) {
    this.rcmSubProcessTitle = this.stringConstants.subProcessLabel;
    this.textAreaPlaceHolder = this.stringConstants.textAreaPlaceHolder;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.backToolTip = this.stringConstants.backToolTip;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.subProcessId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        if (this.subProcessId !== '0') {
          this.getSubProcessById();
        } else {
          this.rcmSubProcess = {} as RcmSubProcessAC;
        }
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
   * Get RCM SubProcess detail by id for edit
   */
  getSubProcessById() {
    this.loaderService.open();
    this.apiService.rcmSubProcessGetRcmSubProcessById(this.subProcessId, this.selectedEntityId).subscribe(result => {
      this.rcmSubProcess = result;
      this.loaderService.close();
    },
      (error) => {
        this.sharedService.handleError(error);
      });
  }
  /**
   * Add and update RCM SubProcess
   */
  saveRcmSubProcess() {
    this.rcmSubProcess.entityId = this.selectedEntityId;
    if (this.rcmSubProcess.id === undefined) {
      this.apiService.rcmSubProcessAddRcmSubProcess(this.rcmSubProcess, this.selectedEntityId).subscribe(result => {
        this.rcmSubProcess = result;
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.setListPageRoute();
      },
        (error) => {
          this.sharedService.handleError(error);
        });
    } else {
      this.apiService.rcmSubProcessUpdateRcmSubProcess(this.rcmSubProcess, this.selectedEntityId).subscribe(result => {
        this.rcmSubProcess = result;
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.setListPageRoute();
      },
        (error) => {
          this.sharedService.handleError(error);
        });
    }
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
    this.router.navigate(['rcm-sub-process/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
