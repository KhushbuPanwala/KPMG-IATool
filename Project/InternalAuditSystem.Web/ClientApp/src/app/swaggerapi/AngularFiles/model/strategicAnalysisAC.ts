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
import { AuditableEntityAC } from './auditableEntityAC';
import { BaseModelAC } from './baseModelAC';
import { QuestionAC } from './questionAC';
import { StrategicAnalysisStatus } from './strategicAnalysisStatus';
import { StrategicAnalysisTeamAC } from './strategicAnalysisTeamAC';
import { UserAC } from './userAC';
import { UserResponseDocumentAC } from './userResponseDocumentAC';
import { UserResponseForDetailsAndEstimatedValueOfOpportunity } from './userResponseForDetailsAndEstimatedValueOfOpportunity';

export interface StrategicAnalysisAC extends BaseModelAC { 
    surveyTitle?: string;
    auditableEntityName?: string;
    message?: string;
    version: number;
    status: StrategicAnalysisStatus;
    isSampling: boolean;
    auditableEntityId?: string;
    rcmId?: string;
    auditableEntity?: AuditableEntityAC;
    questionsCount: number;
    responseCount: number;
    teamCollection?: Array<UserAC>;
    internalUserList?: Array<StrategicAnalysisTeamAC>;
    isVersionToBeChanged: boolean;
    questions?: Array<QuestionAC>;
    isUserResponseToBeUpdated?: boolean;
    files?: Array<Blob>;
    isActive?: boolean;
    userResponseDocumentACs?: Array<UserResponseDocumentAC>;
    userResponseForDetailsOfOppAndEstimatedValue?: UserResponseForDetailsAndEstimatedValueOfOpportunity;
    isResponseDrafted: boolean;
    pdfFileString?: string;
    detailsOfOpportunity?: string;
    estimatedValueOfOpportunity?: string;
    isEmailAttached?: boolean;
    surveyUserList?: Array<UserAC>;
    userName?: string;
    designation?: string;
    email?: string;
}