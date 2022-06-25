import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { WorkProgramsService } from '../../swaggerapi/AngularFiles/api/workPrograms.service';
import { WorkProgramAC } from '../../swaggerapi/AngularFiles/model/workProgramAC';
import { Pagination } from '../../models/pagination';
import { Router, ActivatedRoute } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { async } from 'rxjs/internal/scheduler/async';
import { UploadService } from '../../core/upload.service';
import { BASE_PATH } from '../../swaggerapi/AngularFiles';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-work-program-list',
  templateUrl: './work-program-list.component.html'
})
export class WorkProgramListComponent implements OnInit, OnDestroy {
  workProgramTitle: string; // Variable for work program title
  searchText: string; // Variable for search text
  excelToolTip: string; // Variable for excel tooltip
  processLabel: string; // Variable for srno field
  showingResults: string; // Variable for showing results

  deleteToolTip: string; // Variable for delete tooltip
  editToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for title
  auditTitleText: string; // Variable for audit title
  statusTitle: string; // Variable for status title
  auditPeriodTitle: string; // Varibale for audit period workPapers
  teamLabel: string; // Variable for team title
  planScopeTitle: string; // Variable plan scrope
  workPapers: string; // Variable for work papers
  viewFiles: string; // Variable for view files
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pd tool tip
  fileNameText: string; // Variable for filename
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;
  selectedEntityId: string;
  baseUrl: string;

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

  workProgramList: WorkProgramAC[] = [];
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private workProgramService: WorkProgramsService,
    private sharedService: SharedService,
    public router: Router,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private uploadService: UploadService,
    @Optional() @Inject(BASE_PATH) basePath: string
  ) {
    this.baseUrl = basePath;
    this.workProgramTitle = this.stringConstants.workProgramTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.processLabel = this.stringConstants.processLabel;
    this.auditTitleText = this.stringConstants.auditTitleText;
    this.statusTitle = this.stringConstants.statusTitle;
    this.auditPeriodTitle = this.stringConstants.auditPeriodTitle;
    this.teamLabel = this.stringConstants.teamLabel;
    this.planScopeTitle = this.stringConstants.planScopeTitle;
    this.workPapers = this.stringConstants.workPapers;
    this.viewFiles = this.stringConstants.viewFiles;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;

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

    this.workProgramList = [];

    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getWorkProgramList(this.pageNumber, this.selectedPageItem, this.searchValue);
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
   * Get work program list for list page with pagination
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   */
  getWorkProgramList(pageNumber: number, selectedPageItem: number, searchValue: string) {
    this.loaderService.open();
    this.workProgramService.workProgramsGetWorkProgramList(this.selectedEntityId, pageNumber,
      selectedPageItem, searchValue).subscribe((result: Pagination<WorkProgramAC>) => {
        this.workProgramList = result.items;
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.searchValue = searchValue;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
        this.loaderService.close();
      }, (error) => {
          this.sharedService.handleError(error);
      });
  }
  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getWorkProgramList(pageNumber, this.selectedPageItem, this.searchValue);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getWorkProgramList(null, this.selectedPageItem, this.searchValue);
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
   * @param workPaperId: workpaper id
   */
  openDeleteModal(workProgram: WorkProgramAC, workPaperId: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (workPaperId !== '') {

              this.loaderService.open();
              this.workProgramService.workProgramsDeleteWorkPaper(workPaperId, this.selectedEntityId).subscribe(() => {
                workProgram.workPaperACList.splice(
                  workProgram.workPaperACList.indexOf(workProgram.workPaperACList.filter(x => x.id === workPaperId)[0]), 1);
                workProgram.workPaperACList = [...workProgram.workPaperACList];
                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);

                this.loaderService.close();
              }, (error) => {
                  this.sharedService.handleError(error);
              });
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
  openDocument(workPaperId: string) {
    this.loaderService.open();
    this.workProgramService.workProgramsGetWorkPaperDownloadUrl(workPaperId, this.selectedEntityId).subscribe((result) => {
      this.loaderService.close();
      // Open document in new tab
      const url = this.router.serializeUrl(
        // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
        this.router.createUrlTree(['document-preview/view', { path: btoa(result) }])
      );
      window.open(url, '_blank');
    });

  }

  /**
   * Method to download work paper
   * @param workPaperId: work paper Id
   */
  downloadWorkPaper(workPaperId: string) {
    this.loaderService.open();
    this.workProgramService.workProgramsGetWorkPaperDownloadUrl(workPaperId, this.selectedEntityId).subscribe((result) => {
      this.loaderService.close();
      const a = document.createElement('a');
      a.setAttribute('style', 'display:none;');
      document.body.appendChild(a);
      a.download = '';
      a.href = result;
      a.target = '_blank';
      a.click();
      document.body.removeChild(a);
    });
  }


  /**
   * Delete Workprogram Id from List
   * @param workProgramId: work program id
   */
  deleteWorkProgam(workProgramId: string) {
    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.workProgramService.workProgramsDeleteWorkProgram(workProgramId, this.selectedEntityId).subscribe(() => {
              this.loaderService.close();
              this.getWorkProgramList(null, this.selectedPageItem, this.searchValue);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
            }, (error) => {
                this.sharedService.handleError(error);
            });
          }
        },
      }
    });
  }
  /**
   * Method for export workprogram to excel
   */
  onExportClick() {
      // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.workProgramExportApiPath + this.selectedEntityId + this.stringConstants.workProgramTimeOffSet + timeOffset;
    this.sharedService.exportToExcel(url);
  }
  /**
   * Redirect to add workprogram page
   */
  onAddClick() {
    this.router.navigate(['work-program/add', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
  /**
   * On edit click in list will redirect to list page
   * @param workProgramId : workprogram id to be edited
   */
  editWorkProgram(workProgramId: string) {
    this.router.navigate(['work-program/add', { id: workProgramId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
  /**
   * Search work program on grid
   * @param event: key event tab and enter
   */
  searchWorkProgram(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.getWorkProgramList(null, this.selectedPageItem, this.searchValue);
    }
  }
}
