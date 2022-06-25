import { Component, OnInit, OnDestroy } from '@angular/core';
// import { StringConstants } from '../stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { JsonDocumentModel, RowDataObj } from '../../../../models/JsonDocumentModel';
import { StringConstants } from '../../../../shared/stringConstants';
import { ReportObservationsService } from '../../../../swaggerapi/AngularFiles';
import { SharedService } from '../../../../core/shared.service';
import { ReportSharedService } from '../../report-shared.service';
import { ReportObservationTableAC } from '../../../../swaggerapi/AngularFiles/model/reportObservationTableAC';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-report-add-table-dialog',
  templateUrl: './report-add-table-dialog.component.html',
  styleUrls: ['./report-add-table-dialog.component.scss']
})
export class ReportAddTableDialogComponent implements OnInit, OnDestroy {
  title: string;
  column1: string;
  loremIpsumText: string;
  saveButtonText: string;
  tableList: string;
  addColumn: string;
  addRow: string;
  deleteRow: string;
  deleteColumn: string;
  jsonDocument: JsonDocumentModel;
  columnNames: Array<string>;
  reportObservationId: string;
  data: Array<RowDataObj>;
  serialNumber: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  selectedEntityId: string;


  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              public bsModalRef: BsModalRef,
              private apiService: ReportObservationsService,
              private sharedService: SharedService,
              private reportService: ReportSharedService
  ) {
    this.column1 = this.stringConstants.column1;
    this.loremIpsumText = this.stringConstants.loremIpsumText;
    this.saveButtonText = this.stringConstants.saveButtonText;
    this.addColumn = this.stringConstants.addColumn;
    this.addRow = this.stringConstants.addRow;
    this.deleteRow = this.stringConstants.deleteRow;
    this.deleteColumn = this.stringConstants.deleteColumn;
    this.serialNumber = this.stringConstants.serialNumber;
  }

  /*** Lifecycle hook that is called after data-bound properties of a directive are initialized.
   *   Initialization of properties.
   */
  ngOnInit() {
    // get the current selectedEntityId
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe((entityId) => {
      if (entityId !== '') {
        this.selectedEntityId = entityId;
        this.jsonDocument = JSON.parse(this.tableList).RootElement;
        this.updateColumnArrayLocally();
        this.updateRowArrayLocally();
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

  /** Add column in table */
  addTableColumn() {
    if (this.reportObservationId !== null) {
      this.apiService.reportObservationsUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.reportObservationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.apiService.reportObservationsAddColumnInTable(this.jsonDocument.tableId, this.reportObservationId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            },
            error => {
              this.sharedService.showError(this.stringConstants.somethingWentWrong);
            });
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * To update jsonDocument on clicking on save
   */
  onSave() {
    if (this.reportObservationId !== null) {
      this.apiService.reportObservationsUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.reportObservationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        result => {
          this.jsonDocument = JSON.parse(result).RootElement;
          // set report observation table data into selected observation
          const reportObservation = this.reportService.reportObservation;
          const reportObservationTable = {} as ReportObservationTableAC;
          reportObservation.reportObservationTableList = [];
          reportObservationTable.reportObservationId = reportObservation.id;
          reportObservationTable.table = JSON.parse(result).RootElement;
          reportObservation.reportObservationTableList.push(reportObservationTable);
          reportObservation.reportObservationTableList = reportObservation.reportObservationTableList;
          reportObservation.tableCount = reportObservation.reportObservationTableList.length;
          this.reportService.updateReportObservationList(reportObservation);
          // update table count
          this.reportService.updateAddTableCount(reportObservation.id, reportObservation.tableCount);
          this.bsModalRef.hide();
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /** Add row in table */
  addRowInTable() {
    if (this.reportObservationId !== null) {
      this.apiService.reportObservationsUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.reportObservationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.apiService.reportObservationsAddRow(this.jsonDocument.tableId, this.reportObservationId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            }, (error) => {
              this.sharedService.handleError(error);
            });
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * Delete row in table
   * @param rowId Row id of row which is to be deleted
   */
  deleteRowInTable(rowId: string) {
    if (this.reportObservationId !== null) {
      this.apiService.reportObservationsUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.reportObservationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.apiService.reportObservationsDeleteRow(this.reportObservationId, this.jsonDocument.tableId, rowId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            }, (error) => {
              this.sharedService.handleError(error);
            });
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   * Delete column in table
   * @param columnPosition Column position from where column is to be deleted
   */
  deleteColumnInTable(columnPosition: number) {
    if (this.reportObservationId !== null) {
      this.apiService.reportObservationsUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.reportObservationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.apiService.reportObservationsDeleteColumn(this.reportObservationId, this.jsonDocument.tableId, columnPosition, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            }, (error) => {
              this.sharedService.handleError(error);
            });
        }, (error) => {
          this.sharedService.handleError(error);
        });
    }
  }

  /**
   *  Update columns locally to update in html
   */
  updateColumnArrayLocally() {
    this.columnNames = [] as Array<string>;
    for (let i = 0; i < this.jsonDocument.columnNames.length; i++) {
      this.columnNames[i] = this.jsonDocument.columnNames[i];
    }
  }

  /**
   *  Update rows locally to update in html
   */
  updateRowArrayLocally() {
    this.data = [] as Array<RowDataObj>;
    for (let i = 0; i < this.jsonDocument.data.length; i++) {
      const rowdataObject = { RowData: [] as Array<string> } as RowDataObj;
      this.data.push(rowdataObject);
      for (let j = 0; j < this.jsonDocument.data[i].RowData.length; j++) {
        this.data[i].RowData[j] = this.jsonDocument.data[i].RowData[j];
      }
    }
  }
}
