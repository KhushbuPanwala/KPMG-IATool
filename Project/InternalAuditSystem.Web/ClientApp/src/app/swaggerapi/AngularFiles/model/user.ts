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
import { BaseModel } from './baseModel';
import { UserType } from './userType';

export interface User extends BaseModel { 
    name?: string;
    emailId?: string;
    designation?: string;
    userType: UserType;
    lastInterectedDateTime: Date;
}