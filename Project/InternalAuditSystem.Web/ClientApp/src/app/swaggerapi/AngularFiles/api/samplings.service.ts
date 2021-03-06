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

import { PaginationOfStrategicAnalysisAC } from '../model/paginationOfStrategicAnalysisAC';
import { PaginationOfUserWiseResponseAC } from '../model/paginationOfUserWiseResponseAC';
import { ProblemDetails } from '../model/problemDetails';
import { UserWiseResponseAC } from '../model/userWiseResponseAC';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class SamplingsService {

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
     * @param samplingId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public samplingsDeleteSamplingData(samplingId: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public samplingsDeleteSamplingData(samplingId: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public samplingsDeleteSamplingData(samplingId: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public samplingsDeleteSamplingData(samplingId: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (samplingId === null || samplingId === undefined) {
            throw new Error('Required parameter samplingId was null or undefined when calling samplingsDeleteSamplingData.');
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<any>('delete',`${this.basePath}/api/Samplings/${encodeURIComponent(String(samplingId))}`,
            {
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
     * @param page 
     * @param pageSize 
     * @param searchString 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public samplingsGetAllSamplingPageAndSearchWise(page?: number, pageSize?: number, searchString?: string, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfStrategicAnalysisAC>;
    public samplingsGetAllSamplingPageAndSearchWise(page?: number, pageSize?: number, searchString?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfStrategicAnalysisAC>>;
    public samplingsGetAllSamplingPageAndSearchWise(page?: number, pageSize?: number, searchString?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfStrategicAnalysisAC>>;
    public samplingsGetAllSamplingPageAndSearchWise(page?: number, pageSize?: number, searchString?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {




        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (page !== undefined && page !== null) {
            queryParameters = queryParameters.set('page', <any>page);
        }
        if (pageSize !== undefined && pageSize !== null) {
            queryParameters = queryParameters.set('pageSize', <any>pageSize);
        }
        if (searchString !== undefined && searchString !== null) {
            queryParameters = queryParameters.set('searchString', <any>searchString);
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

        return this.httpClient.request<PaginationOfStrategicAnalysisAC>('get',`${this.basePath}/api/Samplings`,
            {
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
     * @param samplingId 
     * @param rcmId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public samplingsGetRcmWiseResponse(samplingId?: string, rcmId?: string, observe?: 'body', reportProgress?: boolean): Observable<Array<UserWiseResponseAC>>;
    public samplingsGetRcmWiseResponse(samplingId?: string, rcmId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<Array<UserWiseResponseAC>>>;
    public samplingsGetRcmWiseResponse(samplingId?: string, rcmId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<Array<UserWiseResponseAC>>>;
    public samplingsGetRcmWiseResponse(samplingId?: string, rcmId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (samplingId !== undefined && samplingId !== null) {
            queryParameters = queryParameters.set('samplingId', <any>samplingId);
        }
        if (rcmId !== undefined && rcmId !== null) {
            queryParameters = queryParameters.set('rcmId', <any>rcmId);
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

        return this.httpClient.request<Array<UserWiseResponseAC>>('get',`${this.basePath}/api/Samplings/rcmwise/sampling-response`,
            {
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
     * @param page 
     * @param pageSize 
     * @param searchString 
     * @param samplingId 
     * @param rcmId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public samplingsGetUserWiseResponse(page?: number, pageSize?: number, searchString?: string, samplingId?: string, rcmId?: string, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfUserWiseResponseAC>;
    public samplingsGetUserWiseResponse(page?: number, pageSize?: number, searchString?: string, samplingId?: string, rcmId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfUserWiseResponseAC>>;
    public samplingsGetUserWiseResponse(page?: number, pageSize?: number, searchString?: string, samplingId?: string, rcmId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfUserWiseResponseAC>>;
    public samplingsGetUserWiseResponse(page?: number, pageSize?: number, searchString?: string, samplingId?: string, rcmId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {






        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (page !== undefined && page !== null) {
            queryParameters = queryParameters.set('page', <any>page);
        }
        if (pageSize !== undefined && pageSize !== null) {
            queryParameters = queryParameters.set('pageSize', <any>pageSize);
        }
        if (searchString !== undefined && searchString !== null) {
            queryParameters = queryParameters.set('searchString', <any>searchString);
        }
        if (samplingId !== undefined && samplingId !== null) {
            queryParameters = queryParameters.set('samplingId', <any>samplingId);
        }
        if (rcmId !== undefined && rcmId !== null) {
            queryParameters = queryParameters.set('rcmId', <any>rcmId);
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

        return this.httpClient.request<PaginationOfUserWiseResponseAC>('get',`${this.basePath}/api/Samplings/sampling-user-response`,
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
