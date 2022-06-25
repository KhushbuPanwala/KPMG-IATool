import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { MultiSelectService, Rcm } from '../../../core/multi-select.service';
import { map } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { RiskControlMatrixAC } from '../../../swaggerapi/AngularFiles/model/riskControlMatrixAC';
import { LoaderService } from '../../../core/loader.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../../core/shared.service';
import { RiskControlMatrixesService } from '../../../swaggerapi/AngularFiles/api/riskControlMatrixes.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-work-program-rcm',
  templateUrl: './work-program-rcm.component.html'
})
export class WorkProgramRcmComponent implements OnInit, OnDestroy {
  rcm: Rcm[] = []; // Variable for Rcm multiselect array
  selectRCM: string; // variable for select rcm
  addToolTip: string; // Variable for add tooltip
  subProcessLabel: string; // Variable for sub process label
  riskDescription: string; // Variable for risk description
  antiFraudControl: string; // Variable for antiFraud control
  controlDescription: string; // Variable for control description
  controlCategory: string; // Variable for control ctaegory
  controlType: string; // Variable for control type
  natureOfControl: string; // Variable for nature of course
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  saveButtonText: string; // Variable for save button Text
  showNoDataText: string;
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number;
  workProgramId: string;
  pageItems = []; // Per page items for entity list
  rcmSearchList: RiskControlMatrixAC[];
  rcmSearchUnEditedList: RiskControlMatrixAC[];
  selectedRcm: string[];
  newRcmAddList: RiskControlMatrixAC[] = [];
  @Output() rcmEdit = new EventEmitter<string>();
  @Output() rcmId = new EventEmitter<string>();

  @Input() addedWorkProgramId: string;

  // Work program rcm list array
  workProgramRCMList: RiskControlMatrixAC[];
  rcmUnEditedList: RiskControlMatrixAC[];
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  selectedEntityId: string;

  // Creates an instance of documenter
  constructor(
    private multiselctService: MultiSelectService,
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private loaderService: LoaderService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private riskControlMatrixService: RiskControlMatrixesService) {
    this.selectRCM = this.stringConstants.selectRCM;
    this.addToolTip = this.stringConstants.addToolTip;
    this.subProcessLabel = this.stringConstants.subProcessLabel;
    this.riskDescription = this.stringConstants.riskDescription;
    this.controlDescription = this.stringConstants.controlDescription;
    this.controlCategory = this.stringConstants.controlCategory;
    this.controlType = this.stringConstants.controlType;
    this.natureOfControl = this.stringConstants.natureOfControl;
    this.antiFraudControl = this.stringConstants.antiFraudControl;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.selectedRcm = [];
    this.workProgramRCMList = [];
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.multiselctService.getRcm()
          .pipe(map(x => x.filter(y => !y.disabled)))
          .subscribe((res) => {
            this.rcm = res;
          });

        this.route.params.subscribe(params => {
          if (this.addedWorkProgramId !== '') {
            this.workProgramId = this.addedWorkProgramId;
          } else {
            this.workProgramId = params.id;

          }

          if (this.workProgramId !== undefined) {
            this.getRcmList(this.workProgramId, this.pageNumber, this.selectedPageItem, this.searchValue);
          }
        });
        this.getRcmSearchList();
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
   * Method to get RCM list
   * @param workProgamId: Work program Id
   * @param pageNumber: Current page number
   * @param selectedPageItem: Selected Page Item
   * @param searchValue: Searched value if any
   */
  async getRcmList(workProgamId: string, pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.loaderService.open();
    this.riskControlMatrixService.riskControlMatrixesGetRCMListForWorkProgram(workProgamId, pageNumber,
      selectedPageItem, searchValue, this.selectedEntityId).subscribe(result => {
        this.workProgramRCMList = JSON.parse(JSON.stringify(result.items));
        this.rcmUnEditedList = JSON.parse(JSON.stringify(result.items));
        for (const rcm of this.workProgramRCMList) {
          rcm.riskDescription = rcm.riskDescription.replace(/<[^>]*>/g, '');
          rcm.controlDescription = rcm.controlDescription.replace(/<[^>]*>/g, '');
        }
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
        this.loaderService.close();
      },
        (error) => {
          this.sharedService.handleError(error);
        });
  }

  /**
   * Method to open delete confirmation dialogue
   * @param rcmId: rcm id
   */
  openDeleteModal(rcmId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.newRcmAddList = [];
            this.newRcmAddList.push(this.rcmUnEditedList.filter(x => x.id === rcmId)[0]);
            this.newRcmAddList[0].workProgramId = null;
            this.updateRCMList(true);
          }
        }
      }
    });
  }

  /**
   * Methdod to get Rcm list for search dropdown
   */
  getRcmSearchList() {
    this.loaderService.open();

    this.riskControlMatrixService.riskControlMatrixesGetRCMListForWorkProgram('', 0,
      0, '', this.selectedEntityId).subscribe(result => {
        this.rcmSearchList = JSON.parse(JSON.stringify(result.items));
        this.rcmSearchUnEditedList = JSON.parse(JSON.stringify(result.items));
        for (const rcm of this.rcmSearchList) {
          rcm.riskDescription = rcm.riskDescription.replace(/<[^>]*>/g, '');
        }
        this.loaderService.close();

      }, (error) => {
          this.sharedService.handleError(error);
      });
  }

  /**
   * Method to add RCM to workprogram
   */
  onAddRcmClick() {
    if (this.selectedRcm.length > 0) {
      for (const rcm of this.selectedRcm) {
        this.newRcmAddList.push(this.rcmSearchUnEditedList.filter(x => x.id === rcm)[0]);
      }
      for (const rcm of this.newRcmAddList) {
        rcm.workProgramId = this.workProgramId;
      }
      this.updateRCMList(false);
    }
  }

  /**
   * Method to add or delete RCM from workprogram
   * @param isdeleted: Is deleted boolean
   */
  updateRCMList(isdeleted: boolean) {
    this.loaderService.open();

    // set bit to delete user response of deleted rcm
    if (isdeleted) {
      this.newRcmAddList.forEach(x => {
        x.isToDelete = true;
      });
    }
    this.riskControlMatrixService.riskControlMatrixesUpdateRiskControlMatrix(this.newRcmAddList, this.selectedEntityId).subscribe(() => {
      this.getRcmList(this.workProgramId, this.pageNumber, this.selectedPageItem, this.searchValue);
      this.getRcmSearchList();
      this.newRcmAddList = [];
      this.selectedRcm = [];
      this.loaderService.close();
      if (isdeleted) {
        this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      } else {
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
      }
    }, (error) => {
        this.sharedService.handleError(error);
    });
  }

  /**
   * Method on edit click
   * @param id: Rcm Id
   */
  onEditClick(id: string) {
    this.rcmEdit.emit(id);
    this.rcmId.emit(id);
  }
  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getRcmList(this.workProgramId, pageNumber, this.selectedPageItem, this.searchValue);
  }
  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRcmList(this.workProgramId, null, this.selectedPageItem, this.searchValue);
  }
}
