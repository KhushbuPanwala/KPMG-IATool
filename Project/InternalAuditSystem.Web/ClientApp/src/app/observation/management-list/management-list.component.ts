import { Component, OnInit, Inject, Optional, OnDestroy } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Router, ActivatedRoute } from '@angular/router';
import { ObservationsManagementService, ObservationAC, KeyValuePairOfIntegerAndString, BASE_PATH, AuditPlanAC, ProcessAC } from '../../swaggerapi/AngularFiles';
import { Pagination } from '../../models/pagination';
import { SharedService } from '../../core/shared.service';
import { ConfirmationDialogComponent } from '../../shared/confirmation-dialog/confirmation-dialog.component';
import { LoaderService } from '../../core/loader.service';
import { Subscription } from 'rxjs';
import { ObservationStatus } from '../../swaggerapi/AngularFiles/model/observationStatus';
import { UploadService } from '../../core/upload.service';
import { BulkUpload } from '../../shared/bulk-upload';
import { AddTableDialogComponent } from '../../shared/add-table-dialog/add-table-dialog.component';
import { ObservationService } from '../observation.service';

@Component({
  selector: 'app-management-list',
  templateUrl: './management-list.component.html'
})
export class ManagementListComponent implements OnInit, OnDestroy {
  observationManagementTitle: string; // Variable for observation management title
  searchText: string; // Variable for search placeholder
  excelToolTip: string; // Variable for excel tooltip
  headingLabel: string; // Variable for heading label title
  processLabel: string; // Variable for process label creation]
  statusTitle: string; // Variable for status title
  auditorTitle: string; // Variable for auditor title
  personResponsibleLabel: string; // Variable for person responsible text title
  workingFile: string; // Variable for woring file text title
  fileNameText: string; // Variable for file name text title
  viewFiles: string; // Variable for view files
  showingResults: string; // Variable for showing results
  wordToolTip: string; // Variable for word tool tip
  powerPointToolTip: string; // Variable for power point tool tip
  pdfToolTip: string; // Variable for pdf tool tip
  deleteToolTip: string; // Variable for delete tooltip
  editToolTip: string; // Variable for edit tooltip
  addToolTip: string; // Variable for add tooltip
  uploadText: string; // Variable for upload text
  bsModalRef: BsModalRef; // Variable for bs Modal popup
  pageNumber: number = null;
  totalRecords: number;
  searchValue = null;
  selectedPageItem: number; // per page no. of items shows
  pageItems = []; // Per page items for entity list
  selectedEntityId: string;
  observationList = [] as Array<ObservationAC>;
  showingText: string;
  showNoDataText: string;
  unsubscribe: Subscription;
  auditableEntityId: string;

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

  baseApiUrl;
  auditPlanList = [] as Array<AuditPlanAC>;
  observationType = [] as Array<KeyValuePairOfIntegerAndString>;
  disposition = [] as Array<KeyValuePairOfIntegerAndString>;
  observationStatus = [] as Array<KeyValuePairOfIntegerAndString>;
  observationTypeName = '';
  isRepeatedName = '';
  dispositionName = '';
  statusName = '';
  selectedAuditPlan;

  downloadTemplate: string;
  bulkUploadText: string;
  baseUrl: string;
  processList = [] as Array<ProcessAC>;
  subProcessList = [] as Array<ProcessAC>;
  auditPlanLabel: string;
  version: string;
  addTableToolTip: string;
  // Ngx-pagination options
  public responsive = true;
  public labels = {
    previousLabel: ' ',
    nextLabel: ' '
  };

  // observation status array
  statusList = [
    { value: ObservationStatus.NUMBER_0, label: 'Open' },
    { value: ObservationStatus.NUMBER_1, label: 'Closed' },
  ];

  file: File;
  bulkUpload = {} as BulkUpload;
  addTableTitle: string;
  deleteTitle: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;

