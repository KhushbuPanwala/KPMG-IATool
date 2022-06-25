import { Injectable, Optional, Inject } from '@angular/core';
import { HttpClient, HttpRequest, HttpEvent, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { objectToFormData } from 'object-to-formdata';
import { BASE_PATH, ReportObservationsDocumentAC } from '../swaggerapi/AngularFiles';
import { StringConstants } from '../shared/stringConstants';
import { SharedService } from './shared.service';

@Injectable()
export class UploadService {
  protected baseApiUrl: string;

  constructor(
    private httpClient: HttpClient,
    @Optional() @Inject(BASE_PATH) basePath: string,
    private stringConstants: StringConstants,
    private sharedService: SharedService
  ) {
    if (basePath) {
      this.baseApiUrl = basePath;
    }
  }

  /**
   * Method to update api with files
   * @param object: Input object
   * @param files: List of files
   * @param fileFieldName: Field name of object to whom list of files to be append
   * @param apiPath: Api path
   */
  public uploadFileOnUpdate<T>(object: T, files: File[], fileFieldName: string, apiPath: string): Observable<T> {
    let selectedEntityId: string;
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        selectedEntityId = entityId;
      }
    });
    const formData = this.objectToFormData(object);
    for (const file of files) {
      formData.append(fileFieldName, file);
    }
    formData.append(this.stringConstants.selectedEntityKey, selectedEntityId);
    return this.httpClient.put<T>(`${this.baseApiUrl}${apiPath}`, formData);

  }

  /**
   * Method to Add api with files
   * @param object: Input object
   * @param files: List of files
   * @param fileFieldName: Field name of object to whom list of files to be append
   * @param apiPath: Api path
   */
  public uploadFileOnAdd<T>(object: T, files: File[], fileFieldName: string, apiPath: string): Observable<T> {
    let selectedEntityId: string;
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        selectedEntityId = entityId;
      }
    });
    const formData = this.objectToFormData(object);
    for (const file of files) {
      formData.append(fileFieldName, file);
    }
    formData.append(this.stringConstants.selectedEntityKey, selectedEntityId);
    return this.httpClient.post<T>(`${this.baseApiUrl}${apiPath}`, formData);
  }

  /**
   * Method to Add api with files
   * @param object: Input object
   * @param files: List of files
   * @param fileFieldName: Field name of object to whom list of files to be append
   * @param apiPath: Api path
   */
  public uploadQuestionFileOnAdd(object, files: File[], fileFieldName: string, apiPath: string) {
    const formData = this.objectToFormData(object);
    for (const file of files) {
      formData.append(fileFieldName, file);
    }
    const a = this.httpClient.post(`${this.baseApiUrl}${apiPath}`, formData);
    return a;

  }
  /**
   * Method to convert object to form data
   * @param object: Object to be converted
   */
  private objectToFormData(object) {
    const options = {
      indices: true,
      nullsAsUndefineds: false,
      booleansAsIntegers: false,
    };
    return objectToFormData(object, options);
  }

  /**
   * Check if file is mp3 type
   * @param fileName: file name
   */
  checkIfFileIsMp3(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.mp3Type.toLowerCase());
  }

  /**
   * Check if file is mp4 type
   * @param fileName: file name
   */
  checkIfFileIsMp4(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.mp4Type.toLowerCase());
  }

  /**
   * Check if file is svg type
   * @param fileName: file name
   */
  checkIfFileIsSvg(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.svgType.toLowerCase());
  }

  /**
   * Check if file is Zip type
   * @param fileName: file name
   */
  checkIfFileIsZip(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.zipType.toLowerCase());
  }

  /**
   * Check if file is Word type
   * @param fileName: file name
   */
  checkIfFileIsWord(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.docxText.toLowerCase() || extention === this.stringConstants.docText.toLowerCase());
  }

  /**
   * Check if file is Pdf type
   * @param fileName: file name
   */
  checkIfFileIsPdf(fileName: string) {

    const extention: string = this.getFileExtention(fileName);
    return extention === this.stringConstants.pdfText.toLowerCase();
  }

  /**
   * Check if file is ppt type
   * @param fileName: file name
   */
  checkIfFileIsPpt(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.pptText.toLowerCase() || extention === this.stringConstants.pptxText.toLowerCase());
  }

  /**
   * Check if file is excel
   * @param fileName: file name
   */
  checkIfFileIsExcel(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.xls.toLowerCase() || extention === this.stringConstants.xlsx.toLowerCase());
  }

  /**
   * Check if file is csv
   * @param fileName: file name
   */
  checkIfFileIsCsv(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.csv.toLowerCase());
  }
  /**
   * Check if file is gif
   * @param fileName: file name
   */
  checkIfFileIsGif(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.gifText.toLowerCase());
  }
  /**
   * Check if file is png
   * @param fileName: file name
   */
  checkIfFileIsPng(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.pngText.toLowerCase());
  }
  /**
   * Check if file is jpg
   * @param fileName: file name
   */
  checkIfFileIsJpg(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.jpegText.toLowerCase() || extention === this.stringConstants.jpgText.toLowerCase());
  }

  /**
   * Check if file is Ppx
   * @param fileName file name
   */
  checkIfFileIsPpx(fileName: string) {
    const extention: string = this.getFileExtention(fileName);
    return (extention === this.stringConstants.ppxText.toLowerCase());
  }

  /**
   * Check if file is of any other format
   * @param fileName: file name
   */
  checkIfFileIsOtherFormat(fileName: string) {
    return (
      !this.checkIfFileIsWord(fileName) && !this.checkIfFileIsPdf(fileName) && !this.checkIfFileIsPpt(fileName) && !this.checkIfFileIsExcel(fileName)
      && !this.checkIfFileIsCsv(fileName) && !this.checkIfFileIsJpg(fileName) && !this.checkIfFileIsPng(fileName) && !this.checkIfFileIsGif(fileName)
      && !this.checkIfFileIsSvg(fileName) && !this.checkIfFileIsZip(fileName) && !this.checkIfFileIsMp4(fileName) && !this.checkIfFileIsMp3(fileName)
    );
  }

  /**
   * Get file extention
   * @param fileName: file name
   */
  private getFileExtention(fileName: string) {
    if (fileName !== undefined) {
      return fileName.split('?')[0].split('.').pop().toLowerCase();
    }
  }


  /**
   * Upload list form data
   * @param formData : list of form data
   * @param apiPath : api path
   * @param id : any module id for which this list of file will be added
   */
  public uploadCollectionOfFile(formData: FormData, apiPath: string, id: string) {
    return this.httpClient.put(`${this.baseApiUrl}${apiPath}${id}`, formData);
  }

  /**
   * Upload list form data
   * @param formData : list of form data
   * @param apiPath : api path
   * @param id : any module id for which this list of file will be added
   * @param entityId: entity id
   */
  public uploadCollectionOfFileStrategicAnalysis(formData: FormData, apiPath: string, id: string, entityId: string) {
    const queryString = '/' + entityId;
    return this.httpClient.put(`${this.baseApiUrl}${apiPath}${id}${queryString}`, formData);
  }

  /**
   * Method to Add api with files
   * @param formData: formData for post data
   * @param apiPath: Api path
   */
  public uploadFileOnReport(formData: FormData, apiPath: string) {
    let selectedEntityId: string;
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        selectedEntityId = entityId;
      }
    });
    formData.append(this.stringConstants.selectedEntityKey, selectedEntityId);
    return this.httpClient.post(`${this.baseApiUrl}${apiPath}`, formData);
  }
  /**
   * Method to Add api with files
   * @param object: Input object
   * @param files: List of files
   * @param fileFieldName: Field name of object to whom list of files to be append
   * @param apiPath: Api path
   */
  public uploadFileOnReportObservationDocument<T>(object: T, files: File[], fileFieldName: string, apiPath: string): Observable<Array<ReportObservationsDocumentAC>> {
    const formData = this.objectToFormData(object);
    let selectedEntityId: string;
    this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        selectedEntityId = entityId;
      }
    });
    for (const file of files) {
      formData.append(fileFieldName, file);
    }
    formData.append(this.stringConstants.selectedEntityKey, selectedEntityId);
    return this.httpClient.post<Array<ReportObservationsDocumentAC>>(`${this.baseApiUrl}${apiPath}`, formData);
  }
}
