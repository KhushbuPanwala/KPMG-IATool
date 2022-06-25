import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-work-program-add',
  templateUrl: './work-program-add.component.html',
})
export class WorkProgramAddComponent implements OnInit {
  auditTitle: string; // Variable for audit title
  backToolTip: string; // Variable for back tooltip
  addMomTitle: string; // Variable for add mom tab
  rcmTitle: string; // Variable for rcm title
  selectedPageItem: number;
  searchValue: string;
  rcmGuid: string;
  workProgramId: string;
  samplingGuid: string;

  isRcmListPage: boolean;
  isRcmEditPage: boolean;
  isSamplingList: boolean;
  isWorkProgramAdded: boolean;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public router: Router,
    private route: ActivatedRoute,
  ) {
    this.auditTitle = this.stringConstants.auditTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.addMomTitle = this.stringConstants.addMomTitle;
    this.rcmTitle = this.stringConstants.rcmTitle;
    this.isRcmListPage = false;
    this.isRcmEditPage = true;
    this.isSamplingList = false;
    this.isWorkProgramAdded = false;
  }

  /**
   * On add new workprogram
   * @param workProgramId: workProgramId
   */
  onAddWorkProgram(workProgramId: string) {
    this.workProgramId = workProgramId;
    this.isWorkProgramAdded = true;
  }
  /**
   * Method for edit click
   * @param id: rcm Id
   */
  onRcmEditClick(id: string) {
    this.rcmGuid = id;
    this.isRcmEditPage = false;
    this.isRcmListPage = true;
    this.isSamplingList = false;
  }

  /**
   * Method on cancel rcm edit
   * @param isRcmListPage: Is RCM list page or not
   */
  onRcmListCancel(isRcmListPage: boolean) {
    this.rcmGuid = '';
    this.isRcmEditPage = true;
    this.isRcmListPage = isRcmListPage;
    this.isSamplingList = false;
  }

  /**
   * On add sampling click
   * @param isSamplingPage: Is sampling page
   */
  onAddSamplingClick(isSamplingPage: boolean) {
    this.isRcmEditPage = false;
    this.isRcmListPage = false;
    this.isSamplingList = isSamplingPage;
  }
  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.workProgramId = params.id;
      this.selectedPageItem = params.pageItems;
      this.searchValue = params.searchValue;
    });
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['work-program/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
