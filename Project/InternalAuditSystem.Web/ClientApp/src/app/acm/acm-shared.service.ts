import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UploadService } from '../core/upload.service';
import { StringConstants } from '../shared/stringConstants';
import { ACMPresentationAC, KeyValuePairOfIntegerAndString, UserAC, ACMDocumentAC, ACMTableAC } from '../swaggerapi/AngularFiles';
import { AcmAddTabs } from './acm-add/acm-add.model';

@Injectable({
  providedIn: 'root'
})
export class ACMSharedService {
  acmList = [] as Array<ACMPresentationAC>; //  ACM list
  acmAC: ACMPresentationAC; // Current selected  ACM
  acmSubject = new BehaviorSubject<ACMPresentationAC>({} as ACMPresentationAC);
  acmListSubject = new BehaviorSubject<Array<ACMPresentationAC>>([] as Array<ACMPresentationAC>);
  filesCountSubject = new BehaviorSubject<number>(0);

  dispositionList = [] as Array<KeyValuePairOfIntegerAndString>; // disposition list
  acmStatusList = [] as Array<KeyValuePairOfIntegerAndString>; // acm status list
  acmStageList = [] as Array<KeyValuePairOfIntegerAndString>; // acm stage list
  responsiblePersonList = [] as Array<UserAC>; // responsible person list

  tabListSubject = new BehaviorSubject<Array<AcmAddTabs>>([] as Array<AcmAddTabs>);

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
   * Method of setting values in ACMPresentationAC object
   * @param ACMPresentationAC : Application class object of ACM presentation
   */
  setACMs(acmAC: ACMPresentationAC) {
    this.acmAC = this.acmAC;
    this.responsiblePersonList = JSON.parse(JSON.stringify(acmAC.personResponsibleList));
    this.acmAC.reportId = acmAC.reportId;
    this.acmAC.managementResponse = acmAC.managementResponse;
    this.acmAC.ratingsList = JSON.parse(JSON.stringify(acmAC.ratingsList));
    this.acmAC.rating = JSON.parse(JSON.stringify(acmAC.rating));
    this.acmAC.ratingId = acmAC.ratingId;
    this.acmAC.ratings = acmAC.ratings;
    this.acmAC.status = acmAC.status;
    this.acmAC.statusString = acmAC.statusString;
    this.acmAC.acmReportTitle = acmAC.acmReportTitle;
    this.acmAC.personResponsible = acmAC.personResponsible;
    this.acmSubject.next(acmAC);
  }

  /**
   * Method to set ACM files
   * @param acmFiles Files
   */
  setFilesCount(filesCount: number) {
    this.filesCountSubject.next(filesCount);
  }

  /**
   * Complete files count subscribe
   */
  unSubscribeFilesCount() {
    this.filesCountSubject.unsubscribe();
  }

  /**
   * Set Tabs data
   * @param tabs : List of all tabs
   */
  setTabList(tabs: Array<AcmAddTabs>) {
    this.tabListSubject.next(tabs);
  }

  /**
   * Complete tab list  subscribe
   */
  unSubscribeTabList() {
    this.tabListSubject.unsubscribe();
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
