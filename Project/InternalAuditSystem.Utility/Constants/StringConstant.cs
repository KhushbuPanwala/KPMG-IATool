using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalAuditSystem.Utility.Constants
{
    public class StringConstant
    {
        public static string WorkProgramString = "Work Program";
        public static string WorkProgramrTeamString = "Work Program Team";
        public static string WorkProgramClientParticipantsString = "Work Program Client Participants";
        public static string RatingText = "Rating";
        public static string StrategicAnalysisString = "Strategic Analysis";
        public static string Question = "Question";
        public static string EmailIdFieldName = "EmailId";
        public static string ReportText = "Report";
        public static string AuditPeriodString = "{0} to {1}";
        public static string DateFormat = "dd-MM-yyyy";
        public static string DateFormatForFileName = "ddmmyyyyhhmmss";

        public static string DeleteAllLinkedStartegicAnalyses = "Please delete all the linked strategic analysis";

        public static string MinutesOfMeeting = "Minutes of Meeting";
        public static string RiskControlMatrixString = "RiskControlMatrix";
        public static string RcmSectorString = "RCM Sector";
        public static string RcmProcessString = "RCM Process";
        public static string RcmSubProcessString = "RCM Sub Process";
        public static string DistributorText = "Distributor";
        public static string ReportObservationText = "Report Observation";
        public static string Yes = "Yes";
        public static string No = "No";
        public static string Pass = "Pass";
        public static string Fail = "Fail";
        public static string ObservationString = "Observation";
        public static string MomPdfFilePath = "Mom/MomPdf";
        public static string MomPdfFileName = "MomPdf";
        public static string MomHeaderPdfFileName = "MomHeader";
        public static string MomHeaderPdfFilePath = "Mom/MomHeader";
        public static string FileExtension = ".html";
        public static string PuppeteerText = "puppeteer";
        public static string PdfFileDirectory = "puppeteer";
        public static string PdfFileExtantion = ".pdf";
        public static string PdfContentType = "application/pdf";
        public static string PdfFilePath = "Report Observation";
        public static string NotMatchedViewText = "does not match any available view";
        public static string ClosedStatusString = "Closed";
        public static string CompletedStatusString = "Completed";
        public static string PendingStatusString = "Pending";
        public static string InProgressStatusString = "InProgress";
        public static string InternalTypeString = "Internal";
        public static string ExternalTypeString = "External";
        public static string AuditPlanActiveStatusString = "Active";
        public static string AuditPlanUpdateStatusString = "Update";
        public static string TeamString = "Team";
        public static string ClientString = "Client Participant";
        public static string SuperadminTypeString = "Superadmin";
        public static string ProcessText = "Process";
        public static string SubProcessText = "Sub Process";
        public static string AuditPlanText = "Audit Plan";
        public static string RiskAssessmentString = "Risk Assessment";
        public static string DisplayCountryNameFilterKey = "startsWith(Name, '";
        public static string AddCurrentAdminUser = "In strategic analysis team, adding EP/EM who is creating strategic analysis";
        public static string CompletedAllSurvey = "You have completed all the survey assigned to you!";

        #region General
        public static string CurrentUserIdKey = "CurrentUserId";
        public static string CurrentUserRoleKey = "CurrentUserRole";
        public static string HttpGetMethodKey = "GET";
        public static string ParamEntityId1 = "selectedEntityId";
        public static string ParamEntityId2 = "entityId";
        #endregion

        #region Execptions
        public static string NoRecordMessage = "No {0} exists";
        public static string DuplicateDataMessage = "{0} : '{1}' already exists";
        public static string DuplicateCountryNameMessage = "{0} : '{1}' already exists in '{2}'";
        public static string DuplicateRatingNameMeessage = "Rating Name Exists";
        public static string DuplicateWorkProgramMessage = "Work Program already exists for {0}.";
        public static string DuplicateRCMMessage = "Failed to delete as it is used under {0} section";
        public static string DeleteValidationMessage = "{0} already exists for {1}.";
        public static string InvalidDataMessage = "Invalid data cannot be saved";
        public static string DataCannotBeDeletedMessage = "Failed to delete as it is used under {0} section";
        public static string SuperAdminDeleteMessage = "Super Admin can not be deleted";
        public static string UserResponsePresentMessage = "Submitted user response owner can not be deleted";
        public static string InvalidBulkDataMessage = "No {0} : {1} exists";
        public static string MaxLengthDataMessage = "{0}: maximum length exists";
        public static string RequiredDataMessage = "{0} is Required";
        public static string InvalidFileMessage = "Invalid File uploaded";
        public static string ClosedEntityRestrictionMessage = "Unable to perform any action as current entity is closed";
        public static string NotExistMessage = "{0} does not exist";
        public static string InvalidFileFormateMessage = "Please do not upload exe file";
        public static string InvalidFilSizeMessage = "Please do not upload file more than 12MB size";
        public static string InvalidFileLimitExceedMessage = "Only 10 files allows to upload";
        public static int FileLimit = 10;
        public static int FileSize = 12;
        public static string EXEFile = "exe";
        #endregion

        #region Label Or FieldName
        //Module Name or Menu name 
        public static string AuditPlanModuleName = "Audit Plan";
        public static string AuditTypeFieldName = "Audit Type";
        public static string AuditCategoryFieldName = "Audit Category";
        public static string AuditProcessFieldName = "Audit Process";
        public static string AuditSubProcessFieldName = "Audit Sub Process";
        public static string EntityTypeFieldName = "Auditable Entity Type";
        public static string EntityCategoryFieldName = "Auditable Entity Sector";
        public static string AuditableEntityModuleName = "Auditable Entity";
        public static string RelationshipTypeFieldName = "Relationship Type";
        public static string EntityRelationModuleName = "Auditable Entity Relationship";
        public static string LocationFieldName = "Location";
        public static string RegionFieldName = "Region";
        public static string PrimaryGeopgraphicalAreaModuleName = "Primary Geographical Area";
        public static string CountryModuleName = "Country";
        public static string StateModuleName = "State";
        public static string divisionFieldName = "Division";
        public static string ObservationModuleName = "Observation";
        public static string ObservationTableModuleName = "Observation Table";
        public static string ObservationCategoryModuleName = "Observation Category";
        public static string ReportObservationModuleName = "Report Observation";
        public static string ReportObservationFirstPersonModuleName = "Report Observation First Person Responsible";
        public static string MomModuleName = "Minutes of Meeting";
        public static string WorkprogrameModuleName = "Work Program";
        public static string DistributionModuleName = "Distribution List";
        public static string ReportReviewerListModuleName = "Report Reviewer List";
        public static string ReportObservationAuthortModuleName = "Report Observation Auhtor";
        public static string EngagementManager = "Engagement Manager";
        public static string AddComma = ", ";
        public static string AcmReportReviewerModuleName = "ACM Report Reviewer";


        #endregion

        public static string ExcelFileExtantion = ".xlsx";
        public static string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public static string CountryStateFilePath = "\\ClientApp\\Country-state.json";
        public static string PPTFileExtantion = ".pptx";
        public static string PPTContentType = "application/vnd.openxmlformats-officedocument.presentationml.presentation";
        public static string JPEGFileExtantion = "jpeg";
        public static string JPGFileExtantion = "jpg";
        public static string PNGFileExtantion = "png";
        public static string GIFFileExtantion = "gif";

        #region Module Name
        //regex which match tags
        public static System.Text.RegularExpressions.Regex CheckHTMLRegex = new System.Text.RegularExpressions.Regex("<[^>]*>");
        public static string HTMLSpace = "#160;";
        public static string LessThan = "lt;";
        public static string LessThanSymbole = "<";
        public static string GreaterThan = "gt;";
        public static string GreaterThanSymbole = ">";
        public static string DoubleQuatos = "&#34;";
        public static string DoubleQuatosSymbole = "\"\"";
        public static string AmpSymbole = "&amp;";
        public static string AmpersonSymbole = "&";
        public static string BlankSpace = " ";
        public static string DateTimeFormat = "dd-MM-yyyy HH:mm:ss";
        public static string DateTimeFormatWithoutTime = "dd-MM-yyyy";
        public static string DateTimeFormatOnlyTime = "HH:mm";
        public static string RatingModule = "Rating";
        public static string DistributorModule = "Distributor";
        public static string MomModule = "Mom";
        public static string MainPointModule = "MainPoint of discussion";
        public static string SubPointModule = "SubPoint";
        public static string MomUserMappingModule = "Mom Person responsible";
        public static string MomTeamModule = "Mom Team and Client participant";
        public static string ReportModule = "Report";
        public static string ReportCommentModule = "Report Comment";
        public static string ReportReviewerModule = "Report Reviewer";
        public static string ReportDistributorModule = "Report Distributors";
        public static string ReportObservationModule = "Report Observation";
        public static string ReportObservationMemberModule = "Report Observation Member";
        public static string ReportObservationTableModule = "Report Observation Table";
        public static string RemoveHtmlTagsRegex = "<[^>]*>";
        public static string RcmModuleName = "RCM";

        #endregion

        #region Azure configuration selection
        #region keyName
        public static string AzureAdSelectionKeyName = "AzureAd";
        public static string InstanceSelectionKeyName = "Instance";
        public static string TenantIdSelectionKeyName = "TenantId";
        public static string ClientIdSelectionKeyName = "ClientId";
        public static string ClientSecretSelectionKeyName = "ClientSecret";
        public static string ScopeSelectionKeyName = "Scope";
        public static string GrantType = "client_credentials";
        public static string authTokenUrl = "/oauth2/v2.0/token";
        public static string InvalidTokenCode = "InvalidAuthenticationToken";
        public static string RequestResourceCode = "Request_ResourceNotFound";

        public static string DisplayNameFilterKey = "startsWith(displayName, '";
        public static string FirstNameFilterKey = "startsWith(givenName,'";
        public static string SurnameFilterKey = "startsWith(surname, '";
        public static string OrConditionKey = "') OR ";
        public static string EndFilertString = "')";
        public static string GuestMailIdFilterKey = "mail eq '";
        public static string GuestQueryEndString = "'";

        #endregion

        #region ParamName
        public static string ClientIdParamName = "client_id";
        public static string ClientSecretParamName = "client_secret";
        public static string ScopeParamName = "scope";
        public static string GrantTypeParamName = "grant_type";
        #endregion

        #region Exception Message
        public static string TokenRequestFailureMessage = "Token request Failed";
        public static string EmptyTokenContentMessage = "Empty token response received";
        #endregion
        #endregion

        #region AuditPlan
        public static string NotSpecifiedMessage = "Not specified";
        public static string EmptyGuid = "00000000-0000-0000-0000-000000000000";
        #endregion

        #region Bulk Upload Observation
        public static string HeadingText = "Heading";
        public static string BackgroundText = "Background";
        public static string ObservationTypeText = "Observation Type";
        public static string IsRepeatedText = "Is Repeated";
        public static string RootCauseText = "RootCause";
        public static string ImplicationText = "Implication";
        public static string DispositionText = "Disposition";
        public static string StatusText = "Status";
        public static string ObservationsText = "Observations";
        public static string RecommendationText = "Recommendation";
        #endregion

        #region Bulk upload RCM
        public static string sectorText = "Sector";
        public static string processText = "Process";
        public static string subProcessText = "Sub Process";
        public static string riskNameText = "Risk Name";
        public static string riskDescriptionText = "Risk Description";
        public static string controlCategoryText = "Control Category";
        public static string controlTypeText = "Control Type";
        public static string controlObjectiveText = "Control Objective";
        public static string controlDescriptionText = "Control Description";
        public static string natureOfControlText = "Nature Of Control";
        public static string antiFraudControlText = "Anti-Fraud Control";
        public static string riskCategoryText = "Risk Category";
        #endregion

        #region UserDesignations
        public static string CodHeadDesignation = "Co-Head";
        public static string PartnerDesignation = "Partner";
        public static string DirectorDesignation = "Director";
        public static string AsscDirectorDesignation = "Associate Director";
        public static string ManagerDesignation = "Manager";
        public static string TechDirectorDesignation = "Technical Director";
        public static string AsstMangagerDesignation = "Assistant Manager";
        public static string ConsultantDesignation = "Consultant";
        public static string AsscConsultantDesignation = "Associate Consultant";
        public static string AnalystDesignation = "Analyst";
        public static string ExecutiveDesignation = "Executive";
        public static string StaffAccountantDesignation = "Staff-Accountant";
        #endregion

        #region PPT Message
        public static string SrNoText = "Sr. No.";
        public static string toText = " to ";
        #endregion

        #region Template File Name
        public static string TemplateFolder = "Templates";
        public static string WWWRootFolder = "wwwroot";
        public static string ImageFolder = @"\Images\";
        public static string ReportPPTFolder = "ReportPPT"; // report ppt folder in azure for preview 
        public static string TemporaryPPTFolder = @"TempPPT\";
        public static string TemporaryFile = "_temp";
        public static string TemporaryTableFile = "_table";
        public static string TemporaryObservationFile = "Observation_";
        public static string AuditReportTemplate = "Audit Report Template";
        public static string ObservationsTemplate = "Observations Template";
        public static string LastSlideTemplate = "LastSlideTemplate";
        public static string ObservationTemplate = "Observation Management Template";
        public static string ReportObservationTemplate = "Report Observation Management Template";
        public static string ACMTemplate = "ACM Template";
        #endregion

        #region Rcm
        public static string StrategicControlCategory = "Strategic";
        public static string OperationalControlCategory = "Operational";
        public static string FinancialControlCategory = "Financial";
        public static string ComplianceControlCategory = "Compliance";

        public static string ManualControlType = "Manual";
        public static string AutomatedControlType = "Automated";
        public static string SemiAutomatedControlType = "SemiAutomated";


        public static string PreventiveNatureOfControl = "Preventive";
        public static string DetectiveNatureOfControl = "Detective";
        #endregion

        #region ACM Module
        public static string ACMPresentationString = "ACM Representation";
        public static string ACMTableModuleName = "ACM Table";
        #endregion

        public static string StrategyHeaderPdfFileName = "StrategicAnalysisHeader";
        public static string StrategyHeaderPdfFilePath = "StrategicAnalysis/StrategicAnalysisHeader";
        public static string StrategyPdfFileName = "StrategicAnalysisPdf";
        public static string StrategyPdfFilePath = "StrategicAnalysis/StrategicAnalysisPdf";
    }
}