  // Creates an instance of documenter
  constructor(
    private stringConstants: StringConstants,
    private modalService: BsModalService, private router: Router, private route: ActivatedRoute,
    private observationManagementService: ObservationsManagementService, private sharedService: SharedService,
    private loaderService: LoaderService, @Optional() @Inject(BASE_PATH) basePath: string,
    private uploadService: UploadService,
    private observationService: ObservationService) {
    this.observationManagementTitle = this.stringConstants.observationManagementTitle;
    this.searchText = this.stringConstants.searchText;
    this.excelToolTip = this.stringConstants.excelToolTip;
    this.headingLabel = this.stringConstants.headingLabel;
    this.processLabel = this.stringConstants.processLabel;
    this.statusTitle = this.stringConstants.statusTitle;
    this.auditorTitle = this.stringConstants.auditorTitle;
    this.personResponsibleLabel = this.stringConstants.personResponsibleLabel;
    this.workingFile = this.stringConstants.workingFile;
    this.fileNameText = this.stringConstants.fileNameText;
    this.showingResults = this.stringConstants.showingResults;
    this.wordToolTip = this.stringConstants.wordToolTip;
    this.powerPointToolTip = this.stringConstants.powerPointToolTip;
    this.pdfToolTip = this.stringConstants.pdfToolTip;
    this.viewFiles = this.stringConstants.viewFiles;
    this.editToolTip = this.stringConstants.editToolTip;
    this.deleteToolTip = this.stringConstants.deleteToolTip;
    this.addToolTip = this.stringConstants.addToolTip;
    this.uploadText = this.stringConstants.uploadText;
    this.showingText = this.stringConstants.showingText;
    this.auditPlanLabel = this.stringConstants.auditPlanLabel;
    this.pageItems = this.stringConstants.pageItems;
    this.selectedPageItem = this.pageItems[0].noOfItems;
    this.showNoDataText = this.stringConstants.showNoDataText;
    this.selectedEntityId = '';
    this.downloadTemplate = this.stringConstants.downloadTemplate;
    this.bulkUploadText = this.stringConstants.bulkUploadText;
    this.baseUrl = basePath;
    this.version = this.stringConstants.versionTitle;
    this.addTableTitle = this.stringConstants.addTableTitle;
    this.addTableToolTip = this.stringConstants.addTableToolTip;

    // file format assign
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

    this.deleteTitle = this.stringConstants.deleteTitle;
  }

