import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { StrategicAnalysesService, StrategicAnalysisAC, StrategicAnalysisStatus, StrategicAnalysisTeamAC, UserResponseAC, UserResponseForDetailsAndEstimatedValueOfOpportunity } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { SharedService } from '../../core/shared.service';
import { UploadService } from '../../core/upload.service';

@Component({
  selector: 'app-strategic-analysis-user-survey',
  templateUrl: './strategic-analysis-user-survey.component.html'
})
export class StrategicAnalysisUserSurveyComponent implements OnInit {
  auditableEntityId: string;
  surveyTitle: string; // Variable for survey title
  excelToolTip: string; // Variable for excel tooltip
  pdfToolTip: string; // Variable for pdf tooltip
  surveyFinance: string; // Variable for survey finance title
  engagementPartner: string; // Variable for engagement partner
  engagementManager: string; // Variable for engagement manager
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation label
  emailLabel: string; // Variable for email label
  managerText: string; // Variable for manager text
  detailsOportunity: string; // Variable for detail opportunity
  estimatedValue: string; // Variable for estimated value
  auditPlaceholder: string; // Variable for audit placeholder
  saveNextButtonText: string; // Variable fro save and next button
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  passedStrategicAnalysisId: string;
  strategicAnalysis: StrategicAnalysisAC;
  activeStrategicAnalysisList = [] as StrategicAnalysisAC[];
  userResponseForDetailsOfOppAndEstimatedValue: UserResponseAC;
  namePlaceholder: string;
  teamLabel: string;
  backToolTip: string;
  selectSurveyLabel: string;
  dropdownDefaultValue: string;
  strategicAnalysisId: string;
  auditableEntityTitle: string;
  version: string;
  // auditableEntityName: string;
  surveyEntityRequiredMessage: string;
  invalidMessage: string; // Variable for invalid message
  maxLengthExceedMessage: string; // Variable for maxLengthExceedMessage
  // Creates an instance of documenter
  constructor(
    public stringConstants: StringConstants,
    private route: ActivatedRoute,
    private apiService: StrategicAnalysesService,
    private router: Router,
    private loaderService: LoaderService,
    private sharedService: SharedService,
    private uploadService: UploadService) {
    this.surveyTitle = this.stringConstants.surveyTitle;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.surveyFinance = this.stringConstants.surveyFinance;
    this.engagementPartner = this.stringConstants.engagementPartner;
    this.engagementManager = this.stringConstants.engagementManager;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.emailLabel = this.stringConstants.emailLabel;
    this.managerText = this.stringConstants.managerText;
    this.detailsOportunity = this.stringConstants.detailsOportunity;
    this.estimatedValue = this.stringConstants.estimatedValue;
    this.auditPlaceholder = this.stringConstants.auditPlaceholder;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.namePlaceholder = this.stringConstants.namePlaceholder;
    this.teamLabel = this.stringConstants.teamLabel;
    this.backToolTip = this.stringConstants.backToolTip;
    this.selectSurveyLabel = this.stringConstants.selectSurveyLabel;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.auditableEntityTitle = this.stringConstants.auditableEntityTitle;
    this.surveyEntityRequiredMessage = this.stringConstants.surveyEntityRequiredMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.version = this.stringConstants.versionTitle;
    this.strategicAnalysis = {
      id: '0',
      version: 1,
      isSampling: false,
      questionsCount: 0,
      responseCount: 0,
      isDeleted: false,
      isVersionToBeChanged: false,
      status: StrategicAnalysisStatus.NUMBER_0,
      isResponseDrafted: false,
      userResponseForDetailsOfOppAndEstimatedValue: {
        detailsOfOpportunity: '',
        estimatedValueOfOpportunity: ''
      } as UserResponseForDetailsAndEstimatedValueOfOpportunity
    };
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
  *  Initialization of properties.
  */
  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.passedStrategicAnalysisId = params.passedStrategicAnalysisId;
      this.surveyTitle = params.surveyTitle;
      this.strategicAnalysis.surveyTitle = this.surveyTitle;
      this.auditableEntityId = params.auditableEntityId;
    });
    this.loaderService.open();
    this.getStrategicAnalysisById(this.passedStrategicAnalysisId);
  }

  /***
   * On Back click
   */
  onBackClick() {
    this.router.navigate(['/strategic-analysis-user/list']);
  }

  /**
   * Get strategic analysis by id
   * @param id Strategic analysis id
   */
  getStrategicAnalysisById(id: string) {
    if (id !== '0') {
      this.apiService.strategicAnalysesGetStrategicAnalysisById(id, null, true, this.auditableEntityId).subscribe(result => {
        this.loaderService.close();
        this.strategicAnalysis = result;
        if (this.strategicAnalysis.internalUserList === undefined) {
          this.strategicAnalysis.internalUserList = [] as StrategicAnalysisTeamAC[];
        }
        if (this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue === undefined || this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue === null) {
          this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue = {
            isDetailAndEstimatedValueOfOpportunity: true
          } as UserResponseForDetailsAndEstimatedValueOfOpportunity;
        }
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    } else {
      this.apiService.strategicAnalysesGetAllActiveStrategicAnalysis().subscribe(result => {
        this.loaderService.close();
        this.activeStrategicAnalysisList = result;
      },
        (error) => {
          this.loaderService.close();
          this.sharedService.showError(error.error);
        });
    }
  }

  /** Save strategic analysis details */
  saveStrategicAnalysisDetails() {
    this.loaderService.open();
    this.strategicAnalysis.isUserResponseToBeUpdated = false;
    this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue.isDetailAndEstimatedValueOfOpportunity = true;

    this.uploadService.uploadFileOnUpdate<StrategicAnalysisAC>(this.strategicAnalysis, [], this.stringConstants.strategicAnalysisFiles, this.stringConstants.strategicAnalysisApiPath).subscribe((result) => {
      this.loaderService.close();
      this.strategicAnalysis = result;
      this.router.navigate(['/strategic-analysis-user-survey/response', {
        strategicAnalysisId: this.strategicAnalysis.id,
        auditableEntityId: this.strategicAnalysis.auditableEntityId
      }]);
    }, error => {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.somethingWentWrong);
    });
  }
  /***
   * On change survey
   */

  onChangeSurvey() {
    this.strategicAnalysis = this.activeStrategicAnalysisList.find(a => a.id === this.strategicAnalysisId);
    if (this.passedStrategicAnalysisId === '0') {
      this.strategicAnalysis.auditableEntityId = null;
      this.strategicAnalysis.auditableEntityName = '';
      this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue = undefined;
    }

    if (this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue === undefined || this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue === null) {
      this.strategicAnalysis.userResponseForDetailsOfOppAndEstimatedValue = {
        isDetailAndEstimatedValueOfOpportunity: true
      } as UserResponseForDetailsAndEstimatedValueOfOpportunity;
    }
  }

}
