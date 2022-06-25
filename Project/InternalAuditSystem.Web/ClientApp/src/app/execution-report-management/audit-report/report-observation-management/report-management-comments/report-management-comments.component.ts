import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../../../shared/stringConstants';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { EditorDialogComponent } from '../../../../shared/editor-dialog/editor-dialog.component';
import {
  KeyValuePairOfIntegerAndString,
  ReportObservationAC,
  EntityUserMappingAC,
  ReportObservationsMemberAC,
  ReportUserMappingAC
} from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { LoaderService } from '../../../../core/loader.service';
import { ReportSharedService } from '../../report-shared.service';

@Component({
  selector: 'app-report-management-comments',
  templateUrl: './report-management-comments.component.html',
  styleUrls: ['./report-management-comments.component.scss']
})
export class ReportManagementCommentsComponent implements OnInit {
  managementResponseTitle: string; // Variable for management response section
  conclusionTitle: string; // Variable for conclusion title
  personResponsibleLabel: string; // Variable for person responsible title
  targetDateTitle: string; // Variable for target date title
  linkedObservationTitle: string; // Variable for linked observation title
  statusTitle: string; // Variable for status
  dispositionTitle: string; // Variable for disposition title
  auditorTitle: string; // Variable for auditor title
  nameLabel: string; // Variable for name
  emailLabel: string; // Variable for email
  designationTitle: string; // Variable for designation
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  personLabel: string; // Variable for person responsible
  reviewerCommentsTitle: string; // Variable reviewer comments title

  responsiblePersonList: Array<EntityUserMappingAC>;
  auditorList: Array<EntityUserMappingAC>;
  responsiblePerson = {} as ReportObservationsMemberAC;
  selectedResponsiblePerson;
  invalidMessage: string; // Variable for invalid message
  requiredMessage: string; // Variable for required message
  showNoDataText: string;

  bsModalRef: BsModalRef; // Modal ref variable
  dispositionList: Array<KeyValuePairOfIntegerAndString>;
  observationStatusList: Array<KeyValuePairOfIntegerAndString>;
  reportObservation = {} as ReportObservationAC;
  observationReviewerList = [] as Array<ReportUserMappingAC>;
  linkedObservations = [] as Array<ReportObservationAC>;
  isViewObservation = false;
  dropdownDefaultValue;

  // HTML Editor
  config: AngularEditorConfig = {};

  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService,
    private reportService: ReportSharedService,
    private sharedService: SharedService,
    private loaderService: LoaderService
  ) {
    this.managementResponseTitle = this.stringConstants.managementResponseTitle;
    this.conclusionTitle = this.stringConstants.conclusionTitle;
    this.personResponsibleLabel = this.stringConstants.personResponsibleLabel;
    this.targetDateTitle = this.stringConstants.targetDateTitle;
    this.linkedObservationTitle = this.stringConstants.linkedObservationTitle;
    this.statusTitle = this.stringConstants.statusTitle;
    this.dispositionTitle = this.stringConstants.dispositionTitle;
    this.auditorTitle = this.stringConstants.auditorTitle;
    this.nameLabel = this.stringConstants.nameLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.designationTitle = this.stringConstants.designationLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.personLabel = this.stringConstants.personLabel;
    this.reviewerCommentsTitle = this.stringConstants.reviewerCommentsTitle;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;

  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    this.loaderService.open();
    this.reportService.reportObservationSubject.subscribe((reportDetailAC) => {
      this.reportObservation = this.reportService.reportObservation;
      this.reportObservation.targetDate = new Date(this.reportService.reportObservation.targetDate);
      this.dispositionList = JSON.parse(JSON.stringify(this.reportService.dispositionList));
      this.observationStatusList = JSON.parse(JSON.stringify(this.reportService.observationStatusList));
      this.responsiblePersonList = JSON.parse(JSON.stringify(this.reportService.responsiblePersonList));
      this.selectedResponsiblePerson = this.responsiblePersonList[0].userId;
      this.auditorList = JSON.parse(JSON.stringify(this.reportService.auditorList));
      this.linkedObservations = JSON.parse(JSON.stringify(this.reportService.linkedObservationList));
      this.reportObservation.linkedObservation = this.reportObservation.linkedObservation === null ? null : this.reportObservation.linkedObservation;
      this.loaderService.close();
    });
    this.reportService.selectedObservationSubject.subscribe((reportDetailAC) => {
      this.reportObservation = this.reportService.reportObservation;
      this.loaderService.close();
    });
    this.reportService.selectedOperationTypeSubject.subscribe((operationType) => {
      this.isViewObservation = this.reportService.isViewObservation;
      this.loaderService.close();
    });

    // HTML Editor
    this.config = {
      editable: !this.isViewObservation,
      spellcheck: true,
      height: '15rem',
      minHeight: '5rem',
      placeholder: 'Enter text here...',
      translate: 'no',
      defaultParagraphSeparator: 'p',
      defaultFontName: 'Arial',
      toolbarHiddenButtons: [
        [
          'link',
          'unlink',
          'insertImage',
          'insertVideo',
          'toggleEditorMode',
          'undo',
          'redo',
          'removeFormat'
        ]
      ]
    };
  }

  /**
   * Method to open Management Response editor modal
   */
  openManagementResponseModal() {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.conclusionTitle,
        keyboard: true,
        data: this.reportObservation.managementResponse,
        callback: (result) => {
          this.reportObservation.managementResponse = result;
        }
      }
    });
  }

  /**
   * Method to open Conclusion editor modal
   */
  openConclusionModal() {
    this.modalService.config.class = 'page-modal audit-team-add';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.conclusionTitle,
        keyboard: true,
        data: this.reportObservation.conclusion,
        callback: (result) => {
          this.reportObservation.conclusion = result;
        }
      }
    });
  }

  /**
   * Add user in reviewer list
   * @param userId: selected user id
   */
  addPersonResponsible(userId: string) {
    if (userId === undefined || userId === '') {
      return;
    }
    const addPerson = this.reportObservation.personResponsibleList.find(a => a.userId === userId);
    if (addPerson !== undefined) {
      this.sharedService.showError(this.stringConstants.userExistMsg);
      return;
    }
    const personDetail = this.responsiblePersonList.find(x => x.userId === userId);
    this.responsiblePerson = {} as ReportObservationsMemberAC;
    this.responsiblePerson.userId = personDetail.userId;
    this.responsiblePerson.name = personDetail.name;
    this.responsiblePerson.designation = personDetail.designation;
    this.responsiblePerson.emailId = personDetail.emailId;
    this.reportObservation.personResponsibleList.push(this.responsiblePerson);
    this.selectedResponsiblePerson = '';
  }

  /**
   * Delete person responsible from responsible person list
   * @param userId: deleted user id
   */
  deletePersonResponsible(userId: string) {
    const personDetailIndex = this.reportObservation.personResponsibleList.findIndex(x => x.userId === userId);
    this.reportObservation.personResponsibleList.splice(personDetailIndex, 1);
    this.selectedResponsiblePerson = '';
  }

}
