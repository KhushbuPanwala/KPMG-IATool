import { Component, OnInit, Input, EventEmitter, Output, OnDestroy } from '@angular/core';
import { FormControl, FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
import { Time, formatDate } from '@angular/common';
import { StringConstants } from '../../shared/stringConstants';
import { UserAC, WorkProgramAC, MomAC, SubPointOfDiscussionAC, UserType, SubPointStatus, MainDiscussionPointAC } from '../../swaggerapi/AngularFiles';
import { Router, ActivatedRoute } from '@angular/router';
import { DateTimeError } from '../DateTimeError';
import { MomsService } from '../../swaggerapi/AngularFiles/api/moms.service';
import { MomUserMappingAC } from '../../swaggerapi/AngularFiles/model/momUserMappingAC';
import { SharedService } from '../../core/shared.service';
import { LoaderService } from '../../core/loader.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { Subscription } from 'rxjs';
@Component({
  selector: 'app-addmom',
  templateUrl: './mom-add.component.html',
  styleUrls: ['./mom-add.component.scss'],

})

export class MomAddComponent implements OnInit, OnDestroy {
  momAddPageTitle: string; // Variable for page title
  outlookToolTip: string; // Variable for outlook tooltip
  pdfToolTip: string; // Variable for pdf tooltip
  backToolTip: string; // Variable for back tooltip
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Vairable for add tooltip
  dateLabel: string; // Variable for date label
  timeLabel: string; // Variable for time label
  workProgramTitle: string; // Variable for title of work program section
  pointOfDiscussionTitle: string; // Variable for pod section
  statusTargetDateTitle: string; // Variable for std section
  personResponsibleTitle: string; // Variable for person responsible heading
  textAreaPlaceHolder: string; // Variable for textarea
  srNo: string; // Variable for sr no in table
  agendaTitle: string; // Variable for agenda label
  teamLabel: string; // Variable for team label
  clientParticipantLabel: string; // Variable for clientParticipant label
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable Designation label
  closureMeetingLabel: string; // Vairable for closure meeting label
  bottomPlacement: string; // Variable for bottom tooltip placement
  saveButtonText: string; // Variable for save button text
  toText: string; // Variable for to text
  workProgramListByAuditableEntityId: WorkProgramAC[] = [];
  selectedEntity: string;
  auditableEntityId: string; // i will change after audit plan implementation
  userId: string; // TOdo: Change it after login implementaion
  message = '';
  workProgram: WorkProgramAC;
  momStartTime: Date = new Date(Date.now()); // Assign start time
  momEndTime: Date = new Date(Date.now()); // Assign End time
  minTime: Date = new Date(Date.now());
  maxTime: Date = new Date();
  minEndTime: Date = new Date(Date.now());
  maxEndTime: Date = new Date();
  isValid: boolean;
  mom: MomAC;
  momData: MomAC;

  workProgramRequiredMessage: string;
  agendaRequiredMessage: string;
  teamRequiredMessage: string;
  clientParticipantRequiredMessage: string;
  momStartTimeInvalidMessage: string;
  momEndTimeInvalidMessage: string;
  maxLengthExceedMessage: string;
  mainPointRequiredMessage: string;
  subPointRequiredMessage: string;
  momId: string;
  invalidMessage: string;
  // Team and client participant area
  selectedUser: string;
  designation: string;
  clientParticipantsText: string; // Variable for client participants
  selectedInternalUserDesignation: string;
  selectedInternalUser: string;
  selectedInternalTeamUser: MomUserMappingAC;
  selectedExternalUserDesignation: string;
  selectedExternalUser: string;
  selectedExternalTeamUser: MomUserMappingAC;
  error: DateTimeError = { isError: false, errorMessage: '' };
  selectedDesignation: string;
  selectedInternalUsers: Array<MomUserMappingAC> = [];
  selectedExternalUsers: Array<MomUserMappingAC> = [];
  internalUserList: Array<UserAC> = [];
  externalUserList: Array<UserAC> = [];
  internalUserAddList: Array<UserAC> = [];
  externalUserAddList: Array<UserAC> = [];

  // Main point subPoint area
  mainPointDiscussionList: Array<MainDiscussionPointAC> = [];
  subPointDiscussionList: Array<SubPointOfDiscussionAC> = [];
  selectedMainPointAddList: Array<MainDiscussionPointAC> = [];
  selectedSubPointAddList: Array<SubPointOfDiscussionAC> = [];
  finalSubPointDiscussionList: Array<SubPointOfDiscussionAC> = [];
  selectedMainPoint: string;
  selectedSubPoint: string;
  addedMainPointAC: MainDiscussionPointAC;
  addedSubPointOfDiscussionAC: SubPointOfDiscussionAC;
  subPointStatus = SubPointStatus;
  selectedStatus: string;
  currentCode = 0;
  character: string;
  selectedPageItem: number;
  searchValue: string;
  dropdownDefaultValue: string;
  // Person resposible area
  public userList: Array<UserAC> = [];
  personResposibleAddList: Array<UserAC> = [];
  finalPersonResposibleAddList: Array<UserAC> = [];
  userDetailsId: string;
  selectedPersonResposibleUsers: Array<MomUserMappingAC> = [];
  selectedPersonResposibleUser: MomUserMappingAC;
  public dpConfig: Partial<BsDatepickerConfig> = new BsDatepickerConfig();
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Point of discussion status array
  statusList = [
    { value: SubPointStatus.NUMBER_0, label: 'InProgress' },
    { value: SubPointStatus.NUMBER_1, label: 'Completed' },
    { value: SubPointStatus.NUMBER_2, label: 'Pending' },
  ];


  // date picker configurations
  currentDate = new Date();
  form = new FormGroup({
    dateYMD: new FormControl(new Date()),
    dateFull: new FormControl(new Date()),
    dateMDY: new FormControl(new Date()),
    dateRange: new FormControl([
      new Date(),
      new Date(this.currentDate.setDate(this.currentDate.getDate() + 7))
    ])
  });
  timepickerVisible = false; // Condition that hide timepicker
  isOnInit: boolean;
  isSubOnInit: boolean;
  isOnInitPerson: boolean;

  @Input() addedWorkProgramId: string;
  isWorkProgramDisabled: boolean;
  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants, private momService: MomsService,
              private router: Router, private route: ActivatedRoute,
              private sharedService: SharedService, private loaderService: LoaderService, private staticTabs: TabsetComponent) {
    // String constants for titles and labels
    this.momAddPageTitle = this.stringConstants.momAddPageTitle;
    this.outlookToolTip = this.stringConstants.outlookToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.backToolTip = this.stringConstants.backToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.dateLabel = this.stringConstants.dateLabel;
    this.timeLabel = this.stringConstants.timeLabel;

    this.workProgramTitle = this.stringConstants.workProgramTitle;
    this.pointOfDiscussionTitle = this.stringConstants.pointOfDiscussionTitle;
    this.statusTargetDateTitle = this.stringConstants.statusTargetDateTitle;
    this.personResponsibleTitle = this.stringConstants.personResponsibleTitle;
    this.textAreaPlaceHolder = this.stringConstants.textAreaPlaceHolder;
    this.srNo = this.stringConstants.srNo;
    this.agendaTitle = this.stringConstants.agendaTitle;
    this.teamLabel = this.stringConstants.teamLabel;
    this.clientParticipantLabel = this.stringConstants.clientParticipantsText;
    this.nameLabel = this.stringConstants.nameLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.closureMeetingLabel = this.stringConstants.closureMeetingLabel;
    this.bottomPlacement = this.stringConstants.bottomPlacement;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.toText = this.stringConstants.toText;
    this.workProgramRequiredMessage = this.stringConstants.workProgramRequiredMessage;
    this.agendaRequiredMessage = this.stringConstants.agendaRequiredMessage;
    this.teamRequiredMessage = this.stringConstants.teamRequiredMessage;
    this.clientParticipantRequiredMessage = this.stringConstants.clientParticipantRequiredMessage;
    this.momStartTimeInvalidMessage = this.stringConstants.momStartTimeInvalidMessage;
    this.momEndTimeInvalidMessage = this.stringConstants.momEndTimeInvalidMessage;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.mainPointRequiredMessage = this.stringConstants.mainPointRequiredMessage;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.minTime.setHours(1);
    this.minTime.setMinutes(0);
    this.maxTime.setHours(25);
    this.maxTime.setMinutes(0);
    this.minEndTime.setHours(1);
    this.minEndTime.setMinutes(0);
    this.maxEndTime.setHours(25);
    this.maxEndTime.setMinutes(0);
    this.selectedDesignation = '';
    this.clientParticipantsText = this.stringConstants.clientParticipantsText;
    this.auditableEntityId = this.stringConstants.auditableEntityId;
    this.userId = this.stringConstants.userId;
    this.internalUserList = [];
    this.externalUserList = [];
    this.internalUserAddList = [];
    this.externalUserAddList = [];
    this.selectedInternalUsers = [];
    this.selectedExternalUsers = [];
    this.mainPointDiscussionList = [];
    this.subPointDiscussionList = [];
    this.selectedMainPointAddList = [];
    this.selectedSubPointAddList = [];
    this.userList = [];
    this.personResposibleAddList = [];
    this.selectedMainPoint = '';
    this.selectedSubPoint = '';
    this.mom = {} as MomAC;
    this.selectedStatus = '';
    this.character = '';
    this.addedMainPointAC = {} as MainDiscussionPointAC;
    this.addedSubPointOfDiscussionAC = {} as SubPointOfDiscussionAC;
    this.selectedPersonResposibleUser = {} as MomUserMappingAC;
    this.isValid = true;
    this.dpConfig.containerClass = 'theme-dark-blue';
  }

  /*** Lifecycle hook that is called aft+-er data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    this.addedSubPointOfDiscussionAC = {} as SubPointOfDiscussionAC;
    this.workProgram = {} as WorkProgramAC;
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.auditableEntityId = entityId;
        this.route.params.subscribe(params => {
          if (this.addedWorkProgramId !== undefined) {
            this.mom.workProgramId = this.addedWorkProgramId;
            this.isWorkProgramDisabled = true;
          } else {
            this.momId = params.id;
            this.isWorkProgramDisabled = false;
          }
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        if (this.momId !== '0' && this.momId !== undefined) {
          this.getMomById(this.momId, this.auditableEntityId);

        } else {
          this.mom = {} as MomAC;
          this.getAllPredefinedData();
        }
      }
    });
    // setTimeout(() => {
    //  pop.show();
    // });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /***
   * Method for getting list of users
   */
  getAllPredefinedData() {
    this.momService.momsGetPredefinedDataForMom(this.auditableEntityId).subscribe(data => {
      this.mom = data;
      if (this.addedWorkProgramId !== undefined) {
        this.workProgram.id = this.addedWorkProgramId;
        this.workProgram.name = data.workProgramCollection.filter(x => x.id === this.addedWorkProgramId)[0].name;
      }
      this.workProgramListByAuditableEntityId = this.mom.workProgramCollection;
      this.mom.momStartTime = new Date();
      this.mom.momEndTime = new Date();
      this.mom.momDate = new Date();
      this.mom.closureMeetingDate = new Date();
      this.internalUserList = this.mom.teamCollection;
      this.externalUserList = this.mom.clientParticipantCollection;
      this.internalUserAddList = JSON.parse(JSON.stringify(this.internalUserList));
      this.externalUserAddList = JSON.parse(JSON.stringify(this.externalUserList));

      this.userList = this.mom.allPersonResposibleACDataCollection;
      this.addMainPoints(0, false);

    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Method for getting mom detail By Id
   * @param id : Id of mom
   * @param entityId :Id of selected entityId
   */
  getMomById(id: string, entityId: string) {
    this.momService.momsGetMomDetailById(id, entityId).subscribe(result => {
      this.mom = result;
      this.mom.momDate = this.sharedService.convertLocalDateToUTCDate(result.momDate, false);
      this.mom.closureMeetingDate = this.sharedService.convertLocalDateToUTCDate(result.closureMeetingDate, false);
      this.mom.momStartTime = new Date(result.momStartTime + 'Z');
      this.mom.momEndTime = new Date(result.momEndTime + 'Z');
      this.mom.agenda = result.agenda.toString();
      this.workProgramListByAuditableEntityId = this.mom.workProgramCollection;
      this.selectedInternalUsers = this.mom.internalUserList;
      this.selectedExternalUsers = this.mom.externalUserList;
      this.internalUserList = this.mom.teamCollection;
      this.externalUserList = this.mom.clientParticipantCollection;
      this.internalUserAddList = JSON.parse(JSON.stringify(this.internalUserList));
      this.externalUserAddList = JSON.parse(JSON.stringify(this.externalUserList));
      this.workProgram = result.workProgramAc;
      this.workProgramListByAuditableEntityId = this.mom.workProgramCollection;

      for (const user of this.selectedInternalUsers) {
        this.internalUserAddList.splice(
          this.internalUserAddList.indexOf(this.internalUserAddList.filter(x => x.id === user.userId)[0]), 1);
        this.internalUserAddList = [...this.internalUserAddList];
      }

      // Remove external users from dropdown which are already selected to prevent duplicate data
      for (const user of this.selectedExternalUsers) {
        this.externalUserAddList.splice(
          this.externalUserAddList.indexOf(this.externalUserAddList.filter(x => x.id === user.userId)[0]), 1);
        this.externalUserAddList = [...this.externalUserAddList];
      }
      this.mainPointDiscussionList = result.mainDiscussionPointACCollection;
      this.setStatus();
      this.setTargetDate();
      this.userList = result.allPersonResposibleACDataCollection;
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Method for setting status of subPoint
   */
  setStatus() {
    for (const mainPointData of this.mainPointDiscussionList) {
      for (const subPointData of mainPointData.subPointDiscussionACCollection) {

        if (this.statusList[SubPointStatus.NUMBER_0].label === subPointData.statusString) {
          subPointData.status = SubPointStatus.NUMBER_0;
          subPointData.statusString = this.statusList[SubPointStatus.NUMBER_0].label;
        } else if (this.statusList[SubPointStatus.NUMBER_1].label === subPointData.statusString) {
          subPointData.status = SubPointStatus.NUMBER_1;
          subPointData.statusString = this.statusList[SubPointStatus.NUMBER_1].label;
        } else {
          subPointData.status = SubPointStatus.NUMBER_2;
          subPointData.statusString = this.statusList[SubPointStatus.NUMBER_2].label;
        }

      }

    }
  }

  /**
   * Method for setting utc to local date in edit mode
   */
  setTargetDate() {
    for (const mainPointData of this.mainPointDiscussionList) {
      for (const subPointData of mainPointData.subPointDiscussionACCollection) {
        subPointData.targetDate = this.sharedService.convertLocalDateToUTCDate(subPointData.targetDate, false);
      }
    }
  }


  /**
   * Add and update Mom
   */
  saveMom() {
    this.isValid = this.checkValidation();
    if (this.isValid) {
      this.mom.workProgramId = this.workProgram.id;
      this.mom.internalUserList = this.selectedInternalUsers;
      this.mom.externalUserList = this.selectedExternalUsers;
      this.mom.entityId = this.auditableEntityId;
      this.mom.workProgramCollection = [];
      this.mom.workProgramAc = {} as WorkProgramAC;
      this.mom.personResposibleACCollection = [];
      this.mom.mainDiscussionPointACCollection = this.mainPointDiscussionList;
      this.setStatus();

      if (this.momId !== '0' && this.momId !== undefined) {
        this.mom.id = this.momId;
      }
      if (this.mom !== undefined && (this.mom.id === undefined || this.mom.id === null)) {
        this.momService.momsAddMom(this.mom, this.auditableEntityId).subscribe(result => {
          this.mom = result;
          if (this.addedWorkProgramId === undefined) {
            this.setListPageRoute();
          }
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          if (this.addedWorkProgramId !== undefined) {
            this.mom = {} as MomAC;
            this.staticTabs.tabs[2].active = true;
            this.mainPointDiscussionList = [];
            this.selectedInternalUsers = [];
            this.selectedExternalUsers = [];
            this.getAllPredefinedData();
          }
        }, (error) => {
          this.sharedService.handleError(error);
        });
      } else if (this.mom !== undefined && this.mom.id !== undefined) {
        this.momService.momsUpdateMomDetail(this.mom, this.auditableEntityId).subscribe(result => {
          this.mom = result;
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          if (this.addedWorkProgramId !== undefined) {
            this.staticTabs.tabs[2].active = true;
          }
          if (this.addedWorkProgramId === undefined) {
            this.setListPageRoute();
          }
        }, (error) => {
          this.sharedService.handleError(error);
        });
      }
    }

  }

  /**
   * Method for validating mainpoint ,subpoint and person resposible
   */
  checkValidation() {
    for (const mainPointData of this.mainPointDiscussionList) {
      for (const subPointData of mainPointData.subPointDiscussionACCollection) {
        for (const personData of subPointData.personResponsibleACCollection) {
          this.isValid = false;
          if (mainPointData.mainPoint === '') {
            this.sharedService.showError(this.stringConstants.mainPointRequiredMessage);
          } else if (subPointData.subPoint === '') {
            this.sharedService.showError(this.stringConstants.subPointRequiredMessage);

          } else if (subPointData.statusString === null) {
            this.sharedService.showError(this.stringConstants.statusRequiredMessage);
          } else if (subPointData.targetDate === null) {
            this.sharedService.showError(this.stringConstants.targetDateRequiredMessage);

          } else if (personData.userId === '0' || personData.userId === '') {
            this.sharedService.showError(this.stringConstants.personRequiredMessage);

          } else if (mainPointData.mainPoint !== '' && mainPointData.mainPoint.length > Number(this.stringConstants.maxCharecterAllowed)) {
            this.sharedService.showError(this.stringConstants.maxLengthExceedMessage);

          } else if (subPointData.subPoint !== '' && subPointData.subPoint.length > 256) {
            this.sharedService.showError(this.stringConstants.maxLengthExceedMessage);

          } else {
            this.isValid = true;
          }
        }
      }
    }
    return this.isValid;
  }

  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    this.router.navigate(['mom/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.setListPageRoute();
  }

  /**
   * Method for comparing two times
   */
  compareTwoTimes() {
    if (new Date(this.mom.momEndTime) < new Date(this.mom.momStartTime)) {
      this.error = {
        isError: true, errorMessage: this.stringConstants.momTimeCompareMessage
      };
    } else {
      this.error = { isError: false, errorMessage: '' };
    }
  }


  /**
   * Method for adding team users
   */
  addInternalUser() {
    if ((this.selectedInternalUser === undefined && this.selectedInternalUserDesignation === undefined) ||
      (this.selectedInternalUser === '' && this.selectedInternalUserDesignation === '') ||
      (this.selectedInternalUser === null && this.selectedInternalUserDesignation === null)) {
      this.sharedService.showError(this.stringConstants.blankUserAddedWarningMessage);
      return;
    } else {
      this.selectedInternalTeamUser = {
        userId: this.selectedInternalUser,
        name: this.internalUserAddList.filter(x => x.id === this.selectedInternalUser)[0].name,
        designation: this.selectedInternalUserDesignation,
        isDeleted: false,
        momId: this.mom.id
      };
      this.internalUserAddList.splice(
        this.internalUserAddList.indexOf(this.internalUserAddList.filter(x => x.id === this.selectedInternalUser)[0]), 1);
      // Need to create a new array for ng-select change-detection to work. For ref https://github.com/ng-select/ng-select#change-detection
      this.internalUserAddList = [...this.internalUserAddList];
      this.selectedInternalUsers.push(this.selectedInternalTeamUser);
      this.selectedInternalUser = '';
      this.selectedInternalUserDesignation = '';
    }
  }

  /**
   * Method is called when user changes selection of user in dropdown
   * @param user : object of selected user
   */
  onInternalUserSelection(user: UserAC) {
    if (user === undefined) {
      this.selectedInternalUserDesignation = '';
    } else {
      const selectedUser = this.internalUserAddList.filter(x => x.id === user.id)[0];
      if (selectedUser !== undefined) {
        this.selectedInternalUserDesignation = user.designation;

      }
    }
  }

  /**
   * Method for tracking index of userlist
   * @param index : Index of selected user
   */
  trackByIndex(index: number) {
    return index;
  }

  /**
   * Method for removing selected user
   * @param userId : Id of user to be removed
   */
  removeInternalUser(userId) {
    if (this.selectedInternalUsers.length === 1) {
      this.sharedService.showError(this.stringConstants.deleteRowMessage);
      return false;
    } else {
      this.internalUserAddList = [...this.internalUserAddList, this.internalUserList.filter(x => x.id === userId)[0]];
      this.selectedInternalUsers.splice(
        this.selectedInternalUsers.indexOf(this.selectedInternalUsers.filter(x => x.userId === userId)[0]), 1);

    }
  }

  /**
   * Method for adding client participant data
   */
  addExternalUser() {
    if ((this.selectedExternalUser === undefined && this.selectedExternalUserDesignation === undefined) ||
      (this.selectedExternalUser === '' && this.selectedExternalUserDesignation === '') ||
      (this.selectedExternalUser === null && this.selectedExternalUserDesignation === null)) {
      this.sharedService.showError(this.stringConstants.blankUserAddedWarningMessage);
      return;
    } else {
      this.selectedExternalTeamUser = {
        userId: this.selectedExternalUser,
        name: this.externalUserAddList.filter(x => x.id === this.selectedExternalUser)[0].name,
        designation: this.selectedExternalUserDesignation,
        isDeleted: false,
        momId: this.mom.id
      };
      this.externalUserAddList.splice(
        this.externalUserAddList.indexOf(this.externalUserAddList.filter(x => x.id === this.selectedExternalUser)[0]), 1);
      // Need to create a new array for ng-select change-detection to work. For ref https://github.com/ng-select/ng-select#change-detection
      this.externalUserAddList = [...this.externalUserAddList];
      this.selectedExternalUsers.push(this.selectedExternalTeamUser);
      this.selectedExternalUser = '';
      this.selectedExternalUserDesignation = '';
    }
  }

  /**
   * Method is called when user changes selection of client participant in dropdown
   * @param user : object of selected user
   */
  onExternalUserSelection(user: UserAC) {
    if (user === undefined) {
      this.selectedExternalUserDesignation = '';
    } else {
      const selectedUser = this.externalUserAddList.filter(x => x.id === user.id)[0];
      if (selectedUser !== undefined) {
        this.selectedExternalUserDesignation = user.designation;
      }
    }
  }

  /**
   * Method for tracking index of client participant list
   * @param index : index of  selected client
   */
  trackByExternalIndex(index: number) {
    return index;
  }

  /**
   * Method for removing selected client participant
   * @param userId :id of selected client participant
   */
  removeExternalUser(userId) {
    if (this.selectedExternalUsers.length === 1) {
      this.sharedService.showError(this.stringConstants.deleteRowMessage);
      return false;
    } else {
      this.externalUserAddList = [...this.externalUserAddList, this.externalUserList.filter(x => x.id === userId)[0]];
      this.selectedExternalUsers.splice(
        this.selectedExternalUsers.indexOf(this.selectedExternalUsers.filter(x => x.userId === userId)[0]), 1);
    }
  }

  /**
   * Method for adding main Point
   * @param index :Index of mainPoint
   * @param isOnInit : Returns false if it is called from ngOnInit or not
   */
  addMainPoints(index, isOnInit: boolean) {
    this.addedMainPointAC = {
      momId: this.mom.id,
      mainPoint: '',
      isDeleted: false,
    };
    this.isOnInit = isOnInit;
    if (this.isOnInit) {
      index = index + 1;
    }
    this.mainPointDiscussionList.push(this.addedMainPointAC);
    this.addSubPoints(index, 0, false);
    this.subPointDiscussionList = [];
    this.selectedPersonResposibleUsers = [];
    this.isOnInit = false;
  }



  /**
   * Method for adding subpoints of mainpoint
   * @param index :Index of mainpoint
   * @param subIndex : Index of subPoint
   * @param isSubOnInit :Returns false if it is called from ngOnInit
   */
  addSubPoints(index, subIndex, isSubOnInit) {
    this.isSubOnInit = isSubOnInit;

    if (isSubOnInit) {
      subIndex = subIndex + 1;
    }
    this.addedSubPointOfDiscussionAC = {
      subPoint: '',
      isDeleted: false,
      status: 0,
      statusString: this.statusList[SubPointStatus.NUMBER_0].label,
      targetDate: new Date(),
      mainPointId: this.addedMainPointAC.id,
      personResponsibleACCollection: this.selectedPersonResposibleUsers
    };
    if (this.isOnInit && this.mainPointDiscussionList[index].subPointDiscussionACCollection !== undefined) {
      this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex + 1] = this.addedSubPointOfDiscussionAC;
      this.subPointDiscussionList.push(this.addedSubPointOfDiscussionAC);
    } else if (this.isSubOnInit && this.mainPointDiscussionList[index].subPointDiscussionACCollection !== undefined) {
      this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex] = this.addedSubPointOfDiscussionAC;
    } else {
      this.subPointDiscussionList.push(this.addedSubPointOfDiscussionAC);
      this.mainPointDiscussionList[index].subPointDiscussionACCollection = this.subPointDiscussionList;
    }

    this.addPersonResposible(index, subIndex, 0, '0', false);
    this.isSubOnInit = false;

  }

  /**
   * Method for adding person resposible of subpoints
   * @param index :Index of main point
   * @param subIndex :Index of subpoint
   * @param userIndex :Index of person resposible
   * @param userId :Id of person resposible
   * @param isOnInitPerson : Returns false if it is called from ngOnInit
   */
  addPersonResposible(index, subIndex, userIndex, userId, isOnInitPerson: boolean) {
    if (userId !== '' && userId !== undefined) {
      this.selectedPersonResposibleUser = {
        userId: '',
        name: '',
        designation: '',
        isDeleted: false,
        momId: this.mom.id
      };
      this.isOnInitPerson = isOnInitPerson;

      if (this.isOnInit && this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection !== undefined) {
        this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection[userIndex] = this.selectedPersonResposibleUser;
      } else if (this.isOnInitPerson && this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection !== undefined &&
        this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection.length < 2) {
        this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection[userIndex + 1] = this.selectedPersonResposibleUser;

      } else {
        if (this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection.length === 2) {
          this.sharedService.showError(this.stringConstants.personResposibleWarningMessage);
          return;
        }
        if (this.selectedPersonResposibleUsers.length < 1) {
          this.selectedPersonResposibleUsers.push(this.selectedPersonResposibleUser);
        }

      }
    }
    this.isOnInitPerson = false;
    this.selectedPersonResposibleUsers = [];
  }

  /**
   * Method for changing person resposible in dropdown
   * @param momUserMapping : Object of momUserMapping
   * @param index :Index of main point
   * @param subIndex :Index of subpoint
   * @param userIndex :Index of person resposible
   */
  onPersonSelection(momUserMapping, index, subIndex, userIndex) {
    if (momUserMapping !== null || momUserMapping !== undefined) {
      this.selectedPersonResposibleUser = {
        userId: momUserMapping.id,
        name: this.userList.filter(x => x.id === momUserMapping.id)[0].name,
        designation: this.userList.filter(x => x.id === momUserMapping.id)[0].designation,
        isDeleted: false,
        momId: this.mom.id
      };
      this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection[userIndex] = this.selectedPersonResposibleUser;
      if (this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection[userIndex].userId !== '') {
        this.isValid = true;
      } else {
        this.isValid = false;
      }
    }
  }

  /**
   * Method for removing main Point
   * @param index :Index of mainPoint
   */
  removeMainPoint(index) {
    if (this.mainPointDiscussionList.length === 1) {
      this.sharedService.showError(this.stringConstants.deleteRowMessage);
      return;
    } else {
      const mainPointTobeRemoved = this.mainPointDiscussionList;
      mainPointTobeRemoved.splice(index, 1);
      this.isValid = this.checkValidation();
    }
  }

  /**
   * Method for removing subPoint
   * @param index :Index of mainPoint
   * @param subIndex :Index of subPoint
   */
  removeSubPoint(index, subIndex) {
    if (this.mainPointDiscussionList[index].subPointDiscussionACCollection.length === 1) {
      this.sharedService.showError(this.stringConstants.deleteRowMessage);
      return;
    } else {
      const subPointTobeRemoved = this.mainPointDiscussionList[index].subPointDiscussionACCollection;
      subPointTobeRemoved.splice(subIndex, 1);
    }
  }

  /**
   * Method for removing person resposible
   * @param index : Index of mainPoint
   * @param subIndex : Index of subPoint
   * @param personResposibleId : Id of person to be removed
   */
  removePersonResposible(index, subIndex, personResposibleId) {
    if (this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection.length === 1) {
      this.sharedService.showError(this.stringConstants.deleteRowMessage);
      return;
    } else {
      const personToBeRemoved = this.mainPointDiscussionList[index].subPointDiscussionACCollection[subIndex].personResponsibleACCollection;
      personToBeRemoved.splice(personToBeRemoved.indexOf(personToBeRemoved.filter(y => y.userId === personResposibleId)[0]), 1);
    }
  }



  /**
   * Method for validating invalid date
   * @param event : Event of datepicker
   */
  isInvalidDate(event) {
    if (event === null) {
      this.sharedService.showError(this.stringConstants.targetDateRequiredMessage);
      this.isValid = false;
    } else {
      this.isValid = true;
    }
  }

  /**
   * Method for validating status
   * @param event : Event of status
   */
  isStatusBlank(event) {
    if (event !== undefined) {
      const statusString = event.label;
      if (statusString !== null) {
        this.isValid = true;
      }
    } else {
      this.isValid = false;
    }
  }

  /**
   * Method for validating main point
   * @param event : Event of main point  text change
   */
  onTextChangeMainPoint(event) {
    const mainPoint = event.target.value;
    if (mainPoint !== '' && mainPoint.length > 256) {
      this.isValid = false;
      this.sharedService.showError(this.stringConstants.maxLengthExceedMessage);
    } else if (mainPoint !== '' && mainPoint.length <= 256) {
      this.isValid = true;
    } else {
      this.isValid = false;
      this.sharedService.showError(this.stringConstants.mainPointRequiredMessage);
    }
  }

  /**
   * Method for validating sub point
   * @param event : Event of sub point  text change
   */
  onTextChangeSubPoint(event) {
    const subPoint = event.target.value;
    if (subPoint !== '' && subPoint.length > 256) {
      this.isValid = false;
      this.sharedService.showError(this.stringConstants.maxLengthExceedMessage);
    } else if (subPoint !== '' && subPoint.length <= 256) {
      this.isValid = true;
    } else {
      this.isValid = false;
      this.sharedService.showError(this.stringConstants.subPointRequiredMessage);
    }
  }
}
