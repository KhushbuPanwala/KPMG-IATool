import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ActivatedRoute, Router } from '@angular/router';
import { Pagination } from '../../models/pagination';
import { PageChangedEvent } from 'ngx-bootstrap/pagination/public_api';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { LoaderService } from '../../core/loader.service';
import { DatePipe } from '@angular/common';
import { StrategicAnalysesService, StrategicAnalysisAC } from '../../swaggerapi/AngularFiles';
import { StrategicAnalysisAdminAddComponent } from '../strategic-analysis-admin-add/strategic-analysis-admin-add.component';
import { SharedService } from '../../core/shared.service';

@Component({
  selector: 'app-strategic-analysis-admin-list',
  templateUrl: './strategic-analysis-admin-list.component.html'
})
export class StrategicAnalysisAdminListComponent implements OnInit {
  strategicAnalysis: string; // Variable for strategic analysis title
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variable for edit tooltip
  addStratagicAnalysisToolTip: string; // Variable for add tooltip
  addSamplingToolTip: string; // Variable for add tooltip
  surveyTitle: string; // Variable for survey title
  deleteTitle: string; // Variable for title
  dateOfCreation: string; // Variable for title date creation
  dateOfModification: string; // Variable for title data modification
  questions: string; // Variable for questions title
  responses: string; // Vriable for responses title
  version: string; // Variable for version title
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable showing the pagination result
  auditableEntityTitle: string; // Variable for title auditable enity
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  selectedPageItem: number; // Variable for selected page item
  searchValue; // Variable for search value
  pageNumber: number = null; // Variable for page number
  strategicAnalysisList: StrategicAnalysisAC[] = []; // Variable for storage of strategic analysis list
  totalRecords: number; // Variable for storing total records
  showingText: string; // Variable for showing tooltip
  ofText: string; // Variable for of tooltip
  addTooltip: string;
  pageItems = []; // Per page items for entity list
  createdDateTimeList: string[];
  updatedDateTimeList: string[];
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  showNoDataText: string;
  inActiveText: string;
  activeText: string;
  statusTitle: string;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private apiService: StrategicAnalysesService,
    private sharedService: SharedService,
    private router: Router,
    private toastr: ToastrService,
    private loaderService: LoaderService) {
    this.strategicAnalysis = this.stringConstants.strategicAnalysis;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addStratagicAnalysisToolTip = this.stringConstants.addStratagicAnalysisToolTip;
    this.addSamplingToolTip = this.stringConstants.addSamplingToolTip;
    this.surveyTitle = this.stringConstants.surveyTitle;
    this.dateOfCreation = this.stringConstants.dateOfCreation;
    this.questions = this.stringConstants.questions;
    this.responses = this.stringConstants.responses;
    this.version = this.stringConstants.versionTitle;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.showingText = this.stringConstants.showingText;
    this.ofText = this.stringConstants.ofText;
    this.dateOfModification = this.stringConstants.dateOfModification;
    this.strategicAnalysisList = [];
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.createdDateTimeList = [];
    this.updatedDateTimeList = [];
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.addTooltip = this.stringConstants.addToolTip;
    this.inActiveText = this.stringConstants.inActiveText;
    this.activeText = this.stringConstants.activeText;
    this.statusTitle = this.stringConstants.statusTitle;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  async ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.pageItems !== undefined) {
        this.selectedPageItem = Number(params.pageItems);
        this.pageNumber = Number(params.pageNumber);
      }
      this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
    });
    this.loaderService.open();
    await this.getStrategicAnalysis(this.pageNumber, this.selectedPageItem, this.searchValue);
  }

  /***
   * Search strategic analysis data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchStrategicAnalysis(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getStrategicAnalysis(pageNumber, selectedPageItem, searchValue);
    }
  }

  /**
   * Get Strategic Analysis
   * @param pageNumber: page number
   * @param selectedPageItem: selected page item
   * @param searchValue: string to be searched
   */
  async getStrategicAnalysis(pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.apiService.strategicAnalysesGetAllStrategicAnalyses(pageNumber, selectedPageItem, searchValue)
      .subscribe((result: Pagination<StrategicAnalysisAC>) => {
        this.loaderService.close();
        this.strategicAnalysisList = result.items;

        // Change date format
        for (let i = 0; i < this.strategicAnalysisList.length; i++) {
          const pipe = new DatePipe('en-US');
          this.createdDateTimeList[i] = pipe.transform(this.strategicAnalysisList[i].createdDateTime, 'dd/MM/yyyy');
          this.updatedDateTimeList[i] = pipe.transform(this.strategicAnalysisList[i].updatedDateTime, 'dd/MM/yyyy');
        }

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      }, err => {
        this.loaderService.close();
        // show error if any http error caught
        this.toastr.error(this.stringConstants.somethingWentWrong, '', {
          timeOut: 1000
        });
      });
  }

  /**
   * Delete Strategic Analysis Id from List
   * @param strategicAnalysisId: strategic analysis id
   * @index : current index
   */
  deleteStrategicAnalysis(strategicAnalysisId: string, index: number) {
    const initialState = {
      title: this.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.apiService.strategicAnalysesDeleteStrategicAnalysis(strategicAnalysisId).subscribe(data => {
            this.loaderService.close();
            this.strategicAnalysisList.splice(index, 1);
            if (this.strategicAnalysisList.length === 0 && this.pageNumber > 1) {
              this.pageNumber = this.pageNumber - 1;
              this.loaderService.open();
              this.getStrategicAnalysis(null, this.selectedPageItem, this.searchValue);
            } else {
              this.totalRecords = this.totalRecords - 1;
              this.setShowingResult(this.pageNumber, this.selectedPageItem, this.totalRecords);
            }
            this.toastr.success(this.stringConstants.recordDeletedMsg, '', {
              timeOut: 10000
            });
          }, (error) => {
            this.loaderService.close();
            this.toastr.error(error.error, '', {
              timeOut: 10000
            });
          });
        }
      },
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * Get strategic analysis on page change
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.loaderService.open();
    this.getStrategicAnalysis(pageNumber, this.selectedPageItem, this.searchValue);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.loaderService.open();
    this.getStrategicAnalysis(null, this.selectedPageItem, this.searchValue);
  }

  /**
   * Set showing reults
   * @param pageNumber: page number
   * @param selectedPageItem: selected page item
   * @param totalRecords: total number of records
   */
  setShowingResult(pageNumber: number, selectedPageItem: number, totalRecords: number) {
    const startItem = (pageNumber - 1) * selectedPageItem + 1;
    const endItem = (totalRecords < (pageNumber * selectedPageItem)) ? totalRecords : (pageNumber * selectedPageItem);
    this.showingResults = this.showingText + ' ' + startItem + ' - ' + endItem + ' ' + this.ofText + ' ' + totalRecords;
  }

  /**
   * Open Strategic Add Modal Popup
   */
  openStrategicAnalysisAddModal() {
    const initialState = {
      selectedPageItem: this.selectedPageItem,
      pageNumber: this.pageNumber,
      isSampling: false,
    };

    this.bsModalRef = this.modalService.show(StrategicAnalysisAdminAddComponent,
      Object.assign({ initialState }, { class: 'page-modal strategic-add' }));

  }

  /**
   * Edit strategic analysis
   * @param strategicAnalysisId: id to edit strategic analysis
   * @index : current index
   */
  editStrategicAnalysis(id: string, index: number) {
    const initialState = {
      strategicAnalysisId: id,
      selectedPageItem: this.selectedPageItem,
      pageNumber: this.pageNumber,
      isSampling: false
    };
    this.bsModalRef = this.modalService.show(StrategicAnalysisAdminAddComponent,
      Object.assign({ initialState }, { class: 'page-modal strategic-add' }));
  }
  /**
   * View all responses of strategic analysis
   * @param survey : survey obj
   */
  viewResponses(survey: StrategicAnalysisAC) {
    this.router.navigate(['/strategic-analysis-response/list', {
      strategicAnalysisId: survey.id,
      title: survey.surveyTitle,
      pageNumber: this.pageNumber,
      selectedPageItem: this.selectedPageItem
    }]);
  }

  /**
   * set active inacive strategy analysis
   * @param id: id of strategic analysis
   */
  activeStrategicAnalysis(id: string) {
    this.apiService.strategicAnalysesSetStrategicAnalysis(id).subscribe(result => {
      const strategicAnalysis = this.strategicAnalysisList.find(a => a.id === id);
      this.strategicAnalysisList.find(a => a.id === id).isActive = !strategicAnalysis.isActive;
      this.strategicAnalysisList = JSON.parse(JSON.stringify(this.strategicAnalysisList));
      this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
    }, err => {
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
    });
  }
}
