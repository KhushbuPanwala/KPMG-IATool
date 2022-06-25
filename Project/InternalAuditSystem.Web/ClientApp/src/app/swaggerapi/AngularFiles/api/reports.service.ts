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

import { KeyValuePairOfStringAndStringValues } from '../model/keyValuePairOfStringAndStringValues';
import { PaginationOfReportAC } from '../model/paginationOfReportAC';
import { ProblemDetails } from '../model/problemDetails';
import { ReportAC } from '../model/reportAC';
import { ReportCommentHistoryAC } from '../model/reportCommentHistoryAC';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class ReportsService {

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
     * @param selectedEntityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsAddReport(body: ReportAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ReportAC>;
    public reportsAddReport(body: ReportAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReportAC>>;
    public reportsAddReport(body: ReportAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReportAC>>;
    public reportsAddReport(body: ReportAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling reportsAddReport.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
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

        return this.httpClient.request<ReportAC>('post',`${this.basePath}/api/Reports`,
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
     * @param formdata 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsAddReviewerDocuments(formdata?: Array<KeyValuePairOfStringAndStringValues>, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportsAddReviewerDocuments(formdata?: Array<KeyValuePairOfStringAndStringValues>, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportsAddReviewerDocuments(formdata?: Array<KeyValuePairOfStringAndStringValues>, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportsAddReviewerDocuments(formdata?: Array<KeyValuePairOfStringAndStringValues>, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (formdata) {
            formdata.forEach((element) => {
                queryParameters = queryParameters.append('formdata', <any>element);
            })
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

        return this.httpClient.request<any>('post',`${this.basePath}/api/Reports/add-reviewer-documents`,
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
     * @param id 
     * @param entityId 
     * @param timeOffset 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsAddViewAuditReport(id?: string, entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<string>;
    public reportsAddViewAuditReport(id?: string, entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<string>>;
    public reportsAddViewAuditReport(id?: string, entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<string>>;
    public reportsAddViewAuditReport(id?: string, entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {




        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (id !== undefined && id !== null) {
            queryParameters = queryParameters.set('id', <any>id);
        }
        if (entityId !== undefined && entityId !== null) {
            queryParameters = queryParameters.set('entityId', <any>entityId);
        }
        if (timeOffset !== undefined && timeOffset !== null) {
            queryParameters = queryParameters.set('timeOffset', <any>timeOffset);
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

        return this.httpClient.request<string>('get',`${this.basePath}/api/Reports/addViewFile`,
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
     * @param id 
     * @param selectedEntityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsDeleteReport(id: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportsDeleteReport(id: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportsDeleteReport(id: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportsDeleteReport(id: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling reportsDeleteReport.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/Reports/${encodeURIComponent(String(id))}`,
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
     * @param reviewerDocumentId 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsDeleteReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportsDeleteReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportsDeleteReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportsDeleteReviewerDocument(reviewerDocumentId: string, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (reviewerDocumentId === null || reviewerDocumentId === undefined) {
            throw new Error('Required parameter reviewerDocumentId was null or undefined when calling reportsDeleteReviewerDocument.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (entityId !== undefined && entityId !== null) {
            queryParameters = queryParameters.set('entityId', <any>entityId);
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/Reports/delete-reviewer-document/${encodeURIComponent(String(reviewerDocumentId))}`,
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
     * @param reviewerDocumentId 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsDownloadReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<string>;
    public reportsDownloadReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<string>>;
    public reportsDownloadReviewerDocument(reviewerDocumentId: string, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<string>>;
    public reportsDownloadReviewerDocument(reviewerDocumentId: string, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (reviewerDocumentId === null || reviewerDocumentId === undefined) {
            throw new Error('Required parameter reviewerDocumentId was null or undefined when calling reportsDownloadReviewerDocument.');
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
        ];

        return this.httpClient.request<string>('get',`${this.basePath}/api/Reports/download-file/${encodeURIComponent(String(reviewerDocumentId))}`,
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
     * @param entityId 
     * @param timeOffset 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsExportReports(entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportsExportReports(entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportsExportReports(entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportsExportReports(entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (entityId !== undefined && entityId !== null) {
            queryParameters = queryParameters.set('entityId', <any>entityId);
        }
        if (timeOffset !== undefined && timeOffset !== null) {
            queryParameters = queryParameters.set('timeOffset', <any>timeOffset);
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

        return this.httpClient.request<any>('get',`${this.basePath}/api/Reports/exportReports`,
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
     * @param id 
     * @param entityId 
     * @param timeOffset 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsGenerateReportPPT(id?: string, entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public reportsGenerateReportPPT(id?: string, entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public reportsGenerateReportPPT(id?: string, entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public reportsGenerateReportPPT(id?: string, entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {




        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (id !== undefined && id !== null) {
            queryParameters = queryParameters.set('id', <any>id);
        }
        if (entityId !== undefined && entityId !== null) {
            queryParameters = queryParameters.set('entityId', <any>entityId);
        }
        if (timeOffset !== undefined && timeOffset !== null) {
            queryParameters = queryParameters.set('timeOffset', <any>timeOffset);
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

        return this.httpClient.request<any>('get',`${this.basePath}/api/Reports/generateReportPPT`,
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
     * @param id 
     * @param timeOffset 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsGetCommentHistory(id: string, timeOffset?: number, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ReportCommentHistoryAC>;
    public reportsGetCommentHistory(id: string, timeOffset?: number, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReportCommentHistoryAC>>;
    public reportsGetCommentHistory(id: string, timeOffset?: number, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReportCommentHistoryAC>>;
    public reportsGetCommentHistory(id: string, timeOffset?: number, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling reportsGetCommentHistory.');
        }



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (timeOffset !== undefined && timeOffset !== null) {
            queryParameters = queryParameters.set('timeOffset', <any>timeOffset);
        }
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
        ];

        return this.httpClient.request<ReportCommentHistoryAC>('get',`${this.basePath}/api/Reports/commentHistory/${encodeURIComponent(String(id))}`,
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
     * @param id 
     * @param selectedEntityId 
     * @param timeOffset 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsGetReportById(id: string, selectedEntityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<ReportAC>;
    public reportsGetReportById(id: string, selectedEntityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReportAC>>;
    public reportsGetReportById(id: string, selectedEntityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReportAC>>;
    public reportsGetReportById(id: string, selectedEntityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling reportsGetReportById.');
        }



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
        }
        if (timeOffset !== undefined && timeOffset !== null) {
            queryParameters = queryParameters.set('timeOffset', <any>timeOffset);
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

        return this.httpClient.request<ReportAC>('get',`${this.basePath}/api/Reports/${encodeURIComponent(String(id))}`,
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
     * @param selectedEntityId 
     * @param fromYear 
     * @param toYear 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsGetReports(page?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, fromYear?: number, toYear?: number, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfReportAC>;
    public reportsGetReports(page?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, fromYear?: number, toYear?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfReportAC>>;
    public reportsGetReports(page?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, fromYear?: number, toYear?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfReportAC>>;
    public reportsGetReports(page?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, fromYear?: number, toYear?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {







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
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
        }
        if (fromYear !== undefined && fromYear !== null) {
            queryParameters = queryParameters.set('fromYear', <any>fromYear);
        }
        if (toYear !== undefined && toYear !== null) {
            queryParameters = queryParameters.set('toYear', <any>toYear);
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

        return this.httpClient.request<PaginationOfReportAC>('get',`${this.basePath}/api/Reports`,
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
     * @param body 
     * @param selectedEntityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public reportsUpdateReport(body: ReportAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ReportAC>;
    public reportsUpdateReport(body: ReportAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ReportAC>>;
    public reportsUpdateReport(body: ReportAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ReportAC>>;
    public reportsUpdateReport(body: ReportAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling reportsUpdateReport.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
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

        return this.httpClient.request<ReportAC>('put',`${this.basePath}/api/Reports`,
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

}
