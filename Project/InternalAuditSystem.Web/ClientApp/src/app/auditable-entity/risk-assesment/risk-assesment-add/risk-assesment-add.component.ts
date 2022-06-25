import { Component, OnInit, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { UploadService } from '../../../core/upload.service';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { Router } from '@angular/router';
import { RiskAssessmentAC } from '../../../swaggerapi/AngularFiles/model/riskAssessmentAC';
import { RiskAssessmentStatus } from '../../../swaggerapi/AngularFiles/model/riskAssessmentStatus';
import { RiskAssessmentsService } from '../../../swaggerapi/AngularFiles';

@Component({
  selector: 'app-risk-assesment-add',
  templateUrl: './risk-assesment-add.component.html'
})
export class RiskAssesmentAddComponent implements OnInit, AfterViewInit {
  riskAssessmentDetailsTitle: string; // Variable for risk assesment title
  assessmentName: string; // Variable for assesment name
  year: string; // Variable for year name
  summaryOfAssessment: string; // Variable for summary of assesment
  attachment: string; // Variable for attachment
  statusTitle: string; // Variable for status
  saveButtonText: string; // Save button text
  noFileChosen: string; // Variable for no file choosen label
  powerPointToolTip: string; // Variable for powerpoint tooltip
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for file name text
  wordToolTip: string; // Varible for word tooltip
  chooseFileText: string; // Variable for choose file text
  riskAssessmentId: string;
  entityId: string;
  deleteTitle: string; // Variable for delete title
  requiredMessage: string;
  maxLengthExceedMessage: string;
  min4DigitRequired: string;

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


  riskAssessmentDetail: RiskAssessmentAC;
  riskAsssessmentFilesList: File[] = [];
  @ViewChild('riskDocument', { static: false }) myDialog: ElementRef;

  // TODO: Added static code here, respective developer will change it in future
  // Items for status list
  statusList = [
    {
      value: RiskAssessmentStatus.NUMBER_0,
      label: 'Pending',
    },
    {
      value: RiskAssessmentStatus.NUMBER_1,
      label: 'Draft',
    },
    {
      value: RiskAssessmentStatus.NUMBER_2,
      label: 'UnderView',
    },
    {
      value: RiskAssessmentStatus.NUMBER_3,
      label: 'Final',
    },
  ];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    public bsModalRef: BsModalRef,
    public bsModalRefForDelete: BsModalRef,
    private riskAssessmentsService: RiskAssessmentsService,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private uploadService: UploadService,
    private modalService: BsModalService,
    public router: Router) {
    this.riskAssessmentDetailsTitle = this.stringConstants.riskAssessmentDetailsTitle;
    this.assessmentName = this.stringConstants.assessmentName;
    this.year = this.stringConstants.year;
    this.summaryOfAssessment = this.stringConstants.summaryOfAssessment;
    this.attachment = this.stringConstants.attachment;
    this.statusTitle = this.stringConstants.statusTitle;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.min4DigitRequired = this.stringConstants.min4DigitRequired;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.riskAssessmentDetail = {} as RiskAssessmentAC;
    this.riskAssessmentDetail.riskAssessmentDocumentACList = [];
    this.riskAsssessmentFilesList = [];
    this.isFileUploaded = false;

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


  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    if (this.riskAssessmentId !== '') {
      this.getRiskAssessmentDetails();
    }
  }
  /**
   * Called after the view is initially rendered
   */
  ngAfterViewInit() {
    this.myDialog.nativeElement.parentElement.parentElement.parentElement.parentElement.parentElement.classList.add('remove-click');
  }

  /**
   * Method to get RiskAssessment Details
   */
  getRiskAssessmentDetails() {
    this.loaderService.open();
    this.riskAssessmentsService.riskAssessmentsGetRiskAssessmentDetailsById(this.riskAssessmentId).subscribe((result: RiskAssessmentAC) => {
      this.riskAssessmentDetail = result;
      if (result.riskAssessmentDocumentACList === undefined || result.riskAssessmentDocumentACList === null) {
        this.riskAssessmentDetail.riskAssessmentDocumentACList = [];
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
   * Save Risk Assessment
   */
  saveRiskAssessment() {
    this.loaderService.open();
    if (this.riskAssessmentId === '') {
      this.addRiskAssessment();
    } else {
      this.uploadService.uploadFileOnUpdate<RiskAssessmentAC>(this.riskAssessmentDetail, this.riskAsssessmentFilesList,
        this.stringConstants.riskAssessmentDocumentFiles, this.stringConstants.riskAssesmentApiPath).subscribe((result: RiskAssessmentAC) => {
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          result.statusString = this.statusList[result.status].label;
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
   * Add risk assessment
   */
  addRiskAssessment() {

    this.riskAssessmentDetail.entityId = this.entityId;

    this.uploadService.uploadFileOnAdd<RiskAssessmentAC>(this.riskAssessmentDetail, this.riskAsssessmentFilesList,
      this.stringConstants.riskAssessmentDocumentFiles, this.stringConstants.riskAssesmentApiPath).subscribe((result: RiskAssessmentAC) => {
        result.statusString = this.statusList[result.status].label;
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
    const fileCount = event.target.files.length + this.riskAsssessmentFilesList.length + this.riskAssessmentDetail.riskAssessmentDocumentACList.length;
    if (fileCount <= Number(this.stringConstants.fileLimit)) {
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
          this.riskAsssessmentFilesList.push(selectedFile);
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
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
   * Method to open delete confirmation dialogue
   * @param index: index
   * @param riskAssessmentDocId: riskAssessmentDocId
   */
  openDeleteModal(index: number, riskAssessmentDocId: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRefForDelete = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (riskAssessmentDocId !== '') {

              this.loaderService.open();
              this.riskAssessmentsService.riskAssessmentsDeleteRiskAssessmentDocumment(riskAssessmentDocId).subscribe(() => {
                this.riskAssessmentDetail.riskAssessmentDocumentACList.splice(
                  this.riskAssessmentDetail.riskAssessmentDocumentACList.indexOf(this.riskAssessmentDetail.riskAssessmentDocumentACList.filter(x => x.id === riskAssessmentDocId)[0]), 1);
                this.riskAssessmentDetail.riskAssessmentDocumentACList = [...this.riskAssessmentDetail.riskAssessmentDocumentACList];
                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
                if (this.riskAssessmentDetail.riskAssessmentDocumentACList.length === 0) {
                  this.isFileUploaded = false;
                  this.noFileChosen = this.stringConstants.noFileChosen;
                }
                this.loaderService.close();
              }, (error) => {
                this.sharedService.showError(this.stringConstants.somethingWentWrong);
                this.loaderService.close();
              });
            } else {
              this.riskAsssessmentFilesList.splice(index, 1);
              this.riskAsssessmentFilesList = [...this.riskAsssessmentFilesList];
              if (this.riskAsssessmentFilesList.length === 0) {
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
   * Method to download work paper
   * @param riskAssessmentDocId: riskAssessmentDocId
   */
  downloadRiskAssessmentDoc(riskAssessmentDocId: string) {
    this.riskAssessmentsService.riskAssessmentsGetRiskAssessmentDocummentDownloadUrl(riskAssessmentDocId).subscribe((result) => {
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
   * Method to delete file recently added but not uploaded
   * @param index: index
   */
  onAddedFileDelete(index: number) {
    this.riskAsssessmentFilesList.splice(index, 1);
    this.riskAsssessmentFilesList = [...this.riskAsssessmentFilesList];
  }

}
