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
import { EntityUserMappingAC } from './entityUserMappingAC';
import { KeyValuePairOfIntegerAndString } from './keyValuePairOfIntegerAndString';
import { RatingAC } from './ratingAC';
import { ReportStage } from './reportStage';
import { ReportStatus } from './reportStatus';
import { ReportUserMappingAC } from './reportUserMappingAC';

export interface ReportAC extends BaseModelAC { 
    reportTitle?: string;
    entityId: string;
    ratingId: string;
    ratings?: string;
    stage: ReportStage;
    auditPeriodStartDate: Date;
    auditStartDate?: string;
    auditPeriodEndDate: Date;
    auditEndDate?: string;
    auditStatus: ReportStatus;
    comment?: string;
    noOfObservation: number;
    stageName?: string;
    status?: string;
    ratingsList?: Array<RatingAC>;
    statusList?: Array<KeyValuePairOfIntegerAndString>;
    stageList?: Array<KeyValuePairOfIntegerAndString>;
    userList?: Array<EntityUserMappingAC>;
    reviewerList?: Array<ReportUserMappingAC>;
    reviewerStatus?: Array<KeyValuePairOfIntegerAndString>;
    isChecked: boolean;
}