import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { RcmProcessAC } from '../../../swaggerapi/AngularFiles/model/rcmProcessAC';
import { ActivatedRoute, Router } from '@angular/router';
import { RcmProcessService } from '../../../swaggerapi/AngularFiles/api/rcmProcess.service';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-process-add',
  templateUrl: './process-add.component.html',
})
export class ProcessAddComponent implements OnInit, OnDestroy {
  rcmProcessTitle: string; // Variable for rcm process title
  textAreaPlaceHolder: string; // Variable for textarea placeholder
  saveButtonText: string; // Variable for save button
  backToolTip: string; // Variable for back tooltip

  // only to subscripe for the current component
  entitySubscribe: Subscription;

  processId: string;
  rcmProcess: RcmProcessAC;
  selectedPageItem: number;
  searchValue: string;

  invalidMessage: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectedEntityId;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              private route: ActivatedRoute,
              private apiService: RcmProcessService,
              public router: Router,
              private loaderService: LoaderService,
              private sharedService: SharedService) {
    this.rcmProcessTitle = this.stringConstants.rcmProcessTitle;
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
          this.processId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        if (this.processId !== '0') {
          this.getProcessById();
        } else {
          this.rcmProcess = {} as RcmProcessAC;
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
   * Get RCM Process detail by id for edit
   */
  getProcessById() {
    this.loaderService.open();
    this.apiService.rcmProcessGetRcmProcessById(this.processId, this.selectedEntityId).subscribe(result => {
      this.rcmProcess = result;
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Add and update RCM Process
   */
  saveRcmProcess() {
    this.rcmProcess.entityId = this.selectedEntityId;
    if (this.rcmProcess.id === undefined) {
      this.apiService.rcmProcessAddRcmProcess(this.rcmProcess, this.selectedEntityId).subscribe(result => {
        this.rcmProcess = result;
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      this.apiService.rcmProcessUpdateRcmProcess(this.rcmProcess, this.selectedEntityId).subscribe(result => {
        this.rcmProcess = result;
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.setListPageRoute();
      }, (error) => {
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
    this.router.navigate(['process/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}

