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
import { ACMDocumentFor } from './aCMDocumentFor';
import { BaseModelAC } from './baseModelAC';

export interface ACMDocumentAC extends BaseModelAC { 
    documentPath?: string;
    fileName?: string;
    documentFor: ACMDocumentFor;
    acmId: string;
}