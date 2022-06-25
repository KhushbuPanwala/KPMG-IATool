import { Component, OnInit } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import {
  CdkDragDrop,
  moveItemInArray,
  CdkDrag,
  CdkDrop,
  transferArrayItem
} from '@angular/cdk/drag-drop';

import { QuestionAC } from '../../swaggerapi/AngularFiles/model/questionAC';
import { DropdownQuestionAC } from '../../swaggerapi/AngularFiles/model/dropdownQuestionAC';
import { RelatedAnswer } from '../../swaggerapi/AngularFiles/model/relatedAnswer';
import { QuestionType } from '../../swaggerapi/AngularFiles/model/questionType';
import { OptionAC } from '../../swaggerapi/AngularFiles/model/optionAC';
import {
  StrategicAnalysesService, MultipleChoiceQuestionAC, RatingScaleQuestionAC,
  SubjectiveQuestionAC, FileUploadQuestionAC, TextboxQuestionAC, CheckboxQuestionAC
} from '../../swaggerapi/AngularFiles';
import { LoaderService } from '../../core/loader.service';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { QuestionService } from '../../services/strategic-analysis-service/question.service';
import { parse } from 'querystring';

@Component({
  selector: 'app-strategic-analysis-drag-drop',
  templateUrl: './strategic-analysis-drag-drop.component.html'
})
export class StrategicAnalysisDragDropComponent implements OnInit {

  // Dropdown question variables
  dropDown: string; // Vairable for drop down title
  surveyQuestion: string; // Vairable for survey question title
  question: string; // Vairable for question title
  questionAC: QuestionAC; // Variable for dropdownQuestionAC

  // Multiple choice question variables
  multipleChoice: string; // Variable for multiple choice title
  surveyBefore: string; // Vairable for survey before title
  yes: string; // Vairable for yes checkbox
  no: string; // Vairable for no checkbox

  // Rating scale question variables
  ratingScale: string; // Vairable for rating scale title
  previousExperience: string; // Vairable for previous experience field title
  representation: string; // Vairable for representatim field title
  scale: string; // Vairable for scale field title
  toText: string; // Vairable for survey before title
  labels: string; // Vairable for label text feld title
  smileyAlt: string; // Vairable for smiley alt tag

  // Subjective question variables
  subjective: string; // Vairable for subjective title
  financialYear: string; // Vairable for financial year field title
  characterLimit: string; // Vairable for character limit
  characterLowerLimit: number; // Vairable for minimum limit number
  characterUpperLimit: number; // Vairable for maximum limit number
  characterText: string; // Vairable for character limit text
  maxLimit: string; // Vairable for maximum limit

  // Text field question variables
  textField: string; // Vairable for text field number
  nameLabel: string; // Vairable for name label title
  maxLimitText: string; // Vairable for maximum limit text  text-field
  maxNumberText: string; // Vairable for maximum number text-field

  // Upload file question variables
  fileUpload: string; // Vairable for file upload title
  allowedFile: string; // Vairable for allowed files formats title
  docText: string; // Vairable for doc title
  pdfToolTip: string; // Vairable for pdf title
  pngText: string; // Vairable for png title
  ppxText: string; // Vairable for ppx title
  jpegText: string; // Vairable for jpeg title
  gifText: string; // Vairable for gif title
  docLimit: string; // Vairable for doc limit paragraph

  // Checkbox question variables
  checkBox: string; // Variable for checkbox text
  answerCheckbox: string; // Checkbox answers placeholder
  asia: string; // Variable for asia text
  pacific: string; // Variable for pacific text
  europe: string; // Variable for europe text
  countriesText: string; // Countries placeholder

