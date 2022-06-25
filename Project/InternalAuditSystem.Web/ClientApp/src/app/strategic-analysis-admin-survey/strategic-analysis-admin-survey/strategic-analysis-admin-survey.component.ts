import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Renderer2 } from '@angular/core';
import { CdkDragDrop, moveItemInArray, CdkDrag, CdkDrop, transferArrayItem } from '@angular/cdk/drag-drop';
import { StrategicAnalysisDragDropComponent } from '../strategic-analysis-drag-drop/strategic-analysis-drag-drop.component';
import { title } from 'process';
import { LoaderService } from '../../core/loader.service';
import {
  StrategicAnalysisAC, UserAC, StrategicAnalysesService,
  QuestionAC, QuestionType, RelatedAnswer, DropdownQuestionAC, MultipleChoiceQuestionAC,
  RatingScaleQuestionAC, SubjectiveQuestionAC, FileUploadQuestionAC, TextboxQuestionAC, CheckboxQuestionAC, StrategicAnalysisStatus, StrategicAnalysisTeamAC
} from '../../swaggerapi/AngularFiles';
import { QuestionService } from '../../services/strategic-analysis-service/question.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { StrategicAnalysisUserMappingAC } from '../../swaggerapi/AngularFiles/model/strategicAnalysisUserMappingAC';

@Component({
  selector: 'app-strategic-analysis-admin-survey',
  templateUrl: './strategic-analysis-admin-survey.component.html'
})
export class StrategicAnalysisAdminSurveyComponent implements OnInit {

  surveyTitle: string; // Variable for survey title
  surveyFinance: string; // Variable for survey finance title
  dragDrop: string; // Variable for drag and drop title
  dropDown: string; // Variable for dropdown tiltle
  multipleChoice: string; // Variable for multiple choice title
  ratingScale: string; // Variable for rating scale  field title
  subjective: string; // Variable for subjective field title
  fileUpload: string; // Variable for fileupload field title
  textField: string; // Variable for textfield title
  checkBox: string; // Variable for checkbox title
  addToolTip: string; // Variable for add tooltip
  excelToolTip: string; // Variable for exceltooltip
  pdfToolTip: string; // Variable for pdftooltip
  surveyBefore: string; // Variable for survey form field
  geoGraphical: string; // Variable for geographical form fields
  asia: string; // Variable for dropdown option
  africa: string; // Variable for dropdown option
  europe: string; // Variable for dropdown option
  previousExperience: string; // Variable  for previous years of experience title
  surveyQuestion: string; // Variable for rating question form field title
  financialYear: string; // Variable for financial year text field title
  uploadFinancial: string; // Variable for upload button text field title
  choose: string; // Variable for choose
  uploadFile: string; // Variable for upload file title
  nameLabel: string; // Variable for name label
  yes: string; // Variable for yes radio button
  no: string; // Variable for no radio button
  pacific: string; // Variable for  dropdown option
  saveButtonText: string; // Varibale for save button
  saveAsNewVersionText: string;
  backToolTip: string;
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  dots: string; // Dots image alt title
  strategicAnalysis: StrategicAnalysisAC; // Variable for strategic analysis
  internalUserList: Array<UserAC> = []; // Array to store internal user list
  internalUserAddList: Array<UserAC> = []; // Array to add users in internal user list
  selectedInternalUsers: Array<StrategicAnalysisTeamAC> = []; // Array to store selected internal users
  selectedPageItem: number; // Variable to store selected page item
  searchValue: string; // Variable to store search value
  passedStrategicAnalysisid: string; // Variable to store strategic analysis Id passed from StrategicAnalysisAdminAddComponent
  pageNumber: number; // Variable to store page number
  listOfQuestions: Array<QuestionAC> = []; // Variable to store list of questions
  questionAC: QuestionAC; // Variable to store question
  dropdownQuestionAC: DropdownQuestionAC; // Variable to store dropdown question
  multipleChoiceQuestionAC: MultipleChoiceQuestionAC; // Variable to store dropdown question AC model
  ratingScaleQuestionAC: RatingScaleQuestionAC; // Variable to store rating scale question AC model
  subjectiveQuestionAC: SubjectiveQuestionAC; // Variable to store subjective question AC model
  fileUploadQuestionAC: FileUploadQuestionAC; // Variable to store file upload question AC model
  textboxQuestionAC: TextboxQuestionAC; // Variable to store textbox question AC model
  checkboxQuestionAC: CheckboxQuestionAC; // Variable to store checkbox question AC model
  smileyValues: {
    value: string,
    number: number,
    class: string
  }[]; // Object to store smiley value and class

