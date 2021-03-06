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
import { StrategicAnalysisAC } from './strategicAnalysisAC';
import { UserAC } from './userAC';

export interface StrategicAnalysisUserMappingAC extends BaseModelAC { 
    userId: string;
    strategicAnalysisId?: string;
    userAC?: UserAC;
    strategicAnalysisAC?: StrategicAnalysisAC;
}