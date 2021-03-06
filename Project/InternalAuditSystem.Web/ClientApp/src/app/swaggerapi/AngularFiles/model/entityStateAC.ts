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
import { EntityCountryAC } from './entityCountryAC';
import { ProvinceStateAC } from './provinceStateAC';

export interface EntityStateAC extends BaseModelAC { 
    entityName?: string;
    entityCountryId?: string;
    entityId?: string;
    stateId?: string;
    stateName?: string;
    countryName?: string;
    regionName?: string;
    countryACList?: Array<EntityCountryAC>;
    stateACList?: Array<ProvinceStateAC>;
}