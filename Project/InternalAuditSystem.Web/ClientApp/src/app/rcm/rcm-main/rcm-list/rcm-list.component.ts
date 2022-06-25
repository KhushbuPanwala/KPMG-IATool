import { Component, OnInit, OnDestroy, Optional, Inject } from '@angular/core';
import { StringConstants } from '../../../shared/stringConstants';
import { Router, ActivatedRoute } from '@angular/router';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Pagination } from '../../../models/pagination';
import { ConfirmationDialogComponent } from '../../../shared/confirmation-dialog/confirmation-dialog.component';
import { SharedService } from '../../../core/shared.service';
import { RiskControlMatrixAC, RiskControlMatrixesService, KeyValuePairOfIntegerAndString, RcmSectorAC, RcmSubProcessAC, RcmProcessAC, BASE_PATH, AuditableEntityAC } from '../../../swaggerapi/AngularFiles';
import { LoaderService } from '../../../core/loader.service';
import { Subscription } from 'rxjs';
import { BulkUpload } from '../../../shared/bulk-upload';
import { UploadService } from '../../../core/upload.service';
import { RCMUploadService } from '../../rcmUpload.service';


@Component({
  selector: 'app-rcm-list',
  templateUrl: './rcm-list.component.html'
})
export class RcmListComponent implements OnInit, OnDestroy {

  rcmList = [] as Array<RiskControlMatrixAC>;

  pageNumber: number = null;
  totalRecords: number;
  searchValue = '';
  id: string; // Variable for rcm id
  processTitle: string; // Variable for rcm process title
  subProcessTitle: string; // Variable for rcm sub-process title
  riskDescriptionTitle: string; // Variable for risk descriptionss
  controlCategoryName: string;
  controltypeName: string;
  natureOfControlName: string;
  antiFraudControlName: string;
  controlDescription: string; // Variable for control description
  rcmTitle: string; // Variable for rcm  title
  searchText: string; // Variable for search text
  excelToolTip: string; //  Variable for excel tooltip
  addToolTip: string; // Variable for add tooltip
  editToolTip: string; // Variable for edit tooltip
  deleteToolTip: string; // Variable for delete tooltip
  showingResults: string; // Variable for showing results
  ofText: string; // Variable for of tooltip
  showingText: string; // Variable for showing tooltip
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list

  controlCategory = [] as Array<KeyValuePairOfIntegerAndString>;
  controltype = [] as Array<KeyValuePairOfIntegerAndString>;
  natureOfControl = [] as Array<KeyValuePairOfIntegerAndString>;
  controlCategoryDrop = '';
  controlTypeDrop = '';
  natureOfControlDrop = '';
  antiFraudControlDrop = '';

  selectedRcm: string[];
  rcmSearchList: RiskControlMatrixAC[];
  newRcmAddList: RiskControlMatrixAC[] = [];
  rcmUnEditedList: RiskControlMatrixAC[];

  sectorList = [] as Array<RcmSectorAC>;
  processList = [] as Array<RcmProcessAC>;
  subProcessList = [] as Array<RcmSubProcessAC>;

  sectorDrop: string;
  processDrop: string;
  subProcessDrop: string;

  // only to subscripe for the current component
  entitySubscribe: Subscription;

  deleteTitle: string; // Variable for title
  bsModalRef: BsModalRef; // Modal ref variable
  showNoDataText: string; // Variable for showing text for no data
  uploadText: string; // Variable for upload text
  selectedEntityId;
  downloadTemplate: string; // for download template text
  bulkUploadText: string; // for bulk upload text
  baseUrl: string;
  file: File;
  bulkUpload = {} as BulkUpload;
  addTableTitle: string;

