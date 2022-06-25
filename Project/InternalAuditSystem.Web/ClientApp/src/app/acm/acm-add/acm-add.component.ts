import { Component, OnInit, OnDestroy } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { StringConstants } from '../../shared/stringConstants';
import { AcmAddTabs } from './acm-add.model';
import { AcmService, ACMStatus, ACMPresentationAC, RatingAC, ACMDocumentAC, ACMTableAC, RatingsService } from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SharedService } from '../../core/shared.service';
import { AngularEditorConfig } from '@kolkov/angular-editor';
import { EditorDialogComponent } from '../../shared/editor-dialog/editor-dialog.component';
import { Subscription } from 'rxjs';
import { ACMUploadFilesComponent } from '../acm-upload-files/acm-upload-files.component';
import { ObservationUploadFilesComponent } from '../../observation/observation-upload-files/observation-upload-files.component';
import { UploadService } from '../../core/upload.service';
import { ACMSharedService } from '../acm-shared.service';
import { TabDirective } from 'ngx-bootstrap/tabs';

@Component({
  selector: 'app-acm-add',
  templateUrl: './acm-add.component.html',
})
export class AcmAddComponent implements OnInit, OnDestroy {
  addToolTip: string; // Variable for add tooltip
  addedLabel: string; // Variable for added tooltip
  backToolTip: string; // Variable for back tooltip
  downloadToolTip: string; // Variable for download
  pdfToolTip: string; // Variable for pdf tooltip
  wordToolTip: string; // Variable for word tooltip
  powerPointToolTip: string; // Variable for powerpoint tooltip

  acmId: string; // Variable for acm id
  auditCommitteeTitle: string; // Variable for audit committee title
  headingLabel: string; // Variable for heading
  observationTabTitle: string; // Variable for observation
  recommendationTitle: string; // Variable for recommendation
  managementResponseTitle: string; // Variable for management responsible
  statusTitle: string; // Variable for status title
  implicationTitle: string; // Variable for implication
  ratingLabel: string; // Variable for rating
  saveNextButtonText: string; // Variable for save next

  bsModalRef: BsModalRef; // Modal ref variable
  backgroundLabel: string; // Variable for background Label
  saveButtonText: string;
  requiredMessage: string;
  dropdownDefaultValue: string;
  invalidMessage: string;
  maxLengthExceedMessage: string;

  redColor: string; // Variable for red color
  yellowColor: string; // Variable for yellow color
  greenColor: string; // Varibale for greenColor

  // only to subscripe for the current component
  entitySubscribe: Subscription;
  tabSubscribe: Subscription;
  newFileSubscribe: Subscription;
  saveTabSubscription: Subscription;

  isOnPageLoad: boolean;
  isAddtabActive: boolean;
  selectedPageItem: number;
  searchValue: string;
  selectedEntityId;

  recommendationRequiredMessage: string;
  implicationRequiredMessage: string;
  observationRequiredMessage: string;
  headingRequiredMessage: string;
  managementResponseRequiredMessage: string;
  filesAdded: string;
  fileCount: number;

  acmDetailList: ACMPresentationAC[] = [];
  acmDetails = {} as ACMPresentationAC;
  ratingList = [] as Array<RatingAC>;
  acmFiles: File[] = [];
  currentActiveTab: AcmAddTabs;
  savedAcmDocuments = [] as Array<ACMDocumentAC>;

