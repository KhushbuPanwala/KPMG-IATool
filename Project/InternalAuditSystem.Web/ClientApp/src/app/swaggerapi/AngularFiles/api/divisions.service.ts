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

import { DivisionAC } from '../model/divisionAC';
import { PaginationOfDivisionAC } from '../model/paginationOfDivisionAC';
import { ProblemDetails } from '../model/problemDetails';

import { BASE_PATH, COLLECTION_FORMATS }                     from '../variables';
import { Configuration }                                     from '../configuration';


@Injectable()
export class DivisionsService {

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
    public divisionsAddDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<DivisionAC>;
    public divisionsAddDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<DivisionAC>>;
    public divisionsAddDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<DivisionAC>>;
    public divisionsAddDivision(body: DivisionAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling divisionsAddDivision.');
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

        return this.httpClient.request<DivisionAC>('post',`${this.basePath}/api/Divisions`,
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
    public divisionsDeleteDivision(id: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public divisionsDeleteDivision(id: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public divisionsDeleteDivision(id: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public divisionsDeleteDivision(id: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling divisionsDeleteDivision.');
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

        return this.httpClient.request<any>('delete',`${this.basePath}/api/Divisions/${encodeURIComponent(String(id))}`,
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
    public divisionsExportEntityDivisions(entityId?: string, timeOffset?: number, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public divisionsExportEntityDivisions(entityId?: string, timeOffset?: number, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public divisionsExportEntityDivisions(entityId?: string, timeOffset?: number, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public divisionsExportEntityDivisions(entityId?: string, timeOffset?: number, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {



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

        return this.httpClient.request<any>('get',`${this.basePath}/api/Divisions/exportEntityDivisions`,
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
    public divisionsGetAllDivision(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<PaginationOfDivisionAC>;
    public divisionsGetAllDivision(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<PaginationOfDivisionAC>>;
    public divisionsGetAllDivision(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<PaginationOfDivisionAC>>;
    public divisionsGetAllDivision(pageIndex?: number, pageSize?: number, searchString?: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {





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

        return this.httpClient.request<PaginationOfDivisionAC>('get',`${this.basePath}/api/Divisions`,
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
    public divisionsGetDivisionById(id: string, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<DivisionAC>;
    public divisionsGetDivisionById(id: string, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<DivisionAC>>;
    public divisionsGetDivisionById(id: string, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<DivisionAC>>;
    public divisionsGetDivisionById(id: string, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (id === null || id === undefined) {
            throw new Error('Required parameter id was null or undefined when calling divisionsGetDivisionById.');
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

        return this.httpClient.request<DivisionAC>('get',`${this.basePath}/api/Divisions/${encodeURIComponent(String(id))}`,
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
    public divisionsUpdateDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'body', reportProgress?: boolean): Observable<any>;
    public divisionsUpdateDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'response', reportProgress?: boolean): Observable<HttpResponse<any>>;
    public divisionsUpdateDivision(body: DivisionAC, selectedEntityId?: string, observe?: 'events', reportProgress?: boolean): Observable<HttpEvent<any>>;
    public divisionsUpdateDivision(body: DivisionAC, selectedEntityId?: string, observe: any = 'body', reportProgress: boolean = false ): Observable<any> {

        if (body === null || body === undefined) {
            throw new Error('Required parameter body was null or undefined when calling divisionsUpdateDivision.');
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

        return this.httpClient.request<any>('put',`${this.basePath}/api/Divisions`,
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
