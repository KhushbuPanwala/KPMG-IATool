import { Component, OnInit, Output, EventEmitter, OnDestroy } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { WorkProgramAC } from '../../../swaggerapi/AngularFiles/model/workProgramAC';
import { WorkProgramsService } from '../../../swaggerapi/AngularFiles/api/workPrograms.service';
import { ActivatedRoute, Router } from '@angular/router';
import { WorkProgramStatus } from '../../../swaggerapi/AngularFiles/model/workProgramStatus';
import { WorkProgramTeamAC } from '../../../swaggerapi/AngularFiles/model/workProgramTeamAC';
import { UserAC } from '../../../swaggerapi/AngularFiles/model/userAC';
import { ProcessAC } from '../../../swaggerapi/AngularFiles/model/processAC';
import { AuditPlanAC } from '../../../swaggerapi/AngularFiles/model/auditPlanAC';
import { SharedService } from '../../../core/shared.service';
import { LoaderService } from '../../../core/loader.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { AuditProcessesService, PlanProcessMappingAC, UserType } from '../../../swaggerapi/AngularFiles';
import { WorkPaperAC } from '../../../swaggerapi/AngularFiles';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { UploadService } from '../../../core/upload.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-work-program-audit',
  templateUrl: './work-program-audit.component.html'
})
export class WorkProgramAuditComponent implements OnInit, OnDestroy {
  auditTitle: string; // Variable for audit title
  backToolTip: string; // Variable for back tooltip
  workProgramName: string; // Variable for work program name
  auditPlanBreadCrumbTitle: string; // Variable for audit plan title
  processLabel: string; // Variable for process label
  auditTitleText: string; // Variable for audit title text
  statusTitle: string; // Variable for status title
  toText: string; // Variable for to text
  dateLabel: string; // Variable for date label
  planScopeTitle: string; // Variable plan scrope
  teamLabel: string; // Variable for team label
  nameLabel: string; // Variable for name label
  designationLabel: string; // Variable for designation label
  removeToolTip: string; // Variable for remove tooltip
  addToolTip: string; // Variable for add tooltip
  clientParticipantsText: string; // Variable for client participants
  workPapers: string; // Variable for work papers
  noFileChosen: string; // Variable for no file chosen
  chooseFileText: string; // Variable for choose file text
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for power point
  pdfToolTip: string; // Variable for pdf tooltip
  fileNameText: string; // Variable for fileNameText
  saveButtonText: string; // Variable for saveButtonText
  deleteTitle: string; // Variable for delete title
  statusButtonText: string;
  startDate: string;
  endDate: string;
  bsModalRef: BsModalRef; // Modal ref variable

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

  workProgramId: string;
  selectedInternalUserDesignation: string;
  selectedExternalUserDesignation: string;
  selectedInternalUser: string;
  selectedExternalUser: string;
  invalidMessage: string;
  requiredMessage: string;
  dropdownDefaultValue: string;
  selectedEntityId: string;
  isStatusChanged: boolean;
  selectedPageItem: number;
  searchValue: string;
  isAddPage: boolean;
  maxLengthExceedMessage: string;
  isAllInternalUsersDeleted: boolean;
  isAuditplanChanged: boolean;
  workPaperFilesList: File[] = [];
  auditPlanVersion: string;
  subProcessLabel: string;
  isParentProcessChanged: boolean;

  @Output() addedWorkProgramId = new EventEmitter<string>();
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  selectedProcess: ProcessAC;
  selectedInternalTeamUser: WorkProgramTeamAC;
  selectedExternalTeamUser: WorkProgramTeamAC;
  workProgramDetails: WorkProgramAC;
  workPaper: WorkPaperAC;

  auditPlanList: AuditPlanAC[] = [];
  processList: ProcessAC[] = [];
  subProcessList: PlanProcessMappingAC[] = [];
  internalUserList: UserAC[] = [];
  externalUserList: UserAC[] = [];
  internalUserAddList: UserAC[] = [];
  externalUserAddList: UserAC[] = [];
  selectedInternalUsers: WorkProgramTeamAC[] = [];
  selectedExternalUsers: WorkProgramTeamAC[] = [];