  isSampling: string;

  // Creates an instance of documenter
  constructor(
    public stringConstants: StringConstants,
    private route: ActivatedRoute,
    public router: Router,
    private apiService: StrategicAnalysesService,
    private toastr: ToastrService, private modalService: BsModalService, private renderer: Renderer2,
    private loaderService: LoaderService,
    private questionService: QuestionService) {
    this.surveyTitle = this.stringConstants.surveyTitle;
    this.surveyFinance = this.stringConstants.surveyFinance;
    this.dragDrop = this.stringConstants.dragDrop;
    this.dropDown = this.stringConstants.dropDown;
    this.multipleChoice = this.stringConstants.multipleChoice;
    this.ratingScale = this.stringConstants.ratingScale;
    this.subjective = this.stringConstants.subjective;
    this.fileUpload = this.stringConstants.fileUpload;
    this.textField = this.stringConstants.textField;
    this.checkBox = this.stringConstants.checkBox;
    this.addToolTip = this.stringConstants.addToolTip;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.surveyBefore = this.stringConstants.surveyBefore;
    this.geoGraphical = this.stringConstants.geoGraphical;
    this.asia = this.stringConstants.asia;
    this.africa = this.stringConstants.africa;
    this.pacific = this.stringConstants.pacific;
    this.europe = this.stringConstants.europe;
    this.previousExperience = this.stringConstants.previousExperience;
    this.surveyQuestion = this.stringConstants.surveyQuestion;
    this.financialYear = this.stringConstants.financialYear;
    this.uploadFile = this.stringConstants.uploadFile;
    this.choose = this.stringConstants.choose;
    this.nameLabel = this.stringConstants.nameLabel;
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
    this.uploadFinancial = this.stringConstants.uploadFile;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.saveAsNewVersionText = this.stringConstants.saveAsNewVersionText;
    this.dots = this.stringConstants.dots;
    this.backToolTip = this.stringConstants.backToolTip;
    this.strategicAnalysis = {
      version: 0,
      isSampling: false,
      questionsCount: 0,
      responseCount: 0,
      isDeleted: false,
      isVersionToBeChanged: false,
      auditableEntityId: '0',
      isResponseDrafted: false,
      status: StrategicAnalysisStatus.NUMBER_0,
      message: ''
    };
    this.internalUserList = [];
    this.internalUserAddList = [];
    this.selectedInternalUsers = [];
    this.searchValue = '';
    this.dropdownQuestionAC = {
      isDeleted: false,
      relatedAnswer: RelatedAnswer.NUMBER_0
    };

    this.multipleChoiceQuestionAC = {
      isDeleted: false,
      relatedAnswer: RelatedAnswer.NUMBER_0,
      isOtherToBeShown: false
    };
    this.ratingScaleQuestionAC = {
      scaleStart: 1,
      scaleEnd: 2,
      isDeleted: false
    };

    this.subjectiveQuestionAC = {
      isDeleted: false,
      characterLowerLimit: 1,
      characterUpperLimit: 999,
      guidance: ''
    };
    this.fileUploadQuestionAC = {
      isDeleted: false,
      isDocAllowed: false,
      isGifAllowed: false,
      isJpegAllowed: false,
      isPdfAllowed: false,
      isPngAllowed: false,
      isPpxAllowed: false,
      guidance: ''

    };
    this.textboxQuestionAC = {
      isDeleted: false,
      characterLowerLimit: 1,
      characterUpperLimit: 499,
      guidance: ''
    };
    this.checkboxQuestionAC = {
      isDeleted: false,
      isOtherToBeShown: false,
      relatedAnswer: RelatedAnswer.NUMBER_0
    };

    this.questionAC = {
      type: QuestionType.NUMBER_0,
      isRequired: true,
      isUserResponseExists: false,
      strategyAnalysisId: this.passedStrategicAnalysisid,
      isDeleted: false,
      dropdownQuestion: this.dropdownQuestionAC,
      sortOrder: 0
    };
    this.listOfQuestions = [];
    // Smiley Emotions Values
    this.smileyValues = [
      {
        value: 'very sad',
        number: 1,
        class: 'first-emoji mr-35'
      },
      {
        value: 'sad',
        number: 2,
        class: 'second-emoji mr-35'
      },
      {
        value: 'neutral',
        number: 3,
        class: 'third-emoji mr-35'
      },
      {
        value: 'happy',
        number: 4,
        class: 'forth-emoji mr-35'
      },
      {
        value: 'very happy',
        number: 5,
        class: 'fifth-emoji mr-35'
      }
    ];
  }



