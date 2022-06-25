import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { PlanDocumentAddComponent } from './plan-document-add/plan-document-add.component';
import { LoaderService } from '../../core/loader.service';
import { AuditPlansService, AuditPlanAC, AuditPlanSectionType, AuditPlanDocumentAC } from '../../swaggerapi/AngularFiles';
import { AuditPlanSharedService } from '../audit-plan-shared.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { Pagination } from '../../models/pagination';
import { UploadService } from '../../core/upload.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-plan-document',
  templateUrl: './plan-document.component.html'
})
export class PlanDocumentComponent implements OnInit, OnDestroy {
  // pagination varibale
  pageNumber = 1;
  pageItems = [];
  searchValue: string = null;
  itemsPerPage: number;
  totalRecords: number; // per page no. of items shows

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  config = {
    backdrop: true,
    ignoreBackdropClick: false
  };

  searchText: string;
  excelToolTip: string;
  addToolTip: string;
  editToolTip: string;
  deleteToolTip: string;
  showingResults: string;
  bsModalRef: BsModalRef;
  documentsTitle: string;
  purposeTitle: string;
  downloadToolTip: string;
  viewToolTip: string;
  saveButtonText: string;
  previousButton: string;
  existingVersionLabel: string;
  newVersionLabel: string;
  backToolTipMsg: string;
  showNoDataText: string;

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

