import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StrategicAnalysesService, StrategicAnalysisAC, UserWiseResponseAC, SamplingsService } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { Route, ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../core/upload.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { StrategicAnalysisAdminResponseModalComponent } from '../../strategic-analysis-admin-response/strategic-analysis-admin-response-modal/strategic-analysis-admin-response-modal.component';

@Component({
  selector: 'app-sampling-admin-response-list',
  templateUrl: './sampling-admin-response-list.component.html'
})
export class SamplingAdminResponseListComponent implements OnInit {
  showNoDataText: string;
  strategicAnalysis: StrategicAnalysisAC;
  searchText: string;
  isApproved: boolean;
  ofText: string;
  excelToolTip: string;
  addToolTip: string;
  nameOfClientLabel: string;
  rcmDescriptionLabel: string;
  engagementPartner: string;
  engagementManager: string;
  detailsOportunity: string;
  estimatedValue: string;
  responseButtonText: string;
  approve: string;
  survey: string;
  showingResults: string;
  showingText: string;
  bsModalRef: BsModalRef;
  userWiseResponse: UserWiseResponseAC;
  userWiseResponseList = [] as Array<UserWiseResponseAC>;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  pageNumber: number = null;
  selectedPageItem: number;
  pageItems = [];
  searchValue: string;
  totalRecords: number;

  // file string
  wordType: string;
  pdfType: string;
  pptType: string;
  otherFileType: string;
  gifType: string;
  pngType: string;
  jpgType: string;
  svgType: string;
  csvType: string;
  mp3Type: string;
  mp4Type: string;
  excelType: string;
  zipType: string;

  deleteTitle: string;
  viewFiles: string;
  samplingTitle: string;
  samplingId: string;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private apiService: StrategicAnalysesService,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    private route: ActivatedRoute,
    private uploadService: UploadService,
    private samplingService: SamplingsService,
    public router: Router) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.nameOfClientLabel = this.stringConstants.nameClient;
    this.engagementPartner = this.stringConstants.engagementPartner;
    this.engagementManager = this.stringConstants.engagementManager;
    this.detailsOportunity = this.stringConstants.detailsOportunity;
    this.estimatedValue = this.stringConstants.estimatedValue;
    this.responseButtonText = this.stringConstants.responseButtonText;
    this.approve = this.stringConstants.approve;
    this.survey = this.stringConstants.surveyTitle;
    this.showingResults = this.stringConstants.showingResults;
    this.strategicAnalysis = {} as StrategicAnalysisAC;
    this.userWiseResponse = {
      isDeleted: false
    } as UserWiseResponseAC;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.showingText = this.stringConstants.showingText;
    this.ofText = this.stringConstants.ofText;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.viewFiles = this.stringConstants.viewFiles;

    this.deleteTitle = this.stringConstants.deleteTitle;
    this.isApproved = true;
    this.rcmDescriptionLabel = this.stringConstants.rcmDescriptionLabel;
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.samplingId = params.id;
      this.samplingTitle = params.title;
      if (params.pageItems !== undefined) {
        this.selectedPageItem = Number(params.pageItems);
        this.pageNumber = Number(params.pageNumber);
      }
      this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
    });
    this.loaderService.open();
    this.getUserWiseResponse(this.pageNumber, this.selectedPageItem, this.searchValue, this.samplingId);
  }

  /**
   * Open Strategic Add Modal Popup
   * @param strategicId Strategic analysis id
   * @param creatorId User id
   * @param rcmMatrixId : rcm id
   */
  openSurvey(strategicId: string, creatorId: string, rcmMatrixId: string) {

    const initialState = {
      strategicAnalysisId: strategicId,
      userId: creatorId,
      rcmId: rcmMatrixId,
      pageNumber: this.pageNumber,
      selectedPageItem: this.selectedPageItem,
      searchValue: this.searchValue,
      isSampling: true
    };
    this.bsModalRef = this.modalService.show(StrategicAnalysisAdminResponseModalComponent,
      Object.assign({ initialState }, { class: 'page-modal survey-add' }));
  }

  /**
   * Get Strategic Analysis by id for edit
   */
  getStrategicAnalysisById() {
    this.loaderService.open();
    this.apiService.strategicAnalysesGetStrategicAnalysisById(this.samplingId).subscribe(result => {
      this.loaderService.close();
      this.strategicAnalysis = result;
    },
      error => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  /**
   * Get user response for smapling
   * @param pageNumber : Page number
   * @param selectedPageItem : selected page items
   * @param searchValue : Search string
   * @param samplingId : Id of sampling
   */
  getUserWiseResponse(pageNumber: number, selectedPageItem: number, searchValue: string, samplingId: string) {
    const userId = '';
    this.samplingService.samplingsGetUserWiseResponse(pageNumber, selectedPageItem, searchValue, samplingId, userId).subscribe(result => {
      this.loaderService.close();
      this.userWiseResponse.isDeleted = false;
      this.userWiseResponseList = result.items;

      this.userWiseResponseList.forEach(x => {
        x.rcmDescription = x.rcmDescription.replace(/<[^>]*>/g, '');
      });

      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
    },
      error => {
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
    this.getUserWiseResponse(pageNumber, this.selectedPageItem, this.searchValue, this.samplingId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.loaderService.open();
    this.getUserWiseResponse(null, this.selectedPageItem, this.searchValue, this.samplingId);
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

  /***
   * Search user wise response data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchUserWiseResponse(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getUserWiseResponse(null, selectedPageItem, searchValue, this.samplingId);
    }
  }

  /**
   * Back on strategy analysis list page
   */
  onBackClick() {
    this.router.navigate(['sampling/list']);
  }
}