  // TODO: Added static code here, respective developer will change it in future
  // Adults in your company
  numberOfAdults = [
    {
      item: '3',
    },
    {
      item: '4'
    },
    {
      item: '7',
    },
    {
      item: '10',
    },
  ];
  // Sidebar lists
  sideBar = [
    {
      title: this.stringConstants.dropDown, icons: 'zmdi-caret-down', options: 'dropdown', value: QuestionType.NUMBER_1
    },
    {
      title: this.stringConstants.multipleChoice, icons: 'zmdi-view-list-alt', options: 'multiplechoice', value: QuestionType.NUMBER_0
    },
    {
      title: this.stringConstants.ratingScale, icons: 'zmdi-group-work', options: 'ratingscale', value: QuestionType.NUMBER_2
    },
    {
      title: this.stringConstants.subjective, icons: 'zmdi-format-clear-all', options: 'subjective', value: QuestionType.NUMBER_3
    },
    {
      title: this.stringConstants.fileUpload, icons: 'zmdi-upload', options: 'fileupload', value: QuestionType.NUMBER_5
    },
    {
      title: this.stringConstants.textField, icons: 'zmdi-text-format', options: 'textfield', value: QuestionType.NUMBER_4
    },
    {
      title: this.stringConstants.checkBox, icons: 'zmdi-check-square', options: 'checkbox', value: QuestionType.NUMBER_6
    }
  ];

  // Empty array for defining the dropping zone
  itememp = [];

  // On dragging and dropping the element, the modal popup appears and selected index number is passed.
  isAllowed = (drag?: CdkDrag, drop?: CdkDrop) => {
    return false;
  }

  /**
   * Drop event
   * @param event Event passed in drop
   */
  drop(event: CdkDragDrop<string[]>) {
    const questionResponse = this.listOfQuestions.findIndex(x => x.userResponse !== null && x.userResponse.userResponseStatus !== 0);

    if (this.listOfQuestions.length === 0 || questionResponse === -1) {
      if (event.previousContainer !== event.container) {
        const initialState = {
          listOptions: this.sideBar[event.previousIndex].value,
          surveyStrategicAnalysisId: this.passedStrategicAnalysisid,
          surveyTitle: this.strategicAnalysis.surveyTitle,
          version: this.strategicAnalysis.version,
          selectedPageItem: this.selectedPageItem,
          pageNumber: this.pageNumber,
          auditableEntityName: this.strategicAnalysis.auditableEntityName,
          message: this.strategicAnalysis.message
        };
        this.bsModalRef = this.modalService.show(StrategicAnalysisDragDropComponent,
          Object.assign({ initialState }, { class: 'page-modal strategic-drag' }));

      }
    } else {
      this.showError(this.stringConstants.noQuestionAddResponsePresent);
    }
  }

