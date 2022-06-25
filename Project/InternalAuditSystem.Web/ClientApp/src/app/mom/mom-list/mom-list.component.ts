import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination/public_api';
import { StringConstants } from '../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { MomAC, UserAC, BASE_PATH } from '../../swaggerapi/AngularFiles';
import { Pagination } from '../../models/pagination';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { MomsService } from '../../swaggerapi/AngularFiles/api/moms.service';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { HttpClient } from '@angular/common/http';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-momlist',
  templateUrl: './mom-list.component.html',
  styleUrls: ['./mom-list.component.scss']
})

export class MomListComponent implements OnInit, OnDestroy {

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, private router: Router, private route: ActivatedRoute,
              private momService: MomsService, private modalService: BsModalService,
              private sharedService: SharedService,
              @Optional() @Inject(BASE_PATH) basePath: string) {
    this.momListPageTitle = this.stringConstants.momListPageTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.id = this.stringConstants.id;
    this.workProgramTitle = this.stringConstants.workProgramTitle;
    this.agendaTitle = this.stringConstants.agendaTitle;
    this.dateLabel = this.stringConstants.dateLabel;
    this.showingResults = this.stringConstants.showingResults;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.showingText = this.stringConstants.showingText;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.outlookToolTip = this.stringConstants.outlookToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.toField = '';
    this.fromField = '';
    this.subject = '';
    this.tempUserEmailList = [];
    this.toEmailList = [];
    this.auditableEntityId = this.stringConstants.auditableEntityId;
    this.baseUrl = basePath;
  }
  // Note: this code is commented for the reference of developers(taken confirmation)
  // returnedEntityList: any[] ; // Variable that used to display temporaryentitylist
  momListPageTitle: string; // Variable for page title
  searchText: string; // Variable for search text
  excelToolTip: string; // Variable for excel tooltip
  addToolTip: string; // Varibel for add tooltip
  deleteToolTip: string; // Variable for Delete tooltip
  editToolTip: string; // Variable for Edit tooltip
  id: string; // Variable for id in table
  workProgramTitle: string; // Variable for entity list
  agendaTitle: string; // Varible for Agenda Title
  dateLabel: string; // Variable for Date
  showingResults: string; // Variable for showing results
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  momList = [] as Array<MomAC>;
  showingText: string;
  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  showNoDataText: string;
  outlookToolTip: string; // Variable for outlook
  pdfToolTip: string; // Variable for pdfToolTip
  toField: string;
  fromField: string;
  subject: string;
  tempUserEmailList: Array<string>;
  toEmailList: Array<string>;
  tempUserList: Array<UserAC>;
  auditableEntityId: string;
  momData: MomAC;
  data: object;
  baseUrl: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.auditableEntityId = entityId;
        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getMomList(this.pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId);
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
   * On change current page
   * @param pageNumber:page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getMomList(pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getMomList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId);
  }

  /**
   * Add Mom
   */
  openMomAddPage() {
    this.router.navigate(['mom/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.auditableEntityId }]);
  }

  /**
   * Edit mom
   * @param momId: id to edit mom
   */
  editMom(momId: string) {
    this.router.navigate(['mom/add', { id: momId, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.auditableEntityId }]);
  }

  /**
   * Method for getting list of mom
   * @param pageNumber : Current page number
   * @param selectedPageItem : No. of items display on per page
   * @param searchValue: Search value
   * @param entityId: Selected entityId
   */
  getMomList(pageNumber: number, selectedPageItem: number, searchValue: string, entityId: string) {
    this.momService.momsGetAllMoms(pageNumber, selectedPageItem, searchValue, entityId).subscribe((result: Pagination<MomAC>) => {
      this.momList = result.items;
      for (const mom of this.momList) {
        mom.momDate = this.sharedService.convertLocalDateToUTCDate(mom.momDate, false);
      }
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(result.pageIndex, result.pageSize, result.totalRecords);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /***
  * Search Moms Data
  * @param event: key press event
  * @param pageNumber: current page number
  * @param selectedPageItem:  no. of items display on per page
  * @param searchValue: search value
  */
  searchMoms(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getMomList(pageNumber, selectedPageItem, searchValue, this.auditableEntityId);
    }
  }

  /**
   * Set serial no of a list
   */
  setSerialNo() {
    // calculate serial no
    const startindex = (this.pageNumber !== 1) ? (this.selectedPageItem * (this.pageNumber - 1)) + 1 : 1;
    for (let i = 0; i < this.momList.length; i++) {
      this.momList[i].srNo = startindex + i;
    }
  }


  /**
   * Delete mom
   * @param momId: Id to delete mom
   */
  // Method that open delete confirmation modal
  deleteMom(momId: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,

      callback: (result) => {
        if (result === this.stringConstants.yes) {
          this.momService.momsDeleteMom(momId, this.auditableEntityId).subscribe(data => {
            this.getMomList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
          }, (error) => {
            this.sharedService.handleError(error);
          });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * Method for opening outlook
   * @param momId : Id of mom
   */
  openOutlook(momId: string) {
    this.toEmailList = [];
    this.tempUserEmailList = [];
    this.momService.momsGetMomDetailById(momId, this.auditableEntityId).subscribe(res => {
      this.momData = res;
      this.fromField = this.stringConstants.defaultEmailId;

      this.momData.internalUserList.forEach(y => {
        this.tempUserEmailList.push(y.userId);
      });
      this.momData.externalUserList.forEach(e => {
        this.tempUserEmailList.push(e.userId);
      });

      this.tempUserList = this.momData.allPersonResposibleACDataCollection;

      for (const email of this.tempUserEmailList) {
        const emailId = this.tempUserList.filter(x => x.id === email)[0].emailId;
        this.toEmailList.push(emailId);
      }

      const emailCommaSeperatedString = this.toEmailList.join(', ');
      this.toField = emailCommaSeperatedString;
      this.subject = this.momData.agenda;

      const href = this.stringConstants.mailtoString + this.toField + '?' + this.stringConstants.subjectString + '=' + this.subject;

      window.location.href = href;
    });
  }

  /**
   * Method for generating pdf
   * @param momId : Id of mom
   */
  generatePdf(momId) {
    const offset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.pdfApi + momId + this.stringConstants.offsetString + offset + this.stringConstants.entityParamString + this.auditableEntityId;
    this.sharedService.generatePdf(url);
  }

  /**
   * Export Moms to excel
   */
  exportMoms() {
    // create dynamic link for export file download
    const offset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.exportToExcelApi + this.auditableEntityId + this.stringConstants.offsetString + offset;
    this.sharedService.exportToExcel(url);
  }
}
