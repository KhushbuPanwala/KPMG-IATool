export class StringConstants {
  constructor() { }

  // Region - Regex for validations
  static decimalNumberRegex = '^[0-9]+([.][0-9]{1,2})?$';
  static naturalNumberRegex = '^[0-9]+$';
  static emailRegex = '^([a-zA-Z0-9\-._]+)@([a-zA-Z0-9\-.]+)\.([a-zA-Z]{2,63})$';
  static nameRegex = '^[a-zA-Z\' ]+$';
  static commonInputRegex = '^[a-zA-Z0-9\-\'!:;,._@(){}[\\]/& ]+$';
  static countryStateRegex = '^[a-zA-Z\-\'()[\\]/ ]{2,}$';
  static alphanumericRegex = '^[a-zA-Z0-9 ]+$';

  // Entity Selection
  entitySelectionPlaceHolder = 'Select Auditable Entity';

  // Bread crum b section
  dashboardBreadCrumbTitle = 'Dashboard';
  masterBreadCrumbTitle = 'Master';
  auditPlanBreadCrumbTitle = 'Audit Plan';
  breadCrumbText = 'breadCrumb';

  // Copyright section
  copyRightTitle = '©2020 KPMG';

  // Modules main path
  strategicRoutePathName = 'strategic-analysis';

  // Add page mom
  momAddPageTitle = 'Add Minutes of Meeting';
  auditableEntityTitle = 'Auditable Entity';
  dateLabel = 'Date';
  timeLabel = 'Time';
  closureMeetingLabel = 'Closure Meeting Date';
  agendaTitle = 'Agenda';
  teamLabel = 'Team';
  selectSurveyLabel = 'Select Survey';
  nameLabel = 'Name';
  designationLabel = 'Designation';
  outlookToolTip = 'Outlook';
  pdfToolTip = 'Pdf';
  backToolTip = 'Back';
  removeToolTip = 'Remove';
  addToolTip = 'Add';
  addStratagicAnalysisToolTip = 'Add Stratagic Analysis';
  srNo = 'Sr No';
  pointOfDiscussionTitle = 'Point Of Discussion';
  statusTargetDateTitle = 'Status & Target Date';
  personResponsibleTitle = 'Person Responsible';
  textAreaPlaceHolder = 'Following points were briefed by KPMG team at the start';
  bottomPlacement = 'bottom';
  saveButtonText = 'Save';
  addButtonText = 'Add';
  updateButtonText = 'Update';
  saveAsNewVersionText = 'Save As New Version';
  toText = 'To';
  showingResults = 'Showing 1 - 10 of 200';
  workProgramRequiredMessage = 'Please select Work Program';
  agendaRequiredMessage = 'Please enter Agenda';
  teamRequiredMessage = 'Please add Team';
  clientParticipantRequiredMessage = 'Please add Client Participant';
  alreadyAddedUserMessage = 'This user is already added';
  deleteRowMessage = 'Can\'t delete the row when there is only one row';
  warningMessage = 'Warning';
  momStartTimeInvalidMessage = 'Please select valid start time';
  momEndTimeInvalidMessage = 'Please select valid end time';
  momTimeCompareMessage = 'End time can not be less then start time';
  blankUserAddedWarningMessage = 'You can not add blank details';
  personResposibleWarningMessage = 'You can not add more then 2 persons';
  mainPointRequiredMessage = 'Please enter main Point';
  subPointRequiredMessage = 'Please enter sub Point';
  statusRequiredMessage = 'Please select status';
  targetDateRequiredMessage = 'Please select target date';
  personRequiredMessage = 'Please select person resposible';
  auditableEntityId = 'fa12e57b-5ca6-4512-b49a-428d0365b91b';
  userId = '9513c730-c452-4e3a-9ffe-e1f97b8a585e';
  workProgramId = '686b13b3-ca06-4648-83ee-5aa5840e8d3c';
  selectItem = '--Select item--';

  // List page mom
  momListPageTitle = 'Minutes of Meeting';
  searchText = 'Search';
  excelToolTip = 'Excel';
  editToolTip = 'Edit';
  deleteToolTip = 'Delete';
  id = 'Id';
  defaultEmailId = 'niriksha@promactinfo.com';
  mailtoString = 'mailto:';
  subjectString = 'subject';
  bodyString = 'body';
  searchValueText = 'searchValue';
  pageItemsText = 'pageItems';
  pdfApi = '/api/moms/downloadMomPDF?momId=';
  strategicPDFApi = '/api/strategicAnalyses/downloadStrategicPDF?strategicId=';
  exportToExcelApi = '/api/moms/ExportMoms?entityId=';
  exportToExcelObservationApi = '/api/observationsManagement/exportObservations?entityId=';
  exportToExcelAuditTypeApi = '/api/AuditTypes/exportAuditTypes?entityId=';
  exportToExcelAuditCategoryApi = '/api/auditCategories/exportAuditCategories?entityId=';
  exportToExcelAuditProcessApi = '/api/auditProcesses/exportAuditProcesses?entityId=';
  exportToExcelAuditSubProcessApi = '/api/auditProcesses/exportAuditSubProcesses?entityId=';
  exportToExcelAuditPlanApi = '/api/auditPlans/exportAuditPlans?entityId=';
  exportToExcelAuditPlanProcessApi = '/api/auditPlans/exportAuditPlanProcess?entityId=';
  exportToExcelAuditableEntityCategoryApi = '/api/entityCategories/exportEntityCategories?entityId=';
  exportToExcelAuditableEntityTypeApi = '/api/entityTypes/exportEntityTypes?entityId=';
  exportToExcelAuditableEntityDivisionApi = '/api/divisions/exportEntityDivisions?entityId=';
  exportToExcelAuditableEntityRelationShipTypeApi = '/api/relationshipTypes/exportEntityRelationShipTypes?entityId=';
  exportToExcelAuditableEntityRegionApi = '/api/regions/exportEntityRegions?entityId=';
  exportToExcelAuditableEntityLocationApi = '/api/locations/exportEntityLocations?entityId=';
  exportToExcelObservationCategoryApi = '/api/observationCategories/exportObservationCategories?entityId=';
  exportToExcelCountryApi = '/api/countries/exportCountries?entityId=';
  exportToExcelStateApi = '/api/states/exportStates?entityId=';
  exportToExcelAuditTeamApi = '/api/auditTeams/exportAuditTeams?entityId=';
  exportToExcelAuditClientParticipantApi = '/api/clientParticipants/exportClientParticipants?entityId=';
  exportToExcelRcmProcessApi = '/api/rcmProcess/exportRcmProcess?entityId=';
  exportToExcelRcmSubProcessApi = '/api/rcmSubProcess/exportRcmSubProcess?entityId=';
  exportToExcelRcmSectorApi = '/api/rcmSector/exportRcmSector?entityId=';
  exportToExcelRcmMainApi = '/api/riskControlMatrixes/exportRiskControlMatrixes?entityId=';
  exportToExcelAcmApi = '/api/acm/exportAcm?entityId=';
  exportToExcelReportApi = '/api/Reports/exportReports?entityId=';
  offsetString = '&offset=';
  entityParamString = '&entityId=';
  timeOffSet = '&timeOffset=';
  // List page distributor
  distributorListPageTitle = 'Distribution List';

  // Observation Section
  observationManagementTitle = 'Observation Management';
  observationTabTitle = 'Observation';
  observationFiles = 'ObservationFiles';
  managementCommentsTitle = 'Management Comments';
  wordToolTip = 'Word';
  powerPointToolTip = 'PowerPoint';
  headingLabel = 'Heading';
  backgroundLabel = 'Background';
  rootCauseLabel = 'Root Cause';
  auditPlanLabel = 'Audit Plan';
  processLabel = 'Process';
  subProcessLabel = 'Sub Process';
  typeLabel = 'Type';
  categoryLabel = 'Category';
  repeatObservationLabel = 'Repeat Observation?';
  implicationTitle = 'Implication';
  recommendationTitle = 'Recommendation';
  yes = 'Yes';
  no = 'No';
  addedLabel = 'Added';
  managementResponseTitle = 'Management Response';
  conclusionTitle = 'Conclusion';
  personResponsibleLabel = 'First Person Responsible';
  personLabel = 'Person Responsible';
  targetDateTitle = 'Target Date';
  linkedObservationTitle = 'Linked Observation';
  statusTitle = 'Status';
  completedStatusTitle = 'Completed';
  dispositionTitle = 'Disposition';
  auditorTitle = 'Auditor';
  downloadToolTip = 'Download File';
  enterKeyText = 'Enter';
  tabKeyText = 'Tab';
  selectedAuditPlanForDemo = 'Audit Plan (2020-2021)';
  completedStatusString = 'Completed';
  headingRequiredMessage = 'Please enter heading';
  backgroundRequiredMessage = 'Please enter background';
  observationRequiredMessage = 'Please enter observation';
  rootCauseRequiredMessage = 'Please enter rootCause';
  implicationRequiredMessage = 'Please enter implication';
  recommendationRequiredMessage = 'Please enter  recommendation';
  auditPlanRequiredMessage = 'Please enter audit plan';
  processRequiredMessage = 'Please select process';
  subProcessRequiredMessage = 'Please select subProcess';
  observationStatusRequiredMessage = 'Please select status';
  observationDispositionRequiredMessage = 'Please select disposition';
  addColumn = 'Add column';
  addRow = 'Add row';
  deleteRow = 'Delete row';
  deleteColumn = 'Delete column';
  serialNumber = 'Sr. No.';
  observationApiPath = '/api/ObservationsManagement/uploadFiles';

  // Rating add page section
  ratingLabel = 'Rating';
  legendLabel = 'Legend';
  scoreLabel = 'Score';
  qualitativeFactors = 'Qualitative Factors';
  quantitativeFactors = 'Quantitative Factors';
  redColor = 'Red';
  yellowColor = 'Yellow';
  greenColor = 'Green';

  // Rating list page section
  scoreTitle = 'Score';

  // RCM sector page section
  rcmSectorTitle = 'Sector';

  // RCM process page section
  rcmProcessTitle = 'Process';

  // work program rcm section
  observationAddMessage = 'You have to save rcm first then add observation';

  // Audit team section
  auditTeamLabel = 'Audit Team';
  emailLabel = 'Email';
  departmentLabel = 'Department';
  syncToolTip = 'Sync Data';
  syncInProgressToolTip = 'Syncing In Progress';
  noRecordFoundMessage = 'No matched record found';
  syncSucessfullMessage = 'Audit Team synced successfully';



  // Generic http service
  serverError = 'Server Error';

  // Delete confirmation section
  deleteTitle = 'Delete Confirmation';
  confirmationMessage = 'Are you sure you want to delete?';
  ofText = 'of';
  showingText = 'Showing';

  // Per page items for entity list
  pageItems = [
    { noOfItems: 10, item: '10 items per page' },
    { noOfItems: 20, item: '20 items per page' },
    { noOfItems: 30, item: '30 items per page' },
    { noOfItems: 40, item: '40 items per page' }
  ];
  // Per page items for entity list
  smallPageItems = [
    { noOfItems: 5, item: '5 items per page' },
    { noOfItems: 10, item: '10 items per page' },
    { noOfItems: 15, item: '15 items per page' },
    { noOfItems: 20, item: '20 items per page' }
  ];

  showNoDataText = 'No record found';

  // Toaster Message
  recordDeletedMsg = 'Record deleted successfully';
  recordAddedMsg = 'Record added successfully';
  recordUpdatedMsg = 'Record updated successfully';
  recordExistMsg = 'Record already exist';
  userExistMsg = 'User already exist';
  selectObservationMsg = 'Please select Observation';
  invalidMessage = 'Invalid input';
  requiredMessage = 'Required';
  somethingWentWrong = 'Something went wrong';
  maxLengthExceedMessage = 'Maximum limit exceed for this field';
  patternValidatorMessage = 'Please check input of region';
  min4DigitRequired = 'Minimum 4 digits required';
  headingmaxLengthExceedMessage = 'Maximum limit exceed for heading.';
  recordExportedMsg = 'Record exported successfully';
  recordPPTdMsg = 'Presentation generated successfully';
  recordPdfMsg = 'pdf generated successfully';
  requiredDataMissingMsg = 'Please add required Data';
  invalidDataMsg = 'Not able to save data since some fields have blank or invalid value';
  invalidSelection = 'Please select Audit Plan, Process, SubProcess"';
  invalidFileFormat = 'Please do not upload exe file';
  invalidFile = 'File format invalid';
  fileLimit = '10';
  fileSize = '12';
  invalidFileSize = 'Please do not upload file more than 12MB size';
  fileLimitExceed = 'Only 10 files allows to upload';
  // RCM sub process section
  rcmSubProcessTitle = 'Sub Process';
  searchRcmText = 'Search Control Description';

  // Client participants section
  clientParticipantsText = 'Client Participants';

  // Work Program add page
  auditTitle = 'Audit';
  addMomTitle = 'Add Mom';
  rcmTitle = 'RCM';
  workProgramName = 'Work Program Name';
  auditTitleText = 'Audit Title';
  planScopeTitle = 'Plan/Scope';
  workPapers = 'Workpapers';
  noFileChosen = 'No file chosen';
  chooseFileText = 'Choose file';
  filesAdded = ' Added';
  filesPreviouslyAdded = ' file(s) previously added';
  newFilesAdded = ' new file(s) added';
  fileNameText = 'filename.doc';
  workProgramTitle = 'Work Program';
  auditPeriodTitle = 'Audit Period';
  viewFiles = 'View Files';
  statusButtonReopenText = 'Reopen';
  statusButtonCloseText = 'Close';
  workPaperFiles = 'workPaperFiles';
  workProgramApiPath = '/api/workprograms';
  workProgramExportApiPath = '/api/WorkPrograms/export-workprogram?entityId=';
  workProgramTimeOffSet = '&timeOffset=';

  // Distribution List
  updateDistributionList = 'Update Distribution List';
  selectDistributor = 'Select Distributor';

  // Sampling
  samplingAddLabel = 'Sampling Compliance';
  samplingAddMessage = 'You have to save rcm first then add sampling';
  samplingModuleTitle = 'Sampling';
  addSamplingToolTip = 'Add Sampling';
  responseButtonText = 'Sampling Response';
  samplingRoutePathName = 'sampling';

  // Strategic Analysis section
  strategicAnalysis = 'Strategic Analysis';
  surveyTitle = 'Title';
  dateOfCreation = 'Date Of Creation';
  dateOfModification = 'Date of Modification';
  questions = 'Questions';
  responses = 'Responses';
  versionTitle = 'Version';
  addUserInExistingArea = 'Please enter user';
  financeSurveyTitle = 'financeSurveyTitle';
  strategicAnalysisApiPath = '/api/StrategicAnalyses';
  saveEmailAttachmentApiPath = '/api/StrategicAnalyses/user-response/save/email-attachments';
  userResponseFileUploadApiPath = '/api/StrategicAnalyses/user-responses';
  strategicAnalysisFiles = 'Files';
  userResponseFileFieldName = 'files';
  alreadySubmitted = 'Already submitted';
  responsePresent = 'You can not change this question since it has user response';
  noDeleteResponsePresent = 'You can not delete this question since it has user response';
  noDuplicateResponsePresent = 'You can not duplicate this question since it has user response';
  noQuestionAddResponsePresent = 'You can not add question to this survey since it already has user response';
  emailAttachmentValidityError = 'Please submit response before uploading email attachments';
  approved = 'Approved';
  approve = 'Approve';
  strategyAnalysisIdKey = 'strategyAnalysisId';
  finalFileTypeQuestionApiPath = '/api/StrategicAnalyses/final/file-type-question-response/';
  draftFileTypeQuestionApiPath = '/api/StrategicAnalyses/draft/file-type-question-response/';


  // Strategic Analysis admin add section
  surveryFormField = 'Title';
  emailId = 'E-mail ID';
  messageField = 'Message';
  createSurvey = 'Create';
  namePlaceholder = 'Type name here';
  designationPlaceholder = 'Type designation here';
  emailPlaceholder = 'Type email here';
  surveyTitleRequiredMessage = 'Please enter Survey Title';
  surveyEntityRequiredMessage = 'Please enter Auditable Entity';

  // Strategic Analysis admin survey section
  surveyFinance = 'Finance Survey of the 2020';
  dragDrop = 'Drag and Drop Question';
  dropDown = 'Dropdown';
  multipleChoice = 'Multiple Choice';
  ratingScale = 'Rating Scale';
  subjective = 'Subjective';
  fileUpload = 'File Upload';
  textField = 'Text Field';
  checkBox = 'Checkbox';
  surveyBefore = 'Have you seen this survey before?';
  geoGraphical = 'Which type of geographical area your business located? ';
  asia = 'Asia';
  pacific = 'Pacific';
  africa = 'Africa';
  europe = 'Europe';
  previousExperience = 'Rate your previous year experience';
  surveyQuestion = 'How many adults (above the age of 18) are in your company?';
  financialYear = 'Describe your financial aim in 2021 in detail';
  uploadFinancial = 'Upload financial report of 2020';
  choose = 'Choose';
  uploadFile = 'Upload File';
  undefinedVariable = 'undefined';

  // Error messages for question add functionality
  questionRequiredMessage = 'Please enter Question';
  optionRequiredMessage = 'Please add Option';

  // Report list section
  reportManagemenTitle = 'Report Management';
  reports = 'Reports';
  reportTitle = 'Report Title';
  reportListTitle = 'Report List';
  ratingTitle = 'Rating';
  generateReportTitle = 'Generate Report';
  noOfObservationTitle = 'No. of Observation';
  stageTitle = 'Stage';
  periodTitle = 'Period';
  sendForReview = 'Send For Review';
  sendForApproval = 'Send For Approval';
  auditPeriodStartDate = 'Audit Period Start Date';
  auditPeriodEndDate = 'Audit Period End Date';
  commentsTitle = 'Comments';
  selectReviewer = 'Select Reviewer';
  attachmentText = 'Attachment';
  saveNextButtonText = 'Save & Next';
  saveCloseButtonText = 'Save & Close';
  uploadFileTitle = 'Upload File';
  teamMasterLabel = 'Team Master';
  addOperationText = 'Add';
  editOperationText = 'Edit';
  viewOperationText = 'View';
  linkText = 'Report Link';
  // Observation List
  observationListLabel = 'Observation List';
  // Comment history
  observationTitle = 'Observation Title';

  // General
  dropdownDefaultValue = 'Select';
  startDate = 'Start Date';
  endDate = 'End Date';
  dateFormat = 'dd-MM-yyyy';
  datePipe = 'en-US';
  invalidDate = 'Inavalid Date';
  notSpecifiedLabel = 'Not Specified';
  processDropdownDefaultValue = 'Select Item';
  backToListPageTooltipMessage = 'Back to list page';
  createNewVersion = 'Create new version';
  trueString = 'true';
  unauthorizedPath = '401';
  pageNotFoundPath = '404';
  loggedInUser = 'Profile';
  // General - file upload extenstions
  allowedFile = 'Allowed file types';
  docText = 'DOC';
  docxText = 'DOCX';
  pngText = 'PNG';
  ppxText = 'PPX';
  jpegText = 'JPEG';
  jpgText = 'JPG';
  gifText = 'GIF';
  pdfText = 'PDF';
  pptText = 'PPT';
  pptxText = 'PPTX';
  xlsx = 'XLSX';
  xls = 'XLS';
  csv = 'CSV';
  zipType = 'ZIP';
  mp3Type = 'MP3';
  mp4Type = 'MP4';
  svgType = 'SVG';
  closedEntityRestrictionMessage = 'Unable to perform any action as current entity is closed';
  deletedEntityRestrictionMessage = 'Current Entity is no longer exist. Please refresh page to do any action';
  allowedFileTypeQuestionText = 'Allowed file types - ';

  docLimit = 'Maximum document size limit is 100 MB';
  dots = 'dots';
  wordFileType = 'work-file';
  pptFileType = 'ppt-file';
  pdfFileType = 'pdf-file';
  otherFileType = 'other-file';
  selectedEntityKey = 'selectedEntityId';


  // Audit process section
  auditProcessLabel = 'Audit Process';
  auditSubProcessLabel = 'Audit Sub Process';
  auditAddProcesses = 'Add Process';
  auditSubAddProcesses = 'Add Sub Process';
  processNameLabel = 'Process Name';
  subProcessesName = 'Sub Process Name';
  parentProcessName = 'Parent Process Name';
  parentProcess = 'Parent Process';
  scopeBasedOnLabel = 'Scope Based On';
  scopeLabel = 'Scope';

  // Auditable entity type list
  auditableEntityTypeLabel = 'Auditable Entity Type';

  // Auditable entity category
  auditableEntityCategoryLabel = 'Auditable Entity Sector';
  reviewerCommentsTitle = 'Reviewer Comment';
  column1 = 'Column 1';
  loremIpsumText = 'Lorem Ipsum';
  addTableTitle = 'Add Table';
  selectRCM = 'Select RCM';
  riskDescription = 'Risk Description';
  controlDescription = 'Control Description';
  controlCategory = 'Control Category';
  controlType = 'Control Type';
  natureOfControl = 'Nature Of Control';
  antiFraudControl = 'Anti-Fraud Control';


  // Strategic Analysis create survey multiple choice modalpopup
  question = 'Question';
  answerChoice = 'Answers (Choices)';
  relatedAnswers = 'Related Answers';
  selectRelatedAnswer = 'Select related answer';
  addOther = 'Add \'Other\' field';
  cancelButtonText = 'Cancel';

  // Strategic Analysis create survey dropdown modalpopup
  answerDropDown = 'Answers (Dropdown Options)';

  // Strategic Analysis create survey rating scale modapopup
  representation = 'Representation';
  scale = 'Scale';
  labels = 'Labels (Optional)';
  smileyAlt = 'smiley';

  // Strategic Analysis create survey subjective
  characterLimit = 'Character Limit Between';
  characterLimitRequired = 'Please enter character limit';
  characterText = 'character';
  guidanceText = 'Guidance (Optional)';
  guidancePlaceholder = 'You are supposed to write your honest answer.';
  maxLimit = 'Maximum character limit is 1000';
  minNumber = '1';
  maxNumber = '999';
  maxCharecterAllowed = '256';
  characterMaxLimExceed = 'Maximum character limit exceed';
  characterMinLimExceed = 'Minimum character limit exceed';

  // Strategic Analysis create survey textfield
  maxLimitText = 'Maximum character limit is 500';
  maxNumberText = '499';

  // Strategic Analysis Checkbox
  countriesText = 'Countries';
  answerCheckbox = 'Answers (Checkbox Options)';
  blankOptionErrorText = 'Can not add blank option';
  otherSpecify = 'Other (Please specify)';

  // Strategic Analysis survey response
  nameClient = 'Name of Client';
  engagementPartner = 'Engagement Partner';
  engagementManager = 'Engagement Manager';
  detailsOportunity = 'Details of Opportunity';
  estimatedValue = 'Estimated Value of Opportunity';
  surveyResponseText = 'Survey Response';
  linearText = 'Linear';
  smileyText = 'Smiley';
  requiredUserResponse = 'Please fill this required question';
  formInvalidityMessage = 'Please submit all required question with valid answers';
  rcmDescriptionLabel = 'Rcm Description';

  // Strategic Analysis user list
  emailAttachment = 'Email Attachment';
  openText = 'Open';
  managerText = 'Manager';
  auditPlaceholder = 'Annual Audit Plan for Retail Banking';
  submitSurvey = 'Submit Survey';
  saveDraft = 'Save as Draft';
  strategicConfimMessage = 'Do you want to do another Strategic Analysis?';
  strategicNoNextSurveyMessage = 'You have completed all the survey assigned to you!';

  // Risk Control edit section
  riskCategory = 'Risk Category';
  controlObjective = 'Control Objective';
  testSteps = 'Test Steps';
  samplingMethodology = 'Sampling Methodology';
  testResults = 'Test Results';
  createObservationLabel = 'Create Observation';

  // Comment history section
  commentHistoryTitle = 'Comment History';
  authorName = 'Paul Ruggles';
  authorDate = '28-4-2020';
  authorTime = '4:44PM';
  relationshipTypeLabel = 'Relationship Type';

  // Division Title
  divisionLabel = 'Division';
  locationLabel = 'Location';

  // Sampling Methodology
  titleText = 'Title';
  nextButton = 'Next';
  previousButton = 'Previous';

  // Auditable entity list
  levelText = 'Level';
  versionsText = 'Versions';
  markClosed = 'Mark as Closed';
  activeText = 'Active';
  inActiveText = 'InActive';
  entityClosedMessage = 'Auditable entity closed successfully';
  entityActivatedMessage = 'Auditable entity activated successfully';

  // Audit Plan List section
  auditPlanTitleLabel = 'Audit Plan';
  versionLabel = 'Version';
  totalBudgetedHours = 'Total Budgeted Hours';
  hoursLabel = ' Hrs';
  planClosedMessage = 'Audit plan closed successfully';
  planActivatedMessage = 'Audit plan activated successfully';

  // Audit Plan masters
  auditTypeLabel = 'Audit Type';
  auditCategoryLabel = 'Audit Category';

  // audit plan navigation path
  auditPlanGeneralPath = 'audit-plan/general';
  auditPlanOverviewPath = 'audit-plan/overview';
  auditPlanPlanProcessPath = 'audit-plan/plan-process';
  auditPlanDocumentsPath = 'audit-plan/documents';
  auditableEntityDetailsPath = 'auditable-entity/details';
  auditPlanListPath = 'audit-plan/list';

  // Audit Plan general section
  generalTitle = 'General';
  auditPlanPlaceholder = 'Annual Audit Plan for Retail Banking';
  auditCycleLabel = 'Audit Cycle';
  dueDateLabel = 'Due Date';
  identifiedOnLabel = 'Identified On';
  overviewTitle = 'Overview';
  overviewAndBackgroundLabel = 'Overview and Background';
  totalBudgetHoursLabel = 'Total Budgeted Hours';
  financialYearLabel = 'Financial Year';
  budgetLabel = 'Budget';
  planProcessesTitle = 'Plan Processes';
  selectAuditProcessModalTitle = 'Select Audit Process';
  timeLineLabel = 'Timeline';
  minYearRequiredMessage = 'Minimum 4 digit is required';
  newVersionMsg = 'New version created successfully';
  planDeleteMsg = 'Audit plan deleted successfully';
  planAddMsg = 'Audit plan added successfully';
  planUpdateMsg = 'Audit plan updated successfully';

  // Audit Plan Process section
  planProcessListRequiredToolTipMessage = 'Atleast 1 process needs to be added to go the next section';
  selectProcessPlaceholder = 'Select Audit Process';
  selectSubProcessPlaceholder = 'Select Audit Sub Process';
  auditProcessAddMsg = 'Audit Process added successfully';
  auditProcessUpdateMsg = 'Audit Process updated successfully';
  auditProcessDeleteMsg = 'Audit Process deleted successfully';
  auditSubProcessExistMsg = 'Audit Sub Process already exists for this audit plan';

  // Audit Plan Documents
  maxDocumentRestrictionMsg = 'You can only add 1 file at a time';

  // Observation Management
  workingFile = 'Working File';
  uploadText = 'Upload';

  // Audit plan documents section
  documentsTitle = 'Documents';
  purposeTitle = 'Purpose';
  viewToolTip = 'View';
  saveAsText = 'Save As';
  existingVersionLabel = 'Existing Version';
  newVersionLabel = 'New Version';
  planDocumentAddMsg = 'Document added successfully';
  planDocumentUpdateMsg = 'Document updated successfully';
  planDocumentDeleteMsg = 'Document deleted successfully';

  // Region list
  regionText = 'Region';
  riskAssessmentDetailsTitle = 'Risk Assessment Details';
  riskAssesmentNote = 'The 5 most recent assessments related to the Auditable Entity in scope ordered by most recently completed are listed. This list does not include canceled assessments. Details of previous assessments conducted for the Auditable Entity are through the Assessment coverage report.';
  assessmentName = 'Assesment Name';
  year = 'Year';
  summaryOfAssessment = 'Summary of Assessment';
  attachment = 'Attachment';
  riskAssesmentApiPath = '/api/riskassessments';
  riskAssessmentDocumentFiles = 'riskAssessmentDocumentFiles';
  entityDocumentFiles = 'entityDocumentFiles';
  entityDocumentApiPath = '/api/entityDocuments';
  riskNoDocumentError = 'Document missing for risk assessment';
  entityDocumentFileUploadError = 'You can only add 1 file at a time';

  // Çountry list
  countryText = 'Country';

  // Çountry Add
  countryRequiredMessage = 'Please enter country';
  // State list
  stateText = 'Province/state';

  // state add
  stateRequiredMessage = 'Please enter state';

  // Relationship list section
  relationshipTitle = 'Relationship';
  selectEntityTitle = 'Select Entity';
  selectRelationshipType = 'Select Relationship Type';
  applyButton = 'Apply';
  entityTitle = 'Entity';
  primaryAreasTitle = 'Primary Geographical Areas';
  relationErrorOnAdd = 'Entity & Relation Type must be selected';
  noEntityErrorOnAdd = 'Entity must be selected';
  noRelationErrorOnAdd = 'Relation Type must be selected';

  // Auditable entity details section
  detailsTitle = 'Details';
  descriptionTitle = 'Description';
  classification = 'Classification';
  auditableEntityRequiredMessage = 'Atlease one record is mandatory to proceed further';
  auditableEntityExportApi = '/api/AuditableEntities/export-auditable-entity?timeOffset=';

  // ACM section
  acmTitle = 'ACM';
  managementResponseRequiredMessage = 'Please enter management response';
  aCMFiles = 'aCMFiles';
  acmApiUrl = '/api/Acm';

  // Audit committee section
  auditCommitteeTitle = 'Audit Committee';

  // Generate acm report section
  generateAcmReportTitle = 'Generate Audit Committee Report';

  // Audit advisory section
  auditAdvisoryTitle = 'Audit Advisory';
  selectAreaTitle = 'Select Area';
  yearTitle = 'Year';
  newVersionToolTip = 'New Version';
  downloadTemplate = 'Download Template';
  bulkUploadText = 'Bulk Upload';
  auditPlanVersion = 'Version: ';

  // bulk upload sheet
  observationUploadFiles = 'Files';
  uploadObservationsFileApiPath = '/api/ObservationsManagement/uploadObservations';
  instructionTemplateFile = 'Instruction';
  observationTemplateFile = 'Observation';
  excelFileExtention = '.xlsx';
  dataUploadSuccessMsg = 'Data uploaded sucessfully';
  selectFileMsg = 'Please select file for upload';
  fieldNameText = 'Field Name';
  fieldDescriptionText = 'Field Description';
  valueText = 'Examples of Valid Values';
  pleaseSelectExcelFileMsg = 'Please select Excel file only';
  addTableToolTip = 'Add/Edit Table';

  RiskControlMatrixFile = 'RCM';
  uploadRCMFileApiPath = '/api/RiskControlMatrixes/uploadRCMs';

  // Observation category section
  observationCategoryText = 'Observation Category';

  // Download PPT
  downloadObservationPPTApi = '/api/ObservationsManagement/generateObservationPPT?id=';
  downloadReportPPTApi = '/api/Reports/generateReportPPT?id=';
  downloadReportObservationPPTApi = '/api/ReportObservations/generateReportObservationPPT?id=';
  downloadACMPPTApi = '/api/Acm/generateACMPPT?id=';

  // file extention
  exeFileExtention = 'exe';

}