  drop2(event: CdkDragDrop<string[]>) {
    if (this.listOfQuestions.length === 0 || this.listOfQuestions.find(x => x.userResponse === null)) {
      moveItemInArray(this.listOfQuestions, event.previousIndex, event.currentIndex);
    } else {
      if (this.listOfQuestions.find(x => x.userResponse.userResponseStatus === 0)) {
        moveItemInArray(this.listOfQuestions, event.previousIndex, event.currentIndex);
      } else {
        this.showError(this.stringConstants.noQuestionAddResponsePresent);
      }
    }
  }

  /**
   * Open modalpopup on selecting the dots
   * @param questionId Question Id to be passed to open question details
   */
  openModal(questionId: string, event: Event) {
    const modalContainer = document.getElementById('#modalContainer');
    if (modalContainer === null) {
      if (questionId !== undefined) {
        this.apiService.strategicAnalysesGetQuestionById(questionId).subscribe(result => {

          this.questionAC = result;
          if (!this.questionAC.isUserResponseExists) {
            const questionDetails = this.getIsOtherToBeShownAndRelatedAnswer(this.questionAC.type);
            const initialState = {
              listOptions: this.sideBar.find(x => x.value === this.questionAC.type).value,
              surveyStrategicAnalysisId: this.passedStrategicAnalysisid,
              surveyTitle: this.strategicAnalysis.surveyTitle,
              version: this.strategicAnalysis.version,
              selectedPageItem: this.selectedPageItem,
              pageNumber: this.pageNumber,
              auditableEntityName: this.strategicAnalysis.auditableEntityName,
              message: this.strategicAnalysis.message,
              questionAC: this.questionAC,
              dropdownQuestionAC: this.questionAC.dropdownQuestion,
              checkboxQuestionAC: this.questionAC.checkboxQuestion,
              ratingScaleQuestionAC: this.questionAC.ratingScaleQuestion,
              multipleChoiceQuestion: this.questionAC.multipleChoiceQuestion,
              subjectiveQuestion: this.questionAC.subjectiveQuestion,
              fileUploadQuestion: this.questionAC.fileUploadQuestion,
              textboxQuestion: this.questionAC.textboxQuestion,
              isOtherToBeShown: questionDetails.isOtherToBeShown,
              relatedAnswer: questionDetails.relAns,
              isQuestionEdit: true,
              isSampling: this.isSampling
            };
            this.bsModalRef = this.modalService.show(StrategicAnalysisDragDropComponent,
              Object.assign({ initialState }, { class: 'page-modal strategic-drag' }));
          } else {
            this.showError(this.stringConstants.responsePresent);
          }
        },
          (error) => {
            this.loaderService.close();
            this.showError(error.error);
          });
      }
    }
  }
  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  async ngOnInit() {
    this.ratingBar();
    this.questionService.question.subscribe(question => {
      this.listOfQuestions.unshift(question);
      // order based on latest created question show in last
      this.listOfQuestions.sort((a, b) => (a.createdDateTime > b.createdDateTime) ? 1 : -1);
    });
    this.questionService.questionToBeDeleted.subscribe(question => {
      const deleteIndex = this.listOfQuestions.findIndex(x => x.id === question.id);
      this.listOfQuestions.splice(deleteIndex, 1);
    });
    this.questionService.questionToBeUpdated.subscribe(question => {
      const updateIndex = this.listOfQuestions.findIndex(x => x.id === question.id);
      this.listOfQuestions.splice(updateIndex, 1);
      this.listOfQuestions.unshift(question);
    });
    this.route.params.subscribe(params => {
      this.passedStrategicAnalysisid = params.passedStrategicAnalysisid;
      this.strategicAnalysis.surveyTitle = params.surveyTitle;
      this.strategicAnalysis.auditableEntityName = params.auditableEntityName;
      this.strategicAnalysis.auditableEntityId = params.entityId;
      this.strategicAnalysis.message = params.message;
      this.strategicAnalysis.version = params.version;
      this.selectedPageItem = params.selectedPageItem;
      this.pageNumber = params.pageNumber;
      this.isSampling = params.isSampling;
    });
    this.loaderService.open();
    await this.getQuestions(this.passedStrategicAnalysisid);
  }