  constructor(private stringConstants: StringConstants,
              private apiService: RiskControlMatrixesService,
              private rcmService: RCMUploadService,
              public router: Router,
              private modalService: BsModalService,
              private route: ActivatedRoute,
              private sharedService: SharedService,
              private uploadService: UploadService,
              private loaderService: LoaderService,
              @Optional() @Inject(BASE_PATH) basePath: string) {

    this.id = this.stringConstants.id;
    this.processTitle = this.stringConstants.rcmProcessTitle;
    this.subProcessTitle = this.stringConstants.rcmSubProcessTitle;
    this.controlCategoryName = this.stringConstants.controlCategory;
    this.controlDescription = this.stringConstants.controlDescription;
    this.controltypeName = this.stringConstants.controlType;
    this.natureOfControlName = this.stringConstants.natureOfControl;
    this.antiFraudControlName = this.stringConstants.antiFraudControl;

    this.riskDescriptionTitle = this.stringConstants.riskDescription;
    this.rcmTitle = this.stringConstants.rcmTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.showingText = this.stringConstants.showingText;
    this.ofText = this.stringConstants.ofText;
    this.showingResults = '';
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.deleteTitle = this.stringConstants.deleteTitle;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.uploadText = this.stringConstants.uploadText;
    this.downloadTemplate = this.stringConstants.downloadTemplate;
    this.bulkUploadText = this.stringConstants.bulkUploadText;
    this.baseUrl = basePath;
  }

  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit(): void {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {
            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getRcm(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
      }
    });
  }

  /**
   * A lifecycle hook that is called when a directive, pipe, or service is destroyed.
   * Use for any custom cleanup that needs to occur when the instance is destroyed.
   */
  ngOnDestroy() {
    this.entitySubscribe.unsubscribe();
  }

  /***
   * Search RCM Data
   * @param event: key press event
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  searchRcm(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.searchValue = this.searchValue.trim();
      this.loaderService.open();
      this.getRcm(pageNumber, selectedPageItem, searchValue, this.selectedEntityId);
      this.loaderService.close();
    }
  }

  /***
   * Get RCM Data
   * @param pageNumber: current page number
   * @param selectedPageItem:  no. of items display on per page
   * @param searchValue: search value
   */
  async getRcm(pageNumber: number, selectedPageItem: number, searchValue: string, selectedEntityId: string) {
    this.loaderService.open();
    this.apiService.riskControlMatrixesGetRCMListForWorkProgram('', pageNumber, selectedPageItem, searchValue, selectedEntityId).subscribe((result: Pagination<RiskControlMatrixAC>) => {
      this.rcmList = JSON.parse(JSON.stringify(result.items));
      for (const rcm of this.rcmList) {
        rcm.riskDescription = rcm.riskDescription.replace(/<[^>]*>/g, '');
      }
      this.pageNumber = result.pageIndex;
      this.totalRecords = result.totalRecords;
      this.searchValue = searchValue;
      this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);
      this.loaderService.close();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Set showing result data
   * @param pageNumber:  current page no.
   * @param selectedPageItem: items per page
   * @param totalRecords: total no. of records
   */
  setShowingResult(pageNumber: number, selectedPageItem: number, totalRecords: number) {
    const startItem = (pageNumber - 1) * selectedPageItem + 1;
    const endItem = (totalRecords < (pageNumber * selectedPageItem)) ? totalRecords : (pageNumber * selectedPageItem);
    this.showingResults = this.showingText + ' ' + startItem + ' - ' + endItem + ' ' + this.ofText + ' ' + totalRecords;
  }

  /**
   * On change current page
   * @param pageNumber: page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getRcm(pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getRcm(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
  }

  /**
   * Open add - RCM page
   */
  openAddRcm() {
    this.router.navigate(['rcm/add', { id: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Methdod to get Rcm list for search dropdown
   */
  getRcmSearchList() {
    this.apiService.riskControlMatrixesGetRCMListForWorkProgram('', 0, 0, '').subscribe(result => {
      this.rcmSearchList = result.items;
    });
  }

  /**
   * Delete RCM
   * @param rcmId: id to delete RCM
   */
  deleteRcm(rcmId: string) {

    this.modalService.config.class = 'page-modal delete-modal';
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent, {
      initialState: {
        title: this.deleteTitle,
        keyboard: true,
        callback: (result) => {
          if (result === this.stringConstants.yes) {
            this.loaderService.open();
            this.apiService.riskControlMatrixesDeleteRcm(rcmId, this.selectedEntityId).subscribe(data => {
              this.rcmList = [];
              this.getRcm(null, this.selectedPageItem, this.searchValue, this.selectedEntityId);
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
              this.loaderService.close();
            }, (error) => {
              this.loaderService.close();
              this.sharedService.handleError(error);
            });
          }
        }
      }
    });
  }

  /**
   * Edit RCM
   * @param rcmId: id to edit RCM
   */
  editRcmId(rcmId: string) {
    this.router.navigate(['rcm/add', { id: rcmId, pageItems: this.selectedPageItem, searchValue: this.searchValue }]);
  }

  /**
   * Set Drop down data
   */
  setDropDownData() {
    // RCM Sector
    this.sectorDrop = '"';
    for (const row of this.sectorList) {
      this.sectorDrop = this.sectorDrop + row.sector + ',';
    }
    this.sectorDrop = this.sectorDrop.slice(0, -1);
    this.sectorDrop = this.sectorDrop + '"';
    // RCM Process
    this.processDrop = '"';
    for (const row of this.processList) {
      this.processDrop = this.processDrop + row.process + ',';
    }
    this.processDrop = this.processDrop.slice(0, -1);
    this.processDrop = this.processDrop + '"';
    // RCM Sub Process
    this.subProcessDrop = '"';
    for (const row of this.subProcessList) {
      this.subProcessDrop = this.subProcessDrop + row.subProcess + ',';
    }
    this.subProcessDrop = this.subProcessDrop.slice(0, -1);
    this.subProcessDrop = this.subProcessDrop + '"';


    // Control Category
    this.controlCategoryDrop = '"';
    for (const row of this.controlCategory) {
      this.controlCategoryDrop = this.controlCategoryDrop + row.value + ',';
    }
    this.controlCategoryDrop = this.controlCategoryDrop.slice(0, -1);
    this.controlCategoryDrop = this.controlCategoryDrop + '"';
    //  Control Type
    this.controlTypeDrop = '"';
    for (const row of this.controltype) {
      this.controlTypeDrop = this.controlTypeDrop + row.value + ',';
    }
    this.controlTypeDrop = this.controlTypeDrop.slice(0, -1);
    this.controlTypeDrop = this.controlTypeDrop + '"';

    // Nature Of Control
    this.natureOfControlDrop = '"';
    for (const row of this.natureOfControl) {
      this.natureOfControlDrop = this.natureOfControlDrop + row.value + ',';
    }
    this.natureOfControlDrop = this.natureOfControlDrop.slice(0, -1);
    this.natureOfControlDrop = this.natureOfControlDrop + '"';

    // Anti Fraud Control
    this.antiFraudControlDrop = '"Yes,No"';
  }

  /**
   * Get drop down data
   */
  getDropDownData() {
    this.selectedEntityId = this.selectedEntityId;
    this.apiService.riskControlMatrixesGetRCMUploadDetail(this.selectedEntityId).subscribe(result => {
      console.log(result);
      // get drop down data
      this.sectorList = result.sectorList;
      this.processList = result.processList;
      this.subProcessList = result.subProcessList;
      this.controlCategory = result.controlCategory;
      this.controltype = result.controlType;
      this.natureOfControl = result.natureOfControl;
      this.setDropDownData();
    });
  }

  /**
   * Create Instruction sheet for rcm
   * @param instructionsWorkSheet: worksheet for instruction. It will be any type of work sheet so param type is not defined
   */
  createInstructionSheet(instructionsWorkSheet) {
    for (let i = 1; i <= 9; i++) {
      instructionsWorkSheet.mergeCells('A' + i.toString() + ': Q' + i.toString());
    }

    instructionsWorkSheet.getCell('A1').value = this.stringConstants.instructionTemplateFile;
    instructionsWorkSheet.getCell('A1').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('A1').font = { bold: true };

    instructionsWorkSheet.getCell('A2').value = '1.  This file template can be used to upload muliple RCMs';
    instructionsWorkSheet.getCell('A3').value = '2.  Follow the format of this file template to upload RCMs. Do not change the headers or delete any sheet. ';
    instructionsWorkSheet.getCell('A4').value = '3.  Add RCMs and its corresponding details in one row of the RCM Table.';
    instructionsWorkSheet.getCell('A5').value = '4.  Do not leave fields which are marked as "Required" empty.';
    instructionsWorkSheet.getCell('A6').value = '5.  Upload RCMs feature cannot be used to add RCM with images, tables or files.';

    instructionsWorkSheet.getCell('A7').value = 'RCM Table';
    instructionsWorkSheet.getCell('A8').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('A9').font = { bold: true };

    for (let i = 10; i <= 22; i++) {
      instructionsWorkSheet.mergeCells('A' + i.toString() + ': C' + i.toString());
      instructionsWorkSheet.mergeCells('D' + i.toString() + ': M' + i.toString());
      instructionsWorkSheet.mergeCells('N' + i.toString() + ': Q' + i.toString());
    }

    instructionsWorkSheet.getCell('A10').value = this.stringConstants.fieldNameText;
    instructionsWorkSheet.getCell('A10').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('A10').font = { italic: true };

    instructionsWorkSheet.getCell('D10').value = this.stringConstants.fieldDescriptionText;
    instructionsWorkSheet.getCell('D10').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('D10').font = { italic: true };

    instructionsWorkSheet.getCell('P10').value = this.stringConstants.valueText;
    instructionsWorkSheet.getCell('P10').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('P10').font = { italic: true };

    instructionsWorkSheet.getCell('A11').value = '1. Sector';
    instructionsWorkSheet.getCell('D11').value = 'Required. Name of Sector.';
    instructionsWorkSheet.getCell('P11').value = 'Sector 1';

    instructionsWorkSheet.getCell('A12').value = '2. Process';
    instructionsWorkSheet.getCell('D12').value = 'Required. Name of Process.';
    instructionsWorkSheet.getCell('P12').value = 'Process 1';

    instructionsWorkSheet.getCell('A13').value = '3. Subprocess';
    instructionsWorkSheet.getCell('D13').value = 'Required. Name of Sub Process.';
    instructionsWorkSheet.getCell('P13').value = 'Subprocess 1';

    instructionsWorkSheet.getCell('A14').value = '4. Risk Description';
    instructionsWorkSheet.getCell('D14').value = 'Required. Name of risk description.';
    instructionsWorkSheet.getCell('P14').value = 'Risk Description 1';

    instructionsWorkSheet.getCell('A15').value = '5. Control Category';
    instructionsWorkSheet.getCell('D15').value = 'Required. Can be Strategic, Operational, Financial, Compliance. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('P15').value = 'Operational';

    instructionsWorkSheet.getCell('A16').value = '6. Control Type';
    instructionsWorkSheet.getCell('D16').value = 'Required. Can be Manual, Automated or Semi-Automated. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('P16').value = 'Manual';

    instructionsWorkSheet.getCell('A17').value = '7. Control Objective';
    instructionsWorkSheet.getCell('D17').value = 'Required. Control Objective of RCM.';
    instructionsWorkSheet.getCell('P17').value = 'Control Objective 1';

    instructionsWorkSheet.getCell('A18').value = '8. Nature Of Control';
    instructionsWorkSheet.getCell('D18').value = 'Required. Can be Preventive or Detective. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('P18').value = 'Preventive';

    instructionsWorkSheet.getCell('A19').value = '9. Anti Fraud Control';
    instructionsWorkSheet.getCell('D19').value = 'Required. Can be Yes or No. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('P19').value = 'Yes';

    instructionsWorkSheet.getCell('A20').value = '10. Risk Category';
    instructionsWorkSheet.getCell('D20').value = 'Required. risk category detail of RCM.';
    instructionsWorkSheet.getCell('P20').value = 'Risk Category 1';
  }

  /**
   * Create RCM sheet
   * @param rcmWorkSheet: worksheet for RCM. It will be any type of work sheet so param type is not defined
   */
  createRCMSheet(rcmWorkSheet) {
    rcmWorkSheet.columns = [
      { header: 'Sector (Required)', key: 'sector', width: 30 }, // A
      { header: 'Process (Required)', key: 'process', width: 30 }, // B
      { header: 'SubProcess (Required)', key: 'subProcess', width: 30 }, // C
      { header: 'Risk Description (Required)', key: 'riskDescription', width: 30 }, // D
      { header: 'Control Category (Required)', key: 'controlCategory', width: 30 }, // E
      { header: 'Control Type (Required)', key: 'controlType', width: 30 }, // F
      { header: 'Control Objective (Required)', key: 'controlObjective', width: 30 }, // G
      { header: 'Control Description', key: 'controlDescription', width: 30 }, // H
      { header: 'Nature Of Control (Required)', key: 'natureOfControl', width: 30 }, // I
      { header: 'Anti Fraud Control (Required)', key: 'antiFraudControl', width: 30 }, // J
      { header: 'Risk Category (Required)', key: 'riskCategory', width: 30 }, // K
    ];

    for (let i = 2; i <= 10000; i++) {
      // i limit cannot be set to the full extent of an excel sheet - takes too much time to download the excel and fails at the end
      // Rcm Sector
      rcmWorkSheet.getCell('A' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.sectorDrop]
      };
      // RCM process
      rcmWorkSheet.getCell('B' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.processDrop]
      };
      // Rcm Sub process
      rcmWorkSheet.getCell('C' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.subProcessDrop]
      };
      // control category
      rcmWorkSheet.getCell('E' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.controlCategoryDrop]
      };
      // Control type
      rcmWorkSheet.getCell('F' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.controlTypeDrop]
      };
      // nature of control
      rcmWorkSheet.getCell('I' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.natureOfControlDrop]
      };
      // anti fraud control
      rcmWorkSheet.getCell('J' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.antiFraudControlDrop]
      };

    }
  }

  /**
   * View file in new tab
   * @param documentPath : File path
   */
  viewFile(documentPath: string) {
    // Open document in new tab
    const url = this.router.serializeUrl(
      // Convert url to base64 encoding. ref https://stackoverflow.com/questions/41972330/base-64-encode-and-decode-a-string-in-angular-2
      this.router.createUrlTree(['document-preview/view', { path: btoa(documentPath) }])
    );
    window.open(url, '_blank');
  }

  /**
   * Download excel file template for RCM Bulk-upload
   */
  downloadTemplateForRCM() {
    let currentEntity = {} as AuditableEntityAC;
    this.sharedService.selectedEntityObjSubject.subscribe((entityObject) => {
      currentEntity = entityObject;
    });
    const workBook = this.sharedService.createBulkUploadExcelTemplate();
    const instructionsWorkSheet = workBook.addWorksheet(this.stringConstants.instructionTemplateFile);
    const rcmWorkSheet = workBook.addWorksheet(this.stringConstants.RiskControlMatrixFile);

    // create instrction sheet
    this.createInstructionSheet(instructionsWorkSheet);
    // create rcm upload sheet
    this.createRCMSheet(rcmWorkSheet);
    const fileName = this.stringConstants.RiskControlMatrixFile + '$$' + currentEntity.name + '$$' + currentEntity.version + this.stringConstants.excelFileExtention;

    this.sharedService.downloadBulkUploadTemplate(fileName, workBook);
    document.getElementById('close-button').click();
  }

  /**
   * On button click open file upload
   */
  openFileUpload() {
    document.getElementById('rcmUpload').click();
  }
  /**
   *  On file change choose file for upload
   * @param event: it will be any type of event. so param type is not defined
   */
  fileChange(event) {
    if (event.target.files.length !== 0) {
      this.file = event.target.files[0];
      if (this.uploadService.checkIfFileIsExcel(this.file.name)) {
        this.bulkUpload.entityId = this.selectedEntityId;
        this.loaderService.open();
        this.uploadService.uploadFileOnAdd<BulkUpload>(this.bulkUpload, event.target.files, this.stringConstants.observationUploadFiles, this.stringConstants.uploadRCMFileApiPath)
          .subscribe(result => {
            this.sharedService.showSuccess(this.stringConstants.dataUploadSuccessMsg);
            this.loaderService.close();
            this.getRcm(this.pageNumber, this.selectedPageItem, this.searchValue, this.selectedEntityId);
          },
            error => {
              this.loaderService.close();
              this.sharedService.showError(error.error);
            });
      } else {
        this.sharedService.showError(this.stringConstants.pleaseSelectExcelFileMsg);
      }
    } else {
      this.sharedService.showError(this.stringConstants.selectFileMsg);
    }
    document.getElementById('close-button').click();
  }

  /**
   * Method for export to excel
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelRcmMainApi + this.selectedEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }
}

