import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { AuditPlanSectionType, AuditPlanDocumentAC, AuditPlansService } from '../../../swaggerapi/AngularFiles';
import { UploadService } from '../../../core/upload.service';
import { SharedService } from '../../../core/shared.service';
import { AuditPlanSharedService } from '../../audit-plan-shared.service';
import { LoaderService } from '../../../core/loader.service';

@Component({
  selector: 'app-plan-document-add',
  templateUrl: './plan-document-add.component.html'
})
export class PlanDocumentAddComponent implements OnInit, AfterViewInit {
  @ViewChild('planDocument', { static: false }) myDialog: ElementRef;
  documentsLabel: string;
  purposeLabel: string;
  chooseFileButtonText: string;
  wordToolTip: string;
  chooseFilePlaceHolder: string;
  powerPointToolTip: string;
  pdfToolTip: string;
  saveButtonText: string;
  requiredMessage: string;
  maxLengthExceedMessage: string;
  maxDocumentRestrictionMsg: string;
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

  // objects
  planDocumentObj = {} as AuditPlanDocumentAC;
  selectedEntityId: string;
  auditPlanId: string;
  sectionType: AuditPlanSectionType;
  planDocumentFilesList: File[];
  planDocumentObjList = [] as Array<AuditPlanDocumentAC>;

  fileTypeList = [
    { value: 0, label: this.stringConstants.wordFileType },
  ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    public fileUploadService: UploadService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private auditPlanSharedService: AuditPlanSharedService,
    private auditPlanService: AuditPlansService) {
    this.documentsLabel = this.stringConstants.documentsTitle;
    this.purposeLabel = this.stringConstants.purposeTitle;
    this.chooseFilePlaceHolder = this.stringConstants.noFileChosen;
    this.chooseFileButtonText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.maxDocumentRestrictionMsg = this.stringConstants.maxDocumentRestrictionMsg;
    this.planDocumentFilesList = [];
    this.planDocumentObjList = [];

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
  ngOnInit(): void {
    if (this.planDocumentObj.id !== undefined) {
      // get the current selectedEntityId
      this.sharedService.selectedEntitySubject.subscribe((entityId) => {
        this.selectedEntityId = entityId;
      });
      this.planDocumentFilesList = [];
      this.chooseFilePlaceHolder = '';
      this.planDocumentObj.selectedEntityId = this.selectedEntityId;
      this.planDocumentObjList.push(this.planDocumentObj);
    }
  }

  /**
   * Called after the view is initially rendered
   */
  ngAfterViewInit() {
    this.myDialog.nativeElement.parentElement.parentElement.parentElement.parentElement.classList.add('remove-click');
  }

  /**
   * Reload list page on popup close
   */
  reloadListPageOnClose() {
    this.bsModalRef.hide();

    // only reload list page when it is in edit mode
    if (this.planDocumentObj.id !== undefined) {
      this.bsModalRef.content.callback(undefined);
    }
  }

  /**
   * Upload file on
   * @param event : uploaded file object
   */
  uploadFileTemporaryOnChoose(event) {
    for (const file of event.target.files) {
      const selectedFile = file;
      if (this.planDocumentFilesList.length === 0 && this.planDocumentObjList.length === 0) {
        this.planDocumentObj.isNewDocuemntUploaded = true;
        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {
          this.planDocumentFilesList.push(selectedFile);
          this.chooseFilePlaceHolder = '';
        }
      } else {
        this.isFileUploaded = true;
      }
    }
  }

  /**
   * Add/Edit data in plan process list temporarily
   */
  saveUploadedDocument() {
    // set status string
    this.planDocumentObj.planId = this.auditPlanId;
    this.loaderService.open();
    // make server call for saving
    this.fileUploadService.uploadFileOnAdd<AuditPlanDocumentAC>(this.planDocumentObj, this.planDocumentFilesList, 'documentFile', '/api/AuditPlans/add-plan-documents').subscribe((document: AuditPlanDocumentAC) => {
      this.bsModalRef.hide();
      this.bsModalRef.content.callback(document);
    }, error => {
      this.auditPlanSharedService.handleError(error);
    });
  }

  /**
   * Handle error scenario in case of add/ update audit plan
   * @param error : http error
   */
  handleError(error) {
    this.loaderService.close();
    // check if duplicate entry exception then show error message otherwise show something went wrong message
    const errorMessage = error.error !== null ? error.error : this.stringConstants.somethingWentWrong;
    this.sharedService.showError(errorMessage);
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
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsWord(fileName);
        break;
      case this.pdfType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsPdf(fileName);
        break;
      case this.pptType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsPpt(fileName);
        break;
      case this.excelType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsExcel(fileName);
        break;
      case this.pngType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsPng(fileName);
        break;
      case this.jpgType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsJpg(fileName);
        break;
      case this.gifType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsGif(fileName);
        break;
      case this.svgType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsSvg(fileName);
        break;
      case this.mp3Type:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsMp3(fileName);
        break;
      case this.mp4Type:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsMp4(fileName);
        break;
      case this.csvType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsCsv(fileName);
        break;
      case this.zipType:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsZip(fileName);
        break;
      default:
        isUploadedFormatMatched = this.fileUploadService.checkIfFileIsOtherFormat(fileName);
        break;
    }
    return isUploadedFormatMatched;
  }

  /**
   * Method to delete file
   * @param index: index fo the entry
   */
  deleteFile(index: number) {
    if (this.planDocumentObj.id !== undefined) {

      this.auditPlanService.auditPlansDeletePlanDocument(this.planDocumentObj.id, this.selectedEntityId).subscribe(() => {
        this.isFileUploaded = false;
        this.chooseFilePlaceHolder = this.stringConstants.noFileChosen;
        if (this.planDocumentObjList.length !== 0) {
          this.planDocumentObjList.splice(index, 1);
        }
        if (this.planDocumentFilesList.length !== 0) {
          this.planDocumentFilesList.splice(index, 1);
        }

        this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      }, error => {
        this.auditPlanSharedService.handleError(error);
      });
    } else {
      this.isFileUploaded = false;
      this.chooseFilePlaceHolder = this.stringConstants.noFileChosen;
      if (this.planDocumentObjList.length !== 0) {
        this.planDocumentObjList.splice(index, 1);
      }
      if (this.planDocumentFilesList.length !== 0) {
        this.planDocumentFilesList.splice(index, 1);
      }

      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
    }
  }

  /**
   * Download select docuemnt
   * @param planDocuemntId : Select plan docuemnt id
   */
  downloadFile(planDocuemntId: string) {
    this.auditPlanSharedService.downloadPlanDocument(planDocuemntId, this.selectedEntityId);
  }

  /**
   * View file in new tab
   * @param documentPath : File path
   */
  viewFile(documentPath: string) {
    this.auditPlanSharedService.openDocumentToView(documentPath);
  }
}
