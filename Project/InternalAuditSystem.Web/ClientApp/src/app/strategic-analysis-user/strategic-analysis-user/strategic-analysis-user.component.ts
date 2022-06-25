import { Component, OnInit, Optional, Inject } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StrategicAnalysisUserEmailAttachmentComponent } from '../../strategic-analysis-user-survey/strategic-analysis-user-email-attachment/strategic-analysis-user-email-attachment.component';
import { StrategicAnalysesService, StrategicAnalysisAC, BASE_PATH } from '../../swaggerapi/AngularFiles';
import { StrategicAnalysisStatus } from '../../swaggerapi/AngularFiles/model/strategicAnalysisStatus';
import { Router, ActivatedRoute } from '@angular/router';
import { LoaderService } from '../../core/loader.service';
import { Pagination } from '../../models/pagination';
import { DatePipe } from '@angular/common';
import { SharedService } from '../../core/shared.service';

@Component({
  selector: 'app-strategic-analysis-user',
  templateUrl: './strategic-analysis-user.component.html',
})
export class StrategicAnalysisUserComponent implements OnInit {

  strategicAnalysis: StrategicAnalysisAC; // Variable for strategic analysis title
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variable for edit tooltip
  surveryFormField: string; // Variable for survey form field title
  auditableEntityTitle: string; // Variable for title auditable enity
  dateOfCreation: string; // Variable for title date creation
  questions: string; // Variable for questions title
  emailAttachment: string; // Variable for email attachment title
  statusTitle: string; // Variable for status title
  openText: string; // Variable for open text title
  fileNameText: string; // Variable for file name text title
  showingResults: string; // Variable for showing results
  showNoDataText: string;
  wordToolTip: string; // Variable for word tool tip
  powerPointToolTip: string; // Variable for power point tool tip
  pdfToolTip: string; // Variable for pdf tool tip
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  strategicAnalysisList: StrategicAnalysisAC[] = [];
  selectedPageItem: number;
  pageNumber: number = null;
  searchValue: string;
  userId: string; // ToDo: will be removed when Access Management is done
  pageItems = [];
  createdDateTimeList: string[];
  updatedDateTimeList: string[];
  totalRecords: number;
  baseUrl: string;
  addTooltip: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  displayNameList: string[];
  // Status Mapping
  strategicAnalysisUserResponseDetails =
    [
      {
        displayStatus: 'Draft',
        value: StrategicAnalysisStatus.NUMBER_0
      },
      {
        displayStatus: 'Under Review',
        value: StrategicAnalysisStatus.NUMBER_1
      },
      {
        displayStatus: 'Final',
        value: StrategicAnalysisStatus.NUMBER_2
      }
    ];

