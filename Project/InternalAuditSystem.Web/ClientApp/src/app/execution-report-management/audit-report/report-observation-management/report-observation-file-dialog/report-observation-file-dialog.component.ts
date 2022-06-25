import { Component, OnInit, ElementRef, ViewChild, AfterViewInit, OnDestroy } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../../../shared/stringConstants';
import { SharedService } from '../../../../core/shared.service';
import { ReportObservationsService, ReportObservationsDocumentAC } from '../../../../swaggerapi/AngularFiles';
import { UploadService } from '../../../../core/upload.service';
import { LoaderService } from '../../../../core/loader.service';
import { ReportSharedService } from '../../report-shared.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-observation-file-dialog',
  templateUrl: './report-observation-file-dialog.component.html'
})
export class ReportObservationFileDialogComponent implements OnInit, AfterViewInit, OnDestroy {
  saveButtonText: string; // Variable for save button text
  title: string; // Variable for modal title
  chooseFileText: string; // Variable for choose file text
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for fileNameText
  searchText: string; // Variable for search text
  reportObservationId;

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

  reportObservationDocumentFilesList: File[];
  formDataFilesList: FormData;
  reportObservationDocumentObjList = [] as Array<ReportObservationsDocumentAC>;
  reportObservationDocumentObj = {} as ReportObservationsDocumentAC;
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  requiredMessage: string;
  maxLengthExceedMessage: string;
  maxDocumentRestrictionMsg: string;
  isFileUploaded: boolean;

  searchReportObservationDocumentObjList = [] as Array<ReportObservationsDocumentAC>;
  searchReportObservationDocumentFilesList: File[];
  searchValue;
  isShowSearch = false;
  chooseFilePlaceHolder: string;
  @ViewChild('reportObservationDocument', { static: false }) myDialog: ElementRef;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, public bsModalRef: BsModalRef, private sharedService: SharedService,
              private loaderService: LoaderService, public uploadService: UploadService, private apiService: ReportObservationsService,
              private reportSharedService: ReportSharedService) {
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
    this.reportObservationDocumentFilesList = [];
    this.reportObservationDocumentObjList = [];
    this.searchReportObservationDocumentFilesList = [];
    this.searchReportObservationDocumentObjList = [];
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
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.reportObservationDocumentObjList = JSON.parse(JSON.stringify(this.reportSharedService.reportObservation.reportObservationDocumentList));

        // set selected observation detail on save and next, pagination
        this.reportSharedService.updateReportObservationDocumentsSubject.subscribe(() => {
          this.reportObservationDocumentFilesList = [];
          this.reportObservationDocumentFilesList = this.reportSharedService.observationFiles;
        });
        this.chooseFilePlaceHolder = (this.reportObservationDocumentObjList.length !== 0 || this.reportObservationDocumentFilesList.length !== 0) ? '' : this.stringConstants.noFileChosen;
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
    if (this.reportObservationDocumentFilesList.length !== 0) {
      this.reportObservationDocumentFilesList.splice(index, 1);
    }
    this.chooseFilePlaceHolder = this.reportObservationDocumentFilesList.length !== 0 ? '' : this.stringConstants.noFileChosen;
    this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
    this.searchReportObservationDocumentFilesList = [];
    this.searchReportObservationDocumentObjList = [];

  }

  /**
   * Method to open delete confirmation dialogue
   * @param index: index fo the entry
   */
  openDeleteModal(reportObservationDocumentId: string) {
    if (reportObservationDocumentId !== undefined) {
      this.apiService.reportObservationsDeleteReportObservationDocument(reportObservationDocumentId, this.selectedEntityId).subscribe(() => {
        this.reportSharedService.uploadObservationDocumentsAfterDelete(reportObservationDocumentId);
        const documentIndex = this.reportObservationDocumentObjList.findIndex(a => a.id === reportObservationDocumentId);
        if (this.reportObservationDocumentObjList.length !== 0) {
          this.reportObservationDocumentObjList.splice(documentIndex, 1);
        }
        this.chooseFilePlaceHolder = this.reportObservationDocumentObjList.length !== 0 ? '' : this.stringConstants.noFileChosen;
        this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
        this.searchReportObservationDocumentFilesList = [];
        this.searchReportObservationDocumentObjList = [];
      }, (error) => {
        this.sharedService.handleError(error);
      });
    } else {
      const documentIndex = this.reportObservationDocumentObjList.findIndex(a => a.id === reportObservationDocumentId);
      if (this.reportObservationDocumentObjList.length !== 0) {
        this.reportObservationDocumentObjList.splice(documentIndex, 1);
      }
      this.chooseFilePlaceHolder = this.reportObservationDocumentObjList.length !== 0 ? '' : this.stringConstants.noFileChosen;
      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      this.searchReportObservationDocumentFilesList = [];
      this.searchReportObservationDocumentObjList = [];
    }
  }


  /**
   *  On file change choose file for upload
   * @param event: it will be any type of event. so param type is not defined
   */
  uploadFileTemporaryOnChoose(event) {
    const fileCount = event.target.files.length + this.reportObservationDocumentFilesList.length + this.reportObservationDocumentObjList.length;
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
          this.reportObservationDocumentFilesList.push(selectedFile);
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }
  }

  /**
   * Add/Edit report observation documnet
   */
  uploadedDocument() {
    this.reportSharedService.uploadObservationDocuments(this.reportObservationDocumentFilesList, this.reportObservationId);
    const fileCount = this.reportObservationDocumentFilesList.length + this.reportObservationDocumentObjList.length;
    // update document count
    this.reportSharedService.updateAddedDocumentCount(this.reportObservationId, fileCount);
    this.bsModalRef.hide();
  }

  /**
   * Download select docuemnt
   * @param reportObservationDocumentId : Select report observation document id
   */
  downloadFile(reportObservationDocumentId: string) {
    this.reportSharedService.downloadReportObservationDocument(reportObservationDocumentId, this.selectedEntityId);
  }

  /**
   * View file in new tab
   * @param observationDocumentId: observation document id
   */
  viewFile(observationDocumentId: string) {
    this.reportSharedService.openReportObservationDocumentToView(observationDocumentId, this.selectedEntityId);
  }


  /**
   * Search document Data
   * @param event: key press event
   */
  searchFile(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      if (this.searchValue !== '') {
        this.isShowSearch = true;
        for (const row of this.reportObservationDocumentFilesList) {
          if (row.name.toLowerCase().includes(this.searchValue.toLowerCase())) {
            this.searchReportObservationDocumentFilesList.push(row);
          }
        }

        for (const row of this.reportObservationDocumentObjList) {
          if (row.documentName.toLowerCase().includes(this.searchValue.toLowerCase())) {
            this.searchReportObservationDocumentObjList.push(row);
          }
        }
      } else {
        this.isShowSearch = false;
        this.searchReportObservationDocumentFilesList = [];
        this.searchReportObservationDocumentObjList = [];
      }
    }
  }
  /**
   * close file upload dialog
   */
  closeModel() {
    const fileCount = this.reportObservationDocumentFilesList.length + this.reportObservationDocumentObjList.length;
    // update document count
    this.reportSharedService.updateAddedDocumentCount(this.reportObservationId, fileCount);
    this.bsModalRef.hide();


  }
}
