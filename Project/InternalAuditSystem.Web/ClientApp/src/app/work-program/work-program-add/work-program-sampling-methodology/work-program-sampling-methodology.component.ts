import { Component, OnInit, Output, EventEmitter, Input, OnDestroy } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../shared/stringConstants';
import { StrategicAnalysesService, StrategicAnalysisAC, RiskControlMatrixAC, RiskControlMatrixesService, StrategicAnalysisStatus } from '../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../core/loader.service';
import { Pagination } from '../../../models/pagination';
import { SharedService } from '../../../core/shared.service';
import { DatePipe } from '@angular/common';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-work-program-sampling-methodology',
  templateUrl: './work-program-sampling-methodology.component.html'
})
export class WorkProgramSamplingMethodologyComponent implements OnInit, OnDestroy {
  samplingMethodology: string; // Variable for strategic analysis title
  searchText: string; // Variable for search placeholder
  titleText: string; // Variable for title text
  auditableEntityTitle: string; // Variable for auditable entity title
  dateOfCreation: string; // Variable for date of creation
  questions: string; // Variable for questions
  statusTitle: string; // Variable for status title
  nextButton: string; // Variable for next button
  previousButton: string; // Variable for previous button
  showingResults: string; // Variable of showing results
  isShowFinance = false;  // Variable for showing finance survey page
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  isShowSampling: boolean; // Variable for showing sampling page
  showNoDataText: string;
  sampleSearchValue: string;
  isSamplingList = false; // Variable for showing sampling methodology

  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number;
  pageItems = []; // Per page items for entity list
  createdDateTimeList: string[];
  rcmDetailList: RiskControlMatrixAC[] = [];
  draftStatus: string;

  @Output() isShowSample = new EventEmitter<boolean>(); // Output passing the boolean data to edit page

  @Input() samplingRcmDetail: RiskControlMatrixAC;

  // TODO: Added static code here, respective developer will change it in future
  // Sampling list array
  samplingList: StrategicAnalysisAC[] = [];

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  selectedEntityId: string;
// only to subscripe for the current component
  entitySubscribe: Subscription;

  // Status Mapping
  strategicAnalysisUserResponseDetails =
    [
      {
        displayStatus: 'Draft',
        value: StrategicAnalysisStatus.NUMBER_0
      },
      {
        displayStatus: 'UnderReview',
        value: StrategicAnalysisStatus.NUMBER_1
      },
      {
        displayStatus: 'Final',
        value: StrategicAnalysisStatus.NUMBER_2
      }
    ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private strategicAnalysesService: StrategicAnalysesService,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    private riskControlMatrixService: RiskControlMatrixesService,
  ) {
    this.samplingMethodology = this.stringConstants.samplingMethodology;
    this.searchText = this.stringConstants.searchText;
    this.titleText = this.stringConstants.titleText;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.dateOfCreation = this.stringConstants.dateOfCreation;
    this.questions = this.stringConstants.question;
    this.statusTitle = this.stringConstants.statusTitle;
    this.showingResults = this.stringConstants.showingResults;
    this.nextButton = this.stringConstants.nextButton;
    this.previousButton = this.stringConstants.previousButton;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.draftStatus = this.strategicAnalysisUserResponseDetails[StrategicAnalysisStatus.NUMBER_0].displayStatus;

    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.createdDateTimeList = [];
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
      }
    });
    this.getSamplingList(this.pageNumber, this.selectedPageItem, this.sampleSearchValue);
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /**
   * Get work program list for list page with pagination
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   */
  getSamplingList(pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.loaderService.open();
    this.strategicAnalysesService.strategicAnalysesGetAllStrategicAnalyses(pageNumber,
      selectedPageItem, searchValue, true, this.samplingRcmDetail.id).subscribe((result: Pagination<StrategicAnalysisAC>) => {
        this.samplingList = result.items;
        // Change date format
        for (let i = 0; i < this.samplingList.length; i++) {
          const pipe = new DatePipe(this.stringConstants.datePipe);
          this.createdDateTimeList[i] = pipe.transform(this.samplingList[i].createdDateTime, this.stringConstants.dateFormat);
        }
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
        this.loaderService.close();
      }, (error) => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  /**
   * Open work program Survey Page
   */
  openSurvey() {
    this.loaderService.open();
    this.rcmDetailList = [];
    this.rcmDetailList.push(this.samplingRcmDetail);
    this.riskControlMatrixService.riskControlMatrixesUpdateRiskControlMatrix(this.rcmDetailList, this.selectedEntityId).subscribe(() => {
      this.loaderService.close();
      this.isShowFinance = true;
    },
      error => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Open Previous Sampling Page
   */
  previousEdit() {
    this.isShowSampling = false;
    this.isShowSample.emit(this.isShowSampling);
  }

  /**
   * Passing the isFinance boolean to show the work program finance survey survey
   */
  isShowPrevious(isFinance: boolean) {
    this.isShowFinance = isFinance;
  }
  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getSamplingList(pageNumber, this.selectedPageItem, this.sampleSearchValue);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getSamplingList(null, this.selectedPageItem, this.sampleSearchValue);
  }

  /**
   * On sample radio button click
   * @param id: sample id
   */
  onSampleSelection(id: string) {
    this.samplingRcmDetail.strategicAnalysisId = id;
  }

  /**
   * Search PrimaryGeographicalArea on grid
   * @param event: key event tab and enter
   */
  searchSampling(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.sampleSearchValue = this.sampleSearchValue.trim();
      this.getSamplingList(null, this.selectedPageItem, this.sampleSearchValue);
    }
  }


  /**
   * Passing boolean to sampling page to show edit page on previous click
   * @param isShowSampling: is show sample boolean
   */
  isShowSamplingList(isShowSampling: boolean) {
    this.isShowFinance = isShowSampling;
    this.getSamplingList(this.pageNumber, this.selectedPageItem, this.sampleSearchValue);

  }
}
