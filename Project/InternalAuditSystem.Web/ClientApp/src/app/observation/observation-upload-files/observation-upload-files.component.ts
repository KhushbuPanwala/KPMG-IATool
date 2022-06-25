import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { ToastrService } from 'ngx-toastr';
import { ObservationsManagementService, ObservationAC, Disposition, ObservationStatus } from '../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../core/upload.service';
import { SharedService } from '../../core/shared.service';
import { ObservationService } from '../observation.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-observation-upload-files-dialog',
  templateUrl: './observation-upload-files.component.html'
})
export class ObservationUploadFilesComponent implements OnInit, OnDestroy {
  saveButtonText: string;
  title: string;
  noFileChosen: string;
  chooseFileText: string;
  wordToolTip: string;
  powerPointToolTip: string;
  pdfToolTip: string;
  fileNameText: string;
  searchText: string;
  observationId: string;
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
  observationWithFiles = {} as ObservationAC;

  observationAC: ObservationAC;
  entityId: string;
  searchValue = null;
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  dispositionList = [
    { value: Disposition.NUMBER_0, label: 'Reportable' },
    { value: Disposition.NUMBER_1, label: 'NonReportable' },
  ];

  // Per page items for status list
  observationStatusList = [
    {
      value: ObservationStatus.NUMBER_0, label: 'Open',
    },
    {
      value: ObservationStatus.NUMBER_1, label: 'Closed',
    },
    {
      value: ObservationStatus.NUMBER_2, label: 'Pending',
    }

  ];


  // Creates an instance of documenter.
  constructor(public bsModalRef: BsModalRef,
              private stringConstants: StringConstants,
              private modalService: BsModalService,
              private route: ActivatedRoute,
              private apiService: ObservationsManagementService,
              private router: Router,
              private toastr: ToastrService,
              private uploadService: UploadService,
              private sharedService: SharedService,
              private uploadSevice: UploadService,
              private observationService: ObservationService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.searchText = this.stringConstants.searchText;
    this.files = [];
    this.observationAC = {} as ObservationAC;

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
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.getObservationFromSharedService(null);
        this.getObservationFilesFromSharedService(null);
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
   * Get observation by id
   * @param searchValue Search value by which to be searched
   */
  async getObservationById(searchValue: string) {
    this.apiService.observationsManagementGetObservationDetailsById(this.observationId, this.entityId).subscribe(
      result => {
        this.observationAC = result;
        this.observationAC.observationDocuments = result.observationDocuments.filter(x => x.fileName.toLowerCase().includes(this.searchValue.toLowerCase()));
        this.observationService.setObservations(this.observationAC);
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Get observation files from shared service
   * @param searchValue Search value by which to be searched
   */
  getObservationFilesFromSharedService(searchValue: string) {
    this.observationService.observationFilesSubject.subscribe(
      observationFiles => {

        // Filtering files as searched value
        if (searchValue !== null && observationFiles !== undefined) {
          this.files = observationFiles.filter(x => x.name.toLowerCase().includes(searchValue.toLowerCase()));
        } else {
          if (this.files.length === 0) {
            this.files = observationFiles;
          }
        }

        if (this.files === undefined) {
          this.files = [];
        }

      });
  }


  /**
   * Get observation from shared service
   * @param searchValue Search value by which to be searched
   */
  getObservationFromSharedService(searchValue: string) {
    this.observationService.observationSubject.subscribe((observationResultAC) => {
      this.observationAC = observationResultAC;
      if (this.observationAC.id === null || this.observationAC.id === undefined || this.observationAC.id === '0') {
        this.observationAC.linkedObservation = this.observationAC.linkedObservationACList[0].id;
        this.observationAC.targetDate = new Date();
        this.observationAC.dispositionToString = this.dispositionList[0].label;
        this.observationAC.observationStatusToString = this.observationStatusList[0].label;
      } else {
        this.setStatus();
        this.setDisposition();
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
      this.getObservationFilesFromSharedService(searchValue);
      this.getObservationById(searchValue);
    }
  }

  /**
   * Method for setting status of observation
   */
  setStatus() {
    if (this.observationAC.observationStatusToString === this.observationStatusList[ObservationStatus.NUMBER_0].label) {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_0;
    } else if (this.observationAC.observationStatusToString === this.observationStatusList[ObservationStatus.NUMBER_1].label) {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_1;
    } else {
      this.observationAC.observationStatus = ObservationStatus.NUMBER_2;
    }
  }

  /**
   * Method for setting disposition
   */
  setDisposition() {
    if (this.observationAC.dispositionToString === this.dispositionList[Disposition.NUMBER_0].label) {
      this.observationAC.disposition = Disposition.NUMBER_0;
    } else {
      this.observationAC.disposition = Disposition.NUMBER_1;
    }
  }

  /**
   * File input change event
   * @param event: Onchange event
   */
  fileChange(event) {
    let isFileUpload = false;
    const fileCount = event.target.files.length + this.files.length + this.observationAC.observationDocuments.length;
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
          this.files.push(selectedFile);
          isFileUpload = true;
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }
    if (isFileUpload) {
      this.observationService.setObservationFiles(this.files);
    }
  }

  /**
   * Upload files
   */
  onSave() {

    if (this.bsModalRef.content.callback != null) {

      this.observationService.observationFilesSubject.subscribe(
        observationFiles => {
          this.observationService.setObservationFiles(observationFiles);
          this.bsModalRef.hide();
        }, (error) => {
          this.sharedService.handleError(error);
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
   * Download select document
   * @param documentId : Select observation document id
   */
  downloadFile(documentId: string) {
    this.apiService.observationsManagementGetObservationDocumentDownloadUrl(documentId, this.selectedEntityId).subscribe((result) => {
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
   * Delete observation document from azure
   * @param observationDocumentId : Id of the observation document
   */
  deleteDocument(observationDocumentId: string) {

    this.apiService.observationsManagementDeleteObservationDocument(observationDocumentId, this.selectedEntityId).subscribe(() => {
      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      this.getObservationById(null);
      const deleteIndex = this.observationAC.observationDocuments.findIndex(x => x.id === observationDocumentId);
      this.observationAC.observationDocuments.splice(deleteIndex, 1);
      this.observationService.setObservations(this.observationAC);
    }, (error) => {
      this.sharedService.handleError(error);
    });
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
