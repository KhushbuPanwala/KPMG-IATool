import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../app/shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoaderService } from '../core/loader.service';
import { SharedService } from '../core/shared.service';
import { TabDirective } from 'ngx-bootstrap/tabs';
import { BASE_PATH } from '../swaggerapi/AngularFiles';

@Component({
  selector: 'app-observation',
  templateUrl: './observation-management.component.html',
  styleUrls: ['./observation-management.component.scss']
})

export class ObservationComponent implements OnInit {
  observationManagementTitle: string; // Variable for observation main title
  backToolTip: string; // Vairable for back button tooltip
  wordToolTip: string; // Variable for word alt text
  powerPointToolTip: string; // Variable for Powerpoint alt text
  pdfToolTip: string; // Variable for pdf alt text
  observationTabTitle: string; // Variable for observation tab title
  managementCommentsTitle: string; // Varibale for management comments title
  downloadToolTip: string; // Variable for download button
  selectedPageItem: number;
  searchValue: string;
  observationId: string;
  isObservationAdded: boolean;
  auditableEntityId: string;
  isShowManagementTab: boolean;
  isShowObservationTab: boolean;
  rcmId: string;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, private router: Router, private toastr: ToastrService,
              private route: ActivatedRoute, private sharedService: SharedService, private loaderService: LoaderService) {
    this.observationManagementTitle = this.stringConstants.observationManagementTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.managementCommentsTitle = this.stringConstants.managementCommentsTitle;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.isObservationAdded = false;
    this.auditableEntityId = '';
    this.isShowObservationTab = true;
    this.isShowManagementTab = false;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
    *  Initialization of properties.
    */
  ngOnInit(): void {
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.auditableEntityId = entityId;
        this.route.params.subscribe(params => {
          this.observationId = params.observationId;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
          this.rcmId = params.rcmId;
        });
      }
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
    this.router.navigate(['observation-management/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * On add new observation
   * @param observationId: Id of observation
   */
  onAddObservation(observationId: string) {
    this.observationId = observationId;
    this.isObservationAdded = true;
  }

  /**
   * Method checking active tab
   * @param data : Data of tab
   */
  onSelect(data: TabDirective) {
    if (data.heading === this.managementCommentsTitle) {
      this.isShowManagementTab = true;
    } else {
      this.isShowObservationTab = true;
    }
  }

}