  /**
   * Get all questions of strategicAnalysisId
   * @param strategicAnalysisId Strategic Analysis id for which questions are to be fetched
   */
  async getQuestions(strategicAnalysisId: string) {
    this.apiService.strategicAnalysesGetQuestions(strategicAnalysisId).subscribe(result => {
      this.loaderService.close();
      this.listOfQuestions = result;
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });
  }

  /**
   * Smiley Rating bar
   */
  ratingBar() {
    // Very simple JS for updating the text when a radio button is clicked
    // const INPUTS = document.querySelectorAll('#smileys input');
    // const updateValue = e => document.querySelector('#result').innerHTML = e.target.value;

    // INPUTS.forEach(el => el.addEventListener('click', e => updateValue(e)));
  }

  /**
   * Duplicate question
   * @param question Question to be duplicated
   */
  async duplicateQuestion(question: QuestionAC, event: Event) {
    event.stopPropagation();
    this.loaderService.open();
    this.apiService.strategicAnalysesGetQuestionById(question.id).subscribe(result => {
      this.loaderService.close();
      this.questionAC = result;
      const questionDetails = this.getIsOtherToBeShownAndRelatedAnswer(this.questionAC.type);
      if (!this.questionAC.isUserResponseExists) {
        const initialState = {
          listOptions: this.sideBar.find(x => x.value === this.questionAC.type).value,
          surveyStrategicAnalysisId: this.passedStrategicAnalysisid,
          surveyTitle: this.strategicAnalysis.surveyTitle,
          version: this.strategicAnalysis.version,
          selectedPageItem: this.selectedPageItem,
          pageNumber: this.pageNumber,
          auditableEntityName: this.strategicAnalysis.auditableEntityName,
          message: this.strategicAnalysis.message,
          questionAC: this.questionAC,
          dropdownQuestionAC: this.questionAC.dropdownQuestion,
          relatedAnswer: questionDetails.relAns,
          checkboxQuestionAC: this.questionAC.checkboxQuestion,
          ratingScaleQuestionAC: this.questionAC.ratingScaleQuestion,
          multipleChoiceQuestion: this.questionAC.multipleChoiceQuestion,
          subjectiveQuestion: this.questionAC.subjectiveQuestion,
          fileUploadQuestion: this.questionAC.fileUploadQuestion,
          textboxQuestion: this.questionAC.textboxQuestion,
          isOtherToBeShown: questionDetails.isOtherToBeShown
        };
        this.bsModalRef = this.modalService.show(StrategicAnalysisDragDropComponent,
          Object.assign({ initialState }, { class: 'page-modal strategic-drag' }));
      } else {
        this.showError(this.stringConstants.noDuplicateResponsePresent);
      }
      event.stopPropagation();
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });
  }

  /**
   * Delete question by id
   */
  deleteQuestion(questionId: string, event: Event) {
    if (questionId !== undefined) {
      if (!this.questionAC.isUserResponseExists) {
        const initialState = {
          title: this.stringConstants.deleteTitle,
          keyboard: true,
          callback: (res) => {
            if (res === this.stringConstants.yes) {
              this.loaderService.open();

              this.apiService.strategicAnalysesGetQuestionById(questionId).subscribe(result => {
                this.loaderService.close();
                this.questionAC = result;
              },
                (error) => {
                  this.loaderService.close();
                  this.showError(error.error);
                });

              this.apiService.strategicAnalysesDeleteQuestionById(questionId).subscribe(result => {
                this.questionService.onDeleteQuestion(this.questionAC);
                this.loaderService.close();
                this.showSuccess(this.stringConstants.recordDeletedMsg);
              },
                (error) => {
                  this.loaderService.close();
                  this.showError(error.error);
                });


            }
          },
        };
        this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
          Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
      } else {
        this.showError(this.stringConstants.noDeleteResponsePresent);
      }
    }
    event.stopPropagation();
  }