  // HTML Editor
  config: AngularEditorConfig = {
    editable: true,
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

  tabs: AcmAddTabs[] = [];

  // Status items for acm report
  statusItems = [
    {
      value: ACMStatus.NUMBER_0,
      label: 'Open'
    },
    {
      value: ACMStatus.NUMBER_1,
      label: 'Close'
    }
  ];

  // Creates an instance of documenter.
  constructor(
    private stringConstants: StringConstants,
    private loaderService: LoaderService,
    private acmService: AcmService,
    private acmSharedService: ACMSharedService,
    private route: ActivatedRoute,
    public router: Router,
    private sharedService: SharedService,
    private modalService: BsModalService,
    private ratingService: RatingsService,

    private uploadService: UploadService) {
    this.addToolTip = this.stringConstants.addToolTip;
    this.backToolTip = this.stringConstants.backToolTip;
    this.addedLabel = this.stringConstants.addedLabel;
    this.downloadToolTip = this.stringConstants.downloadToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.auditCommitteeTitle = this.stringConstants.auditCommitteeTitle;
    this.headingLabel = this.stringConstants.headingLabel;
    this.observationTabTitle = this.stringConstants.observationTabTitle;
    this.recommendationTitle = this.stringConstants.recommendationTitle;
    this.managementResponseTitle = this.stringConstants.managementResponseTitle;
    this.implicationTitle = this.stringConstants.implicationTitle;
    this.ratingLabel = this.stringConstants.ratingLabel;
    this.statusTitle = this.stringConstants.statusTitle;
    this.saveNextButtonText = this.stringConstants.saveNextButtonText;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.dropdownDefaultValue = this.stringConstants.dropdownDefaultValue;
    this.invalidMessage = this.stringConstants.invalidMessage;
    this.redColor = this.stringConstants.redColor; // Variable for red color
    this.yellowColor = this.stringConstants.yellowColor; // Variable for yellow color
    this.greenColor = this.stringConstants.greenColor; // Varibale for greenColor
    this.maxLengthExceedMessage = this.stringConstants.maxLengthExceedMessage;

    this.recommendationRequiredMessage = this.stringConstants.recommendationRequiredMessage;
    this.implicationRequiredMessage = this.stringConstants.implicationRequiredMessage;
    this.observationRequiredMessage = this.stringConstants.observationRequiredMessage;
    this.headingRequiredMessage = this.stringConstants.headingRequiredMessage;
    this.managementResponseRequiredMessage = this.stringConstants.managementResponseRequiredMessage;
    this.filesAdded = this.stringConstants.filesAdded;
    this.fileCount = 0;
    this.savedAcmDocuments = [];
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit(): void {
    this.acmSharedService.setFilesCount(0);
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          this.acmId = params.id;
          this.selectedPageItem = params.pageItems;
          this.searchValue = params.searchValue;
        });
        if (this.acmId !== '0') {
          this.getAcmDetailsById();
        } else {
          this.acmDetails = {} as ACMPresentationAC;
          this.getDefaultDetailsOfAcmAddPage();
        }
      }
    });

    if (this.acmSharedService.acmSubject !== undefined) {
      this.acmSharedService.acmSubject.subscribe((acmResultAC) => { this.acmDetails = this.acmSharedService.acmAC; });
    }
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    if (this.entitySubscribe !== undefined) {
      this.entitySubscribe.unsubscribe();
    }
  }

  /**
   * Get default data of acm add page
   */
  getDefaultDetailsOfAcmAddPage() {
    this.ratingService.ratingsGetRatingByEntityId(this.selectedEntityId).subscribe((result: Array<RatingAC>) => {
      // set all default rating list
      this.ratingList = JSON.parse(JSON.stringify(result));
      this.acmDetails = {} as ACMPresentationAC;
      this.acmDetails.status = ACMStatus.NUMBER_0;

      // set primary tab details
      this.setSavedTabDetails();

      // get new added files
      if (this.acmSharedService.filesCountSubject !== undefined) {
        this.acmSharedService.filesCountSubject.subscribe((count) => {
          if (count !== undefined) {
            this.fileCount = count;
          }
        });
      }
    });
  }

  /**
   * Get ACM Process detail by id for edit
   */
  getAcmDetailsById() {
    this.loaderService.open();
    this.acmService.acmGetACMDetailsById(this.acmId, this.selectedEntityId).subscribe((result: ACMPresentationAC) => {
      this.acmDetails = JSON.parse(JSON.stringify(result));
      this.ratingList = JSON.parse(JSON.stringify(result.ratingsList));
      this.savedAcmDocuments = JSON.parse(JSON.stringify(this.acmDetails.acmDocuments));

      this.loaderService.close();

      // set primary tab details
      this.setSavedTabDetails();

      // set files of acm
      this.updateTotalFileCount(this.acmDetails.acmDocuments, 0);

      // get new added files
      if (this.acmSharedService.filesCountSubject !== undefined) {
        this.acmSharedService.filesCountSubject.subscribe((count) => {
          if (count !== undefined) {
            this.fileCount = count;
          }
        });
      }
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }


  /**
   * Change tab and populate next tabs data
   * @param event : change tab event
   * @param changedTab : new tab data
   */
  changeTab(event: TabDirective, changedTab: AcmAddTabs) {

    this.acmSharedService.tabListSubject.subscribe((updatedTabList) => {

      if (updatedTabList.length > 0) {

        // update currentActive tab
        this.currentActiveTab = updatedTabList.find(x => x.title === changedTab.title);

        // set new uploaded files of current tab
        let tempFileCount = 0;
        if (this.currentActiveTab !== undefined && this.currentActiveTab.temporaryFiles !== undefined) {
          tempFileCount = this.currentActiveTab.temporaryFiles.length;
        }

        // calculate count if acm is saved
        if (changedTab.acmDetails.id !== undefined && changedTab.acmDetails.id !== null) {
          this.updateTotalFileCount(this.currentActiveTab.acmDetails.acmDocuments, tempFileCount);
        } else {
          this.updateTotalFileCount([], tempFileCount);
        }

      } else {
        // when tab is changed for the first time
        this.currentActiveTab = JSON.parse(JSON.stringify(changedTab));
      }

    });
  }

  /**
   * Set saved tab data
   */
  setSavedTabDetails() {
    const primaryTab = {} as AcmAddTabs;
    const newTabIndex = this.tabs.length + 1;

    primaryTab.id = this.acmId;
    primaryTab.title = `Theme ${newTabIndex}`;
    primaryTab.content = ``;
    primaryTab.disabled = false;
    primaryTab.removable = true;
    primaryTab.active = true;
    primaryTab.acmDetails = this.acmDetails;

    this.tabs.push(primaryTab);
    this.currentActiveTab = primaryTab;
    this.acmSharedService.setTabList(this.tabs);
    let totalFileCount = 0;
    if (this.currentActiveTab.acmDetails.acmDocuments !== null && this.currentActiveTab.acmDetails.acmDocuments !== undefined) {
      totalFileCount = this.currentActiveTab.acmDetails.acmDocuments.length;
    }
    this.acmSharedService.setFilesCount(totalFileCount);
  }

  /**
   * Add new Tab with its default data
   */
  addNewTab() {
    const newTabIndex = this.tabs.length + 1;
    const newACMDetails = {} as ACMPresentationAC;
    // set default status
    newACMDetails.status = this.statusItems[0].value;
    this.tabs.push({
      id: '',
      title: `Theme ${newTabIndex}`,
      content: ``,
      disabled: false,
      removable: true,
      active: false,
      acmDetails: newACMDetails,
    });
    this.acmId = null;
    this.acmSharedService.setFilesCount(0);
    setTimeout(() => {
      this.tabs[this.tabs.length - 1].active = true;
    }, 10);
  }

  /**
   * Update total files count
   * @param acmDocuments : list of acm documents
   * @param tempFilesCount : COunt of temporary files added recently
   */
  updateTotalFileCount(acmDocuments: Array<ACMDocumentAC>, tempFilesCount: number) {
    this.fileCount = (acmDocuments !== null && acmDocuments !== undefined) ? acmDocuments.length : 0;

    this.fileCount += tempFilesCount;
    // set files count
    this.acmSharedService.setFilesCount(this.fileCount);
  }

  /**
   * set route for list page redirection
   * @param acmId: id to edit ACM
   */
  setListPageRoute(acmId: string) {
    this.router.navigate(['/acm/generate-acm-report', { id: acmId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method on save click
   * @param acmDetails: acm details
   */
  onSaveClick(acmDetails: ACMPresentationAC) {

    // get new added files
    this.saveTabSubscription = this.acmSharedService.tabListSubject.subscribe((tabList) => {
      this.acmFiles = tabList.find(x => x.title === this.currentActiveTab.title).temporaryFiles;
      if (this.acmFiles === undefined) {
        this.acmFiles = [];
      }
      acmDetails.entityId = this.selectedEntityId;
      this.loaderService.open();
      acmDetails.ratingsList = [];
      acmDetails.ratings = this.ratingList.find(k => k.id = acmDetails.ratingId).ratings;
      if (acmDetails.id !== undefined && acmDetails.id !== null) {
        this.acmDetailList = [];
        this.acmDetailList.push(acmDetails);
        this.uploadService.uploadFileOnUpdate<ACMPresentationAC>(acmDetails, this.acmFiles, this.stringConstants.aCMFiles, this.stringConstants.acmApiUrl).subscribe((result) => {
          this.acmDetails = result;
          this.acmFiles = [];
          this.loaderService.close();
          this.sharedService.showSuccess(this.stringConstants.recordUpdatedMsg);
          this.setListPageRoute(this.acmDetails.id);
          this.unsubScribeSetSubjects();

        }, (error) => {
          this.sharedService.handleError(error);
        });

      } else {
        this.uploadService.uploadFileOnAdd<ACMPresentationAC>(acmDetails, this.acmFiles, this.stringConstants.aCMFiles, this.stringConstants.acmApiUrl).subscribe((result) => {
          this.loaderService.close();
          this.acmFiles = [];
          this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
          this.setListPageRoute(result.id);
          this.unsubScribeSetSubjects();

        }, (error) => {
          this.sharedService.handleError(error);
        });
      }

    });
  }

  /**
   * UnSubscibe on complete
   */
  unsubScribeSetSubjects() {
    this.saveTabSubscription.unsubscribe();
  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.router.navigate(['acm/list', { pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Method to comment editor modal
   * @param text: Text of editor
   * @param label: Label of text editor
   */
  openEditorModal(text: string, label: string, tabIndex: number) {
    this.modalService.config.class = 'page-modal audit-team-add upload-file-dialog modal-dialog-centered';
    this.bsModalRef = this.modalService.show(EditorDialogComponent, {
      initialState: {
        title: this.backgroundLabel,
        keyboard: true,
        data: text,
        callback: (result) => {
          if (label === this.observationTabTitle) {
            this.tabs[tabIndex].acmDetails.observation = result;
          } else if (label === this.managementResponseTitle) {
            this.tabs[tabIndex].acmDetails.managementResponse = result;
          } else if (label === this.recommendationTitle) {
            this.tabs[tabIndex].acmDetails.recommendation = result;
          }
        }
      }
    });
  }

  /**
   * Method to open upload modal
   * @param recentTab : Tab from where upload modal is opened
   */
  openUploadModal(recentTab: AcmAddTabs) {
    this.acmFiles = [];
    const currentAcmId = recentTab.acmDetails.id !== undefined ? recentTab.acmDetails.id : null;
    const initialState = {
      title: this.stringConstants.uploadFileTitle,
      keyboard: true,
      acmId: currentAcmId,
      activeTabTitle: this.currentActiveTab.title,
      savedDocuments: this.savedAcmDocuments,
      entityId: this.selectedEntityId,
    };
    this.bsModalRef = this.modalService.show(ACMUploadFilesComponent,
      Object.assign({ initialState }, { class: 'page-modal audit-team-add upload-file-dialog' }));
  }
}
