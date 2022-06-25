import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { RiskAssesmentAddComponent } from './risk-assesment-add/risk-assesment-add.component';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Pagination } from '../../models/pagination';
import { UploadService } from '../../core/upload.service';
import { RiskAssessmentAC } from '../../swaggerapi/AngularFiles/model/riskAssessmentAC';
import { RiskAssessmentsService, UserAC, UserRole } from '../../swaggerapi/AngularFiles';
import { Subscription, combineLatest } from 'rxjs';

@Component({
  selector: 'app-risk-assesment',
  templateUrl: './risk-assesment.component.html',
  styleUrls: ['./risk-assesment.component.scss']
})

export class RiskAssesmentComponent implements OnInit, OnDestroy {
  riskAssessmentDetailsTitle: string; // Variable for risk assestment title
  riskAssesmentNote: string;
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  bsModalRef: BsModalRef; // Modal ref variable
  deleteTitle: string; // Variable for delete title
  viewFiles: string; // Variable for view files
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pd tool tip
  fileNameText: string; // Variable for filename
  assessmentName: string; // Varibale for assesment name
  year: string; // Variable for year
  summaryOfAssessment: string; // Variable for summary of assesment
  attachment: string; // Variable for attachment
  backToolTip: string; // Variable for back tooltip
  statusTitle: string; // Variable for status
  previousButton: string; // VAriable for previous button
  nextButton: string; // VAriable for next
  selectedPageItem: number;
  searchValue: string;
  entityId: string;
  selectedEntityId: string;
  saveNextButtonText: string;
  validationMessage: string;

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

  pageNumber: number = null;
  totalRecords: number;
  riskSearchValue = null;
  riskSelectedPageItem: number;
  pageItems = []; // Per page items for entity list
  showNoDataText: string;

