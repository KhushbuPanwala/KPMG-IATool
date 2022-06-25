import { Component, OnInit, ElementRef, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AcmService, ACMReviewerDocumentAC } from '../../swaggerapi/AngularFiles';
import { AcmReportSharedService } from '../acm-report-shared.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { UploadService } from '../../core/upload.service';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { StringConstants } from '../../shared/stringConstants';
import { ACMReportsService } from '../../swaggerapi/AngularFiles/api/aCMReports.service';

@Component({
  selector: 'app-acm-report-upload-files',
  templateUrl: './acm-report-upload-files.component.html',
  styleUrls: ['./acm-report-upload-files.component.css']
})
export class AcmReportUploadFilesComponent implements OnInit, OnDestroy, AfterViewInit {
  saveButtonText: string; // Variable for save button text
  title: string; // Variable for modal title
  chooseFilePlaceHolder: string;
  chooseFileText: string; // Variable for choose file text
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for fileNameText
  searchText: string; // Variable for search text
  userId;

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

  reviewerDocumentFilesList: File[];
  reviewerDocumentObjList = [] as Array<ACMReviewerDocumentAC>;
  reviewerDocumentObj = {} as ACMReviewerDocumentAC;

  searchReviewerDocumentObjList = [] as Array<ACMReviewerDocumentAC>;
  searchReviewerDocumentFilesList: File[];
  searchValue;
  isShowSearch = false;

  requiredMessage: string;
  maxLengthExceedMessage: string;
  maxDocumentRestrictionMsg: string;
  isFileUploaded: boolean;
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  @ViewChild('reviewerDocument', { static: false }) myDialog: ElementRef;
  constructor(private stringConstants: StringConstants, public bsModalRef: BsModalRef, private sharedService: SharedService,
              private loaderService: LoaderService, public uploadService: UploadService, private acmapiService: ACMReportsService,
              private acmSharedService: AcmReportSharedService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.chooseFilePlaceHolder = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.searchText = this.stringConstants.searchText;

    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.maxDocumentRestrictionMsg = this.stringConstants.maxDocumentRestrictionMsg;
    this.reviewerDocumentFilesList = [];
    this.reviewerDocumentObjList = [];
    this.searchReviewerDocumentFilesList = [];
    this.searchReviewerDocumentObjList = [];
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

  /**
   *  Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        // set selected observation detail on save and next, pagination
        this.acmSharedService.updatedReviewerDocumentsSubject.subscribe(() => {
          this.reviewerDocumentFilesList = [];
          const reviewerDocumentList = this.acmSharedService.reviewerDocumentList.filter(a => a.userId === this.userId);
          for (const row of reviewerDocumentList) {
            this.reviewerDocumentFilesList.push(row.uploadedFile as File);
          }
        }, (error) => {
          this.sharedService.handleError(error);
        });

        this.chooseFilePlaceHolder = (this.reviewerDocumentObjList.length !== 0 || this.reviewerDocumentFilesList.length !== 0) ? '' : this.stringConstants.noFileChosen;
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
   * Called after the view is initially rendered
   */
  ngAfterViewInit() {
    this.myDialog.nativeElement.parentElement.parentElement.parentElement.parentElement.classList.add('remove-click');
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
   * Method to open delete confirmation dialogue
   * @param index: index fo the entry
   */
  openDeleteModalFromList(index: number) {
    if (this.reviewerDocumentFilesList.length !== 0) {
      this.reviewerDocumentFilesList.splice(index, 1);
    }
    this.chooseFilePlaceHolder = this.reviewerDocumentFilesList.length !== 0 ? '' : this.stringConstants.noFileChosen;
    this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
    this.searchReviewerDocumentFilesList = [];
    this.searchReviewerDocumentObjList = [];
  }

  /**
   * Method to open delete confirmation dialogue
   * @param reviewerDocumentId: reviewer Document Id
   */
  openDeleteModal(reviewerDocumentId: string) {
    if (reviewerDocumentId !== undefined) {
      this.acmapiService.aCMReportsDeleteReviewerDocument(reviewerDocumentId, this.selectedEntityId).subscribe(() => {
        this.acmSharedService.uploadReviewerDocumentAfterDelete(reviewerDocumentId);
        const documentIndex = this.reviewerDocumentObjList.findIndex(a => a.id === reviewerDocumentId);
        if (this.reviewerDocumentObjList.length !== 0) {
          this.reviewerDocumentObjList.splice(documentIndex, 1);
        }
        this.chooseFilePlaceHolder = this.reviewerDocumentObjList.length !== 0 ? '' : this.stringConstants.noFileChosen;

        this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
        this.searchReviewerDocumentFilesList = [];
        this.searchReviewerDocumentObjList = [];
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      const documentIndex = this.reviewerDocumentObjList.findIndex(a => a.id === reviewerDocumentId);
      if (this.reviewerDocumentObjList.length !== 0) {
        this.reviewerDocumentObjList.splice(documentIndex, 1);
      }
      this.chooseFilePlaceHolder = this.reviewerDocumentObjList.length !== 0 ? '' : this.stringConstants.noFileChosen;

      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      this.searchReviewerDocumentFilesList = [];
      this.searchReviewerDocumentObjList = [];
    }
  }


  /**
   *  On file change choose file for upload
   * @param event: it will be any type of event. so param type is not defined
   */
  uploadFileTemporaryOnChoose(event) {
    const fileCount = event.target.files.length + this.reviewerDocumentFilesList.length + this.reviewerDocumentObjList.length;
    if (fileCount <= Number(this.stringConstants.fileLimit)) {
      for (const file of event.target.files) {
        const selectedFile = file;
        this.chooseFilePlaceHolder = '';
        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {
          this.reviewerDocumentFilesList.push(selectedFile);
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }
  }

  /**
   * Add/Edit reviewer document
   */
  uploadedDocument() {
    this.acmSharedService.uploadReviewerDocuments(this.reviewerDocumentFilesList, this.userId);
    this.bsModalRef.hide();
  }

  /**
   * Download select docuemnt
   * @param reviewerDocuemntId: Select reviewer docuemnt id
   */
  downloadFile(reviewerDocumentId: string) {
    this.acmSharedService.downloadReviewerDocument(reviewerDocumentId, this.selectedEntityId);
  }

  /**
   * View file in new tab
   * @param reviewerDocumentId: reviewer Document Id
   */
  viewFile(reviewerDocumentId: string) {
    this.acmSharedService.openDocumentToView(reviewerDocumentId, this.selectedEntityId);
  }

  /***
   * Search document Data
   * @param event: key press event
   */
  searchFile(event: KeyboardEvent) {
    this.searchReviewerDocumentFilesList = [];
    this.searchReviewerDocumentObjList = [];
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      if (this.searchValue !== '') {
        this.isShowSearch = true;
        for (const row of this.reviewerDocumentFilesList) {
          if (row.name.toLowerCase().includes(this.searchValue.toLowerCase())) {
            this.searchReviewerDocumentFilesList.push(row);
          }
        }

        for (const row of this.reviewerDocumentObjList) {
          if (row.documentName.toLowerCase().includes(this.searchValue.toLowerCase())) {
            this.searchReviewerDocumentObjList.push(row);
          }
        }
      } else {
        this.isShowSearch = false;
        this.searchReviewerDocumentFilesList = [];
        this.searchReviewerDocumentObjList = [];
      }
    }
  }
}
