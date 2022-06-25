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

import { AuditCategoryAC } from '../model/auditCategoryAC';
import { PaginationOfAuditCategoryAC } from '../model/paginationOfAuditCategoryAC';
import { ProblemDetails } from '../model/problemDetails';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class AuditCategoriesService {

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
    public auditCategoriesAddAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<AuditCategoryAC>;
    public auditCategoriesAddAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AuditCategoryAC>>;
    public auditCategoriesAddAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AuditCategoryAC>>;
    public auditCategoriesAddAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling auditCategoriesAddAuditCategory.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
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

        return this.httpClient.request<AuditCategoryAC>('post',`${this.basePath}/api/AuditCategories`,
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
     * @param id 
     * @param selectedEntityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public auditCategoriesDeleteAuditCategory(id: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public auditCategoriesDeleteAuditCategory(id: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public auditCategoriesDeleteAuditCategory(id: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public auditCategoriesDeleteAuditCategory(id: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling auditCategoriesDeleteAuditCategory.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/AuditCategories/${encodeURIComponent(String(id))}`,
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
    public auditCategoriesExportAuditCategories(entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public auditCategoriesExportAuditCategories(entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public auditCategoriesExportAuditCategories(entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public auditCategoriesExportAuditCategories(entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



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
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<any>('get',`${this.basePath}/api/AuditCategories/exportAuditCategories`,
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
     * @param pageIndex 
     * @param pageSize 
     * @param searchString 
     * @param selectedEntityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public auditCategoriesGetAllAuditCategories(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfAuditCategoryAC>;
    public auditCategoriesGetAllAuditCategories(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfAuditCategoryAC>>;
    public auditCategoriesGetAllAuditCategories(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfAuditCategoryAC>>;
    public auditCategoriesGetAllAuditCategories(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {





        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (pageIndex !== undefined && pageIndex !== null) {
            queryParameters = queryParameters.set('pageIndex', <any>pageIndex);
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

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<PaginationOfAuditCategoryAC>('get',`${this.basePath}/api/AuditCategories`,
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
    public auditCategoriesGetAuditCategoryById(id: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<AuditCategoryAC>;
    public auditCategoriesGetAuditCategoryById(id: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<AuditCategoryAC>>;
    public auditCategoriesGetAuditCategoryById(id: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<AuditCategoryAC>>;
    public auditCategoriesGetAuditCategoryById(id: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling auditCategoriesGetAuditCategoryById.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
        ];
        const httpHeaderAcceptSelected: string | undefined = this.configuration.selectHeaderAccept(httpHeaderAccepts);
        if (httpHeaderAcceptSelected != undefined) {
            headers = headers.set('Accept', httpHeaderAcceptSelected);
        }

        // to determine the Content-Type header
        const consumes: string[] = [
        ];

        return this.httpClient.request<AuditCategoryAC>('get',`${this.basePath}/api/AuditCategories/${encodeURIComponent(String(id))}`,
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
    public auditCategoriesUpdateAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public auditCategoriesUpdateAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public auditCategoriesUpdateAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public auditCategoriesUpdateAuditCategory(body: AuditCategoryAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling auditCategoriesUpdateAuditCategory.');
        }


        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (selectedEntityId !== undefined && selectedEntityId !== null) {
            queryParameters = queryParameters.set('selectedEntityId', <any>selectedEntityId);
        }

        let headers = this.defaultHeaders;

        // to determine the Accept header
        let httpHeaderAccepts: string[] = [
            'text/plain',
            'application/json',
            'text/json'
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

        return this.httpClient.request<any>('put',`${this.basePath}/api/AuditCategories`,
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