  /***
   *  update questions order
   */
  setQuestionOrders() {
    for (let i = 0; i < this.listOfQuestions.length; i++) {
      this.listOfQuestions[i].sortOrder = i + 1;
    }
    this.apiService.strategicAnalysesUpdatedQuestionsOrder(this.listOfQuestions).subscribe(result => {
      this.loaderService.close();
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });
  }



  /***
   *  Save survey
   */
  saveSurvey() {
    this.setQuestionOrders();
    this.setListPageRoute();
  }

  /***
   * Save survey as new version
   */
  saveSurveyAsNewVersion() {
    if (this.isSampling !== 'false') {
      const temp = this.strategicAnalysis;
      this.strategicAnalysis = {} as StrategicAnalysisAC;
      this.strategicAnalysis.version = temp.version;
      this.strategicAnalysis.surveyTitle = temp.surveyTitle;
      this.strategicAnalysis.isSampling = true;
    }
    this.strategicAnalysis.isVersionToBeChanged = true;
    this.strategicAnalysis.id = this.passedStrategicAnalysisid;
    this.loaderService.open();
    this.setQuestionOrders();
    // set auditable entity id and name
    this.strategicAnalysis.auditableEntityId = '00000000-0000-0000-0000-000000000000';
    this.strategicAnalysis.auditableEntityName = undefined;

    this.apiService.strategicAnalysesAddStrategicAnalysis(this.strategicAnalysis).subscribe(result => {
      this.loaderService.close();
      this.strategicAnalysis = result;
      this.showSuccess(this.stringConstants.recordAddedMsg);

      // back to list page
      this.setListPageRoute();
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });

  }

  /**
   * on back button route to list page
   */
  onBackClick() {
    this.setListPageRoute();
  }
  /**
   * set route for list page redirection
   */
  setListPageRoute() {
    const routePath = this.isSampling === 'true' ? '/sampling/list' : '/strategic-analysis/list';
    this.router.navigate([routePath, { pageItems: this.selectedPageItem, pageNumber: this.pageNumber }]);
  }

  /**
   * Get isOtherToBeShowm and related answer value of questionAC
   * @param type Type from model
   */
  getIsOtherToBeShownAndRelatedAnswer(type: QuestionType) {
    const questionDetails = {
      isOtherToBeShown: false,
      relAns: RelatedAnswer.NUMBER_0
    };

    // Getting related answer
    switch (type) {
      case QuestionType.NUMBER_0:
        questionDetails.relAns = this.questionAC.multipleChoiceQuestion.relatedAnswer;
        questionDetails.isOtherToBeShown = this.questionAC.multipleChoiceQuestion.isOtherToBeShown;
        break;
      case QuestionType.NUMBER_1:
        questionDetails.relAns = this.questionAC.dropdownQuestion.relatedAnswer;
        break;
      case QuestionType.NUMBER_6:
        questionDetails.relAns = this.questionAC.checkboxQuestion.relatedAnswer;
        questionDetails.isOtherToBeShown = this.questionAC.checkboxQuestion.isOtherToBeShown;
        break;
    }
    return questionDetails;
  }


  /**
   * Show error message toaster
   * @param errorMsg: Error message
   */
  showError(errorMsg: string) {
    this.toastr.error(errorMsg, '', {
      timeOut: 10000
    });
  }


  /**
   * Show sucess message toaster
   * @param successMsg: Success message
   */
  showSuccess(successMsg: string) {
    this.toastr.success(successMsg, '', {
      timeOut: 10000
    });
  }
}
