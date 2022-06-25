import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { EntityDocumentAC, EntityDocumentsService } from '../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { UploadService } from '../../../core/upload.service';
import { Router } from '@angular/router';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-entity-documents-add',
  templateUrl: './entity-documents-add.component.html'
})
export class EntityDocumentsAddComponent implements OnInit {

  documentsTitle: string; // Variable for documents label
  purposeTitle: string; // Variable for purpose title
  chooseFileText: string; // Variable for choose file text
  wordToolTip: string; // Variable for word Tooltip
  noFileChosen: string; // Variable for no file choosen label
  powerPointToolTip: string; // Variable for powerpoint tooltip
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for file name text
  saveButtonText: string; // Variable for save button text
  entityDocumentId: string;
  entityId: string;
  deleteTitle: string; // Variable for delete title
  requiredMessage: string;
  maxLengthExceedMessage: string;

  invalidMessage: string;
  isFileUploaded: boolean;

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

  entityDocumentDetail: EntityDocumentAC;
  entityDocumentFilesList: File[] = [];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    public bsModalRefForDelete: BsModalRef,
    private entityDocumentsService: EntityDocumentsService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private uploadService: UploadService,
    private modalService: BsModalService,
    public router: Router) {
    this.documentsTitle = this.stringConstants.documentsTitle;
    this.purposeTitle = this.stringConstants.purposeTitle;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.saveButtonText = this.stringConstants.saveButtonText;

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
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.entityDocumentDetail = {} as EntityDocumentAC;
    this.entityDocumentFilesList = [];
    this.isFileUploaded = false;
    this.entityDocumentId = '';
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    if (this.entityDocumentId !== '') {
      this.getEntityDocumentDetails();
    }
  }


  /**
   * Method to get entityDocument Details
   */
  getEntityDocumentDetails() {
    this.loaderService.open();
    this.entityDocumentsService.entityDocumentsGetEntityDocumentDetailsById(this.entityDocumentId).subscribe((result: EntityDocumentAC) => {
      this.entityDocumentDetail = result;
      if (result.path === undefined || result.path === null) {
        this.isFileUploaded = false;
        this.noFileChosen = this.stringConstants.noFileChosen;
      } else {
        this.isFileUploaded = true;
        this.noFileChosen = '';
      }

      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.sharedService.showError(error.error);
      });
  }

  /**
   * Save Entity Document
   */
  saveEntityDocument() {
    this.loaderService.open();
    if (this.entityDocumentId === '') {
      this.addEntityDocument();
    } else {
      this.uploadService.uploadFileOnUpdate<EntityDocumentAC>(this.entityDocumentDetail, this.entityDocumentFilesList,
        this.stringConstants.entityDocumentFiles, this.stringConstants.entityDocumentApiPath).subscribe((result: EntityDocumentAC) => {
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          this.loaderService.close();
          this.bsModalRef.hide();
          this.bsModalRef.content.callback(result);
        },
          (error) => {
            this.loaderService.close();
            this.sharedService.showError(error.error);
          });
    }
  }

  /**
   * Add Entity Document
   */
  addEntityDocument() {

    this.entityDocumentDetail.entityId = this.entityId;

    this.uploadService.uploadFileOnAdd<EntityDocumentAC>(this.entityDocumentDetail, this.entityDocumentFilesList,
      this.stringConstants.entityDocumentFiles, this.stringConstants.entityDocumentApiPath).subscribe((result: EntityDocumentAC) => {

        this.bsModalRef.hide();
        this.loaderService.close();
        this.bsModalRef.content.callback(result);

      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
  }


  /**
   * File input change event
   * @param event: Onchange event
   */
  fileChange(event) {
    if (this.entityDocumentId !== '' && (this.entityDocumentDetail.path !== '' || this.entityDocumentDetail.fileName !== '')) {
      this.sharedService.showError(this.stringConstants.entityDocumentFileUploadError);
    } else {
      this.entityDocumentDetail.path = '';
      this.entityDocumentDetail.fileName = '';
      this.entityDocumentFilesList = [];

      for (const file of event.target.files) {
        this.isFileUploaded = true;
        this.noFileChosen = '';
        const selectedFile = file;
        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {
          this.entityDocumentFilesList.push(selectedFile);
        }
      }
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
  openDocument(documnetPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documnetPath) }])
    );
    window.open(url, '_blank');
  }

  /**
   * Method to download doc
   * @param docId: entity DocId
   */
  downloadDoc(docId: string) {
    this.entityDocumentsService.entityDocumentsGetEntityDocumentDownloadUrl(docId).subscribe((result) => {
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
   * @param index: index
   * @param docPath: doc path
   */
  openDeleteModal(index: number, docPath: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRefForDelete = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (docPath !== '') {

              this.loaderService.open();
              this.entityDocumentDetail.path = '';
              this.entityDocumentDetail.fileName = '';
              this.isFileUploaded = false;
              this.noFileChosen = this.stringConstants.noFileChosen;
              this.loaderService.close();

            } else {
              this.entityDocumentFilesList.splice(index, 1);
              this.entityDocumentFilesList = [...this.entityDocumentFilesList];
              if (this.entityDocumentFilesList.length === 0) {
                this.isFileUploaded = false;
                this.noFileChosen = this.stringConstants.noFileChosen;
              }
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }
          }
        }
      }
    });
  }
}
