import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { AcmService, RatingAC, KeyValuePairOfIntegerAndString, EntityUserMappingAC, ACMReviewerAC } from '../swaggerapi/AngularFiles';
import { ACMReportsService } from '../swaggerapi/AngularFiles/api/aCMReports.service';
import { SharedService } from '../core/shared.service';
import { ACMReportDetailAC } from '../swaggerapi/AngularFiles/model/aCMReportDetailAC';
import { ACMReportAC } from '../swaggerapi/AngularFiles/model/aCMReportAC';
import { ACMReportMappingAC } from '../swaggerapi/AngularFiles/model/aCMReportMappingAC';
import { ACMReviewerDocumentAC } from '../swaggerapi/AngularFiles/model/aCMReviewerDocumentAC';

@Injectable()
export class AcmReportSharedService {

  constructor(
    private acmsService: AcmService,
    private acmReportsService: ACMReportsService,
    private router: Router,
    private sharedService: SharedService) { }

  acmDetailAC = {} as ACMReportDetailAC; // all detail for report observation
  acmReportList = [] as Array<ACMReportAC>; // report observations list
  acmReport: ACMReportAC; // Current selected report observation
  totalRecord: number; // total count of report observations
  processId: string; // selected process id
  subProcessId: string; // selected sub process id
  entityId: string; // selected entity id
  acmId: string; // current report id

  ratingList = [] as Array<RatingAC>; // rating list
 // observationTypeList = [] as Array<KeyValuePairOfIntegerAndString>; // observation type list
 // dispositionList = [] as Array<KeyValuePairOfIntegerAndString>; // disposition list
  reportStatusList = [] as Array<KeyValuePairOfIntegerAndString>; // report status list
  reportStageList = [] as Array<KeyValuePairOfIntegerAndString>; // report stage list
  responsiblePersonList = [] as Array<EntityUserMappingAC>; // responsible person list
  auditorList = [] as Array<EntityUserMappingAC>; // auditor user list
  linkedReportList = [] as Array<ACMReportAC>;
  acmReviewerList = [] as Array<ACMReportMappingAC>; // all available reviewer
  reportId: string;  // current selected observation id
  reportList = [] as Array<ACMReportDetailAC>;

  isObservationDelete = false; // set observation deleted ot not from observation list page


  // Reviewer document
  allReviewerDocumentList = [] as Array<ACMReviewerAC>;
  reviewerDocumentList = [] as Array<ACMReviewerDocumentAC>;
  reviewerDocumentAC = {} as ACMReviewerDocumentAC;

  reportFiles = [] as Array<File>;

  acmReportSubject = new BehaviorSubject<ACMReportDetailAC>({} as ACMReportDetailAC);
  reportListSubject = new BehaviorSubject<Array<ACMReportAC>>([] as Array<ACMReportAC>);
  selectedReportSubject = new BehaviorSubject<string>('');
  acmReportListSubject = new BehaviorSubject<Array<ACMReportAC>>([] as Array<ACMReportAC>);
  updateLinkedReportSubject = new BehaviorSubject<Array<ACMReportAC>>([] as Array<ACMReportAC>);
  selectedReportDeletedSubject = new BehaviorSubject<boolean>(false);
  updatedReviewerDocumentsSubject = new BehaviorSubject<Array<ACMReviewerDocumentAC>>([] as Array<ACMReviewerDocumentAC>);
  updateReportTableCount = new BehaviorSubject<number>(0);
  updateReportFileCount = new BehaviorSubject<number>(0);

  /**
   * Update observation whenever data changes from pagination
   * @param observationId : Selected observation id
   * @param fileCount : file count
   */
   updateAddedDocumentCount(observationId: string, fileCount: number) {
     this.acmDetailAC = this.reportList.find(a => a.id === observationId);
     this.acmDetailAC.fileCount = fileCount;
     this.updateReportFileCount.next(fileCount);
   }

  /**
   * Set reviewerdocument list
   * @param reviewerList: reviewer list
   */
  setReviewerDocumentList(reviewerList: Array<ACMReviewerAC>) {
    this.allReviewerDocumentList = reviewerList;
  }
  /**
   * Update reviewer document list
   * @param reviewerDocuments: document list
   * @param userId: reviewer user id
   */
  uploadReviewerDocuments(reviewerDocuments: Array<File>, userId: string) {
    if (userId !== '') {
      // remove previosly added document
      const removeDocuments = this.reviewerDocumentList.filter(a => a.userId === userId);
      for (const row of removeDocuments) {
        const index = this.reviewerDocumentList.findIndex(a => a.userId === userId);
        this.reviewerDocumentList.splice(index, 1);
      }
      // reassign documentlist
      for (const row of reviewerDocuments) {
        this.reviewerDocumentAC = {} as ACMReviewerDocumentAC;
        this.reviewerDocumentAC.uploadedFile = {} as File;
        this.reviewerDocumentAC.userId = userId;
        this.reviewerDocumentAC.uploadedFile = row;
        this.reviewerDocumentList.push(this.reviewerDocumentAC);
      }
    } else {
      this.reviewerDocumentList = [];
    }

    this.updatedReviewerDocumentsSubject.next(this.reviewerDocumentList);
  }

  /**
   * Update reviewer document list after delete
   * @param reviewerDocumentId: reviewer document id
   */
  uploadReviewerDocumentAfterDelete(reviewerDocumentId: string) {
    for (const row of this.allReviewerDocumentList) {
      const index = row.reviewerDocumentList.findIndex(a => a.id === reviewerDocumentId);
      row.reviewerDocumentList.splice(index, 1);
    }
  }

  /**
   * Download reviewer document
   * @param reviewerDocumentId : Id of the reviewer document
   * @param selectedEntityId : Current selected auditable entiity
   */
  downloadReviewerDocument(reviewerDocumentId: string, selectedEntityId: string) {
    this.acmReportsService.aCMReportsDownloadReviewerDocument(reviewerDocumentId, selectedEntityId).subscribe((result) => {
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
   * Method to open document
   * @param reviewerDocumentId: reviewer Document Id
   * @param selectedEntityId : Current selected auditable entiity
   */
  openDocumentToView(reviewerDocumentId: string, selectedEntityId: string) {
    this.acmReportsService.aCMReportsDownloadReviewerDocument(reviewerDocumentId, selectedEntityId).subscribe((result) => {
      // Open document in new tab
      const url = this.router.serializeUrl(
        // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
        this.router.createUrlTree(['document-preview/view', { path: btoa(result) }])
      );
      window.open(url, '_blank');
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }
}