  // Common variables
  requiredMessage: string; // Vairable for required title
  answerChoice: string; // Vairable for answer choice title
  relatedAnswers: string; // Vairable for related answers  dropdown title
  cancelButtonText: string; // Vairable for cancel button text
  saveButtonText: string; // Variable for save button
  addToolTip: string; // Vairable for add tooltip
  deleteToolTip: string; // Vairable for delete tooltip
  textAreaPlaceHolder: string; // Variable for textarea placeholder
  listOptions: QuestionType; // Selected element index number
  guidanceText: string; // Vairable for guidance text title
  guidancePlaceholder: string; // Vairable for guidance placeholder title
  addOther: string; // Vairable for add other checkbox title
  smileyText: string; // Variable for smiley text
  linearText: string; // Variable for linear text
  isShowMultiple: boolean; // Boolean for showing multiple field modal popup
  isShowdropDown: boolean; // Boolean for showing drop down modal popup
  isShowRating: boolean; // Boolean for showing rating scale modal popup
  isShowSubjective: boolean; // Boolean for showing subjective modal popup
  isShowTextField: boolean; // Vairable for showing text field modal popup
  isFileUpload: boolean; // Vairable for file-upload modal popup
  isShowCheckbox: boolean; // Variable for checkbox modal popup
  isIconShow = false; // Variable for showing and hiding icon of selction of drodown representation
  answerDropDown: string; // Vairable for answer drop down title
  surveyStrategicAnalysisId: string; // Variable for strategic analysis id
  options: Array<string> = []; // Variable to store options array
  optionDropdown: string; // Variable to store dropdown option
  optionMultiple: string; // Variable to set option
  optionCheckbox: string;
  surveyTitle: string; // Variable to set survey title
  version: number;
  selectedPageItem: number; // Variable to set selected page number
  pageNumber: number; // Variable to set page number
  auditableEntityName: string; // Variable to set Auditable entitiy name
  message: string; // Variable to set message
  isQuestionEdit: boolean; // Variable to determine if the operation is edit or add
  relatedAnswer: RelatedAnswer; // Variable to set related answer
  isOtherToBeShown: boolean; // Variable to set other value
  fieldCheckbox: string;
  fieldMultipleChoice: string;
  startLabelText: string; // Variable to set startLabel text
  endLabelText: string; // Variable to set end label
  guidanceOptionalText: string; // Variable to set guidance text
  showPopUpType: string; // Variable to show pop up of particular question type
  dropdownQuestionAC: DropdownQuestionAC; // Variable to store dropdown question AC model
  multipleChoiceQuestionAC: MultipleChoiceQuestionAC; // Variable to store dropdown question AC model
  ratingScaleQuestionAC: RatingScaleQuestionAC; // Variable to store rating scale question AC model
  subjectiveQuestionAC: SubjectiveQuestionAC; // Variable to store subjective question AC model
  fileUploadQuestionAC: FileUploadQuestionAC; // Variable to store file upload question AC model
  textboxQuestionAC: TextboxQuestionAC; // Variable to store textbox question AC model
  checkboxQuestionAC: CheckboxQuestionAC; // Variable to store checkbox question AC model

