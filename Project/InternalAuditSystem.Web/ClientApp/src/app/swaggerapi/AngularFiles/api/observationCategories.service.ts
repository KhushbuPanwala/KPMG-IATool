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

import { ObservationCategoryAC } from '../model/observationCategoryAC';
import { PaginationOfObservationCategoryAC } from '../model/paginationOfObservationCategoryAC';
import { ProblemDetails } from '../model/problemDetails';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class ObservationCategoriesService {

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
    public observationCategoriesAddObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ObservationCategoryAC>;
    public observationCategoriesAddObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ObservationCategoryAC>>;
    public observationCategoriesAddObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ObservationCategoryAC>>;
    public observationCategoriesAddObservationCategory(body: ObservationCategoryAC, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling observationCategoriesAddObservationCategory.');
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

        return this.httpClient.request<ObservationCategoryAC>('post',`${this.basePath}/api/ObservationCategories`,
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
     * @param observationCategoryId 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public observationCategoriesDeleteObservationCategory(observationCategoryId: string, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public observationCategoriesDeleteObservationCategory(observationCategoryId: string, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public observationCategoriesDeleteObservationCategory(observationCategoryId: string, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public observationCategoriesDeleteObservationCategory(observationCategoryId: string, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (observationCategoryId === null || observationCategoryId === undefined) {
            throw new Error('Required parameter observationCategoryId was null or undefined when calling observationCategoriesDeleteObservationCategory.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/ObservationCategories/${encodeURIComponent(String(observationCategoryId))}`,
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
    public observationCategoriesExportObservationCategories(entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public observationCategoriesExportObservationCategories(entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public observationCategoriesExportObservationCategories(entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public observationCategoriesExportObservationCategories(entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



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

        return this.httpClient.request<any>('get',`${this.basePath}/api/ObservationCategories/exportObservationCategories`,
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
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public observationCategoriesGetAllObservationCategories(pageIndex?: number, pageSize?: number, searchString?: string, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfObservationCategoryAC>;
    public observationCategoriesGetAllObservationCategories(pageIndex?: number, pageSize?: number, searchString?: string, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfObservationCategoryAC>>;
    public observationCategoriesGetAllObservationCategories(pageIndex?: number, pageSize?: number, searchString?: string, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfObservationCategoryAC>>;
    public observationCategoriesGetAllObservationCategories(pageIndex?: number, pageSize?: number, searchString?: string, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {





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

        return this.httpClient.request<PaginationOfObservationCategoryAC>('get',`${this.basePath}/api/ObservationCategories/getObservationCategories`,
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
     * @param observationCategoryId 
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public observationCategoriesGetObservationCategoryDetailsById(observationCategoryId?: string, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ObservationCategoryAC>;
    public observationCategoriesGetObservationCategoryDetailsById(observationCategoryId?: string, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ObservationCategoryAC>>;
    public observationCategoriesGetObservationCategoryDetailsById(observationCategoryId?: string, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ObservationCategoryAC>>;
    public observationCategoriesGetObservationCategoryDetailsById(observationCategoryId?: string, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



        let queryParameters = new HttpParams({encoder: new CustomHttpUrlEncodingCodec()});
        if (observationCategoryId !== undefined && observationCategoryId !== null) {
            queryParameters = queryParameters.set('observationCategoryId', <any>observationCategoryId);
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

        return this.httpClient.request<ObservationCategoryAC>('get',`${this.basePath}/api/ObservationCategories`,
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
     * @param entityId 
     * @param observe set whether or not to return the data Observable as the body, response or events. defaults to returning the body.
     * @param reportProgress flag to report request and response progress.
     */
    public observationCategoriesUpdateObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'body', reportProgress?: boolean): Observable<ObservationCategoryAC>;
    public observationCategoriesUpdateObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<ObservationCategoryAC>>;
    public observationCategoriesUpdateObservationCategory(body: ObservationCategoryAC, entityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<ObservationCategoryAC>>;
    public observationCategoriesUpdateObservationCategory(body: ObservationCategoryAC, entityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling observationCategoriesUpdateObservationCategory.');
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

        return this.httpClient.request<ObservationCategoryAC>('put',`${this.basePath}/api/ObservationCategories`,
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
