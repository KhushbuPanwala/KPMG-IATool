/**
 * My Title
 * No description provided (generated by Swagger Codegen https://github.com/swagger-api/swagger-codegen)
 *
 * OpenAPI spec version: 1.0.0
 * 
 *
 * NOTE: This class is auto generated by the swagger code generator program.
 * https://github.com/swagger-api/swagger-codegen.git
 * Do not edit the class manually.
 */
import { OptionAC } from './optionAC';
import { QuestionAC } from './questionAC';
import { SamplingResponseStatus } from './samplingResponseStatus';
import { StrategicAnalysisStatus } from './strategicAnalysisStatus';
import { UserAC } from './userAC';

export interface UserResponseForDetailsAndEstimatedValueOfOpportunity { 
    questionId?: string;
    question?: QuestionAC;
    userId?: string;
    user?: UserAC;
    options?: Array<OptionAC>;
    other?: string;
    representationNumber: number;
    answerText?: string;
    strategicAnalysisId?: string;
    multipleChoiceQuestionId?: string;
    checkboxQuestionId?: string;
    dropdownQuestionId?: string;
    files?: Array<Blob>;
    checkboxOptions?: Array<string>;
    mcqOption?: string;
    selectedDropdownOption?: string;
    userResponseStatus: StrategicAnalysisStatus;
    samplingResponseStatus?: SamplingResponseStatus;
    userResponseStatusString?: string;
    isEmailAttachements: boolean;
    riskControlMatrixId?: string;
    detailsOfOpportunity?: string;
    estimatedValueOfOpportunity?: string;
    isDetailAndEstimatedValueOfOpportunity: boolean;
    auditableEntityName?: string;
}