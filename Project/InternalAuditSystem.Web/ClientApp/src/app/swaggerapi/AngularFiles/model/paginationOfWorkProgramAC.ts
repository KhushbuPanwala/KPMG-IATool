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
import { LoggedInUserDetails } from './loggedInUserDetails';
import { WorkProgramAC } from './workProgramAC';

export interface PaginationOfWorkProgramAC { 
    totalRecords: number;
    pageIndex: number;
    pageSize: number;
    entityId: string;
    searchText?: string;
    items?: Array<WorkProgramAC>;
    currentUserDetails?: LoggedInUserDetails;
}