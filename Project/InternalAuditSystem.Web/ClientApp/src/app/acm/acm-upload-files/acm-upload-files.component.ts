import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { ObservationsManagementService, ObservationAC, Disposition, ObservationStatus, ACMPresentationAC, ACMStatus, AcmService, ACMDocumentAC, ACMTableAC } from '../../swaggerapi/AngularFiles';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../core/upload.service';
import { SharedService } from '../../core/shared.service';
import { Subscription } from 'rxjs';
import { LoaderService } from '../../core/loader.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ACMSharedService } from '../acm-shared.service';
import { count } from 'rxjs/operators';
import { AcmAddTabs } from '../acm-add/acm-add.model';



@Component({
  selector: 'app-acm-upload-files-dialog',
  templateUrl: './acm-upload-files.component.html'
})
export class ACMUploadFilesComponent implements OnInit, OnDestroy {
  saveButtonText: string;
  title: string;
  noFileChosen: string;
  chooseFileText: string;
  wordToolTip: string;
  powerPointToolTip: string;
  pdfToolTip: string;
  fileNameText: string;
  searchText: string;
  acmId: string;
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
  acmWithFiles = {} as ACMPresentationAC;
  chooseFilePlaceHolder: string;
  activeTabTitle: string;

  acmAC: ACMPresentationAC;
  entityId: string;
  searchValue = null;
  selectedEntityId: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  unSavedFileSubScribe: Subscription;
  savedFileSubscribe: Subscription;
  tabSubscribe: Subscription;
  savedDocuments = {} as Array<ACMDocumentAC>;
  newFilesOFActiveTab: File[] = [];
  // Per page items for status list
  acmStatusList = [
    {
      value: ACMStatus.NUMBER_0, label: 'Open',
    },
    {
      value: ACMStatus.NUMBER_1, label: 'Close',
    }
  ];
  newUploadedFileCount: number;
  savedFileCount: number;
  isToHideSearh = true;
  currentActiveTab: AcmAddTabs;


  // Creates an instance of documenter.
  constructor(
    public bsModalRef: BsModalRef,
    private stringConstants: StringConstants,
    private apiService: AcmService,
    private router: Router,
    private sharedService: SharedService,
    private acmSharedService: ACMSharedService,
    private loaderService: LoaderService) {
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.searchText = this.stringConstants.searchText;
    this.files = [];
    this.acmAC = {} as ACMPresentationAC;

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
    this.chooseFilePlaceHolder = this.stringConstants.noFileChosen;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  async ngOnInit() {
    this.files = [];
    this.isToHideSearh = !((this.savedDocuments !== null && this.savedDocuments.length > 0) || (this.files !== null && this.files.length > 0));

    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.acmSharedService.tabListSubject.subscribe((tabList) => {

          // find current tab
          this.currentActiveTab = tabList.find(x => x.title === this.activeTabTitle);

          // set newly uploaded file list
          if (this.currentActiveTab !== undefined) {
            this.files = this.currentActiveTab.temporaryFiles === undefined ? [] : this.currentActiveTab.temporaryFiles;
            if (this.acmId === null) {
              this.savedDocuments = [];
            }
            this.isToHideSearh = !((this.savedDocuments !== null && this.savedDocuments.length > 0) || (this.files !== null && this.files.length > 0));
          }
        });
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
    if (this.savedFileSubscribe !== undefined) {
      this.savedFileSubscribe.unsubscribe();
    }
  }

  /**
   * Search file by name
   * @param event Event
   * @param searchValue Search value by which to be searched
   */
  searchFileByName(event: KeyboardEvent, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      // update in the deleted list
      this.acmSharedService.tabListSubject.subscribe((updatedTabList: Array<AcmAddTabs>) => {
        const newFiles = this.currentActiveTab.temporaryFiles === undefined ? [] : this.currentActiveTab.temporaryFiles;
        const savedFiles = this.currentActiveTab.acmDetails.acmDocuments;

        // search of new files
        if (searchValue !== '' && searchValue !== null && newFiles !== undefined && newFiles.length > 0) {
          this.files = newFiles;
          this.files = this.files.filter(x => x.name.toLowerCase().includes(searchValue.toLowerCase()));
        } else {
          this.files = newFiles;
        }

        // search on saved files
        if (searchValue !== '' && searchValue !== null && savedFiles !== undefined && savedFiles.length > 0) {
          this.savedDocuments = savedFiles;
          this.savedDocuments = this.savedDocuments.filter(x => x.fileName.toLowerCase().includes(searchValue.toLowerCase()));
        } else {
          this.savedDocuments = savedFiles;
        }

      });
    }
  }