  // Objects
  auditPlanObj = {} as AuditPlanAC;
  selectedEntityId: string;
  auditPlanId: string;
  sectionType: AuditPlanSectionType;
  planDocumentsList = [] as Array<AuditPlanDocumentAC>;
  isFromAddPage: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private loaderService: LoaderService,
    private auditPlanService: AuditPlansService,
    private auditPlanSharedService: AuditPlanSharedService,
    private activeRoute: ActivatedRoute,
    private router: Router,
    public fileUploadService: UploadService,
    private sharedService: SharedService) {
    this.documentsTitle = this.stringConstants.documentsTitle;
    this.purposeTitle = this.stringConstants.purposeTitle;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.viewToolTip = this.stringConstants.viewToolTip;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.previousButton = this.stringConstants.previousButton;
    this.existingVersionLabel = this.stringConstants.existingVersionLabel;
    this.newVersionLabel = this.stringConstants.newVersionLabel;
    this.sectionType = AuditPlanSectionType.NUMBER_3;
    this.backToolTipMsg = this.stringConstants.backToListPageTooltipMessage;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.searchText = this.stringConstants.searchText;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;

    // pagination settings
    this.pageItems = this.stringConstants.pageItems;
    this.itemsPerPage = this.pageItems[0].noOfItems;

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
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.activeRoute.params.subscribe(params => {
          this.auditPlanId = params.id;
          this.isFromAddPage = params.isFromAdd;
        });
        this.loadPageData();
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
   * Load the current page with selected item , page wise
   */
  loadPageData() {
    this.loaderService.open();
    this.getPlanDocumentsPageAndSerachWise(this.pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId, this.auditPlanId);
  }

  /**
   * Get all plan documents under an audtiable entity based on page size and search value
   * @param pageNumber : Current Page no
   * @param itemsPerPage : No of items per page selected
   * @param searchValue : Search text
   * @param selectedEntityId : Current selected auditable entiity
   * @param auditPlanId : Current selected audit plan Id
   */
  getPlanDocumentsPageAndSerachWise(pageNumber: number, itemsPerPage: number, searchValue: string, selectedEntityId: string, auditPlanId: string) {
    this.auditPlanService.auditPlansGetPlanDocumentsPageWiseAndSearchWiseByPlanId(auditPlanId, pageNumber, itemsPerPage, searchValue, selectedEntityId)
      .subscribe((result: Pagination<AuditPlanDocumentAC>) => {
        this.loaderService.close();
        this.planDocumentsList = [];
        this.planDocumentsList = JSON.parse(JSON.stringify(result.items));

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, itemsPerPage, this.totalRecords);
      }, error => {
          this.auditPlanSharedService.handleError(error);
      });
  }

  /***
   * Search audit plan documents basis of purpose
   * @param event: key press event
   * @param pageNumber: current page number
   * @param itemsPerPage:  no. of items display on per page
   * @param searchValue: search value
   */
  searchPlanDocuments(event: KeyboardEvent, pageNumber: number, itemsPerPage: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      pageNumber = 1;
      this.loaderService.open();
      this.getPlanDocumentsPageAndSerachWise(pageNumber, itemsPerPage, searchValue, this.selectedEntityId, this.auditPlanId);
    }
  }

  /**
   * Open upload document modal
   */
  openDocumentUploadModal() {
    const initialState = {
      title: this.documentsTitle,
      class: 'page-modal audit-team-add',
      keyboard: true,
      auditPlanId: this.auditPlanId,
      planDocumentObj: {} as AuditPlanDocumentAC,
      callback: (result: AuditPlanDocumentAC) => {
        if (result !== undefined) {
          this.loadPageData();
          this.sharedService.showSuccess(this.stringConstants.planDocumentAddMsg);
        }
      },
    };
    this.bsModalRef = this.modalService.show(PlanDocumentAddComponent,
        Object.assign({ initialState }, { class: 'page-modal audit-team-add' }, { ignoreBackdropClick: false }));
  }

  /**
   * Open edit document modal
   * @param index : index of the selected plan document
   */
  openDocumentEditModal(index: number) {
    const planDocumentData = JSON.parse(JSON.stringify(this.planDocumentsList[index]));
    const initialState = {
      title: this.documentsTitle,
      keyboard: true,
      auditPlanId: this.auditPlanId,
      planDocumentObj: planDocumentData,
      callback: (result: AuditPlanDocumentAC) => {
        this.loadPageData();
        if (result !== undefined) {
          this.sharedService.showSuccess(this.stringConstants.planDocumentUpdateMsg);
        }
      },
    };
    this.bsModalRef = this.modalService.show(PlanDocumentAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Delete plan document from azure
   * @param planDocumentId : Id of the plan document
   */
  deleteDocument(planDocumentId: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.auditPlanService.auditPlansDeletePlanDocument(planDocumentId, this.selectedEntityId).subscribe(() => {
            this.loaderService.close();
            this.loadPageData();
            this.sharedService.showSuccess(this.stringConstants.planDocumentDeleteMsg);
          }, error => {
              this.auditPlanSharedService.handleError(error);
          });

        }
      }
    };

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * On page change get data for current page
   * @param pageNumber: Page no. which user has selected
   */
  onPageChange(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.loaderService.open();
    this.getPlanDocumentsPageAndSerachWise(pageNumber, this.itemsPerPage, this.searchValue, this.selectedEntityId, this.auditPlanId);
  }

  /**
   * On item change change page no and its data
   * @param pageItem : Array of page items defined
   */
  onItemPerPageChange(pageItem) {
    this.itemsPerPage = pageItem.noOfItems;

    // if last element then back to previous page
    if (this.planDocumentsList.length === 1) {
      this.pageNumber = this.pageNumber - 1;
    }

    // if current selected item wise page size is greater then start from page first
    if ((this.pageNumber * this.itemsPerPage) > this.planDocumentsList.length) {
      this.pageNumber = 1;
    }
    this.loadPageData();
  }


  /**
   * Redirect to plan-process page
   */
  redirectToPreviousPage() {
    this.auditPlanSharedService.redirectToPreviousPageSectionWise(this.sectionType, this.auditPlanId, this.isFromAddPage);
  }

  /**
   * Redirect to list page
   */
  rediectToListPage() {
    const message = this.isFromAddPage === this.stringConstants.trueString ? this.stringConstants.planAddMsg : this.stringConstants.planUpdateMsg;
    this.sharedService.showSuccess(message);
    this.router.navigate([this.stringConstants.auditPlanListPath]);
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
}