  /** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *  Initialization of properties.
   */
  ngOnInit() {
    // clear observation files
    this.observationService.setObservationFiles([]);
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(entityId => {
      if (entityId !== '') {
        this.auditableEntityId = entityId;
        this.route.params.subscribe(params => {
          if (params.pageItems !== undefined) {

            this.selectedPageItem = Number(params.pageItems);
          }
          this.searchValue = (params.searchValue !== undefined && params.searchValue !== null) ? params.searchValue : '';
        });
        this.getObservationsList(this.pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId);
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

  /**
   * Method for getting all the observations under an auditable entity
   * @param pageNumber : Current page number
   * @param selectedPageItem :  No. of items display on per page
   * @param searchValue : Search value
   * @param selectedEntityId : selected Entity Id
   */
  getObservationsList(pageNumber: number, selectedPageItem: number, searchValue: string, auditableEntityId: string) {
    this.loaderService.open();
    this.observationManagementService.observationsManagementGetAllObservations(pageNumber, selectedPageItem, searchValue, auditableEntityId, 0, 0)
      .subscribe((result: Pagination<ObservationAC>) => {
        this.observationList = result.items;
        this.pageNumber = result.pageIndex;
        this.totalRecords = result.totalRecords;
        this.showingResults = this.sharedService.setShowingResult(this.pageNumber, selectedPageItem, this.totalRecords);

      }, (error) => {
        this.sharedService.handleError(error);
      });
  }

  /**
   * On change current page
   * @param pageNumber:page no. which is user selected
   */
  onPageChange(pageNumber) {
    this.getObservationsList(pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId);
  }

  /**
   * On per page items change
   */
  onPageItemChange() {
    this.getObservationsList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId);
  }

  /**
   * Search observation Data
   * @param event : key press event
   * @param pageNumber :  current page number
   * @param selectedPageItem :  no. of items display on per page
   * @param searchValue : search value
   */
  searchObservation(event: KeyboardEvent, pageNumber: number, selectedPageItem: number, searchValue: string) {
    if (event.key === this.stringConstants.enterKeyText || event.key === this.stringConstants.tabKeyText) {
      this.getObservationsList(pageNumber, selectedPageItem, searchValue, this.auditableEntityId);
    }
  }

  /**
   * Method for opening add page
   */
  openObservationAddPage() {
    this.router.navigate(['observation-management/add', { observationId: 0, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.auditableEntityId }]);
  }

  /**
   * Method for edit observation
   * @param observationId : Id of observation
   */
  editObservation(observationId: string) {
    this.router.navigate(['observation-management/add', { observationId, pageItems: this.selectedPageItem, searchValue: this.searchValue, entityId: this.auditableEntityId }]);
  }

  /**
   * Method for deleting observation
   * @param observationId : Id of observation
   */
  deleteObservation(observationId: string) {
    const initialState = {
      title: this.stringConstants.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {

          this.observationManagementService.observationsManagementDeleteObservation(observationId, this.auditableEntityId).subscribe(data => {
            this.getObservationsList(null, this.selectedPageItem, this.searchValue, this.auditableEntityId);
            this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);

          }, (error) => {
            this.sharedService.handleError(error);
          });
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
  }

  /**
   * Method for export to excel
   */
  exportToExcel() {
    const offset = new Date().getTimezoneOffset();

    const url = this.baseUrl + this.stringConstants.exportToExcelObservationApi + this.auditableEntityId + '&timeOffset=' + offset;

    this.sharedService.exportToExcel(url);
  }


  /**
   * Get Audit plans
   */
  getAuditPlans() {
    this.selectedEntityId = this.auditableEntityId;
    this.observationManagementService.observationsManagementGetObservationUploadDetail(this.selectedEntityId).subscribe(result => {
      // get drop down data
      this.auditPlanList = result.auditPlanList;
      this.selectedAuditPlan = this.auditPlanList[0].id;
      this.observationType = result.observationType;
      this.disposition = result.disposition;
      this.observationStatus = result.observationStatus;

      this.setDropDownData();
    }, (error) => {
      this.sharedService.handleError(error);
    });
  }

  /**
   * Set Drop down data
   */
  setDropDownData() {
    // observation Type
    this.observationTypeName = '"';
    for (const row of this.observationType) {
      this.observationTypeName = this.observationTypeName + row.value + ',';
    }
    this.observationTypeName = this.observationTypeName.slice(0, -1);
    this.observationTypeName = this.observationTypeName + '"';


    // disposition
    this.dispositionName = '"';
    for (const row of this.disposition) {
      this.dispositionName = this.dispositionName + row.value + ',';
    }
    this.dispositionName = this.dispositionName.slice(0, -1);
    this.dispositionName = this.dispositionName + '"';

    // observation status
    this.statusName = '"';
    for (const row of this.observationStatus) {
      if (row.value !== this.stringConstants.completedStatusTitle) {
        this.statusName = this.statusName + row.value + ',';
      }
    }
    this.statusName = this.statusName.slice(0, -1);
    this.statusName = this.statusName + '"';

    // is repeated
    this.isRepeatedName = '"Yes,No"';
  }

  /**
   * Create Instruction sheet for observation
   * @param instructionsWorkSheet: worksheet for instruction. It will be any type of work sheet so param type is not defined
   */
  createInstructionSheet(instructionsWorkSheet) {
    for (let i = 1; i <= 9; i++) {
      instructionsWorkSheet.mergeCells('A' + i.toString() + ': Q' + i.toString());
    }

    instructionsWorkSheet.getCell('A1').value = this.stringConstants.instructionTemplateFile;
    instructionsWorkSheet.getCell('A1').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('A1').font = { bold: true };

    instructionsWorkSheet.getCell('A2').value = '1.  This file template can be used to upload muliple Observations';
    instructionsWorkSheet.getCell('A3').value = '2.  Follow the format of this file template to upload observations. Do not change the headers or delete any sheet. ';
    instructionsWorkSheet.getCell('A4').value = '3.  Add Observations and its corresponding details in one row of the Observation Table.';
    instructionsWorkSheet.getCell('A5').value = '4.  Do not leave fields which are marked as "Required" empty.';
    instructionsWorkSheet.getCell('A6').value = '5.  Upload observations feature cannot be used to add observation with images, tables or files.';

    instructionsWorkSheet.getCell('A7').value = 'Observation Table';
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

    instructionsWorkSheet.getCell('N10').value = this.stringConstants.valueText;
    instructionsWorkSheet.getCell('N10').alignment = { vertical: 'middle', horizontal: 'center' };
    instructionsWorkSheet.getCell('N10').font = { italic: true };

    instructionsWorkSheet.getCell('A11').value = '1. Process';
    instructionsWorkSheet.getCell('D11').value = 'Required. Name of Process.';
    instructionsWorkSheet.getCell('N11').value = 'Process 1';

    instructionsWorkSheet.getCell('A12').value = '2. Subprocess';
    instructionsWorkSheet.getCell('D12').value = 'Required. Name of Sub Process.';
    instructionsWorkSheet.getCell('N12').value = 'Subprocess 1';

    instructionsWorkSheet.getCell('A13').value = '3. Heading';
    instructionsWorkSheet.getCell('D13').value = 'Required. Name of Observation.';
    instructionsWorkSheet.getCell('N13').value = 'Observation 1';

    instructionsWorkSheet.getCell('A14').value = '4. Background';
    instructionsWorkSheet.getCell('D14').value = 'Required. Background detail of Observation.';
    instructionsWorkSheet.getCell('N14').value = 'Background 1';

    instructionsWorkSheet.getCell('A15').value = '5. Observations';
    instructionsWorkSheet.getCell('D15').value = 'Required. Observation detail.';
    instructionsWorkSheet.getCell('N15').value = 'Observations 1';

    instructionsWorkSheet.getCell('A16').value = '6. Observation Type';
    instructionsWorkSheet.getCell('D16').value = 'Required. Can be Legal, Compliance, Process or Finacial. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('N16').value = 'Legal';

    instructionsWorkSheet.getCell('A17').value = '7. Is Repeated';
    instructionsWorkSheet.getCell('D17').value = 'Required. Can be Yes or No. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('N17').value = 'Yes';

    instructionsWorkSheet.getCell('A18').value = '8. RootCause';
    instructionsWorkSheet.getCell('D18').value = 'Required. RootCause detail of Osbservation.';
    instructionsWorkSheet.getCell('N18').value = 'RootCause 1';

    instructionsWorkSheet.getCell('A19').value = '9. Implication';
    instructionsWorkSheet.getCell('D19').value = 'Required. Implication detail of Observation.';
    instructionsWorkSheet.getCell('N19').value = 'Implication 1';

    instructionsWorkSheet.getCell('A20').value = '10. Disposition';
    instructionsWorkSheet.getCell('D20').value = 'Required. Can be Reportable or NonReportable. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('N20').value = 'Reportable';

    instructionsWorkSheet.getCell('A21').value = '11. Status';
    instructionsWorkSheet.getCell('D21').value = 'Required. Can be Open, Closed, Panding or Completed. Dropdowns have been provided in cells for selection.';
    instructionsWorkSheet.getCell('N21').value = 'Open';

    instructionsWorkSheet.getCell('A22').value = '12. Recommendation';
    instructionsWorkSheet.getCell('D22').value = 'Required. Recommendation detail of Observation.';
    instructionsWorkSheet.getCell('N22').value = 'Recommendation 1';
  }

  /**
   * Create observation sheet
   * @param observationWorkSheet: worksheet for observation. It will be any type of work sheet so param type is not defined
   */
  createObservationSheet(observationWorkSheet) {
    observationWorkSheet.columns = [
      { header: 'Process (Required)', key: 'process', width: 30 }, // A
      { header: 'SubProcess (Required)', key: 'subProcess', width: 30 }, // B
      { header: 'Heading (Required)', key: 'heading', width: 30 },      // C
      { header: 'Background (Required)', key: 'background', width: 30 }, // D
      { header: 'Observations (Required)', key: 'observations', width: 30 }, // E
      { header: 'Observation Type (Required)', key: 'observationType', width: 30 }, // F
      { header: 'Is Repeated (Required)', key: 'isRepeated', width: 30 }, // G
      { header: 'RootCause (Required)', key: 'rootCause', width: 30 }, // H
      { header: 'Implication (Required)', key: 'implication', width: 30 }, // I
      { header: 'Disposition (Required)', key: 'disposition', width: 30 }, // J
      { header: 'Status (Required)', key: 'status', width: 30 }, // K
      { header: 'Recommendation (Required)', key: 'recommendation', width: 30 }, // L
      { header: 'ManagementResponse', key: 'managementResponse', width: 30 }, // M
      { header: 'Conclusion', key: 'conclusion', width: 30 }, // N
    ];

    for (let i = 2; i <= 10000; i++) {
      // i limit cannot be set to the full extent of an excel sheet - takes to much time to download the excel and fails at the end

      // process
      observationWorkSheet.getCell('A' + i.toString()).dataValidation = {
        allowBlank: false,
      };
      // subprocess
      observationWorkSheet.getCell('B' + i.toString()).dataValidation = {
        allowBlank: false,
      };

      // heading
      observationWorkSheet.getCell('C' + i.toString()).dataValidation = {
        allowBlank: false,
      };
      // Observation Type
      observationWorkSheet.getCell('F' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.observationTypeName]
      };
      // Is Repeated
      observationWorkSheet.getCell('G' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.isRepeatedName]
      };
      // Disposition
      observationWorkSheet.getCell('J' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.dispositionName]
      };
      // Status
      observationWorkSheet.getCell('K' + i.toString()).dataValidation = {
        type: 'list',
        allowBlank: false,
        formulae: [this.statusName]
      };
    }
  }

  /**
   * Download excel file template for Observation Bulk-upload
   */
  downloadTemplateForObservation() {
    const workBook = this.sharedService.createBulkUploadExcelTemplate();
    const instructionsWorkSheet = workBook.addWorksheet(this.stringConstants.instructionTemplateFile);
    const observationWorkSheet = workBook.addWorksheet(this.stringConstants.observationTemplateFile);

    // create instrction sheet
    this.createInstructionSheet(instructionsWorkSheet);
    // create observation upload sheet
    this.createObservationSheet(observationWorkSheet);
    const selectedPlan = this.auditPlanList.find(a => a.id === this.selectedAuditPlan);
    const fileName = this.stringConstants.observationTemplateFile + '$$' + selectedPlan.title + '$$' + selectedPlan.version + this.stringConstants.excelFileExtention;
    this.sharedService.downloadBulkUploadTemplate(fileName, workBook);
    document.getElementById('close-button').click();
  }

  /**
   * On button click open file upload
   */
  openFileUpload() {
    document.getElementById('observationUpload').click();
  }
  /**
   *  On file change choose file for upload
   * @param event: it will be any type of event. so param type is not defined
   */
  fileChange(event) {
    if (event.target.files.length !== 0) {
      this.file = event.target.files[0];
      if (this.uploadService.checkIfFileIsExcel(this.file.name)) {
        this.selectedEntityId = this.auditableEntityId;
        this.bulkUpload.entityId = this.selectedEntityId;
        this.loaderService.open();
        this.uploadService.uploadFileOnAdd<BulkUpload>(this.bulkUpload, event.target.files, this.stringConstants.observationUploadFiles, this.stringConstants.uploadObservationsFileApiPath)
          .subscribe(result => {
            this.sharedService.showSuccess(this.stringConstants.dataUploadSuccessMsg);
            this.getObservationsList(this.pageNumber, this.selectedPageItem, this.searchValue, this.auditableEntityId);
            this.loaderService.close();
          }, (error) => {
              this.loaderService.close();
              this.sharedService.handleError(error);
          });
      } else {
        this.loaderService.close();
        this.sharedService.showError(this.stringConstants.pleaseSelectExcelFileMsg);
      }
    } else {
      this.loaderService.close();
      this.sharedService.showError(this.stringConstants.selectFileMsg);
    }
    document.getElementById('close-button').click();
  }



  /**
   * Method to open add table dialog
   * @param observationId Observation id of which table data is to be shown
   */
  openAddTableDialog(observationObjectId: string) {
    this.observationManagementService.observationsManagementGetObservationTable(observationObjectId, this.auditableEntityId).subscribe(
      result => {
        const initialState = {
          title: this.addTableTitle,
          keyboard: true,
          tableList: result,
          observationId: observationObjectId
        };
        this.bsModalRef = this.modalService.show(AddTableDialogComponent,
          Object.assign({ initialState }, { class: 'page-modal audit-team-add' }));
      }, (error) => {
        this.sharedService.handleError(error);
      });
  }


  /**
   * Check file type to display icon accordingly
   * @param fileName :Name of the File
   * @param fileTypeCheck : type to check
   */
  checkFileExtention(fileName: string, fileTypeCheck: string) {
    let isUploadedFormatMatched = false;
    isUploadedFormatMatched = this.observationService.checkFileExtention(fileName, fileTypeCheck);
    return isUploadedFormatMatched;
  }

  /**
   * Method to open delete confirmation dialog
   * @param observation: index
   * @param observationId: observation Id
   */
  openDeleteModal(observation: ObservationAC, observationDocumentId: string) {
    const initialState = {
      title: this.deleteTitle,
      keyboard: true,
      callback: (result) => {
        if (result === this.stringConstants.yes) {
          if (observationDocumentId !== '') {
            this.loaderService.open();
            this.observationManagementService.observationsManagementDeleteObservationDocument(observationDocumentId, this.auditableEntityId).subscribe(() => {
              observation.observationDocuments.splice(
                observation.observationDocuments.indexOf(observation.observationDocuments.filter(x => x.id === observationDocumentId)[0]), 1);
              observation.observationDocuments = [...observation.observationDocuments];
              this.sharedService.showSuccess(this.stringConstants.recordDeletedMsg);
              this.loaderService.close();
            }, (error) => {
              this.sharedService.handleError(error);
            });
          }
        }
      }
    };
    this.bsModalRef = this.modalService.show(ConfirmationDialogComponent,
      Object.assign({ initialState }, { class: 'page-modal delete-modal' }));
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
   * Download select document
   * @param documentId : Select observation document id
   */
  downloadFile(documentId: string) {
    this.observationManagementService.observationsManagementGetObservationDocumentDownloadUrl(documentId, this.auditableEntityId).subscribe((result) => {
      const aTag = document.createElement('a');
      aTag.setAttribute('style', 'display:none;');
      document.body.appendChild(aTag);
      aTag.download = '';
      aTag.href = result;
      aTag.target = '_blank';
      aTag.click();
      document.body.removeChild(aTag);
    });
  }

  /**
   * Create PPT Observation
   * @param observationId : Select observation id
   */
  createObservationPPT(observationId: StringConstants) {
    // get current system timezone
    const timeOffset = new Date().getTimezoneOffset();
    const url = this.baseUrl + this.stringConstants.downloadObservationPPTApi + observationId + this.stringConstants.entityParamString + this.auditableEntityId + this.stringConstants.timeOffSet + timeOffset;
    this.sharedService.createPPT(url);
  }
}
