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
 *//* tslint:disable:no-unused-variable member-ordering */

import { Inject, Injectable, Optional }                      from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams,
         HttpResponse, HttpEvent }                           from '@angular/common/http';
import { CustomHttpUrlEncodingCodec }                        from '../encoder';

import { Observable }                                        from 'rxjs';

import { ProblemDetails } from '../model/problemDetails';
import { ReportDistributorsAC } from '../model/reportDistributorsAC';
import { ReportUserMappingAC } from '../model/reportUserMappingAC';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class ReportDistributorsService {

    protected basePath = 'http://localhost:5000/';
    public defaultHeaders = new HttpHeaders();
    public configuration = new Configuration();

    constructor(protected httpClient: HttpClient, @Optional()@Inject(BASE_PATH) basePath: string, @Optional() configuration: Configuration) {
        if (basePath) {
            this.basePath = basePath;
        }
        if (configuration) {
            this.configuration = configuration;
            this.basePath = basePath || configuration.basePath || this.basePath;
        }
    }

    /**
     * @param consumes string[] mime-types
     * @return true: consumes contains 'multipart/form-data', false: otherwise
     */
    private canConsumeForm(consumes: string[]): boolean {
        const form = 'multipart/form-data';
        for (const consume of consumes) {
            if (form === consume) {
                return true;
            }
        }
        return false;
    }


    /**
     * 
     * 
     * @param body 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportDistributorsAddReportDistributors(body: Array<ReportUserMappingAC>, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportDistributorsAddReportDistributors(body: Array<ReportUserMappingAC>, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportDistributorsAddReportDistributors(body: Array<ReportUserMappingAC>, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportDistributorsAddReportDistributors(body: Array<ReportUserMappingAC>, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling reportDistributorsAddReportDistributors.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (entityId !== undefined && entityId !== null) {
            queryParameters = queryParameters.set('entityId', <any>entityId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
            'application/json-patch+json',
            'application/json',
            'text/json',
            'application/_*+json'
        ];
        const httpContentTypeSelected: string | undefined = this.configuration.selectHeaderContentType(consumes);
        if (httpContentTypeSelected != undefined) {
            headers = headers.set('Content-Type', httpContentTypeSelected);
        }

        return this.httpClient.request<any>('post',`${this.basePath}/api/ReportDistributors/add`,
            {
                body: body,
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

    /**
     * 
     * 
     * @param entityId 
     * @param reportId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportDistributorsGetDistributorsByReportId(entityId: string, reportId?: string, observe?: 'body', reportProgress?: boolean): Observable<ReportDistributorsAC>;
    public reportDistributorsGetDistributorsByReportId(entityId: string, reportId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReportDistributorsAC>>;
    public reportDistributorsGetDistributorsByReportId(entityId: string, reportId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReportDistributorsAC>>;
    public reportDistributorsGetDistributorsByReportId(entityId: string, reportId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (entityId === null || entityId === undefined) {
            throw new Error('Required parameter entityId was null or undefined when calling reportDistributorsGetDistributorsByReportId.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (reportId !== undefined && reportId !== null) {
            queryParameters = queryParameters.set('reportId', <any>reportId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'application/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<ReportDistributorsAC>('get',`${this.basePath}/api/ReportDistributors/get/${encodeURIComponent(String(entityId))}`,
            {
                params: queryParameters,
                withCredentials: this.configuration.withCredentials,
                headers: headers,
                observe: observe,
                reportProgress: reportProgress
            }
        );
    }

}