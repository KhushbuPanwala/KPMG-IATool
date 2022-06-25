import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StrategicAnalysisAdminResponseModalComponent } from '../strategic-analysis-admin-response-modal/strategic-analysis-admin-response-modal.component';
import { StrategicAnalysesService, StrategicAnalysisAC, UserWiseResponseAC } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { Route, ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../core/upload.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-strategic-analysis-admin-response',
  templateUrl: './strategic-analysis-admin-response.component.html'
})
export class StrategicAnalysisAdminResponseComponent implements OnInit {
  showNoDataText: string;
  strategicAnalysis: StrategicAnalysisAC;
  searchText: string;
  isApproved: boolean;
  ofText: string;
  excelToolTip: string;
  addToolTip: string;
  nameClient: string;
  engagementPartner: string;
  engagementManager: string;
  detailsOportunity: string;
  estimatedValue: string;
  auditableEntityTitle: string;
  surveyResponseText: string;
  approve: string;
  survey: string;
  showingResults: string;
  showingText: string;
  bsModalRef: BsModalRef;
  strategicAnalysisId: string;
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

  surveyTitle: string;

  // Creates an instance of documenter
  constructor(private stringConstants: StringConstants,
              private modalService: BsModalService,
              private apiService: StrategicAnalysesService,
              private loaderService: LoaderService,
              private sharedService: SharedService,
              private route: ActivatedRoute,
              private uploadService: UploadService,
              public router: Router) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.nameClient = this.stringConstants.nameClient;
    this.engagementPartner = this.stringConstants.engagementPartner;
    this.engagementManager = this.stringConstants.engagementManager;
    this.detailsOportunity = this.stringConstants.detailsOportunity;
    this.estimatedValue = this.stringConstants.estimatedValue;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.surveyResponseText = this.stringConstants.surveyResponseText;
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

    // file format assign
    this.wordType = this.stringConstants.docText;
    this.pdfType = this.stringConstants.pdfText;
    this.pptType = this.stringConstants.pptText;
    this.otherFileType = this.stringConstants.otherFileType;
    this.gifType = this.stringConstants.gifText;
    this.pngType = this.stringConstants.pngText;
    this.jpgType = this.stringConstants.jpegText;
    this.svgType = this.stringConstants.svgType;
    this.csvType = this.stringConstants.csv;
    this.mp3Type = this.stringConstants.mp3Type;
    this.mp4Type = this.stringConstants.mp4Type;
    this.excelType = this.stringConstants.xlsx;
    this.zipType = this.stringConstants.zipType;