  // Creates an instance of documenter
  constructor(
    public stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    private apiService: StrategicAnalysesService,
    private router: Router,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    @Optional() @Inject(BASE_PATH) basePath: string) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.surveryFormField = this.stringConstants.surveryFormField;
    this.dateOfCreation = this.stringConstants.dateOfCreation;
    this.questions = this.stringConstants.questions;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.emailAttachment = this.stringConstants.emailAttachment;
    this.statusTitle = this.stringConstants.statusTitle;
    this.openText = this.stringConstants.openText;
    this.fileNameText = this.stringConstants.fileNameText;
    this.showingResults = this.stringConstants.showingResults;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.strategicAnalysisList = [];
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.createdDateTimeList = [];
    this.updatedDateTimeList = [];
    this.displayNameList = [];
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.baseUrl = basePath;
    this.addTooltip = this.stringConstants.addToolTip;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    this.route.params.subscribe(params => {
      if (params.pageItems !== undefined) {
        this.selectedPageItem = Number(params.pageItems);
        this.pageNumber = Number(params.pageNumber);
      }
      this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
    });
    this.loaderService.open();
    this.getStrategicAnalysisList(this.pageNumber, this.selectedPageItem, this.searchValue, this.userId);
  }



  /**
   * Open Email template Popup
   * @param surveyObj  : Object of strategy analysis
   */
  openEmailUploadModal(surveyObj: StrategicAnalysisAC) {
    const strategicAnalysisUserResponseStatus = this.strategicAnalysisList.find(x => x.id === surveyObj.id && x.auditableEntityId === surveyObj.auditableEntityId).status;
    const showDelete = strategicAnalysisUserResponseStatus !== StrategicAnalysisStatus.NUMBER_2 && surveyObj.isEmailAttached;
    if (strategicAnalysisUserResponseStatus === StrategicAnalysisStatus.NUMBER_1 || strategicAnalysisUserResponseStatus === StrategicAnalysisStatus.NUMBER_2) {
      const initialState = {
        strategicAnalysisId: surveyObj.id,
        showDelete,
        strategyAnalysisTitle: surveyObj.surveyTitle,
        auditableEntityId: surveyObj.auditableEntityId,
        callback: (result) => {
          this.strategicAnalysisList.find(x => x.id === surveyObj.id && x.auditableEntityId === surveyObj.auditableEntityId).status = result;
          // set for showing delete icon for email attachment
          this.strategicAnalysisList.find(x => x.id === surveyObj.id && x.auditableEntityId === surveyObj.auditableEntityId).isEmailAttached = true;
        }
      };
      this.bsModalRef = this.modalService.show(StrategicAnalysisUserEmailAttachmentComponent,
        Object.assign({ initialState }, { class: 'page-modal strategic-confirm strategic-email' }));
    } else {
      this.sharedService.showError(this.stringConstants.emailAttachmentValidityError);
    }
  }

  /**
   * Open Survey by Id
   * @param id : strategy analysis id
   * @param auditableEntityId : entity id
   * @param surveyTitle :  survey title
   */
  openSurvey(id: string, auditableEntityId: string, surveyTitle: string) {
    if (id === '0') {
      // for add
      this.router.navigate(['/strategic-analysis-user-survey/general', {
        passedStrategicAnalysisId: 0,
        surveyTitle: 'Create New Survey',
        auditableEntityId: 0
      }]);

    } else {
      // for update
      this.router.navigate(['/strategic-analysis-user-survey/general', {
        passedStrategicAnalysisId: id,
        surveyTitle,
        auditableEntityId
      }]);
    }
  }

  /**
   * Get Strategic Analysis
   * @param pageNumber: page number
   * @param selectedPageItem: selected page item
   * @param searchValue: string to be searched
   * @param userId: user on basis of which strategic analyses is to be obtained
   */
  getStrategicAnalysisList(pageNumber: number, selectedPageItem: number, searchValue: string, userId: string) {
    this.apiService.strategicAnalysesGetAllStrategicAnalyses(pageNumber, selectedPageItem, searchValue, false, '', false)
      .subscribe((result: Pagination<StrategicAnalysisAC>) => {
        this.loaderService.close();
        this.strategicAnalysisList = result.items;
        // Change date format
        for (let j = 0; j < this.strategicAnalysisList.length; j++) {
          const pipe = new DatePipe('en-US');
          this.createdDateTimeList[j] = pipe.transform(this.strategicAnalysisList[j].createdDateTime, 'dd/MM/yyyy');
          this.updatedDateTimeList[j] = pipe.transform(this.strategicAnalysisList[j].updatedDateTime, 'dd/MM/yyyy');
        }
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      }, err => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  /**
   * Get strategic analysis on page change
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.loaderService.open();
    this.getStrategicAnalysisList(pageNumber, this.selectedPageItem, this.searchValue, this.userId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.loaderService.open();
    this.getStrategicAnalysisList(null, this.selectedPageItem, this.searchValue, this.userId);
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
      this.getStrategicAnalysisList(pageNumber, selectedPageItem, searchValue, this.userId);
    }
  }




  /**
   * Method for generating pdf
   * @param strategyId: Id of strategy
   * @param entityId: Entity Id
   */
  generatePdf(strategyId: string, entityId: string) {
    const offset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.strategicPDFApi + strategyId + this.stringConstants.entityParamString + entityId + this.stringConstants.offsetString + offset;
    this.sharedService.generatePdf(url);
  }

}
