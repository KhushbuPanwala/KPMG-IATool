import { Component, OnInit, OnDestroy } from '@angular/core';
import { StringConstants } from '../stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { debuglog } from 'util';
import { ObservationsManagementService } from '../../swaggerapi/AngularFiles';
import { SharedService } from '../../core/shared.service';
import { JsonDocumentModel, RowDataObj } from '../../models/JsonDocumentModel';
import { AcmService} from '../../swaggerapi/AngularFiles';
import { ObservationService } from '../../observation/observation.service';
import { Subscription } from 'rxjs';


@Component({
  selector: 'app-add-table-dialog',
  templateUrl: './add-table-dialog.component.html',
  styleUrls: ['./add-table-dialog.component.scss']
})
export class AddTableDialogComponent implements OnInit, OnDestroy {
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
  observationId: string;
  acmId: string;
  data: Array<RowDataObj>;
  serialNumber: string;
  // only to subscripe for the current component
  entitySubscribe: Subscription;
  selectedEntityId: string;

  // Creates an instance of documenter.
  constructor(private stringConstants: StringConstants,
              public bsModalRef: BsModalRef,
              private observationService: ObservationService,
              private observationSwaggerService: ObservationsManagementService,
              private acmService: AcmService,
              private sharedService: SharedService) {
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
  ngOnInit(): void {
    this.entitySubscribe = this.sharedService.selectedEntitySubject.subscribe(entityId => {
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
    if (this.observationId !== null && this.acmId == null) {
        this.observationSwaggerService.observationsManagementUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.observationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.observationSwaggerService.observationsManagementAddColumnInTable(this.jsonDocument.tableId, this.observationId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            },
            error => {
              this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    } else {
      this.acmService.acmUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.acmId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.acmService.acmAddColumnInTable(this.jsonDocument.tableId, this.acmId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            },
            error => {
                this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    }
  }

  /** To update jsonDocument on clicking on save */
  onChange() {
    if (this.observationId !== null && this.acmId == null) {
        this.observationSwaggerService.observationsManagementUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.observationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        result => {
          this.jsonDocument = JSON.parse(result).RootElement;
          this.bsModalRef.hide();
        },
        error => {
          this.sharedService.handleError(error);
        });
    } else {
      this.acmService.acmUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.acmId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        result => {
          this.jsonDocument = JSON.parse(result).RootElement;
          this.bsModalRef.hide();
        },
        error => {
            this.sharedService.handleError(error);
        });
    }
  }

  /** Add row in table */
  addRowInTable() {
    if (this.observationId !== null && this.acmId == null) {
        this.observationSwaggerService.observationsManagementUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.observationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.observationSwaggerService.observationsManagementAddRow(this.jsonDocument.tableId, this.observationId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            },
            error => {
              this.sharedService.handleError(error);
            });
        },
        error => {
          this.sharedService.handleError(error);
        });
    } else {
      this.acmService.acmUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.acmId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.acmService.acmAddRow(this.jsonDocument.tableId, this.acmId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            },
            error => {
                this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    }
  }

  /**
   * Delete row in table
   * @param rowId Row id of row which is to be deleted
   */
  deleteRowInTable(rowId: string) {
    if (this.observationId !== null && this.acmId == null) {
        this.observationSwaggerService.observationsManagementUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.observationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.observationSwaggerService.observationsManagementDeleteRow(this.observationId, this.jsonDocument.tableId, rowId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            },
            error => {
              this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    } else {
      this.acmService.acmUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.acmId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.acmService.acmDeleteRow(this.acmId, this.jsonDocument.tableId, rowId, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateRowArrayLocally();
            },
            error => {
                this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    }
  }

  /**
   * Delete column in table
   * @param columnPosition Column position from where column is to be deleted
   */
  deleteColumnInTable(columnPosition: number) {
    if (this.observationId !== null && this.acmId == null) {
        this.observationSwaggerService.observationsManagementUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.observationId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.observationSwaggerService.observationsManagementDeleteColumn(this.observationId, this.jsonDocument.tableId, columnPosition, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            },
            error => {
              this.sharedService.handleError(error);
            });
        },
        error => {
          this.sharedService.handleError(error);
        });
    } else {
      this.acmService.acmUpdateJsonDocument(JSON.stringify(this.jsonDocument), this.acmId, this.jsonDocument.tableId, this.selectedEntityId).subscribe(
        res => {
          this.acmService.acmDeleteColumn(this.acmId, this.jsonDocument.tableId, columnPosition, this.selectedEntityId).subscribe(
            result => {
              this.jsonDocument = JSON.parse(result).RootElement;
              this.updateColumnArrayLocally();
              this.updateRowArrayLocally();
            },
            error => {
                this.sharedService.handleError(error);
            });
        },
        error => {
            this.sharedService.handleError(error);
        });
    }
  }

  /** Update columns locally to update in html */
  updateColumnArrayLocally() {
    this.columnNames = [] as Array<string>;
    for (let i = 0; i < this.jsonDocument.columnNames.length; i++) {
      this.columnNames[i] = this.jsonDocument.columnNames[i];
    }
  }

  /** Update rows locally to update in html */
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