    this.deleteTitle = this.stringConstants.deleteTitle;
    this.isApproved = true;
  }


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.strategicAnalysisId = params.strategicAnalysisId;
      if (params.pageItems !== undefined) {
        this.selectedPageItem = Number(params.pageItems);
        this.pageNumber = Number(params.pageNumber);
      }
      this.surveyTitle = params.title;
      this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
    });
    this.loaderService.open();
    this.getUserWiseResponse(this.pageNumber, this.selectedPageItem, this.searchValue, this.strategicAnalysisId);
  }

  /**
   * Back on strategy analysis list page
   */
  onBackClick() {
    this.router.navigate(['strategic-analysis/list']);
  }

  /**
   * Open Strategic Add Modal Popup
   * @param strategicId Strategic analysis id
   * @param surveyUserId User id
   */
  openSurvey(strategicId: string, surveyUserId: string, entityId: string) {
    const initialState = {
      strategicAnalysisId: strategicId,
      userId: surveyUserId,
      pageNumber: this.pageNumber,
      selectedPageItem: this.selectedPageItem,
      searchValue: this.searchValue,
      auditableEntityId: entityId
    };
    this.bsModalRef = this.modalService.show(StrategicAnalysisAdminResponseModalComponent,
      Object.assign({ initialState }, { class: 'page-modal survey-add' }));
  }

  /**
   * Get Strategic Analysis by id for edit
   */
  getStrategicAnalysisById() {
    this.loaderService.open();
    this.apiService.strategicAnalysesGetStrategicAnalysisById(this.strategicAnalysisId).subscribe(result => {
      this.loaderService.close();
      this.strategicAnalysis = result;
    },
      error => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  getUserWiseResponse(pageNumber: number, selectedPageItem: number, searchValue: string, strategicAnalysisId: string) {
    const userId = '';
    this.apiService.strategicAnalysesGetUserWiseResponse(pageNumber, selectedPageItem, searchValue, strategicAnalysisId, userId).subscribe(result => {
      this.loaderService.close();
      this.userWiseResponse.isDeleted = false;
      this.userWiseResponseList = result.items;
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
    this.getUserWiseResponse(pageNumber, this.selectedPageItem, this.searchValue, this.strategicAnalysisId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.loaderService.open();
    this.getUserWiseResponse(null, this.selectedPageItem, this.searchValue, this.strategicAnalysisId);
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
      this.getUserWiseResponse(null, selectedPageItem, searchValue, this.strategicAnalysisId);
    }
  }

  /**
   * Check file type to display icon accordingly
   * @param fileName :Name of the File
   * @param fileTypeCheck : type to check
   */
  checkFileExtention(fileName: string, fileTypeCheck: string) {
    let isUploadedFormatMatched = false;
    switch (fileTypeCheck) {
      case this.wordType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsWord(fileName);
        break;
      case this.pdfType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPdf(fileName);
        break;
      case this.pptType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPpt(fileName);
        break;
      case this.excelType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsExcel(fileName);
        break;
      case this.pngType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPng(fileName);
        break;
      case this.jpgType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsJpg(fileName);
        break;
      case this.gifType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsGif(fileName);
        break;
      case this.svgType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsSvg(fileName);
        break;
      case this.mp3Type:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsMp3(fileName);
        break;
      case this.mp4Type:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsMp4(fileName);
        break;
      case this.csvType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsCsv(fileName);
        break;
      case this.zipType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsZip(fileName);
        break;
      default:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsOtherFormat(fileName);
        break;
    }
    return isUploadedFormatMatched;
  }

  /**
   * Method to open document
   * @param documnetPath: documnetPath
   */
  openDocument(workPaperId: string) {
    this.loaderService.open();
    this.apiService.strategicAnalysesGetDocumentDownloadUrl(workPaperId).subscribe((result) => {
      this.loaderService.close();
      // Open document in new tab
      const url = this.router.serializeUrl(
        // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
        this.router.createUrlTree(['document-preview/view', { path: btoa(result) }])
      );
      window.open(url, '_blank');
    });

  }


  /**
   * Method to download document
   * @param documentId: document Id
   */
  downloadDocument(documentId: string) {
    this.loaderService.open();
    this.apiService.strategicAnalysesGetDocumentDownloadUrl(documentId).subscribe((result) => {
      this.loaderService.close();
      const a = document.createElement('a');
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      a.download = '';
      a.href = result;
      a.target = '_blank';
      a.click();
      document.body.removeChild(a);
    });
  }

  /**
   * Method to open delete confirmation dialogue
   * @param userResponseIndex user response index
   * @param documentIndex document index
   * @param docId document id
   */
  openDeleteModal(userResponseIndex: number, documentIndex: number, docId: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.stringConstants.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (docId !== '') {

              this.loaderService.open();
              this.apiService.strategicAnalysesDeleteFile(docId).subscribe(() => {
                this.userWiseResponseList[userResponseIndex].strategicAnalysis.userResponseDocumentACs.splice(
                  this.userWiseResponseList[userResponseIndex].strategicAnalysis.userResponseDocumentACs.indexOf(this.userWiseResponseList[userResponseIndex].strategicAnalysis.userResponseDocumentACs.filter(x => x.id === docId)[0]), 1);
                this.userWiseResponseList[userResponseIndex].strategicAnalysis.userResponseDocumentACs = [...this.userWiseResponseList[userResponseIndex].strategicAnalysis.userResponseDocumentACs];
                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
                this.loaderService.close();
              }, (error) => {
                this.sharedService.showError(this.stringConstants.somethingWentWrong);
                this.loaderService.close();
              });
            }
          }
        }
      }
    });
  }

  /**
   * On approve click of user response
   * @param strategicAnalysisId Strategic analysis id
   * @param userId reponded user id
   * @param entityId entityId
   */
  onApprove(strategicAnalysisId: string, userId: string, entityId: string) {
    this.apiService.strategicAnalysesUpdateStrategicAnalysisDoneInAuditableEntity(strategicAnalysisId, userId, entityId).subscribe(
      (result) => {
        this.sharedService.showSuccess(this.stringConstants.approved);
      },
      error => {
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }
}