  // Creates an instance of documenter.
  constructor(
    public stringConstants: StringConstants, public bsModalRef: BsModalRef, private apiService: StrategicAnalysesService, private loaderService: LoaderService, private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router,
    private questionService: QuestionService) {
    this.textAreaPlaceHolder = this.stringConstants.textAreaPlaceHolder;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.multipleChoice = this.stringConstants.multipleChoice;
    this.surveyBefore = this.stringConstants.surveyBefore;
    this.requiredMessage = this.stringConstants.requiredMessage;
    this.relatedAnswers = this.stringConstants.relatedAnswers;
    this.answerChoice = this.stringConstants.answerChoice;
    this.addOther = this.stringConstants.addOther;
    this.yes = this.stringConstants.yes;
    this.no = this.stringConstants.no;
    this.cancelButtonText = this.stringConstants.cancelButtonText;
    this.question = this.stringConstants.question;
    this.addToolTip = this.stringConstants.addToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.dropDown = this.stringConstants.dropDown;
    this.answerDropDown = this.stringConstants.answerDropDown;
    this.surveyQuestion = this.stringConstants.surveyQuestion;
    this.ratingScale = this.stringConstants.ratingScale;
    this.previousExperience = this.stringConstants.previousExperience;
    this.representation = this.stringConstants.representation;
    this.scale = this.stringConstants.scale;
    this.labels = this.stringConstants.labels;
    this.toText = this.stringConstants.toText;
    this.smileyAlt = this.stringConstants.smileyAlt;
    this.subjective = this.stringConstants.subjective;
    this.financialYear = this.stringConstants.financialYear;
    this.characterLimit = this.stringConstants.characterLimit;
    this.characterText = this.stringConstants.characterText;
    this.guidanceText = this.stringConstants.guidanceText;
    this.guidancePlaceholder = this.stringConstants.guidancePlaceholder;
    this.maxLimit = this.stringConstants.maxLimit;
    this.textField = this.stringConstants.textField;
    this.nameLabel = this.stringConstants.nameLabel;
    this.maxLimitText = this.stringConstants.maxLimitText;
    this.maxNumberText = this.stringConstants.maxNumberText;
    this.fileUpload = this.stringConstants.fileUpload;
    this.allowedFile = this.stringConstants.allowedFile;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.docText = this.stringConstants.docText;
    this.pngText = this.stringConstants.pngText;
    this.ppxText = this.stringConstants.pptText;
    this.jpegText = this.stringConstants.jpegText;
    this.gifText = this.stringConstants.gifText;
    this.docLimit = this.stringConstants.docLimit;
    this.smileyText = this.stringConstants.smileyText;
    this.linearText = this.stringConstants.linearText;
    this.checkBox = this.stringConstants.checkBox;
    this.asia = this.stringConstants.asia;
    this.pacific = this.stringConstants.pacific;
    this.europe = this.stringConstants.europe;
    this.countriesText = this.stringConstants.countriesText;
    this.answerCheckbox = this.stringConstants.answerCheckbox;
    this.fieldCheckbox = '';
    this.fieldMultipleChoice = '';
    if (this.relatedAnswer === undefined) {
      this.relatedAnswer = RelatedAnswer.NUMBER_0;
    }
    this.dropdownQuestionAC = {
      isDeleted: false,
      relatedAnswer: this.relatedAnswer
    };
    this.multipleChoiceQuestionAC = {
      isDeleted: false,
      relatedAnswer: this.relatedAnswer,
      isOtherToBeShown: false
    };
    this.ratingScaleQuestionAC = {
      scaleStart: 1,
      scaleEnd: 5,
      isDeleted: false,
      representation: 'Smiley',
      startLabel: '',
      endLabel: ''
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
      relatedAnswer: this.relatedAnswer
    };

    this.questionAC = {
      type: QuestionType.NUMBER_0,
      isRequired: true,
      strategyAnalysisId: this.surveyStrategicAnalysisId,
      isDeleted: false,
      isUserResponseExists: false,
      sortOrder: 0,
      dropdownQuestion: this.dropdownQuestionAC,
      multipleChoiceQuestion: this.multipleChoiceQuestionAC,
      ratingScaleQuestion: this.ratingScaleQuestionAC,
      subjectiveQuestion: this.subjectiveQuestionAC,
      fileUploadQuestion: this.fileUploadQuestionAC,
      textboxQuestion: this.textboxQuestionAC,
      checkboxQuestion: this.checkboxQuestionAC
    };

    this.options = [];
    this.optionDropdown = '';
    this.optionMultiple = '';
    this.optionCheckbox = '';
    this.characterLowerLimit = 1;
    this.characterUpperLimit = 999;
    this.guidanceOptionalText = '';
  }
  // Related Answer multiple choice
  // Related answers array, this are static values developers will change
  relatedAnswersArray = [
    {
      item: 'Choose related answer',
      type: RelatedAnswer.NUMBER_0,
      options: []
    },
    {
      item: 'Countries',
      type: RelatedAnswer.NUMBER_1,
      options: ['India', 'US', 'Pakistan', 'China']
    },
    {
      item: 'Yes/No',
      type: RelatedAnswer.NUMBER_2,
      options: ['Yes', 'No']
    },
    {
      item: 'True/False',
      type: RelatedAnswer.NUMBER_3,
      options: ['True', 'False']
    },
    {
      item: 'Days of the week',
      type: RelatedAnswer.NUMBER_4,
      options: ['Monday', 'Tuesday', 'Wednesday', 'Thrusday', 'Friday', 'Saturday', 'Sunday']
    },
    {
      item: 'Months',
      type: RelatedAnswer.NUMBER_5,
      options: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December']
    },
    {
      item: '>1000',
      type: RelatedAnswer.NUMBER_6,
      options: ['>1000']
    }
  ];
  // Sidebar lists
  questionTypeDetails = [
    {
      options: 'dropdown', value: QuestionType.NUMBER_1
    },
    {
      options: 'multiplechoice', value: QuestionType.NUMBER_0
    },
    {
      options: 'ratingscale', value: QuestionType.NUMBER_2
    },
    {
      options: 'subjective', value: QuestionType.NUMBER_3
    },
    {
      options: 'fileupload', value: QuestionType.NUMBER_5
    },
    {
      options: 'textfield', value: QuestionType.NUMBER_4
    },
    {
      options: 'checkbox', value: QuestionType.NUMBER_6
    }
  ];
  selectedRelatedAnswer = this.relatedAnswersArray[0].item;