  /**
   * File input change event
   * @param event: Onchange event
   */
  uploadFileTemporaryOnChoose(event) {
    const fileCount = event.target.files.length + this.files.length + this.savedDocuments.length;
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
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }
    // update in the deleted list
    this.acmSharedService.tabListSubject.subscribe((updatedTabList: Array<AcmAddTabs>) => {
      this.currentActiveTab = updatedTabList.find(x => x.title === this.activeTabTitle);
      if (this.currentActiveTab !== undefined) {
        updatedTabList.find(x => x.title === this.activeTabTitle).temporaryFiles = this.files;
      }

      const totalSavedFiles = (this.savedDocuments !== undefined && this.savedDocuments !== null) ? this.savedDocuments.length : 0;
      const totalNewFiles = (this.files.length !== undefined && this.files.length !== null) ? this.files.length : 0;

      // seyt updated file count
      const updatedFileCount = totalSavedFiles + totalNewFiles;

      this.isToHideSearh = false;
      // update total count
      this.acmSharedService.setFilesCount(updatedFileCount);
    });
  }

  /**
   * Close modal window on save
   */
  onSave() {
    this.bsModalRef.hide();
  }

  /**
   * Check file type to display icon accordingly
   * @param fileName :Name of the File
   * @param fileTypeCheck : type to check
   */
  checkFileExtention(fileName: string, fileTypeCheck: string) {
    let isUploadedFormatMatched = false;
    isUploadedFormatMatched = this.acmSharedService.checkFileExtention(fileName, fileTypeCheck);
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
   * @param documentId : Select acm document id
   */
  downloadFile(documentId: string) {
    this.apiService.acmGetACMDocumentDownloadUrl(documentId, this.selectedEntityId).subscribe((result) => {
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
   * Delete saved acm document from azure
   * @param acmDocumentId : Id of the acm document
   */
  deleteSavedFile(acmDocumentId: string) {
    this.apiService.acmDeleteACMDocument(acmDocumentId, this.selectedEntityId).subscribe(() => {
      const isFileDeleted = true;
      this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
      // this.getACMById(null);
      const index = this.savedDocuments.findIndex(x => x.id === acmDocumentId);
      this.savedDocuments.splice(index, 1);

      // update in the deleted list
      this.acmSharedService.tabListSubject.subscribe((updatedTabList: Array<AcmAddTabs>) => {
        if (isFileDeleted) {
          this.currentActiveTab = updatedTabList.find(x => x.id === this.acmId);
          if (this.currentActiveTab !== undefined) {
            updatedTabList.find(x => x.id === this.acmId).acmDetails.acmDocuments = JSON.parse(JSON.stringify(this.savedDocuments));
          }
        }
      });

      this.updateFileCountOnDelete(this.savedDocuments.length, this.files.length);

    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Delete document from newly added files list
   * @param index Index from where file to be deleted
   */
  deleteFileFromNewlyAdded(index: number) {
    const isFileDeleted = true;
    this.files.splice(index, 1);
    this.files = [...this.files];

    // update in the deleted list
    this.acmSharedService.tabListSubject.subscribe((updatedTabList: Array<AcmAddTabs>) => {
      if (isFileDeleted) {
        this.currentActiveTab = updatedTabList.find(x => x.id === this.acmId);
        if (this.currentActiveTab !== undefined) {
          updatedTabList.find(x => x.title === this.activeTabTitle).temporaryFiles = JSON.parse(JSON.stringify(this.files));
        }
      }
    });
    this.updateFileCountOnDelete(this.savedDocuments.length, this.files.length);
  }

  /**
   * Update file count on delete of files
   * @param savedFilesCount : Total count of saved files
   * @param newFilesCount : Total count of new files
   */
  updateFileCountOnDelete(savedFilesCount: number, newFilesCount: number) {

    const latestFileCount = savedFilesCount + newFilesCount;
    this.isToHideSearh = latestFileCount === 0;
    this.acmSharedService.setFilesCount(latestFileCount);
  }
}
