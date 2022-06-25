import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { Router } from '@angular/router';
import { StrategicAnalysesService, StrategicAnalysisAC, StrategicAnalysisStatus, UserResponseAC, UserResponseDocumentAC } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { UploadService } from '../../core/upload.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { ObservationService } from '../../observation/observation.service';

@Component({
  selector: 'app-strategic-analysis-user-email-attachment',
  templateUrl: './strategic-analysis-user-email-attachment.component.html'
})
export class StrategicAnalysisUserEmailAttachmentComponent implements OnInit {
  showDelete: boolean;
  emailAttachment: string; // Variable for email attachment
  surveryFormField: string; // Variable for survey form field
  surveyFinance: string; // Variable for survey finance field
  chooseFileText: string; // Variable for choose file text
  fileNameText: string; // Variable for file name
  saveButtonText: string; // Variable for save button text
  wordToolTip: string; // Variable for word tool tip
  powerPointToolTip: string; // Variable for powerpoint tool tip
  pdfToolTip: string; // Variable for pdf tool tip
  noFileChosen: string; // Variable for no file choosen text
  strategicAnalysisId: string;
  sendForApproval: string;
  strategicAnalysis: StrategicAnalysisAC;
  strategicAnalysisFilesList: File[] = [];
  strategyAnalysisTitle: string;

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
  auditableEntityId: string;

  // Creates an instance of documenter
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private apiService: StrategicAnalysesService,
    private router: Router,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    private uploadService: UploadService,
    private observationService: ObservationService) {
    this.emailAttachment = this.stringConstants.emailAttachment;
    this.surveryFormField = this.stringConstants.surveryFormField;
    this.surveyFinance = this.stringConstants.surveyFinance;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.fileNameText = this.stringConstants.fileNameText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.sendForApproval = this.stringConstants.sendForApproval;
    this.strategicAnalysis = {
      version: 1,
      isSampling: false,
      questionsCount: 0,
      responseCount: 0,
      isDeleted: false,
      isResponseDrafted: false,
      isVersionToBeChanged: false,
      status: StrategicAnalysisStatus.NUMBER_0
    };
    this.strategicAnalysisFilesList = [];

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
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
  *  Initialization of properties.
  */
  async ngOnInit() {
    this.loaderService.open();
    await this.getStrategicAnalysis(this.strategicAnalysisId);
  }

  /**
   * Get strategic analysis by id
   * @param strategicId Strategic Analysis id on basis of which, strategic analysis is to be fetched
   */
  async getStrategicAnalysis(strategicId: string) {

    this.apiService.strategicAnalysesGetEmailAttachmentDocuments(strategicId, this.auditableEntityId).subscribe(result => {
      this.loaderService.close();
      this.strategicAnalysis = JSON.parse(JSON.stringify(result));
      if (this.strategicAnalysis.userResponseDocumentACs === null) {
        this.strategicAnalysis.userResponseDocumentACs = [] as UserResponseDocumentAC[];
      }
      if (this.strategicAnalysisFilesList === undefined || this.strategicAnalysisFilesList === null) {
        this.strategicAnalysisFilesList = [] as File[];
      }
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  /**
   * File input change event
   * @param event: Onchange event
   */
  fileChange(event) {
    const fileCount = event.target.files.length + this.strategicAnalysisFilesList.length + this.strategicAnalysis.userResponseDocumentACs.length;
    if (fileCount <= Number(this.stringConstants.fileLimit)) {
      for (const file of event.target.files) {
        const selectedFile = file;
        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {
          this.strategicAnalysisFilesList.push(selectedFile);
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }
  }

  /**
   * Save email attachments
   */
  onSave() {
    this.loaderService.open();
    this.strategicAnalysis.isUserResponseToBeUpdated = false;
    const emailAttachmentResponse = {} as UserResponseAC;
    emailAttachmentResponse.strategicAnalysisId = this.strategicAnalysisId;
    emailAttachmentResponse.auditableEntityId = this.auditableEntityId;
    if (this.strategicAnalysisFilesList.length > 0) {
      this.uploadService.uploadFileOnUpdate<UserResponseAC>(emailAttachmentResponse, this.strategicAnalysisFilesList,
        this.stringConstants.userResponseFileFieldName, this.stringConstants.saveEmailAttachmentApiPath).subscribe(() => {
          this.strategicAnalysisFilesList = [];
          this.loaderService.close();
          this.bsModalRef.hide();

          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          this.bsModalRef.content.callback(StrategicAnalysisStatus.NUMBER_1);
        },
          (error) => {
            this.loaderService.close();
            this.sharedService.showError(this.stringConstants.somethingWentWrong);
          });
    } else {
      this.loaderService.close();
      this.bsModalRef.hide();
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
  openDocument(documentPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documentPath) }])
    );
    window.open(url, '_blank');
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

  /**
   * Method to open delete confirmation dialogue
   * @param docId: document id
   */
  openDeleteModal(docId: string) {
    this.loaderService.open();
    this.apiService.strategicAnalysesDeleteFile(docId).subscribe(() => {
      this.strategicAnalysis.userResponseDocumentACs.splice(
        this.strategicAnalysis.userResponseDocumentACs.indexOf(this.strategicAnalysis.userResponseDocumentACs.filter(x => x.id === docId)[0]), 1);
      this.strategicAnalysis.userResponseDocumentACs = [...this.strategicAnalysis.userResponseDocumentACs];
      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
      this.loaderService.close();
    });

  }
}
