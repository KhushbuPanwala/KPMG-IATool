import { Component, OnInit, Input, ChangeDetectorRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import {  Router } from '@angular/router';
import { StrategicAnalysisUserMappingAC } from '../../swaggerapi/AngularFiles/model/strategicAnalysisUserMappingAC';
import { StrategicAnalysisAC } from '../../swaggerapi/AngularFiles/model/strategicAnalysisAC';
import { StrategicAnalysesService } from '../../swaggerapi/AngularFiles/api/strategicAnalyses.service';
import { LoaderService } from '../../core/loader.service';
import { StrategicAnalysisStatus, UserAC,  StrategicAnalysisTeamAC } from '../../swaggerapi/AngularFiles';
import { UploadService } from '../../core/upload.service';
import { Subject, Observable } from 'rxjs';
import { SharedService } from '../../core/shared.service';
import { UserDropdownToHide } from '../../models/UserDropdownToHide';

@Component({
  selector: 'app-strategic-analysis-admin-add',
  templateUrl: './strategic-analysis-admin-add.component.html'
})


export class StrategicAnalysisAdminAddComponent implements OnInit {

  surveryFormField: string; // Variable for suvery first field
  auditableEntityTitle: string; // Variable for auditable entity
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation label
  emailIdString: string; // Variable for email id
  messageField: string; // Variable for message field
  createSurveyText: string; // Variable for create survery button
  namePlaceholder: string; // Variable for name placeholder
  designationPlaceholder: string; // Variable for designation placeholder
  emailPlaceholder: string; // Variable for email placeholder
  addToolTip: string; // Variable for add tooltip
  deleteToolTip: string; // Variable for delete tooltip
  backToolTip: string; // Variable for back tooltip
  strategicAnalysisText: string; // Variable for strategic analysis
  strategicAnalysisId: string; // Variable for strategic analysis id
  strategicAnalysis: StrategicAnalysisAC; // Variable for strategic analysis object
  selectedPageItem: number; // Variable for selected page item
  pageNumber: number; // Variable to store page number
  searchValue: string; // Variable for search value
  selectedInternalUsers: Array<StrategicAnalysisUserMappingAC> = []; // Variable for selected internal users
  invalidMessage: string; // Variable for invalid message
  requiredMessage: string; // Variable for required message
  maxLengthExceedMessage: string; // Variable for maxLengthExceedMessage
  message: string; // Variable for message of strategic analysis
  surveyTitleRequiredMessage: string; // Variable of survey title required message
  surveyEntityRequiredMessage: string; // Variable of entity name required message
  isSamplingMessage: string;
  isEdit: boolean;
  isLoading = false;
  teamUser: UserAC;
  isToHideList = true;
  userDropdowns: UserDropdownToHide[];
  selectedUserIndex = 0;
  isDataPopulated: boolean;
  isNoRecordMatched: boolean;
  subject = new Subject<string>();
  dropdownList: Observable<UserAC[]>;
  selectedEntityId: string;
  userAcFromAdList = [] as Array<UserAC>;
  noRecordFound: string;
  teamUserObjectList = [] as Array<UserAC>;
  isCreateSurveyPressed: boolean;
  teamLabel: string;
  isSampling: boolean;

  // Creates an instance of documenter.
  constructor(
    public bsModalRef: BsModalRef,
    public bsModalRefDeleteModal: BsModalRef,
    private modalService: BsModalService,
    public stringConstants: StringConstants,
    private apiService: StrategicAnalysesService,
    public router: Router,
    private loaderService: LoaderService,
    private uploadService: UploadService,
    private sharedService: SharedService
  ) {
    this.backToolTip = this.stringConstants.backToolTip;
    this.surveryFormField = this.stringConstants.surveryFormField;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.emailIdString = this.stringConstants.emailId;
    this.messageField = this.stringConstants.messageField;
    this.createSurveyText = this.stringConstants.createSurvey;
    this.namePlaceholder = this.stringConstants.namePlaceholder;
    this.designationPlaceholder = this.stringConstants.designationPlaceholder;
    this.emailPlaceholder = this.stringConstants.emailPlaceholder;
    this.addToolTip = this.stringConstants.addToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.surveyTitleRequiredMessage = this.stringConstants.surveyTitleRequiredMessage;
    this.surveyEntityRequiredMessage = this.stringConstants.surveyEntityRequiredMessage;
    this.strategicAnalysisText = this.stringConstants.strategicAnalysis;
    this.isSamplingMessage = this.stringConstants.samplingModuleTitle;
    this.noRecordFound = this.stringConstants.noRecordFoundMessage;
    this.teamLabel = this.stringConstants.teamLabel;
    this.selectedInternalUsers = [];
    this.teamUser = {} as UserAC;
    this.strategicAnalysis = {
      id: '0',
      version: 1,
      isSampling: false,
      questionsCount: 0,
      responseCount: 0,
      isDeleted: false,
      isVersionToBeChanged: false,
      auditableEntityId: '0',
      status: StrategicAnalysisStatus.NUMBER_0,
      message: '',
      isResponseDrafted: false,
      internalUserList: [] as StrategicAnalysisTeamAC[]
    };
    this.isEdit = false;
    this.isCreateSurveyPressed = false;
    this.userDropdowns = [{
      isHidden: false,
      isLoading: false
    }] as UserDropdownToHide[];
    this.teamUserObjectList = [] as UserAC[];
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    if (this.isSampling) {
      this.strategicAnalysisText = this.stringConstants.samplingModuleTitle;
      this.surveryFormField = this.stringConstants.samplingAddLabel;
    }
    if (this.strategicAnalysisId !== '0' && this.strategicAnalysisId !== undefined) {
      this.getStrategicAnalysisById();
    } else {
      this.strategicAnalysis = {
        version: 1
      } as StrategicAnalysisAC;
      this.strategicAnalysis.internalUserList = [] as StrategicAnalysisTeamAC[];
      this.teamUserObjectList.push(this.teamUser);
    }

    this.userDropdowns[0].isLoading = false;
    this.userDropdowns[0].isHidden = true;
    this.userDropdowns[0].isLoading = false;

    // get the current selectedEntityId
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      this.selectedEntityId = entityId;
    });
  }


  /**
   * Get Strategic Analysis by id for edit
   */
  getStrategicAnalysisById() {
    this.apiService.strategicAnalysesGetStrategicAnalysisById(this.strategicAnalysisId, null, true).subscribe(result => {
      this.strategicAnalysis = result;
      this.isEdit = true;
      this.message = this.strategicAnalysis.message === null ? '' : this.strategicAnalysis.message;
    });
  }


  /**
   * Add and update survey
   */
  createSurvey() {
    this.isCreateSurveyPressed = true;

    // assign sampling bit
    this.strategicAnalysis.isSampling = this.isSampling;

    // trim text fields value
    this.strategicAnalysis.surveyTitle = this.strategicAnalysis.surveyTitle.trim();

    if (this.strategicAnalysis.surveyTitle !== undefined) {
      this.bsModalRef.hide();
      // Add Survey
      if (this.strategicAnalysisId === undefined) {
        this.loaderService.open();
        this.strategicAnalysis.status = StrategicAnalysisStatus.NUMBER_0;

        this.apiService.strategicAnalysesAddStrategicAnalysis(this.strategicAnalysis).subscribe(result => {
          this.loaderService.close();
          this.strategicAnalysis = result;
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          const routePath = this.isSampling ? '/sampling/create-survey' : '/strategic-analysis-survey/create';
          this.router.navigate([routePath, {
            isSampling: this.isSampling,
            passedStrategicAnalysisid: this.strategicAnalysis.id,
            surveyTitle: this.strategicAnalysis.surveyTitle,
            selectedPageItem: this.selectedPageItem,
            pageNumber: this.pageNumber,
            auditableEntityName: this.strategicAnalysis.auditableEntityName,
            message: this.strategicAnalysis.message === null ? '' : this.strategicAnalysis.message,
            entityId: this.strategicAnalysis.auditableEntityId,
            version: this.strategicAnalysis.version
          }]);
        },
          (error) => {
            this.loaderService.close();
            this.sharedService.showError(error.error);
          });
      } else {
        this.strategicAnalysis.id = this.strategicAnalysisId;
        this.strategicAnalysis.isUserResponseToBeUpdated = false;
        this.uploadService.uploadFileOnUpdate<StrategicAnalysisAC>(this.strategicAnalysis, [], this.stringConstants.strategicAnalysisFiles, this.stringConstants.strategicAnalysisApiPath).subscribe(result => {
          this.loaderService.close();
          this.strategicAnalysis = result;
          this.strategicAnalysis.internalUserList = null;

          // set routing path
          const routePath = this.isSampling ? '/sampling/create-survey' : '/strategic-analysis-survey/create';
          this.router.navigate([routePath, {
            isSampling: this.isSampling,
            passedStrategicAnalysisid: this.strategicAnalysis.id,
            surveyTitle: this.strategicAnalysis.surveyTitle,
            selectedPageItem: this.selectedPageItem,
            pageNumber: this.pageNumber,
            message: this.strategicAnalysis.message === null ? '' : this.strategicAnalysis.message,
            entityId: this.strategicAnalysis.auditableEntityId,
            version: this.strategicAnalysis.version
          }]);
        },
          (error) => {
            this.loaderService.close();
            this.sharedService.showError(error.error);
          });
      }
    }
  }

}
