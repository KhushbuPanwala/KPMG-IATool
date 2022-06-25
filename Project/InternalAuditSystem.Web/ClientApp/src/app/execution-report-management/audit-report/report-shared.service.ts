import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import {
  ReportDetailAC, ReportObservationAC, RatingAC, KeyValuePairOfIntegerAndString, ObservationCategoryAC,
  ReportObservationsMemberAC, ReportUserMappingAC, EntityUserMappingAC, ObservationAC, ReviewerDocumentAC, ReportsService, ReportObservationsDocumentAC, ReportObservationsService
} from '../../swaggerapi/AngularFiles';
import { Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';

@Injectable()
export class ReportSharedService {

  constructor(
    private reportsService: ReportsService,
    private reportObservationsService: ReportObservationsService,
    private router: Router,
    private sharedService: SharedService) { }

  reportDetailAC = {} as ReportDetailAC; // all detail for report observation
  reportObservationList = [] as Array<ReportObservationAC>; // report observations list
  reportObservation: ReportObservationAC; // Current selected report observation
  totalRecord: number; // total count of report observations
  processId: string; // selected process id
  subProcessId: string; // selected sub process id
  entityId: string; // selected entity id
  reportId: string; // current report id

  ratingList = [] as Array<RatingAC>; // rating list
  observationTypeList = [] as Array<KeyValuePairOfIntegerAndString>; // observation type list
  dispositionList = [] as Array<KeyValuePairOfIntegerAndString>; // disposition list
  observationStatusList = [] as Array<KeyValuePairOfIntegerAndString>; // observation status list
  observationCategoryList = [] as Array<ObservationCategoryAC>; // observation category list
  responsiblePersonList = [] as Array<EntityUserMappingAC>; // responsible person list
  auditorList = [] as Array<EntityUserMappingAC>; // auditor user list
  linkedObservationList = [] as Array<ReportObservationAC>;
  observationReviewerList = [] as Array<ReportUserMappingAC>; // all available reviewer
  observationId: string;  // current selected observation id
  isViewObservation = false; // check for report observation is on view mode
  observationList = [] as Array<ReportObservationAC>;

  isObservationDelete = false; // set observation deleted ot not from observation list page


  // Reviewer document
  allReviewerDocumentList = [] as Array<ReportUserMappingAC>;
  reviewerDocumentList = [] as Array<ReviewerDocumentAC>;
  reviewerDocumentAC = {} as ReviewerDocumentAC;

  observationFiles = [] as Array<File>;
  reportObservationDocumentList = [] as Array<ReportObservationsDocumentAC>;
  reportObservationDocumentAC = {} as ReportObservationsDocumentAC;

  reportObservationSubject = new BehaviorSubject<ReportDetailAC>({} as ReportDetailAC);
  observationListSubject = new BehaviorSubject<Array<ReportObservationAC>>([] as Array<ReportObservationAC>);
  selectedObservationSubject = new BehaviorSubject<string>('');
  selectedOperationTypeSubject = new BehaviorSubject<boolean>(false);
  reportObservationListSubject = new BehaviorSubject<Array<ReportObservationAC>>([] as Array<ReportObservationAC>);
  updateLinkedObservationSubject = new BehaviorSubject<Array<ReportObservationAC>>([] as Array<ReportObservationAC>);
  selectedOperationDeletedSubject = new BehaviorSubject<boolean>(false);
  updatedReviewerDocumentsSubject = new BehaviorSubject<Array<ReviewerDocumentAC>>([] as Array<ReviewerDocumentAC>);
  updateReportObservationDocumentsSubject = new BehaviorSubject<Array<ReportObservationsDocumentAC>>([] as Array<ReportObservationsDocumentAC>);
  updateObservationTableCount = new BehaviorSubject<number>(0);
  updateObservationFileCount = new BehaviorSubject<number>(0);
  /**
   * Update observation list whenever data delete from observation list
   * @param isDeleted: set observation is deleted or not
   */
  updateDeletedObservationList(isDeleted: boolean) {
    this.isObservationDelete = isDeleted;
    this.selectedOperationDeletedSubject.next(isDeleted);
  }

  /**
   * Update observation whenever data changes from save and next
   * @param reportObservation: updated report observation
   */
  updateReportObservationList(reportObservation: ReportObservationAC) {
    const index = this.reportObservationList.findIndex(a => a.id === reportObservation.id);
    this.reportObservationList[index] = reportObservation;
    this.reportObservationListSubject.next(this.reportObservationList);
  }

  /**
   * Update observation whenever data changes from observation list page
   * @param observations : Selected observation
   */
  updateObservationList(observations: ReportObservationAC) {
    this.observationList.push(observations);
    this.observationListSubject.next(this.observationList);
  }

  /**
   * Update linked observation whenever heading change for observation
   * @param reportObservation : updated observation
   */
  updateLinkedObservationList(reportObservation: ReportObservationAC) {
    const linkedObservationIndex = this.linkedObservationList.findIndex(a => a.id === reportObservation.id);
    this.linkedObservationList[linkedObservationIndex] = reportObservation;
    this.updateLinkedObservationSubject.next(this.linkedObservationList);
  }

  /**
   * Update observation whenever data changes from observation list page
   * @param operationType : operation type for add / edit
   */
  updateOperationType(operationType: boolean) {
    this.isViewObservation = operationType;
    this.selectedOperationTypeSubject.next(this.isViewObservation);
  }

  /**
   * Update observation whenever data changes from pagination
   * @param observationId : Selected observation id
   */
  updateObservation(observationId: string) {
    this.observationFiles = [];
    this.observationId = observationId;
    this.reportObservation = this.reportObservationList.find(a => a.observationId === this.observationId);
    this.selectedObservationSubject.next(observationId);
  }


  /**
   * Update observation whenever data changes from pagination
   * @param observationId : Selected observation id
   * @param tableCount : table count
   */
  updateAddTableCount(observationId: string, tableCount: number) {
    this.reportObservation = this.reportObservationList.find(a => a.id === observationId);
    this.reportObservation.tableCount = tableCount;
    this.updateObservationTableCount.next(tableCount);
  }


  /**
   * Update observation whenever data changes from pagination
   * @param observationId : Selected observation id
   * @param fileCount : file count
   */
  updateAddedDocumentCount(observationId: string, fileCount: number) {
    this.reportObservation = this.reportObservationList.find(a => a.id === observationId);
    this.reportObservation.fileCount = fileCount;
    this.updateObservationFileCount.next(fileCount);
  }

  /**
   * Update reportDetailAC whenever data changes for selection
   * @param reportDetailAC : Selected report observation details
   */
  setReportObservations(reportDetailAC: ReportDetailAC) {
    this.reportDetailAC = reportDetailAC;
    this.reportObservationList = reportDetailAC.reportObservationList; // all report
    this.ratingList = reportDetailAC.ratingList;
    this.observationTypeList = reportDetailAC.observationTypeList;
    this.dispositionList = reportDetailAC.disposition;
    this.observationStatusList = reportDetailAC.observationStatus;
    this.observationCategoryList = reportDetailAC.observationCategoryList;
    this.responsiblePersonList = reportDetailAC.responsibleUserList;
    this.observationReviewerList = reportDetailAC.observationReviewerList;
    this.auditorList = reportDetailAC.auditorList;
    this.linkedObservationList = reportDetailAC.linkedObservationList;
    this.totalRecord = reportDetailAC.totalReportObservation;
    this.observationId = this.reportObservationList[0].id; // set first report observation as selected
    this.reportObservation = this.reportObservationList.find(a => a.id === this.observationId); // set selected observation
    this.reportObservationSubject.next(reportDetailAC);
  }


  /**
   * Set reviewerdocument list
   * @param reviewerList: reviewer list
   */
  setReviewerDocumentList(reviewerList: Array<ReportUserMappingAC>) {
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
        this.reviewerDocumentAC = {} as ReviewerDocumentAC;
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
    this.reportsService.reportsDownloadReviewerDocument(reviewerDocumentId, selectedEntityId).subscribe((result) => {
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
    this.reportsService.reportsDownloadReviewerDocument(reviewerDocumentId, selectedEntityId).subscribe((result) => {
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

  /**
   * Update observation document list
   * @param observationDocuments: document list
   * @param reportObservationId: reviewer user id
   */
  uploadObservationDocuments(observationDocuments: Array<File>, reportObservationId: string) {
    this.observationFiles = [];
    this.observationFiles = observationDocuments;
    this.reportObservationDocumentAC = {} as ReportObservationsDocumentAC;
    this.reportObservationDocumentAC.documentFile = [] as Array<File>;
    for (const row of observationDocuments) {
      this.reportObservationDocumentAC.reportObservationId = reportObservationId;
      this.reportObservationDocumentAC.documentFile.push(row);
      this.reportObservationDocumentList.push(this.reportObservationDocumentAC);
    }
    this.updateReportObservationDocumentsSubject.next(this.reportObservationDocumentList);
  }

  /**
   * Update observation document list after delete
   * @param reportObservationDocumentId: report observation document id
   */
  uploadObservationDocumentsAfterDelete(reportObservationDocumentId: string) {
    const index = this.reportObservation.reportObservationDocumentList.findIndex(a => a.id === reportObservationDocumentId);
    this.reportObservation.reportObservationDocumentList.splice(index, 1);
  }


  /**
   * Update observation document list
   * @param observationDocuments: document list
   * @param reportObservationId: report observation id
   */
  uploadObservationDocumentsAfterSave(observationDocuments: Array<ReportObservationsDocumentAC>, reportObservationId: string) {
    this.observationFiles = [];
    this.reportObservationList.find(a => a.id === reportObservationId).reportObservationDocumentList = observationDocuments;
    this.reportObservationDocumentList = [];
  }


  /**
   * Download report observation document
   * @param reportObservationDocumentId: Id of the report Observation Document
   * @param selectedEntityId : Current selected auditable entiity
   */
  downloadReportObservationDocument(reportObservationDocumentId: string, selectedEntityId: string) {
    this.reportObservationsService.reportObservationsDownloadReportObservationDocument(reportObservationDocumentId, selectedEntityId).subscribe((result) => {
      const aTag = document.createElement('a');
      aTag.setAttribute('style', 'display:none;');
      document.body.appendChild(aTag);
      aTag.download = '';
      aTag.href = result;
      aTag.target = '_blank';
      aTag.click();
      document.body.removeChild(aTag);
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Method to open document
   * @param observationDocumentId: observation document id
   * @param selectedEntityId : Current selected auditable entiity
   */
  openReportObservationDocumentToView(observationDocumentId: string, selectedEntityId: string) {
    this.reportObservationsService.reportObservationsDownloadReportObservationDocument(observationDocumentId, selectedEntityId).subscribe((result) => {
      // Open document in new tab
      const url = this.router.serializeUrl(
        // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
        this.router.createUrlTree(['document-preview/view', { path: btoa(result) }])
      );
      window.open(url, '_blank');
    });
  }

  /**
   * Method to view report document
   * @param reportId: selected report id
   * @param selectedEntityId : Current selected auditable entiity
   * @param timeOffset: current user system timezone
   */
  viewReportPPT(reportId: string, selectedEntityId: string, timeOffset: number) {
    this.reportsService.reportsAddViewAuditReport(reportId, selectedEntityId, timeOffset).subscribe((result) => {
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
