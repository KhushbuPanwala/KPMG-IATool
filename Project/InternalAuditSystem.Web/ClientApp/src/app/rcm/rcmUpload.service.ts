import { Injectable } from '@angular/core';
import { KeyValuePairOfIntegerAndString, UserAC, RiskControlMatrixAC } from '../swaggerapi/AngularFiles';
import { BehaviorSubject } from 'rxjs';
import { UploadService } from '../core/upload.service';
import { StringConstants } from '../shared/stringConstants';

@Injectable({
  providedIn: 'root'
})
export class RCMUploadService {
  rcmList = [] as Array<RiskControlMatrixAC>; //  observations list
  rcmAC: RiskControlMatrixAC; // Current selected  observation
  rcmFiles = [] as Array<File>;
  rcmSubject = new BehaviorSubject<RiskControlMatrixAC>({} as RiskControlMatrixAC);
  rcmListSubject = new BehaviorSubject<Array<RiskControlMatrixAC>>([] as Array<RiskControlMatrixAC>);
  rcmFilesSubject = new BehaviorSubject<Array<File>>([] as Array<File>);

  controlCategoryList = [] as Array<KeyValuePairOfIntegerAndString>; // observation type list
  controlTypeList = [] as Array<KeyValuePairOfIntegerAndString>; // disposition list
  natureOfControlList = [] as Array<KeyValuePairOfIntegerAndString>; // observation status list
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
   * @param riskControlAC : Application class object of observation
   */
  setRCMs(riskControlAC: RiskControlMatrixAC) {
    this.rcmAC = riskControlAC;
    this.rcmAC.sectorId = riskControlAC.sectorId;
    this.rcmAC.rcmProcessId = riskControlAC.rcmProcessId;
    this.rcmAC.rcmSubProcessId = riskControlAC.rcmSubProcessId;
    this.rcmAC.controlCategory = JSON.parse(JSON.stringify(riskControlAC.controlCategory));
    this.rcmAC.controlType = JSON.parse(JSON.stringify(riskControlAC.controlType));
    this.rcmAC.natureOfControl = JSON.parse(JSON.stringify(riskControlAC.natureOfControl));
    this.rcmAC.antiFraudControl = JSON.parse(JSON.stringify(riskControlAC.antiFraudControl));
    this.rcmAC.riskName = riskControlAC.riskName;
    this.rcmAC.riskDescription = riskControlAC.riskDescription;
    this.rcmAC.riskCategory = riskControlAC.riskCategory;
    this.rcmAC.controlDescription = riskControlAC.controlDescription;
    this.rcmAC.controlObjective = riskControlAC.controlObjective;
    this.rcmAC.rcmSectorName = riskControlAC.rcmSectorName;
    this.rcmAC.rcmProcessName = riskControlAC.rcmProcessName;
    this.rcmAC.rcmSubProcessName = riskControlAC.rcmSubProcessName;
    this.rcmAC.strategicAnalysisId = riskControlAC.strategicAnalysisId;
    this.rcmSubject.next(riskControlAC);
  }

  /**
   * Method to set observation files
   * @param observationFiles Observation Files
   */
  setRCMFiles(riskControlFiles: Array<File>) {
    this.rcmFiles = riskControlFiles;
    this.rcmFilesSubject.next(riskControlFiles);
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
