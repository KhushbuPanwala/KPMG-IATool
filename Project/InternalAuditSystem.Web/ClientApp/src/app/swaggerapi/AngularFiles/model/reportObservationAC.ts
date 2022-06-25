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
import { BaseModelAC } from './baseModelAC';
import { ReportDisposition } from './reportDisposition';
import { ReportObservationReviewerAC } from './reportObservationReviewerAC';
import { ReportObservationStatus } from './reportObservationStatus';
import { ReportObservationTableAC } from './reportObservationTableAC';
import { ReportObservationType } from './reportObservationType';
import { ReportObservationsDocumentAC } from './reportObservationsDocumentAC';
import { ReportObservationsMemberAC } from './reportObservationsMemberAC';

export interface ReportObservationAC extends BaseModelAC { 
    reportId: string;
    reportTitle?: string;
    observationId: string;
    auditPlanId: string;
    auditPlanName?: string;
    processId: string;
    processName?: string;
    subProcessId?: string;
    subProcessName?: string;
    observationCategoryId?: string;
    observationCategory?: string;
    heading?: string;
    background?: string;
    observations?: string;
    ratingId?: string;
    rating?: string;
    observationType: ReportObservationType;
    observationTypeName?: string;
    isRepeatObservation: boolean;
    isRepeated?: string;
    rootCause?: string;
    implication?: string;
    disposition: ReportDisposition;
    dispositionName?: string;
    observationStatus: ReportObservationStatus;
    status?: string;
    recommendation?: string;
    managementResponse?: string;
    targetDate: Date;
    observationTargetDate?: string;
    linkedObservation?: string;
    conclusion?: string;
    auditorId?: string;
    auditor?: string;
    sortOrder: number;
    reviewerName?: string;
    comment?: string;
    commentCreatedDateTime?: Date;
    commentCreatedDate?: string;
    personResponsibleList?: Array<ReportObservationsMemberAC>;
    observationReviewerList?: Array<ReportObservationReviewerAC>;
    isSelected: boolean;
    isAllowEdit: boolean;
    isAllowDelete: boolean;
    isAllowView: boolean;
    reportObservationTableList?: Array<ReportObservationTableAC>;
    reportObservationDocumentList?: Array<ReportObservationsDocumentAC>;
    tableCount: number;
    fileCount: number;
}