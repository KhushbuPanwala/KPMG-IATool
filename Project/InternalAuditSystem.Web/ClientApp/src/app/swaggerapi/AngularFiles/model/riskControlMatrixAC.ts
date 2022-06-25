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
import { ControlCategory } from './controlCategory';
import { ControlType } from './controlType';
import { NatureOfControl } from './natureOfControl';
import { RcmProcessAC } from './rcmProcessAC';
import { RcmSectorAC } from './rcmSectorAC';
import { RcmSubProcessAC } from './rcmSubProcessAC';

export interface RiskControlMatrixAC extends BaseModelAC { 
    workProgramName?: string;
    workProgramId?: string;
    riskName?: string;
    riskDescription?: string;
    controlCategory: ControlCategory;
    controlCategoryString?: string;
    controlType: ControlType;
    controlTypeString?: string;
    controlObjective?: string;
    controlDescription?: string;
    natureOfControl: NatureOfControl;
    natureOfControlString?: string;
    antiFraudControl: boolean;
    antiFraudControlString?: string;
    sectorId: string;
    rcmSectorName?: string;
    rcmProcessId: string;
    riskCategory?: string;
    rcmProcessName?: string;
    rcmSubProcessId: string;
    rcmSubProcessName?: string;
    testSteps?: string;
    testResults?: boolean;
    testResultString?: string;
    riskControlMatrixProcessACList?: Array<RcmProcessAC>;
    riskControlMatrixSubProcessACList?: Array<RcmSubProcessAC>;
    riskControlMatrixSectorACList?: Array<RcmSectorAC>;
    strategicAnalysisId?: string;
    entityId: string;
    isToDelete: boolean;
}