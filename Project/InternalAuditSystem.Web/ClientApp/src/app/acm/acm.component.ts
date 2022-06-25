import { Component, OnInit, OnDestroy, Inject, Optional } from '@angular/core';
import { StringConstants } from '../shared/stringConstants';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../core/shared.service';
import { LoaderService } from '../core/loader.service';
import { Pagination } from '../models/pagination';
import { AcmService, ACMPresentationAC, RatingAC, BASE_PATH } from '../swaggerapi/AngularFiles';
import { AddTableDialogComponent } from '../shared/add-table-dialog/add-table-dialog.component';
import { Subscription } from 'rxjs';
import { ACMSharedService } from './acm-shared.service';

@Component({
  selector: 'app-acm',
  templateUrl: './acm.component.html'
})
export class AcmComponent implements OnInit, OnDestroy {
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  statusTitle: string; // Variable for status title
  activeText: string; // Variable for active button text
  editToolTip: string; // Variable for edit tool tip
  deleteToolTip: string; // Variable for delete tool tip
  showingResults: string; // Variable of showing results
  acmTitle: string; // Variable for acm title
  headingLabel: string; // Variable for heading
  observationTabTitle: string; // Variable for observation tab title
  recommendationTitle: string; // Variable for recommendationn title
  managementResponseTitle: string; // Variable for management response
  ratingLabel: string; // Variable for rating label
  implicationTitle: string; // Variable for implication
  wordToolTip: string; // Varible for word tooltip
  powerPointToolTip: string; // Variable for powerpoint
  pdfToolTip: string; // Variable for pdf tooltip
  bsModalRef: BsModalRef; // Modal ref variable
  viewFiles: string; // Variable for view files

  // only to subscripe for the current component
  entitySubscribe: Subscription;

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

  // ACM list array
  acmList = [] as Array<ACMPresentationAC>;
  acmSearchList: ACMPresentationAC[];
  selectedAcm: string[];
  newAcmAddList: ACMPresentationAC[] = [];
  rating: RatingAC[];

  addTableTitle: string;
  pageNumber: number = null;
  totalRecords: number;
  searchValue = '';
  id: string; // Variable for acm id

  ofText: string; // Variable for of tooltip
  showingText: string; // Variable for showing tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list

