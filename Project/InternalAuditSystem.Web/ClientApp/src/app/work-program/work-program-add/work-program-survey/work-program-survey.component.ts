import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { StrategicAnalysisAC, UserResponseAC, OptionAC, QuestionAC, StrategicAnalysesService, StrategicAnalysisStatus, RiskControlMatrixAC, FileUploadQuestionAC, UserResponseDocumentAC } from '../../../swaggerapi/AngularFiles';
import { StringConstants } from '../../../shared/stringConstants';
import { ActivatedRoute, Router } from '@angular/router';
import { LoaderService } from '../../../core/loader.service';
import { SharedService } from '../../../core/shared.service';
import { UploadService } from '../../../core/upload.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-work-program-survey',
  templateUrl: './work-program-survey.component.html'
})
export class WorkProgramSurveyComponent implements OnInit {

  subjective: string; // Variable for subjective field
  surveyFinance: string; // Variable for survey finance title
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
  selectedPageItem: number; // Variable to store selected page item
  searchValue: string; // Variable to store search value
  submitSurvey: string; // Variable for submit survey button
  save: string; // Variable for save draft button
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  strategicAnalysis: StrategicAnalysisAC;
  smileyValues: {
    value: string,
    number: number,
    class: string
  }[];
  userResponse = {} as UserResponseAC;
  options: Array<OptionAC> = [];
  checkboxOptions: Array<string>;
  questions: Array<QuestionAC> = [];
  // To check form validity
  isFormValid: boolean;
  previousButton: string;
  @Input() riskControlMatrixDetail: RiskControlMatrixAC;
  @Input() samplingRcmId: string;
  showNoDataText: string;
  allowedFileTypesText: string;
  wordType: string;
  pdfType: string;
  pptType: string;
  otherFileType: string;
  gifType: string;
  pngType: string;
  jpgType: string;
  ppxType: string;
  jpegType: string;
  noQuestionAvailable: string;
  @Output() isShowSamplingList = new EventEmitter<boolean>(); // Output passing the boolean data to edit page
  chooseFileText: string;
  deleteToolTip: string;
  viewFilesToolTip: string;
  downloadToolTip: string;
  formData: FormData = new FormData();