  // Point of discussion status array as swagger dont create enum properly
  statusList = [
    { value: WorkProgramStatus.NUMBER_0, label: 'Active' },
    { value: WorkProgramStatus.NUMBER_1, label: 'Update' },
    { value: WorkProgramStatus.NUMBER_2, label: 'Closed' }
  ];

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private workProgramService: WorkProgramsService,
    private auditProcessService: AuditProcessesService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private loaderService: LoaderService,
    private staticTabs: TabsetComponent,
    private uploadService: UploadService,
    private modalService: BsModalService,
  ) {
    this.auditTitle = this.stringConstants.auditTitle;
    this.backToolTip = this.stringConstants.backToolTip;
    this.workProgramName = this.stringConstants.workProgramName;
    this.auditPlanBreadCrumbTitle = this.stringConstants.auditPlanBreadCrumbTitle;
    this.processLabel = this.stringConstants.processLabel;
    this.auditTitleText = this.stringConstants.auditTitleText;
    this.statusTitle = this.stringConstants.statusTitle;
    this.toText = this.stringConstants.toText;
    this.dateLabel = this.stringConstants.dateLabel;
    this.planScopeTitle = this.stringConstants.planScopeTitle;
    this.teamLabel = this.stringConstants.teamLabel;
    this.designationLabel = this.stringConstants.designationLabel;
    this.nameLabel = this.stringConstants.nameLabel;
    this.removeToolTip = this.stringConstants.removeToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.clientParticipantsText = this.stringConstants.clientParticipantsText;
    this.workPapers = this.stringConstants.workPapers;
    this.noFileChosen = this.stringConstants.noFileChosen;
    this.chooseFileText = this.stringConstants.chooseFileText;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.fileNameText = this.stringConstants.fileNameText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.auditPlanVersion = this.stringConstants.auditPlanVersion;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.startDate = this.stringConstants.startDate;
    this.endDate = this.stringConstants.endDate;
    this.subProcessLabel = this.stringConstants.subProcessLabel;

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

    this.isStatusChanged = false;
    this.isAddPage = true;
    this.isAuditplanChanged = false;
    this.isAllInternalUsersDeleted = false;

    this.workProgramDetails = {} as WorkProgramAC;

    this.auditPlanList = [];
    this.processList = [];
    this.subProcessList = [];
    this.internalUserList = [];
    this.externalUserList = [];
    this.internalUserAddList = [];
    this.externalUserAddList = [];
    this.selectedInternalUsers = [];
    this.selectedExternalUsers = [];
    this.workPaperFilesList = [];
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    File.name.split('.').slice(0, -1).join('.');
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(async (entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;

        this.route.params.subscribe(params => {
          this.workProgramId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });

        if (this.workProgramId !== undefined && this.workProgramId !== '0') {
          await this.getWorkProgramDetailById(this.selectedEntityId, this.workProgramId);
          this.isAddPage = false;
        } else {
          this.workProgramDetails = {} as WorkProgramAC;
          await this.getWorkProgramDetailById(this.selectedEntityId, '');
        }
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
   * Get workprogram details for edit page
   * @param id : workprogram id will come from query string
   */
  getWorkProgramDetailById(entityId: string, id: string) {
    this.loaderService.open();
    this.workProgramService.workProgramsGetWorkProgramDetailsById(entityId, id).subscribe(result => {

      this.auditPlanList = JSON.parse(JSON.stringify(result.auditPlanACList));

      this.auditPlanList = result.auditPlanACList;
      this.externalUserList = result.externalUserAC;
      this.internalUserList = result.internalUserAC;

      // Json parse and stringify is done to prevent duplicate values in similar variables
      this.internalUserAddList = JSON.parse(JSON.stringify(this.internalUserList));
      this.externalUserAddList = JSON.parse(JSON.stringify(result.externalUserAC));

      this.workProgramDetails.statusString = this.statusList[WorkProgramStatus.NUMBER_0].label;

      // To fill details on edit page
      if (id !== '') {
        // Populate process list as per selected audit plan id
        this.onAuditPlanChange(result.auditPlanId);
        this.onProcessChange(result.parentProcessId);
        this.workProgramDetails = result;
        if (result.workPaperACList === undefined || result.workPaperACList === null) {
          this.workProgramDetails.workPaperACList = [];
        }
        this.setStatus(result.status);

        if (result.auditPeriodStartDate != null) {
          this.workProgramDetails.auditPeriodStartDate = new Date(result.auditPeriodStartDate);
        }
        if (result.auditPeriodEndDate != null) {
          this.workProgramDetails.auditPeriodEndDate = new Date(result.auditPeriodEndDate);
        }
        this.selectedInternalUsers = result.workProgramTeamACList;
        this.selectedExternalUsers = result.workProgramClientParticipantsACList;

        // Remove internal users from dropdown which are already selected to prevent duplicate data
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
      }
      this.loaderService.close();
    },
      (error) => {
        this.router.navigate(['work-program/list']);
        this.sharedService.handleError(error);
      });
  }

  /**
   * Add and update WorkProgram
   */
  saveWorkProgram() {
    // This array is only used for get data so it is set null
    this.workProgramDetails.auditPlanACList = [];
    this.workProgramDetails.processACList = [];
    this.workProgramDetails.subProcessACList = [];

    this.workProgramDetails.processName = this.subProcessList.filter(x => x.processId === this.workProgramDetails.processId)[0].processName;
    this.workProgramDetails.workProgramTeamACList = this.selectedInternalUsers;
    this.workProgramDetails.workProgramClientParticipantsACList = this.selectedExternalUsers;
    this.workProgramDetails.selectedEntityId = this.selectedEntityId;
    if (this.workProgramDetails.id === undefined) {
      this.loaderService.open();
      this.uploadService.uploadFileOnAdd<WorkProgramAC>(this.workProgramDetails, this.workPaperFilesList,
        this.stringConstants.workPaperFiles, this.stringConstants.workProgramApiPath).subscribe((result: WorkProgramAC) => {
          this.workProgramDetails = result;


          this.workPaperFilesList = [];
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          this.loaderService.close();
          this.addedWorkProgramId.emit(result.id);
          if (this.staticTabs.tabs[1] !== undefined) {
            this.staticTabs.tabs[1].active = true;
          }
          this.workProgramDetails.auditPlanId = result.auditPlanId;
          this.workProgramDetails.parentProcessId = result.parentProcessId;
          this.workProgramDetails.processId = result.processId;
        },
          (error) => {
            this.loaderService.close();
            this.sharedService.handleError(error);
          });
    } else {
      this.loaderService.open();
      if (this.workProgramDetails.status === WorkProgramStatus.NUMBER_1) {
        this.workProgramDetails.status = WorkProgramStatus.NUMBER_0;
      }
      this.uploadService.uploadFileOnUpdate<WorkProgramAC>(this.workProgramDetails, this.workPaperFilesList,
        this.stringConstants.workPaperFiles, this.stringConstants.workProgramApiPath).subscribe((result: WorkProgramAC) => {
          this.workPaperFilesList = [];
          for (const workPaperAC of result.workPaperACList) {
            this.workProgramDetails.workPaperACList.push(workPaperAC);
          }
          this.workProgramDetails.workPaperACList = [...this.workProgramDetails.workPaperACList];
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          this.loaderService.close();
          this.staticTabs.tabs[1].active = true;
        }, (error) => {
          this.loaderService.close();
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * File input change event
   * @param event: Onchange event
   */
  fileChange(event) {
    const fileCount = event.target.files.length + this.workPaperFilesList.length + this.workProgramDetails.workPaperACList.length;
    if (fileCount <= Number(this.stringConstants.fileLimit)) {
      for (const file of event.target.files) {
        const selectedFile = file;
        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {
          this.workPaperFilesList.push(selectedFile);
        }
      }
    } else {
      this.sharedService.showError(this.stringConstants.fileLimitExceed);
    }

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
  openDeleteModal(index: number, workPaperId: string) {

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
                this.workProgramDetails.workPaperACList.splice(
                  this.workProgramDetails.workPaperACList.indexOf(this.workProgramDetails.workPaperACList.filter(x => x.id === workPaperId)[0]), 1);
                this.workProgramDetails.workPaperACList = [...this.workProgramDetails.workPaperACList];
                this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);

                this.loaderService.close();
              }, (error) => {
                this.sharedService.handleError(error);
              });
            } else {
              this.workPaperFilesList.splice(index, 1);
              this.workPaperFilesList = [...this.workPaperFilesList];
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
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
  openDocument(documnetPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documnetPath) }])
    );
    window.open(url, '_blank');
  }

  /**
   * Method to download work paper
   * @param workPaperId: work paper Id
   */
  downloadWorkPaper(workPaperId: string) {
    this.workProgramService.workProgramsGetWorkPaperDownloadUrl(workPaperId, this.selectedEntityId).subscribe((result) => {
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
   * Method to delete file recently added but not uploaded
   * @param index: index
   */
  onAddedFileDelete(index: number) {
    this.workPaperFilesList.splice(index, 1);
    this.workPaperFilesList = [...this.workPaperFilesList];
  }

  /**
   * Change event for audit plan dropdown
   * @param auditPlanId : selected audit plan id
   */
  onAuditPlanChange(auditPlanId: string) {
    this.isAuditplanChanged = true;
    this.processList = [];
    const planDetail = this.auditPlanList.find(a => a.id === auditPlanId);
    this.workProgramDetails.auditPlanId = auditPlanId;
    if (this.isAuditplanChanged) {
      this.workProgramDetails.processId = '';
      this.workProgramDetails.parentProcessId = '';
    }
    if (planDetail !== undefined) {
      this.processList = JSON.parse(JSON.stringify(planDetail.parentProcessList));
    }
    this.subProcessList = [] as Array<PlanProcessMappingAC>;
  }

  /**
   * Get process list for dropdown by audit plan id
   * @param auditPlanId : selected audit plan id
   */
  async getProcessList(auditPlanId: string) {
    this.loaderService.open();
    this.auditProcessService.auditProcessesGetPlanWiseAllProcessesByPlanId(auditPlanId).subscribe(result => {
      this.processList = result;
      if (this.isAuditplanChanged) {
        this.workProgramDetails.processId = '';
      }
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Method for making subprocess field blank on selection of process
   * @param processId: process id
   */
  onProcessChange(processId: string) {
    // To clear subprocess when process is changed so no other subprocess will be selected that is not child of process
    this.isParentProcessChanged = true;
    const planIndex = this.auditPlanList.findIndex(a => a.id === this.workProgramDetails.auditPlanId);
    if (planIndex !== -1) {
      this.subProcessList = this.auditPlanList[planIndex].planProcessList.filter(a => a.parentProcessId === processId);
    }
    if (this.isParentProcessChanged) {
      this.workProgramDetails.processId = '';
    }
  }

  /**
   * User selection method to set designation for selected internal team user
   * @param user : user object
   */
  onInternalUserSelection(user: UserAC) {
    if (user === undefined) {
      this.selectedInternalUserDesignation = '';
    } else {
      const selectedUser = this.selectedInternalUsers.filter(x => x.userId === user.id)[0];
      if (selectedUser === undefined) {
        this.selectedInternalUserDesignation = user.designation;
      }
    }
  }

  /**
   * User selection method to set designation for selected client participants user
   * @param user : user object
   */
  onExternalUserSelection(user: UserAC) {
    if (user === undefined) {
      this.selectedExternalUserDesignation = '';
    } else {
      const selectedUser = this.selectedExternalUsers.filter(x => x.userId === user.id)[0];
      if (selectedUser === undefined) {
        this.selectedExternalUserDesignation = user.designation;
      }
    }
  }

  /**
   * Add internal user to team
   */
  addInternalUser() {
    if ((this.selectedInternalUser === undefined && this.selectedInternalUserDesignation === undefined) ||
      (this.selectedInternalUser === '' && this.selectedInternalUserDesignation === '')) {

    } else {
      this.selectedInternalTeamUser = {
        userId: this.selectedInternalUser,
        name: this.internalUserAddList.filter(x => x.id === this.selectedInternalUser)[0].name,
        designation: this.selectedInternalUserDesignation,
        isDeleted: false,
        workProgramId: this.workProgramId,
        userType: UserType.NUMBER_0
      };

      this.internalUserAddList.splice(
        this.internalUserAddList.indexOf(this.internalUserAddList.filter(x => x.id === this.selectedInternalUser)[0]), 1);
      // Need to create a new array for ng-select change-detection to work. For ref https://github.com/ng-select/ng-select#change-detection
      this.internalUserAddList = [...this.internalUserAddList];

      this.selectedInternalUsers.push(this.selectedInternalTeamUser);
      this.selectedInternalUser = '';
      this.selectedInternalUserDesignation = '';

      this.isAllInternalUsersDeleted = false;
    }
  }

  /**
   * Remove user from team
   * @param userId : user id to be removed
   */
  removeInternalUser(userId: string) {
    this.internalUserAddList = [...this.internalUserAddList, this.internalUserList.filter(x => x.id === userId)[0]];
    this.selectedInternalUsers.splice(
      this.selectedInternalUsers.indexOf(this.selectedInternalUsers.filter(x => x.userId === userId)[0]), 1);
    if (this.selectedInternalUsers.length === 0) {
      this.isAllInternalUsersDeleted = true;
    }
  }

  /**
   * Add user to client participants
   */
  addExternalUser() {
    if ((this.selectedExternalUser === undefined && this.selectedExternalUserDesignation === undefined) ||
      (this.selectedExternalUser === '' && this.selectedExternalUserDesignation === '')) {

    } else {
      this.selectedExternalTeamUser = {
        userId: this.selectedExternalUser,
        name: this.externalUserList.filter(x => x.id === this.selectedExternalUser)[0].name,
        designation: this.selectedExternalUserDesignation,
        isDeleted: false,
        workProgramId: this.workProgramId,
        userType: UserType.NUMBER_1
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
   * Remove user from client participants
   * @param userId : user id to be removed
   */
  removeExternalUser(userId: string) {
    this.externalUserAddList = [...this.externalUserAddList, this.externalUserList.filter(x => x.id === userId)[0]];

    this.selectedExternalUsers.splice(
      this.selectedExternalUsers.indexOf(this.selectedExternalUsers.filter(x => x.userId === userId)[0]), 1);
  }
  /**
   * Change WorkProgram Status
   */
  changeWorkProgramStatus() {
    this.isStatusChanged = true;
    if (this.workProgramDetails.status === WorkProgramStatus.NUMBER_1) {
      this.workProgramDetails.status = WorkProgramStatus.NUMBER_2;
      this.workProgramDetails.statusString = this.statusList[WorkProgramStatus.NUMBER_2].label;
    } else {
      this.workProgramDetails.status = WorkProgramStatus.NUMBER_1;
      this.workProgramDetails.statusString = this.statusList[WorkProgramStatus.NUMBER_1].label;
    }
  }

  /**
   * Set Status
   * @param status: WorkProgramStatus
   */
  setStatus(status: WorkProgramStatus) {
    if (WorkProgramStatus.NUMBER_0 === status || WorkProgramStatus.NUMBER_1 === status) {
      this.workProgramDetails.status = WorkProgramStatus.NUMBER_1;
      this.workProgramDetails.statusString = this.statusList[WorkProgramStatus.NUMBER_1].label;

      this.statusButtonText = this.stringConstants.statusButtonCloseText;
    } else {
      this.statusButtonText = this.stringConstants.statusButtonReopenText;
    }
    this.isStatusChanged = false;
  }

  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    this.router.navigate(['work-program/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }
}