  deleteTitle: string; // Variable for title
  showNoDataText: string; // Variable for showing text for no data
  uploadText: string; // Variable for upload text
  selectedEntityId;
  addTableToolTip: string;
  baseUrl: string;
  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              private acmServices: AcmService,
              public router: Router,
              private modalService: BsModalService,
              private route: ActivatedRoute,
              private sharedService: SharedService,
              private acmSharedService: ACMSharedService,
              private loaderService: LoaderService,
              @Optional() @Inject(BASE_PATH) basePath: string) {
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.activeText = this.stringConstants.activeText;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.acmTitle = this.stringConstants.acmTitle;
    this.headingLabel = this.stringConstants.headingLabel;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.recommendationTitle = this.stringConstants.recommendationTitle;
    this.managementResponseTitle = this.stringConstants.managementResponseTitle;
    this.viewFiles = this.stringConstants.viewFiles;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.implicationTitle = this.stringConstants.implicationTitle;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.statusTitle = this.stringConstants.statusTitle;
    this.addTableTitle = this.stringConstants.addTableTitle;

    this.ofText = this.stringConstants.ofText;
    this.showingResults = '';
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.uploadText = this.stringConstants.uploadText;
    this.addTableToolTip = this.stringConstants.addTableToolTip;

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

    this.baseUrl = basePath;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getACM(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
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

  /***
    * Search ACM Data
    * @param event: key press event
    * @param pageNumber: current page number
    * @param selectedPageItem:  no. of items display on per page
    * @param searchValue: search value
    */
  searchACM(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getACM(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
    }
  }

  /***
   * Get ACM Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  async getACM(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.loaderService.open();
    this.acmServices.acmGetACMData(pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<ACMPresentationAC>) => {
      this.acmList = result.items;
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Set showing result data
   * @param pageNumber:  current page no.
   * @param selectedPageItem: items per page
   * @param totalRecords: total no. of records
   */
  setShowingResult(pageNumber: number, selectedPageItem: number, totalRecords: number) {
    const startItem = (pageNumber - 1) * selectedPageItem + 1;
    const endItem = (totalRecords < (pageNumber * selectedPageItem)) ? totalRecords : (pageNumber * selectedPageItem);
    this.showingResults = this.showingText + ' ' + startItem + ' - ' + endItem + ' ' + this.ofText + ' ' + totalRecords;
  }

  /**
   * On change current page
   * @param pageNumber: Current page no.
   */
  onPageChange(pageNumber) {
    this.getACM(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getACM(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Open add - ACM page
   */
  openAddACM() {
    this.router.navigate(['acm/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method to get Acm list for search dropdown
   * @param pageNumber: current page no.
   * @param fromYear: starting year - selected from year
   * @param toYear: ending year - selected from year
   */
  getACMSearchList(pageNumber: number, fromYear: number, toYear: number) {
    this.acmServices.acmGetACMData(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId, fromYear, toYear).subscribe((result: Pagination<ACMPresentationAC>) => {
      this.acmSearchList = result.items;
    }, (error) => {
      this.sharedService.handleError(error);
    });
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
   * Delete ACM
   * @param acmId: id to delete ACM
   */
  openDeleteModalACM(acmId: string) {

    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.acmServices.acmDeleteAcmPresentation(acmId, this.selectedEntityId).subscribe(data => {
              this.acmList = [];
              this.getACM(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
              this.loaderService.close();
            }, (error) => {
              this.sharedService.handleError(error);
            });
          }
        }
      }
    });
  }

  /**
   * Edit ACM
   * @param acmId: id to edit ACM
   */
  editACMId(acmId: string) {
    this.router.navigate(['acm/add', { id: acmId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method to open add table dialog
   * @param acmId ACM id of which table data is to be shown
   */
  openAddTableDialog(acmObjectId: string) {

    this.acmServices.acmGetACMTable(acmObjectId, this.selectedEntityId).subscribe(
      result => {
        const initialState = {
          title: this.addTableTitle,
          keyboard: true,
          tableList: result,
          acmId: acmObjectId
        };
        this.bsModalRef = this.modalService.show(AddTableDialogComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      },
      error => {
        this.sharedService.showError(this.stringConstants.somethingWentWrong);
      }
    );
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
   * @param documentId : Select ACM document id
   */
  downloadFile(documentId: string) {
    this.acmServices.acmGetACMDocumentDownloadUrl(documentId, this.selectedEntityId).subscribe((result) => {
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
   * Method to open delete confirmation dialog
   * @param ACMPresentation: index
   * @param acmId: ACM Id
   */
  openDeleteModal(acm: ACMPresentationAC, acmDocumentId: string) {
    const initialState = {
      title: this.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          if (acmDocumentId !== '') {
            this.loaderService.open();
            this.acmServices.acmDeleteACMDocument(acmDocumentId, this.selectedEntityId).subscribe(() => {
              acm.acmDocuments.splice(
                acm.acmDocuments.indexOf(acm.acmDocuments.filter(x => x.id === acmDocumentId)[0]), 1);
              acm.acmDocuments = [...acm.acmDocuments];
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
              this.loaderService.close();
            }, (error) => {
              this.sharedService.showError(this.stringConstants.somethingWentWrong);
              this.loaderService.close();
            });
          }
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   *  Method for export to excel of acm
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelAcmApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }

  /**
   * Create ACM PPT
   * @param acmId: selected acm id
   */
  createACMPPT(acmId: string) {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.downloadACMPPTApi + acmId + this.stringConstants.entityParamString + this.selectedEntityId + this.stringConstants.timeOffSet + timeOffset;
    this.sharedService.createPPT(url);
  }

}
