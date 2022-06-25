import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { ObservationsManagementService, ObservationAC, Disposition, ObservationStatus, RiskControlMatrixAC, ControlCategory, ControlType, RiskControlMatrixesService, NatureOfControl } from '../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../core/upload.service';
import { SharedService } from '../../core/shared.service';
import { RCMUploadService } from '../rcmUpload.service';



@Component({
  selector: 'app-rcm-upload-files-dialog',
  templateUrl: './rcm-upload-files.component.html'
})
export class RCMUploadFilesComponent implements OnInit {
  saveButtonText: string;
  title: string;
  noFileChosen: string;
  chooseFileText: string;
  wordToolTip: string;
  powerPointToolTip: string;
  pdfToolTip: string;
  fileNameText: string;
  searchText: string;
  rcmId: string;
  files: File[] = [];
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
  filesAdded: string;
  newFilesAdded: string;
  filesPreviouslyAdded: string;
  rcmWithFiles = {} as RiskControlMatrixAC;

  rcmAC: RiskControlMatrixAC;
  entityId: string;
  searchValue = null;

  controlCategoryList = [
    { value: ControlCategory.NUMBER_0, label: 'Strategic' },
    { value: ControlCategory.NUMBER_1, label: 'Operational' },
    { value: ControlCategory.NUMBER_1, label: 'Financial' },
    { value: ControlCategory.NUMBER_1, label: 'Compliance' }
  ];

  controlTypeList = [
    { value: ControlType.NUMBER_0, label: 'Manual' },
    { value: ControlType.NUMBER_1, label: 'Automated' },
    { value: ControlType.NUMBER_2, label: 'SemiAutomated' }
  ];

  natureOfControlList = [
    { value: NatureOfControl.NUMBER_0, label: ' Preventive' },
    { value: NatureOfControl.NUMBER_1, label: ' Detective' }
  ];



  // Creates an instance of documenter.
  constructor(public bsModalRef: BsModalRef,
              private stringConstants: StringConstants,
              private modalService: BsModalService,
              private route: ActivatedRoute,
              private apiService: RiskControlMatrixesService,
              private router: Router,
              private toastr: ToastrService,
              private uploadService: UploadService,
              private sharedService: SharedService,
              private uploadSevice: UploadService,
              private rcmService: RCMUploadService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.searchText = this.stringConstants.searchText;
    this.files = [];
    this.rcmAC = {} as RiskControlMatrixAC;

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

    this.filesAdded = this.stringConstants.filesAdded;
    this.newFilesAdded = this.stringConstants.newFilesAdded;
    this.filesPreviouslyAdded = this.stringConstants.filesPreviouslyAdded;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  async ngOnInit() {
    this.getRCMFilesFromSharedService(null);
  }

  /**
   * Get observation by id
   * @param searchValue Search value by which to be searched
   */
  async getRCMById(searchValue: string) {
    this.apiService.riskControlMatrixesGetRiskControlMatrixDetailsById(this.rcmId, this.entityId).subscribe(
      result => {
        this.rcmAC = result;
        this.rcmService.setRCMs(this.rcmAC);
      },
      error => {
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      });
  }

  /**
   * Get observation files from shared service
   * @param searchValue Search value by which to be searched
   */
  getRCMFilesFromSharedService(searchValue: string) {
    this.rcmService.rcmFilesSubject.subscribe(
      rcmFiles => {

        // Filtering files as searched value
        if (searchValue !== null && rcmFiles !== undefined) {
          this.files = rcmFiles.filter(x => x.name.toLowerCase().includes(searchValue.toLowerCase()));
        } else {
          if (this.files.length === 0) {
            this.files = rcmFiles;
          }
        }

        if (this.files === undefined) {
          this.files = [];
        }

      });
  }

  /**
   * Search file by name
   * @param event Event
   * @param searchValue Search value by which to be searched
   */
  searchFileByName(event: KeyboardEvent, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getRCMFilesFromSharedService(searchValue);
      this.getRCMById(searchValue);
    }
  }

  /**
   * Method for setting status of observation
   */
  setControlCategory() {
    if (this.rcmAC.controlCategoryString === this.controlCategoryList[ControlCategory.NUMBER_0].label) {
      this.rcmAC.controlCategory = ControlCategory.NUMBER_0;
    } else if (this.rcmAC.controlCategoryString === this.controlCategoryList[ControlCategory.NUMBER_1].label) {
      this.rcmAC.controlCategory = ControlCategory.NUMBER_1;
    } else {
      this.rcmAC.controlCategory = ControlCategory.NUMBER_2;
    }
  }

  /**
   * Method for setting disposition
   */
  setControlType() {
    if (this.rcmAC.controlTypeString === this.controlTypeList[ControlType.NUMBER_0].label) {
      this.rcmAC.controlType = ControlType.NUMBER_0;
    } else if (this.rcmAC.controlTypeString === this.controlTypeList[ControlType.NUMBER_1].label) {
      this.rcmAC.controlCategory = ControlCategory.NUMBER_1;
    } else {
      this.rcmAC.controlType = ControlType.NUMBER_2;
    }
  }

  /**
   * Method for setting disposition
   */
  setNatureOfControl() {
    if (this.rcmAC.natureOfControlString === this.natureOfControlList[NatureOfControl.NUMBER_0].label) {
      this.rcmAC.natureOfControl = NatureOfControl.NUMBER_0;
    } else {
      this.rcmAC.natureOfControl = NatureOfControl.NUMBER_1;
    }
  }

  /**
   * File input change event
   * @param event: Onchange event
   */
  fileChange(event) {
    for (const file of event.target.files) {
      const selectedFile = file;
      this.files.push(selectedFile);
    }
    this.rcmService.setRCMFiles(this.files);
  }

  /**
   * Upload files
   */
  onSave() {
    if (this.bsModalRef.content.callback != null) {

      this.rcmService.rcmFilesSubject.subscribe(
        rcmFiles => {
          this.bsModalRef.content.callback(rcmFiles);
          this.bsModalRef.hide();
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
    isUploadedFormatMatched = this.rcmService.checkFileExtention(fileName, fileTypeCheck);
    return isUploadedFormatMatched;
  }

  /**
   * View file in new tab
   * @param documentPath : File path
   */
  viewFile(documentPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documentPath) }])
    );
    window.open(url, '_blank');

  }
  /**
   * Delete document from files list
   * @param index Index from where file to be deleted
   */
  deleteFromFiles(index: number) {
    this.files.splice(index, 1);
    this.files = [...this.files];
  }

}
