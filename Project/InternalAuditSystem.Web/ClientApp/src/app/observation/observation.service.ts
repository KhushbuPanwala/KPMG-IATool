import { Injectable } from '@angular/core';
import { ObservationAC, KeyValuePairOfIntegerAndString, ObservationCategoryAC, UserAC, EntityUserMappingAC, ObservationDocumentAC } from '../swaggerapi/AngularFiles';
import { BehaviorSubject } from 'rxjs';
import { UploadService } from '../core/upload.service';
import { StringConstants } from '../shared/stringConstants';

@Injectable({
  providedIn: 'root'
})
export class ObservationService {
  observationList = [] as Array<ObservationAC>; //  observations list
  observationAC: ObservationAC; // Current selected  observation
  observationFiles = [] as Array<File>;
  observationSubject = new BehaviorSubject<ObservationAC>({} as ObservationAC);
  observationListSubject = new BehaviorSubject<Array<ObservationAC>>([] as Array<ObservationAC>);
  observationFilesSubject = new BehaviorSubject<Array<File>>([] as Array<File>);

  observationTypeList = [] as Array<KeyValuePairOfIntegerAndString>; // observation type list
  dispositionList = [] as Array<KeyValuePairOfIntegerAndString>; // disposition list
  observationStatusList = [] as Array<KeyValuePairOfIntegerAndString>; // observation status list
  observationCategoryList = [] as Array<ObservationCategoryAC>; // observation category list
  responsiblePersonList = [] as Array<UserAC>; // responsible person list

  wordType: string;
  pdfType: string;
  pptType: string;
  otherFileType: string;
  gifType: string;
  pngType: string;
  jpgType: string;
  svgType: string;
  csvType: string;
  mp3Type: string;
  mp4Type: string;
  excelType: string;
  zipType: string;

  constructor(private uploadService: UploadService, private stringConstants: StringConstants) {
    this.wordType = this.stringConstants.docText;
    this.pdfType = this.stringConstants.pdfText;
    this.pptType = this.stringConstants.pptText;
    this.otherFileType = this.stringConstants.otherFileType;
    this.gifType = this.stringConstants.gifText;
    this.pngType = this.stringConstants.pngText;
    this.jpgType = this.stringConstants.jpegText;
    this.svgType = this.stringConstants.svgType;
    this.csvType = this.stringConstants.csv;
    this.mp3Type = this.stringConstants.mp3Type;
    this.mp4Type = this.stringConstants.mp4Type;
    this.excelType = this.stringConstants.xlsx;
    this.zipType = this.stringConstants.zipType;
  }

  /**
   * Method of setting values in observationAC object
   * @param observationAC : Application class object of observation
   */
  setObservations(observationAC: ObservationAC) {
    this.observationAC = observationAC;
    this.observationCategoryList = JSON.parse(JSON.stringify(observationAC.observationCategoryList));
    this.responsiblePersonList = JSON.parse(JSON.stringify(observationAC.personResponsibleList));
    this.observationAC.auditPlanId = observationAC.auditPlanId;
    this.observationAC.processId = observationAC.processId;
    this.observationAC.parentProcessId = observationAC.parentProcessId;
    this.observationAC.linkedObservation = observationAC.linkedObservation;
    this.observationAC.observationCategoryList = JSON.parse(JSON.stringify(observationAC.observationCategoryList));
    this.observationAC.observationType = JSON.parse(JSON.stringify(observationAC.observationType));
    this.observationAC.observationStatus = observationAC.observationStatus;
    this.observationAC.disposition = observationAC.disposition;
    this.observationAC.observationStatusToString = observationAC.observationStatusToString;
    this.observationAC.dispositionToString = observationAC.dispositionToString;
    this.observationAC.observationTypeToString = observationAC.observationTypeToString;
    this.observationAC.observationCategoryId = observationAC.observationCategoryId;
    this.observationAC.linkedObservationACList = JSON.parse(JSON.stringify(observationAC.linkedObservationACList));
    this.observationAC.personResponsible = observationAC.personResponsible;
    this.observationAC.personResponsibleList = JSON.parse(JSON.stringify(observationAC.personResponsibleList));
    this.observationAC.riskAndControlId = observationAC.riskAndControlId;
    this.observationSubject.next(observationAC);
  }

  /**
   * Method to set observation files
   * @param observationFiles Observation Files
   */
  setObservationFiles(observationFiles: Array<File>) {
    this.observationFiles = observationFiles;
    this.observationFilesSubject.next(observationFiles);
  }

  /**
   * Check file type to display icon accordingly
   * @param fileName :Name of the File
   * @param fileTypeCheck : type to check
   */
  checkFileExtention(fileName: string, fileTypeCheck: string) {
    let isUploadedFormatMatched = false;
    switch (fileTypeCheck) {
      case this.wordType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsWord(fileName);
        break;
      case this.pdfType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPdf(fileName);
        break;
      case this.pptType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPpt(fileName);
        break;
      case this.excelType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsExcel(fileName);
        break;
      case this.pngType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsPng(fileName);
        break;
      case this.jpgType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsJpg(fileName);
        break;
      case this.gifType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsGif(fileName);
        break;
      case this.svgType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsSvg(fileName);
        break;
      case this.mp3Type:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsMp3(fileName);
        break;
      case this.mp4Type:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsMp4(fileName);
        break;
      case this.csvType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsCsv(fileName);
        break;
      case this.zipType:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsZip(fileName);
        break;
      default:
        isUploadedFormatMatched = this.uploadService.checkIfFileIsOtherFormat(fileName);
        break;
    }
    return isUploadedFormatMatched;
  }
}