  // Representation dropdown
  representDropDown = [
    {
      item: 'Smiley', isIconShow: true,
    }
  ];
  selectedSmiley = this.representDropDown[0].item;

  // Scale dropdown for rating scale
  scaleDropDownStart = [
    {
      item: '1',
    }
  ];
  // Scale dropdown for rating scale
  scaleDropDownEnd = [
    {
      item: '3',
    },
    {
      item: '4',
    },
    {
      item: '5',
    }
  ];
  isSampling: string;

  // For hiding and showing the smiley image from representation dropdown
  selectedOption() {
    if (this.selectedSmiley === 'Smiley') {
      this.isIconShow = true;
    } else {
      this.isIconShow = false;
    }
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
 *   Initialization of properties.
 */
  ngOnInit(): void {
    this.selectedOption();
    this.questionAC.type = this.listOptions;
    // assign value to show which Type Popup
    this.showPopUpType = this.questionTypeDetails.find(x => x.value === this.listOptions).options;
    this.questionAC.strategyAnalysisId = this.surveyStrategicAnalysisId;
    if (this.questionAC.dropdownQuestion != null) {
      this.questionAC.dropdownQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    if (this.questionAC.multipleChoiceQuestion != null) {
      this.questionAC.multipleChoiceQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    if (this.questionAC.checkboxQuestion != null) {
      this.questionAC.checkboxQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    this.setSelectedRelatedAnswer(this.relatedAnswer);
    if (this.questionAC.options != null) {
      this.options = this.questionAC.options;
    }
  }

  /**
   * * Adding options to question
   * @param option Option to be pushed
   */
  enterOptions(option: string) {
    if (option !== null) {
      if (option.length > 0 && option.length <= 256) {
        this.optionDropdown = '';
        this.optionMultiple = '';
        this.optionCheckbox = '';

        if (option !== null) {
          this.options.push(option);
        }
        this.options = [...this.options];
      } else {
        if (option.length <= 0) {
          this.showError(this.stringConstants.blankOptionErrorText);
        } else if (option.length > 256) {
          this.showError(this.stringConstants.maxLengthExceedMessage);
        }
      }
    }


  }

  /**
   * To trackIndex in html
   * @param index Index of array
   * @param obj Object whose index to be tracked
   */
  trackByIndex(index: number, obj: string): number {
    return index;
  }

  /**
   * Delete options from optionList
   * @param option Option to be deleted from optionList
   * @param i Index
   */
  deleteOptions(option: string, i: number) {
    this.options.splice(i, 1);
    this.options = [...this.options];
  }

  /**
   * Save question
   */
  saveQuestion() {
    this.questionAC.options = this.options;
    if (this.questionAC.dropdownQuestion != null) {
      this.questionAC.dropdownQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    if (this.questionAC.multipleChoiceQuestion != null) {
      this.questionAC.multipleChoiceQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    if (this.questionAC.checkboxQuestion != null) {
      this.questionAC.checkboxQuestion.relatedAnswer = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer).type;
    }
    if (this.questionAC.ratingScaleQuestion != null) {
      this.questionAC.ratingScaleQuestion.resultSmileyList = this.getResultSmileyArray(this.questionAC.ratingScaleQuestion.scaleStart, this.questionAC.ratingScaleQuestion.scaleEnd);
    }

    let isValid = false;

    if (this.surveyStrategicAnalysisId !== undefined && this.questionAC.questionText) {

      switch (this.questionAC.type) {
        case 3:
        case 4:
          isValid = this.isValidSubjectiveOrTextFieldQuestion();
          break;
        case 5: // file upload
          isValid = this.isFileUploadQuestionValid(this.questionAC);
          break;
        default:
          isValid = true;
      }
    }

    if (!this.isQuestionEdit) {
      // To add question
      if (isValid) {
        this.addQuestion();
      }
    } else {
      // To update question
      if (isValid) {
        this.updateQuestion();
      }
    }

  }

  /**
   * Check if file upload question is valid or not, if any one allowed file types are checked or not
   * @param questionAC Question to be checked
   */
  isFileUploadQuestionValid(questionAC: QuestionAC) {

    const isValid = questionAC.fileUploadQuestion.isDocAllowed ||
      questionAC.fileUploadQuestion.isPdfAllowed ||
      questionAC.fileUploadQuestion.isGifAllowed ||
      questionAC.fileUploadQuestion.isPngAllowed ||
      questionAC.fileUploadQuestion.isPpxAllowed ||
      questionAC.fileUploadQuestion.isJpegAllowed ? true : false;
    if (!isValid) {
      this.toastr.error('Choose allowed file types');
    }
    return isValid;

  }

  /**
   * Call add question api
   */
  addQuestion() {
    this.loaderService.open();
    this.apiService.strategicAnalysesAddQuestion(this.questionAC, this.surveyStrategicAnalysisId).subscribe(result => {
      this.loaderService.close();
      this.questionAC = result;
      this.questionService.onAddQuestion(this.questionAC);
      this.showSuccess(this.stringConstants.recordAddedMsg);
      this.setSurveyListPageRoute();
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });
    this.bsModalRef.hide();
  }

  /**
   * Call update question api
   */
  updateQuestion() {
    this.loaderService.open();
    this.apiService.strategicAnalysesUpdateQuestion(this.questionAC).subscribe(result => {
      this.loaderService.close();
      this.questionAC = result;
      this.questionService.onEditQuestion(this.questionAC);
      this.showSuccess(this.stringConstants.recordUpdatedMsg);
      this.setSurveyListPageRoute();
    },
      (error) => {
        this.loaderService.close();
        this.showError(error.error);
      });
    this.bsModalRef.hide();
  }
  /**
   * Show error message toaster
   * @param errorMsg: Error message
   */
  showError(errorMsg: string) {
    console.log(errorMsg);
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

  /** Set survey list page route and send necessary data */
  setSurveyListPageRoute() {
    const routePath = this.isSampling === 'true' ? '/sampling/create-survey' : '/strategic-analysis-survey/create';

    this.router.navigate([routePath, {
      isSampling: this.isSampling.toString(),
      passedStrategicAnalysisid: this.surveyStrategicAnalysisId,
      surveyTitle: this.surveyTitle,
      version: this.version,
      selectedPageItem: this.selectedPageItem,
      pageNumber: this.pageNumber,
      auditableEntityName: this.auditableEntityName,
      message: this.message,
      // QuestionAC parameters
      questionAC: this.questionAC,
      questionACId: this.questionAC.id,
      questionACText: this.questionAC.questionText,
      questionACOptions: this.questionAC.options
    }]);
  }

  /**
   * Set Selected related answer
   * @param relatedAnswer answer
   */
  setSelectedRelatedAnswer(relatedAnswer: RelatedAnswer) {
    if (relatedAnswer !== undefined) {
      this.selectedRelatedAnswer = this.relatedAnswersArray[relatedAnswer].item;
    }
  }

  /**
   * To set isOtherToBeShown value
   */
  onOtherClick(type) {

    switch (type) {
      case this.stringConstants.checkBox:
        this.questionAC.checkboxQuestion.isOtherToBeShown = this.questionAC.checkboxQuestion.isOtherToBeShown ? false : true;
        break;
      case this.stringConstants.multipleChoice:
        this.questionAC.multipleChoiceQuestion.isOtherToBeShown = this.questionAC.multipleChoiceQuestion.isOtherToBeShown ? false : true;
        break;
    }
  }

  addFieldLabel(type, fieldLabel) {
    switch (type) {
      case this.stringConstants.multipleChoice:
        this.questionAC.multipleChoiceQuestion.fieldLabel = this.questionAC.multipleChoiceQuestion.fieldLabel;
        break;
      case this.stringConstants.checkBox:
        this.questionAC.checkboxQuestion.fieldLabel = this.questionAC.checkboxQuestion.fieldLabel;
        break;
    }
  }

  /**
   * Set file types required value in file upload question
   * @param type Type passed from model
   */
  fileRequiredType(type: string) {
    switch (type) {
      case this.stringConstants.docText:
        this.questionAC.fileUploadQuestion.isDocAllowed = this.questionAC.fileUploadQuestion.isDocAllowed ? false : true;
        break;
      case this.stringConstants.pdfText:
        this.questionAC.fileUploadQuestion.isPdfAllowed = this.questionAC.fileUploadQuestion.isPdfAllowed ? false : true;
        break;
      case this.stringConstants.pngText:
        this.questionAC.fileUploadQuestion.isPngAllowed = this.questionAC.fileUploadQuestion.isPngAllowed ? false : true;
        break;
      case this.stringConstants.ppxText:
        this.questionAC.fileUploadQuestion.isPpxAllowed = this.questionAC.fileUploadQuestion.isPpxAllowed ? false : true;
        break;
      case this.stringConstants.jpegText:
        this.questionAC.fileUploadQuestion.isJpegAllowed = this.questionAC.fileUploadQuestion.isJpegAllowed ? false : true;
        break;
      case this.stringConstants.gifText:
        this.questionAC.fileUploadQuestion.isGifAllowed = this.questionAC.fileUploadQuestion.isGifAllowed ? false : true;
        break;
    }
  }

  /**
   * Find if subjective or text field question's character limits are valid or not
   */
  isValidSubjectiveOrTextFieldQuestion() {
    let characterLowerLim = 1;
    let characterUpperLim = 999;
    const characterLowerLimActual = 1;
    let characterUpperLimActual = 1000;

    if (this.questionAC.type === 3) {
      characterLowerLim = this.questionAC.subjectiveQuestion.characterLowerLimit;
      characterUpperLim = this.questionAC.subjectiveQuestion.characterUpperLimit;
    } else {
      characterLowerLim = this.questionAC.textboxQuestion.characterLowerLimit;
      characterUpperLim = this.questionAC.textboxQuestion.characterUpperLimit;
      characterUpperLimActual = 500;
    }
    if (characterLowerLim < characterLowerLimActual || characterUpperLim > characterUpperLimActual) {
      this.showError('Please enter a value between 0 to ' + characterUpperLimActual);
      return false;
    }
    if (characterLowerLim > characterUpperLim) {
      this.showError('The minimum value must be less than maximum value');
      return false;
    } else {
      return true;
    }
  }

  /**
   * Get value array of smiley to be shown
   * @param userLowerValue lower limit set by user
   * @param userUpperValue upper limit set by user
   */
  getResultSmileyArray(userLowerValue, userUpperValue): Array<number> {

    const result = [];
    const actualArray = [1, 2, 3, 4, 5];
    const required = userUpperValue;
    const total = actualArray[4];
    let increment = 0.0;
    increment = (total - 1) / (required - 1);
    let i = 0;
    let value = 1.0;
    for (i = 1; i <= required; i++) {
      result.push(Math.round(value));
      value = value + increment;
    }
    return result;
  }

  /** To add related answer's predefined options */
  addRelatedAnswerOptions() {
    const relAns = this.relatedAnswersArray.find(x => x.item === this.selectedRelatedAnswer);
    let i = 0;
    for (i = 0; i < relAns.options.length; i++) {
      this.options.push(relAns.options[i]);
    }
  }
}
