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

export interface FileUploadQuestionAC extends BaseModelAC { 
    guidance?: string;
    isDocAllowed: boolean;
    isGifAllowed: boolean;
    isJpegAllowed: boolean;
    isPpxAllowed: boolean;
    isPngAllowed: boolean;
    isPdfAllowed: boolean;
}