  // Creates an instance of documenter
  constructor(
    public stringConstants: StringConstants,
    private router: Router,
    private route: ActivatedRoute,
    private loaderService: LoaderService,
    private modalService: BsModalService,
    private apiService: StrategicAnalysesService,
    private sharedService: SharedService,
    private uploadService: UploadService,
    private toastr: ToastrService) {
    this.surveyFinance = this.stringConstants.surveyFinance;
    this.subjective = this.stringConstants.subjective;
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
    this.submitSurvey = this.stringConstants.submitSurvey;
    this.save = this.stringConstants.saveButtonText;
    this.previousButton = this.stringConstants.previousButton;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.allowedFileTypesText = this.stringConstants.allowedFileTypeQuestionText;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.viewFilesToolTip = this.stringConstants.viewFiles;
    this.downloadToolTip = this.stringConstants.downloadToolTip;

    // file format assign
    this.wordType = this.stringConstants.docText;
    this.pdfType = this.stringConstants.pdfText;
    this.pptType = this.stringConstants.pptText;
    this.otherFileType = this.stringConstants.otherFileType;
    this.gifType = this.stringConstants.gifText;
    this.pngType = this.stringConstants.pngText;
    this.jpgType = this.stringConstants.jpgText;
    this.ppxType = this.stringConstants.ppxText;
    this.jpegType = this.stringConstants.jpegText;
    this.noQuestionAvailable = 'No question available';
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

    this.options = [{
      optionText: '',
      isDeleted: false
    }];


    this.strategicAnalysis = {
      version: 1,
      isSampling: false,
      questionsCount: 0,
      responseCount: 0,
      isDeleted: false,
      isVersionToBeChanged: false,
      isResponseDrafted: false,
      status: StrategicAnalysisStatus.NUMBER_0
    };
    this.isFormValid = false;
    this.chooseFileText = this.stringConstants.chooseFileText;

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


  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */

  async ngOnInit() {
    this.ratingBar();
    this.loaderService.open();
    await this.getStrategicAnalysis(this.riskControlMatrixDetail.strategicAnalysisId);
  }


  // Smiley Rating bar
  ratingBar() {
    // Very simple JS for updating the text when a radio button is clicked
    const INPUTS = document.querySelectorAll('#smileys input');
    const updateValue = e => document.querySelector('#result').innerHTML = e.target.value;

    INPUTS.forEach(el => el.addEventListener('click', e => updateValue(e)));
  }


  /** Submit user response */
  submitUserResponse() {
    this.loaderService.open();
    this.strategicAnalysis.isUserResponseToBeUpdated = true;

    this.isFormValid = this.validationCheck();
    if (this.isFormValid) {
      if (this.strategicAnalysis.questions[0].userResponse.userResponseStatus === StrategicAnalysisStatus.NUMBER_1 || this.strategicAnalysis.questions[0].userResponse.userResponseStatus === StrategicAnalysisStatus.NUMBER_2) {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.alreadySubmitted);
      } else {
        // Set the User response status as UnderReview
        this.strategicAnalysis.questions.forEach((ques) => {
          if (ques.userResponse !== null) {
            ques.userResponse.riskControlMatrixId = this.riskControlMatrixDetail.id;
            ques.userResponse.userResponseStatus = StrategicAnalysisStatus.NUMBER_2;
            ques.isUserResponseExists = true;
          }
        });
        this.uploadService.uploadFileOnUpdate<StrategicAnalysisAC>(this.strategicAnalysis, [], this.stringConstants.strategicAnalysisFiles, this.stringConstants.strategicAnalysisApiPath).subscribe(result => {
          this.saveFileTypeQuestion(false);
        },
          error => {
            this.sharedService.handleError(error);
          });
      }
    } else {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.formInvalidityMessage);
    }
  }

  /**
   * Save file type question if saved as draft or final
   * @param isDraftType : Bit to check where draft type response or final type
   */
  saveFileTypeQuestion(isDraftType: boolean) {
    // call upload file
    for (const response of this.strategicAnalysis.questions) {
      const questionId = response.id;
      const uploadedFiles = [] as Array<File>;
      if (response.files !== null && response.files !== undefined) {
        for (const perFile of response.files) {
          this.formData.append(questionId, perFile);
        }
      }

    }
    const apiPath = isDraftType ? this.stringConstants.draftFileTypeQuestionApiPath : this.stringConstants.finalFileTypeQuestionApiPath;

    const entityId = '00000000-0000-0000-0000-000000000000';
    this.uploadService.uploadCollectionOfFileStrategicAnalysis(this.formData, apiPath, this.strategicAnalysis.questions[0].strategyAnalysisId, entityId)
      .subscribe(() => {
        this.loaderService.close();
        this.sharedService.showSuccess(this.stringConstants.recordAddedMsg);
        this.onPreviousClick();
      },
        (error) => {
          this.sharedService.handleError(error);
        });
  }

  /**
   * Get strategic analysis by id
   * @param strategicId Strategic Analysis id on basis of which, strategic analysis is to be fetched
   */
  async getStrategicAnalysis(strategicId: string) {
    this.apiService.strategicAnalysesGetStrategicAnalysisById(strategicId, this.riskControlMatrixDetail.id).subscribe(result => {
      this.loaderService.close();
      this.strategicAnalysis = JSON.parse(JSON.stringify(result));
      this.strategicAnalysis.questions = JSON.parse(JSON.stringify(result.questions));
      if (this.strategicAnalysis.questions !== null) {
        this.strategicAnalysis.questions.forEach((ques) => {
          if (ques.files == null) {
            ques.files = [];
          }
          if (ques.userResponseDocumentACs == null) {
            ques.userResponseDocumentACs = [] as Array<UserResponseDocumentAC>;
          }
          if (ques.userResponse === null) {
            ques.userResponse = {
              answerText: '',
              strategicAnalysisId: strategicId
            } as UserResponseAC;
          }
          ques.userResponse.questionId = ques.id;
        });
      } else {
        this.strategicAnalysis.questions = [];
      }
    },
      (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * Add selected smiley value to respective question
   * @param questionId Question id to which value to be attached
   * @param smileyNumber smileyNumber to assign smiley number to user response
   * @param smileyValue smileyValue to assign smiley value to user response
   */
  onSmileySelection(questionId: string, smileyNumber: number, smileyValue: string) {
    const ratingQuestionIndex = this.strategicAnalysis.questions.findIndex(x => x.id === questionId);
    this.strategicAnalysis.questions[ratingQuestionIndex].userResponse.representationNumber = smileyNumber;
  }

  /**
   * Add selected option in user response
   * @param questionId Question id to which user response to be added
   * @param selectedOption Option to be added in user response
   */
  onOptionClick(questionId: string, selectedOption: string, questionType: number) {

    const tempResponse = {} as UserResponseAC;
    tempResponse.options = [] as OptionAC[];
    tempResponse.strategicAnalysisId = this.strategicAnalysis.id;

    const option = {} as OptionAC;
    option.optionText = selectedOption;

    const ques = this.strategicAnalysis.questions.find(x => x.id === questionId);
    tempResponse.id = ques.userResponse.id !== undefined ? ques.userResponse.id : undefined;

    const optionIndex = ques.userResponse !== null && ques.userResponse.options !== undefined ? ques.userResponse.options.findIndex(x => x.optionText === option.optionText) : -1;
    switch (questionType) {
      case 0:
        this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.other = '';
        if (optionIndex === -1) {
          tempResponse.options = [] as OptionAC[];
          tempResponse.options.push(option);
        }
        tempResponse.multipleChoiceQuestionId = this.strategicAnalysis.questions.find(x => x.id === questionId).multipleChoiceQuestion.id;
        tempResponse.questionId = questionId;
        this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse = tempResponse;
        break;
      case 1:
        if (optionIndex === -1) {
          tempResponse.options = [] as OptionAC[];
          tempResponse.options.push(option);
        }
        tempResponse.dropdownQuestionId = this.strategicAnalysis.questions.find(x => x.id === questionId).dropdownQuestion.id;
        tempResponse.questionId = questionId;
        tempResponse.selectedDropdownOption = selectedOption;
        this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse = tempResponse;
        break;
      case 6:
        if (optionIndex === -1) {
          if (ques.userResponse !== null && ques.userResponse.options !== undefined) {
            this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.options.push(option);
            this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.questionId = questionId;
          } else {
            tempResponse.options.push(option);
            tempResponse.questionId = questionId;
            this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse = tempResponse;
          }
        } else if (optionIndex !== -1) {
          this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.options.splice(optionIndex, 1);
        }
        this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.checkboxQuestionId = this.strategicAnalysis.questions.find(x => x.id === questionId).checkboxQuestion.id;
        break;
    }
  }

  /**
   * To check if selected option is already in user response
   * @param question Question in which option is to be searched for
   * @param opt Option to be searched for
   */
  checkIfSelectedOption(question: QuestionAC, opt: string) {
    if (question.userResponse.options !== undefined) {
      const index = question.userResponse.options.findIndex(x => x.optionText === opt);
      if (index !== -1) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  /**
   * To check if other text exists
   * @param question Question where other is to be checked for
   * @param other Other text which is to be checked
   */
  checkIfOtherExists(question: QuestionAC, other: string) {
    if (question.userResponse.other !== null) {
      if (question.userResponse.other !== undefined) {
        if (question.userResponse.other.length > 0) {
          return true;
        }
      }
    }
    return false;
  }

  /**
   * To check if option array is empty
   * @param question Question where options are to be checked for
   */
  checkIfOptionsEmpty(question: QuestionAC) {
    if (question.userResponse.options !== undefined) {
      if (question.userResponse.options !== null) {
        if (question.userResponse.options.length > 0) {
          return false;
        }
      }
    }
    return true;
  }

  /**
   * For mcq question, removal of selected option
   * @param questionId Question id of question where selected option is to be seached for
   */
  removeSelectedOptions(questionId: string) {
    this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.options = [] as OptionAC[];
  }

  /**
   * For checkbox question, to remove other text on option click
   * @param questionId Checkbox question id
   */
  removeOtherTextOfCheckbox(questionId: string) {
    this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.other = null;
  }

  /**
   * Add answer text to user response of subjective and textbox question type
   * @param answerText Answer text to be send with user response
   * @param questionId Question id of question with which user response is to be attached
   * @param questionType Type of question
   */
  enterAnswer(answerText: string, questionId: string, questionType: number) {
    this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.answerText = answerText;
  }

  /**
   * To check form validation for required questions
   */
  validationCheck() {
    if (this.strategicAnalysis.questions !== null) {
      let i = 0;
      for (i = 0; i < this.strategicAnalysis.questions.length; i++) {
        const question = this.strategicAnalysis.questions[i];
        if (question.isRequired) {
          switch (question.type) {
            case 0: case 6:
              if (question.userResponse.options === undefined && (question.userResponse.other === undefined || question.userResponse.other === null)) {
                return false;
              } else if (question.userResponse.options !== undefined && (question.userResponse.other === undefined || question.userResponse.other === null)) {
                if (question.userResponse.options.length === 0) {
                  return false;
                }
              }
              break;
            case 1:
              if (question.userResponse.selectedDropdownOption === undefined) {
                return false;
              }
              break;
            case 2:
              if (question.userResponse.representationNumber === undefined) {
                return false;
              }
              break;
            case 3:
              if (question.subjectiveQuestion === null) {
                return false;
              } else if (question.userResponse.answerText.length > question.subjectiveQuestion.characterUpperLimit || question.userResponse.answerText.length < question.subjectiveQuestion.characterLowerLimit || question.userResponse.answerText.length === 0) {
                return false;
              }
              break;
            case 4:
              if (question.textboxQuestion === null) {
                return false;
              } else if (question.userResponse.answerText.length > question.textboxQuestion.characterUpperLimit || question.userResponse.answerText.length < question.textboxQuestion.characterLowerLimit || question.userResponse.answerText.length === 0) {
                return false;
              }
              break;
            case 5:
              if (question.files.length === 0) {
                if (question.userResponseDocumentACs !== undefined && question.userResponseDocumentACs.length === 0) {
                  return false;
                }
              }
              break;
          }
        }
      }
    }
    return true;
  }

  /**
   * To check if the option is selected or not
   * @param userResponseOptions User response options
   * @param checkop Check option
   * @param questionId Question id
   */
  checkOptionSelection(userResponseOptions: Array<OptionAC>, checkop: string, questionId?: string) {
    if (userResponseOptions !== undefined) {
      if (userResponseOptions.findIndex(x => x.optionText === checkop) !== -1) {
        if (this.strategicAnalysis.questions.find(x => x.id === questionId) !== undefined) {
          this.strategicAnalysis.questions.find(x => x.id === questionId).userResponse.other = '';
        }
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  /**
   * Upload question type file
   * @param event : file upload event
   * @param question : file upload question
   */
  fileChange(event, question: QuestionAC) {
    let invalidFilesCount = 0;
    let validFileCount = 0;
    const fileList: File[] = [];

    // todo: panding check count for user response
    const fileCount = event.target.files.length;
    if (fileCount <= Number(this.stringConstants.fileLimit)) {
      let showError = false;

      for (const file of event.target.files) {
        const selectedFile = file;
        let isFormatValid = false;

        const extention = selectedFile.name.split('.')[selectedFile.name.split('.').length - 1];
        const fileSize = (selectedFile.size / (1024 * 1024));
        if (fileSize >= Number(this.stringConstants.fileSize)) {
          showError = true;
          this.sharedService.showError(this.stringConstants.invalidFileSize);
        } else if (extention === this.stringConstants.exeFileExtention) {
          showError = true;
          this.sharedService.showError(this.stringConstants.invalidFileFormat);
        } else {

          if (question.fileUploadQuestion.isDocAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.wordType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }
          if (question.fileUploadQuestion.isGifAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.gifType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }
          if (question.fileUploadQuestion.isJpegAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.jpgType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }
          if (question.fileUploadQuestion.isPdfAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.pdfType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }
          if (question.fileUploadQuestion.isPngAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.pngType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }
          if (question.fileUploadQuestion.isPpxAllowed) {
            if (this.checkFileExtentionForFileUploadQuestionType(file.name, this.pptType)) {
              fileList.push(selectedFile);
              isFormatValid = true;
            }
          }

          if (!isFormatValid) {
            invalidFilesCount++;
          } else {
            validFileCount++;
          }
          isFormatValid = false;
        }
      }
      if (invalidFilesCount > 0 && validFileCount === 0) {
        this.sharedService.showError(this.stringConstants.invalidFile);
      } else if (!showError) {
        if (this.strategicAnalysis.questions.find(x => x.id === question.id) !== undefined) {
          if (fileList.length > 0) {
            fileList.forEach(x => {
              this.strategicAnalysis.questions.find(p => p.id === question.id).files.push(x);
            });

          }
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
  checkFileExtentionForFileUploadQuestionType(fileName: string, fileTypeCheck: string) {
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
      case this.ppxType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPpt(fileName);
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
      default:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsOtherFormat(fileName);
        break;
    }
    return isUploadedFormatMatched;
  }

  /**
   * Method to open document
   * @param documentPath: documnetPath
   */
  openDocument(documentPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documentPath) }])
    );
    window.open(url, '_blank');
  }

  /**
   * Method to download document
   * @param documentId: document Id
   */
  downloadDocument(documentId: string) {
    this.apiService.strategicAnalysesGetDocumentDownloadUrl(documentId).subscribe((result) => {
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
   * On previous button click
   */
  onPreviousClick() {
    this.isShowSamplingList.emit(false);
  }

  /**
   * Method to open delete confirmation dialogue
   * @param index: index
   * @param docId: document id
   * @param strategicId: strategicId analysis id
   */
  openDeleteModal(index: number, docId: string, questionId: string) {

    this.modalService.config.class = 'page-modal delete-modal';

    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.stringConstants.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.strategicAnalysis.questions.find(x => x.id === questionId).files.splice(index, 1);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
          }
        }
      }
    });
  }

}
