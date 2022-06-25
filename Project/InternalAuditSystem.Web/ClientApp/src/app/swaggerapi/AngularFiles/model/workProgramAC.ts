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
import { AuditPlanAC } from './auditPlanAC';
import { BaseModelAC } from './baseModelAC';
import { PlanProcessMappingAC } from './planProcessMappingAC';
import { ProcessAC } from './processAC';
import { UserAC } from './userAC';
import { WorkPaperAC } from './workPaperAC';
import { WorkProgramStatus } from './workProgramStatus';
import { WorkProgramTeamAC } from './workProgramTeamAC';

export interface WorkProgramAC extends BaseModelAC { 
    name?: string;
    parentProcessId: string;
    processId: string;
    processName?: string;
    subProcessName?: string;
    auditPlanName?: string;
    auditTitle?: string;
    statusString?: string;
    status: WorkProgramStatus;
    auditPlanId: string;
    auditPeriodStartDate?: Date;
    auditPeriodEndDate?: Date;
    auditPeriod?: string;
    scope?: string;
    workPaperACList?: Array<WorkPaperAC>;
    workProgramTeamACList?: Array<WorkProgramTeamAC>;
    teamFirstName?: string;
    workProgramClientParticipantsACList?: Array<WorkProgramTeamAC>;
    auditPlanACList?: Array<AuditPlanAC>;
    processACList?: Array<ProcessAC>;
    subProcessACList?: Array<PlanProcessMappingAC>;
    internalUserAC?: Array<UserAC>;
    externalUserAC?: Array<UserAC>;
    workPaperFiles?: Array<Blob>;
    selectedEntityId?: string;
}