  riskAssesmentList: RiskAssessmentAC[] = [];
  userSubscribe: Subscription;
  currentUserDetails: UserAC;
  subscriptions: Subscription[] = [];

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private riskAssessmentsService: RiskAssessmentsService,
    private uploadService: UploadService,
    private changeDetection: ChangeDetectorRef) {
    this.riskAssessmentDetailsTitle = this.stringConstants.riskAssessmentDetailsTitle;
    this.riskAssesmentNote = this.stringConstants.riskAssesmentNote;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingResults = this.stringConstants.showingResults;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.viewFiles = this.stringConstants.viewFiles;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.backToolTip = this.stringConstants.backToolTip;
    this.assessmentName = this.stringConstants.assessmentName;
    this.year = this.stringConstants.year;
    this.summaryOfAssessment = this.stringConstants.summaryOfAssessment;
    this.attachment = this.stringConstants.attachment;
    this.statusTitle = this.stringConstants.statusTitle;
    this.previousButton = this.stringConstants.previousButton;
    this.nextButton = this.stringConstants.nextButton;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.validationMessage = this.stringConstants.auditableEntityRequiredMessage;


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

    this.pageItems = this.stringConstants.smallPageItems;
    this.riskSelectedPageItem = this.pageItems[0].noOfItems;
  }
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // current logged in user details
    this.userSubscribe = this.sharedService.currentUserDetailsSubject.subscribe((currentUserDetails) => {
      if (currentUserDetails !== null) {
        this.currentUserDetails = currentUserDetails.userDetails;
        // if current user is team member restrict access
        if (this.currentUserDetails.userRole === UserRole.NUMBER_2) {
          this.router.navigate([this.stringConstants.unauthorizedPath]);
        } else {
          this.route.params.subscribe(params => {
            this.selectedPageItem = params.pageItems;
            this.searchValue = params.searchValue;
            if (params.id !== undefined) {
              this.entityId = params.id;
              this.getRiskAssessmentList(this.pageNumber, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);
            }
          });
        }
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.userSubscribe.unsubscribe();
  }

  /**
   * Get PrimaryGeographicalArea list of list page
   * @param pageNumber: current page number
   * @param selectedPageItem: selected item per page
   * @param searchValue: search value for search bar
   * @param id: entityid
   */
  getRiskAssessmentList(pageNumber: number, selectedPageItem: number, searchValue: string, id: string) {
    this.loaderService.open();
    this.riskAssessmentsService.riskAssessmentsGetRiskAssessmentList(id, pageNumber,
      selectedPageItem, searchValue).subscribe((result: Pagination<RiskAssessmentAC>) => {

        this.riskAssesmentList = result.items;
        if (this.riskAssesmentList.length === 0) {
          this.validationMessage = this.stringConstants.auditableEntityRequiredMessage;
        } else {
          this.validationMessage = '';
        }

        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);

        this.loaderService.close();
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
  }

  /**
   * Method that open PrimaryGeographicalArea add/update modal
   * @param id: id if in edit else blanl
   * @param index: index number
   */
  openRiskAssesmentAddModal(id: string, index: number) {
    const combine = combineLatest([
      this.modalService.onShow,
      this.modalService.onShown,
      this.modalService.onHide,
      this.modalService.onHidden]
    ).subscribe(() => this.changeDetection.markForCheck());

    this.subscriptions.push(
      this.modalService.onHide.subscribe((reason: string) => {
        this.getRiskAssessmentList(this.pageNumber, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);

      })
    );

    this.subscriptions.push(combine);

    const initialState = {
      title: this.assessmentName,
      class: 'page-modal audit-team-add',
      keyboard: true,
      entityId: this.entityId,
      riskAssessmentId: id,

      callback: (result: RiskAssessmentAC) => {
        if (result !== undefined) {
          this.bsModalRef.hide();
          if (id === '') {
            // if selected item per page is less than the item per page then only push in current list
            if (this.riskAssesmentList.length < this.riskSelectedPageItem) {
              this.riskAssesmentList.push(result);
            }
            this.getRiskAssessmentList(this.pageNumber, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);

            this.totalRecords = this.totalRecords + 1;
            // set footer showing message
            this.showingResults = this.sharedService.setShowingResult(this.pageNumber, this.riskSelectedPageItem, this.totalRecords);

            this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          } else {
            // update particular value of the entry
            for (const doc of result.riskAssessmentDocumentACList) {
              if (doc.fileName !== null) {
                doc.path = doc.fileName;
              }
            }
            this.riskAssesmentList[index] = result;
            this.getRiskAssessmentList(this.pageNumber, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);
          }
        }
      },
    };
    this.bsModalRef = this.modalService.show(RiskAssesmentAddComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
  }

  /**
   * Method to open delete confirmation dialogue
   * @param riskAssessmentAC: riskAssessmentAC object
   * @param riskAssessmentId: riskAssessmentId
   */
  openDeleteModal(riskAssessmentAC: RiskAssessmentAC, riskAssessmentId: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            if (riskAssessmentId !== '') {

              this.loaderService.open();
              this.riskAssessmentsService.riskAssessmentsDeleteRiskAssessmentDocumment(riskAssessmentId).subscribe(() => {
                riskAssessmentAC.riskAssessmentDocumentACList.splice(
                  riskAssessmentAC.riskAssessmentDocumentACList.indexOf(riskAssessmentAC.riskAssessmentDocumentACList.filter(x => x.id === riskAssessmentId)[0]), 1);
                riskAssessmentAC.riskAssessmentDocumentACList = [...riskAssessmentAC.riskAssessmentDocumentACList];
                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);

                this.loaderService.close();
              }, (error) => {
                this.sharedService.showError(this.stringConstants.somethingWentWrong);
                this.loaderService.close();
              });
            }
          }
        }
      }
    });
  }

  /**
   * Unsubcribe method for modal pop up
   */
  unsubscribe() {
    this.subscriptions.forEach((subscription: Subscription) => {
      subscription.unsubscribe();
    });
    this.subscriptions = [];
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getRiskAssessmentList(pageNumber, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRiskAssessmentList(null, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);
  }

  /**
   * Search PrimaryGeographicalArea on grid
   * @param event: key event tab and enter
   */
  searchRiskAssessment(event: KeyboardEvent) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.riskSearchValue = this.riskSearchValue.trim();
      this.getRiskAssessmentList(null, this.selectedPageItem, this.riskSearchValue, this.entityId);
    }
  }
  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['auditable-entity/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method to open document
   * @param documnetPath: documnetPath
   */
  openDocument(riskAssessmentId: string) {
    this.loaderService.open();
    this.riskAssessmentsService.riskAssessmentsGetRiskAssessmentDocummentDownloadUrl(riskAssessmentId).subscribe((result) => {
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
   * Method to download riskAssessment
   * @param riskAssessmentId: riskAssessmentId
   */
  downloadRiskAssessmentDoc(riskAssessmentId: string) {
    this.loaderService.open();
    this.riskAssessmentsService.riskAssessmentsGetRiskAssessmentDocummentDownloadUrl(riskAssessmentId).subscribe((result) => {
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
   * Delete RiskAssessment from List
   * @param riskAssessmentId: riskAssessment Id
   */
  deleteRiskAssessment(riskAssessmentId: string) {

    const initialState = {
      title: this.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.loaderService.open();
          this.riskAssessmentsService.riskAssessmentsDeleteRiskAssessmentAync(riskAssessmentId).subscribe(() => {
            this.loaderService.close();
            this.getRiskAssessmentList(null, this.riskSelectedPageItem, this.riskSearchValue, this.entityId);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
          }, (error) => {
            this.loaderService.close();
            this.sharedService.showError(error.error);
          });
        }

      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
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
   * On save and next click route to relation entity mapping page
   */
  onSaveNextClick() {
    let isErrorExist: boolean;
    for (const riskAssessment of this.riskAssesmentList) {

      if (riskAssessment.riskAssessmentDocumentACList !== null && riskAssessment.riskAssessmentDocumentACList.length === 0) {
        isErrorExist = true;
      }
      if (riskAssessment.riskAssessmentDocumentACList === null) {
        isErrorExist = true;
      }
    }
    if (isErrorExist) {
      this.sharedService.showError(this.stringConstants.riskNoDocumentError);
    } else {
      this.router.navigate(['auditable-entity/relationship', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
    }
  }

  /**
   * On previous button route to list page
   */
  onPreviousClick() {
    this.router.navigate(['auditable-entity/geographical-area', { pageItems: this.selectedPageItem, searchValue: this.searchValue, id: this.entityId }]);
  }
}
