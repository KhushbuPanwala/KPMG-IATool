import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { RcmSectorService } from '../../../swaggerapi/AngularFiles/api/rcmSector.service';
import { ActivatedRoute, Router } from '@angular/router';
import { RcmSectorAC } from '../../../swaggerapi/AngularFiles/model/rcmSectorAC';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sector-add',
  templateUrl: './sector-add.component.html',
})
export class SectorAddComponent implements OnInit, OnDestroy {
  rcmSectorTitle: string; // Variable for rcm sector title
  textAreaPlaceHolder: string; // Variable for textarea placeholder
  saveButtonText: string; // Variable for save button
  backToolTip: string; // Variable for back tooltip

  sectorId: string;
  rcmSector: RcmSectorAC;
  selectedPageItem: number;
  searchValue: string;

  invalidMessage: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  selectedEntityId;

  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private route: ActivatedRoute,
    private apiService: RcmSectorService,
    private loaderService: LoaderService,
    public router: Router,
    private sharedService: SharedService) {
    this.rcmSectorTitle = this.stringConstants.rcmSectorTitle;
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
          this.sectorId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;

        });
        if (this.sectorId !== '0') {
          this.getSectorById();
        } else {
          this.rcmSector = {} as RcmSectorAC;
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
   * Get RCM Sector detail by id for edit
   */
  getSectorById() {
    this.loaderService.open();
    this.apiService.rcmSectorGetRcmSectorById(this.sectorId, this.selectedEntityId).subscribe(result => {
      this.rcmSector = result;
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Add and update RCM Sector
   */
  saveRcmSector() {
    this.rcmSector.entityId = this.selectedEntityId;
    this.loaderService.open();
    if (this.rcmSector.id === undefined) {
      this.apiService.rcmSectorAddRcmSector(this.rcmSector, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.rcmSector = result;
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.setListPageRoute();
      }, (error) => {
        this.loaderService.close();
        this.sharedService.handleError(error);
      });
    } else {
      this.apiService.rcmSectorUpdateRcmSector(this.rcmSector, this.selectedEntityId).subscribe(result => {
        this.loaderService.close();
        this.rcmSector = result;
        this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
        this.setListPageRoute();

      }, (error) => {
        this.loaderService.close();
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
    this.router.navigate(['sector/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
