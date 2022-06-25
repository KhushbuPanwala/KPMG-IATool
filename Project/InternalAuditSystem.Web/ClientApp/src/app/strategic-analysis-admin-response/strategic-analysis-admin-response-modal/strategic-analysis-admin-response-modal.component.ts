import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { StrategicAnalysesService, UserWiseResponseAC, SamplingsService } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { Router } from '@angular/router';
import { ObservationService } from '../../observation/observation.service';

@Component({
  selector: 'app-strategic-analysis-admin-response-modal',
  templateUrl: './strategic-analysis-admin-response-modal.component.html'
})
export class StrategicAnalysisAdminResponseModalComponent implements OnInit {

  surveyResponseText: string; // Variable for survey response text
  surveyBefore: string; // Variable for survey before text form field
  geoGraphical: string; // Variable for geographical text in form field
  previousExperience: string; // Variable for previous experience in form field
  nameLabel: string; // Variable for name text label
  surveyQuestion: string; // Variable for rating scale question in form field
  financialYear: string; // Varianble for financial year in form field
  uploadFinancial: string; // Variable for form field upload financial
  wordToolTip: string; // Variable for button dropdown word file
  fileNameText: string; // Variable for button dropdown file name
  strategicAnalysisId: string;
  userId: string;
  userWiseResponse: UserWiseResponseAC;
  userWiseResponseList = [] as Array<UserWiseResponseAC>;
  smileyValues: {
    value: string,
    number: number,
    class: string
  }[]; // Object to store smiley value and class
  pageNumber: number;
  selectedPageItem: number;
  searchValue: string;
  pdfToolTip: string;
  powerPointToolTip: string;
  viewFiles: string;
  downloadToolTip: string;
  rcmId: string;

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
  isSampling: boolean;

  // Creates an instance of documenter.
  constructor(
    public bsModalRef: BsModalRef,
    private router: Router,
    private stringConstants: StringConstants,
    private apiService: StrategicAnalysesService,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    private observationService: ObservationService,
    private samplingService: SamplingsService) {

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

    this.surveyBefore = this.stringConstants.surveyBefore;
    this.geoGraphical = this.stringConstants.geoGraphical;
    this.previousExperience = this.stringConstants.previousExperience;
    this.nameLabel = this.stringConstants.nameLabel;
    this.surveyQuestion = this.stringConstants.surveyQuestion;
    this.financialYear = this.stringConstants.financialYear;
    this.uploadFinancial = this.stringConstants.uploadFinancial;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.viewFiles = this.stringConstants.viewFiles;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.smileyValues = [
      {
        value: 'very sad',
        number: 1,
        class: 'first-emoji mr-35'
      },
      {
        value: 'sad',
        number: 2,
        class: 'second-emoji mr-35'
      },
      {
        value: 'neutral',
        number: 3,
        class: 'third-emoji mr-35'
      },
      {
        value: 'happy',
        number: 4,
        class: 'forth-emoji mr-35'
      },
      {
        value: 'very happy',
        number: 5,
        class: 'fifth-emoji mr-35'
      }
    ];
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
  *  Initialization of properties.
  */
  auditableEntityId;
  ngOnInit(): void {
    this.ratingBar();
    this.loaderService.open();
    this.surveyResponseText = this.isSampling ? this.stringConstants.responseButtonText : this.stringConstants.surveyResponseText;
    this.getUserWiseResponse(this.pageNumber, this.selectedPageItem, this.searchValue, this.strategicAnalysisId, this.userId);
  }

  // Smiley Rating bar
  ratingBar() {
    // Very simple JS for updating the text when a radio button is clicked
    const INPUTS = document.querySelectorAll('#smileys input');
    const updateValue = e => document.querySelector('#result').innerHTML = e.target.value;

    INPUTS.forEach(el => el.addEventListener('click', e => updateValue(e)));
  }

  /**
   * Get user wise response
   * @param strategicAnalysisId Strategic analysis of which user responses are to be found
   * @param userId User id on which basis user responses are to be fetched
   */
  getUserWiseResponse(pageNumber: number, selectedPageItem: number, searchValue: string, strategicAnalysisId: string, userId: string) {
    if (!this.isSampling) {
      this.apiService.strategicAnalysesGetUserWiseResponse(pageNumber, selectedPageItem, searchValue, strategicAnalysisId, userId, this.auditableEntityId).subscribe(result => {
        this.loaderService.close();
        this.userWiseResponseList = result.items;
      },
        error => {
          this.loaderService.close();
          this.sharedService.showError(this.stringConstants.somethingWentWrong);
        });
    } else {
      this.samplingService.samplingsGetRcmWiseResponse(strategicAnalysisId, this.rcmId).subscribe(result => {
        this.loaderService.close();
        this.userWiseResponseList = result;
      },
        error => {
          this.loaderService.close();
          this.sharedService.showError(this.stringConstants.somethingWentWrong);
        });
    }
  }

  /**
   * Check file type to display icon accordingly
   * @param fileName :Name of the File
   * @param fileTypeCheck : type to check
   */
  checkFileExtention(fileName: string, fileTypeCheck: string) {
    let isUploadedFormatMatched = false;
    isUploadedFormatMatched = this.observationService.checkFileExtention(fileName, fileTypeCheck);
    return isUploadedFormatMatched;
  }

  /**
   * Method to open document
   * @param documentPath: documnetPath
   */
  openDocument(documentId: string) {
    this.apiService.strategicAnalysesGetDocumentDownloadUrl(documentId).subscribe((result) => {
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
    this.apiService.strategicAnalysesGetDocumentDownloadUrl(documentId).subscribe((result) => {
      const aTag = document.createElement('a');
      aTag.setAttribute('style', 'display:none;');
      document.body.appendChild(aTag);
      aTag.download = '';
      aTag.href = result;
      aTag.target = '_blank';
      aTag.click();
      document.body.removeChild(aTag);
    });
  }
}
